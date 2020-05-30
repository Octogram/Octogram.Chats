using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Octogram.Chats.Application.Web.Commands;

namespace Octogram.Chats.Application.Web.IntegrationTests.Stubs
{
	public class SilentCommandsBusStub : ICommandsBus
	{
		/// <inheritdoc />
		public IEnumerable<object> Commands => Enumerable.Empty<object>();

		/// <inheritdoc />
		public void Enqueue<T>(object command) where T : class
		{
		}

		/// <inheritdoc />
		public Task SendAllAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
