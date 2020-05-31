using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Messenger.Domain.Chats;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Domain.Abstractions.Exceptions;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.Commands.Chats
{
	public class ChangeChatInfoHandler : IRequestHandler<ChangeChatInfoCommand, bool>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly IAccountService _accountService;

		public ChangeChatInfoHandler(RepositoryDbContext dbContext, IAccountService accountService)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <inheritdoc />
		public async Task<bool> Handle(ChangeChatInfoCommand request, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);

			DirectChat chat = await _dbContext
				.Chats
				.OfType<DirectChat>()
				.FirstOrDefaultAsync(
					ch => ch.Id == request.ChatId && ch.Owner.Id == account.Id,
					cancellationToken);

			if (chat is null)
			{
				throw new EntityNotFoundException(typeof(Chat), $"Chat with id {request.ChatId} not found.");
			}
			
			chat.SetChatName(request.Name);
			
			return true;
		}
	}
}
