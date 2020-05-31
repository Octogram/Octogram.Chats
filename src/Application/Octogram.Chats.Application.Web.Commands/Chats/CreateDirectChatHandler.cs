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
	public class CreateDirectChatHandler : IRequestHandler<CreateDirectChatCommand, bool>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly IAccountService _accountService;

		public CreateDirectChatHandler(RepositoryDbContext dbContext, IAccountService accountService)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <inheritdoc />
		public async Task<bool> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
		{
			Account owner = await _accountService.GetCurrentAsync(cancellationToken);
			Account member = await _dbContext
				.Accounts
				.FirstOrDefaultAsync(acc => acc.Id == request.To, cancellationToken);

			if (member is null)
			{
				throw new EntityNotFoundException(typeof(Account), $"Account with id {request.To} not found");
			}

			var chat = new DirectChat(DateTimeOffset.UtcNow, owner, member);

			await _dbContext.Chats.AddAsync(chat, cancellationToken);

			return true;
		}
	}
}
