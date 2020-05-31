using System;
using System.Threading;
using System.Threading.Tasks;

namespace Octogram.Chats.Domain.Members
{
	public interface IAccountService
	{
		Task<Account> GetCurrentAsync(CancellationToken cancellationToken);
	}
}
