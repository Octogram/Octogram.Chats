using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Domain.Abstractions.Exceptions;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.Commands.Messages
{
	public class EditMessageHandler : IRequestHandler<EditMessageCommand, bool>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly IAccountService _accountService;

		public EditMessageHandler(RepositoryDbContext dbContext, IAccountService accountService)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <inheritdoc />
		public async Task<bool> Handle(EditMessageCommand request, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);

			Message existedMessage = await _dbContext
				.Messages
				.SingleOrDefaultAsync(m => m.Id == request.MessageId && m.Chat.Owner.Id == account.Id, cancellationToken);

			if (existedMessage is null)
			{
				throw new EntityNotFoundException(
					typeof(Message),
					message: $"Message with id {request.MessageId} not found");
			}
			
			existedMessage.EditContent(request.Content);

			return true;
		}
	}
}
