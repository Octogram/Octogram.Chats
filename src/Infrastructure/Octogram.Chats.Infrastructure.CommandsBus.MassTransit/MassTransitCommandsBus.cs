using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Octogram.Chats.Application.Web.Commands;

namespace Octogram.Chats.Infrastructure.CommandsBus.MassTransit
{
	public class MassTransitCommandsBus : ICommandsBus
	{
		private readonly IBus _bus;
		private readonly Queue<(object Command, Type Type)> _commands;

		public MassTransitCommandsBus(IBus bus)
		{
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
			_commands = new Queue<(object, Type)>();
		}

		/// <inheritdoc />
		public IEnumerable<object> Commands => _commands
			.Select(c => c.Command)
			.AsEnumerable();

		/// <inheritdoc />
		public void Enqueue<T>(object command) where T : class
		{
			_commands.Enqueue((command, typeof(T)));
		}

		/// <inheritdoc />
		public async Task SendAllAsync(CancellationToken cancellationToken)
		{
			while (_commands.Any())
			{
				(object command, Type type) = _commands.Dequeue();
				await _bus.Send(command, type, cancellationToken);
			}
		}
	}
}
