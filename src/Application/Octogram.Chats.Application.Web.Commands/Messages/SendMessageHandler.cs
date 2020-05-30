using System;
using System.Threading;
using System.Threading.Tasks;
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
		private readonly ICommandsBus _commandsBus;
		private readonly RepositoryDbContext _dbContext;

		public SendMessageHandler(ICommandsBus commandsBus, RepositoryDbContext dbContext)
		{
			_commandsBus = commandsBus ?? throw new ArgumentNullException(nameof(commandsBus));
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
		
		/// <inheritdoc />
		public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
		{
			Chat chat = await _dbContext.Chats.FirstOrDefaultAsync(
				ch => ch.Id == request.ChatId,
				cancellationToken);
			
			var message = new Message(DateTimeOffset.UtcNow, chat, request.Content);
			await _dbContext.Messages.AddAsync(message, cancellationToken);
			
			_commandsBus.Enqueue<IMessageSendCommand>(new
			{
				MessageId = message.Id,
				ChatId = request.ChatId,
				Content = request.Content
			});

			return true;
		}
	}
}
