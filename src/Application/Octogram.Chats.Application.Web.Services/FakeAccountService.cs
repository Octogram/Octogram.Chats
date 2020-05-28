using System;
using System.Threading;
using System.Threading.Tasks;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Application.Web.Services
{
	public class FakeAccountService : IAccountService
	{
		/// <inheritdoc />
		public async Task<Account> GetCurrentAsync(CancellationToken cancellationToken)
		{
			return new Account(Guid.Parse("4552C40C-E9DB-4236-BD7F-B1C23DA61B68"));
		}
	}
}
