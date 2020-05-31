using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Messenger.Domain.Chats;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.Commands.Chats
{
	public class CreateChatHandler : IRequestHandler<CreateChatCommand, bool>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly IAccountService _accountService;

		public CreateChatHandler(RepositoryDbContext dbContext, IAccountService accountService)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <inheritdoc />
		public async Task<bool> Handle(CreateChatCommand request, CancellationToken cancellationToken)
		{
			Chat chat = await this.CreateChat(request, cancellationToken);

			await _dbContext.Chats.AddAsync(chat, cancellationToken);

			return true;
		}

		private async Task<Chat> CreateChat(CreateChatCommand request, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);
			
			switch (request.Type)
			{
				case "Direct":
					var member = new Member(request.To);
					var owned = new Member(account.Id);
					var chat = new DirectChat(DateTimeOffset.UtcNow, owned, member);

					return chat;
				
				default:
					throw new ArgumentOutOfRangeException(
						paramName: nameof(request.Type),
						message: $"{nameof(request.Type)} has unexpected value: {request.Type}");
			}
		}
	}
}
