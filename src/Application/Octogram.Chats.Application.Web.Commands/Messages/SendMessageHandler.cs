using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Messenger.Domain.Chats;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;
using Octogram.Contracts.Commands;

namespace Octogram.Chats.Application.Web.Commands.Messages
{
	public class SendMessageHandler : IRequestHandler<SendMessageCommand, bool>
	{
		private readonly IBus _bus;
		private readonly RepositoryDbContext _dbContext;

		public SendMessageHandler(IBus bus, RepositoryDbContext dbContext)
		{
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
		
		/// <inheritdoc />
		public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
		{
			Chat chat = await _dbContext.Chats.FirstOrDefaultAsync(
				ch => ch.Id == request.ChatId,
				cancellationToken);
			
			var message = new Message(NewId.NextGuid(), DateTimeOffset.UtcNow, chat, request.Content);

			await _dbContext.Messages.AddAsync(message, cancellationToken);
			
			await _bus.Send<IMessageSendCommand>(new
			{
				MessageId = message.Id,
				ChatId = request.ChatId,
				Content = request.Content
			}, cancellationToken);

			return true;
		}
	}
}
