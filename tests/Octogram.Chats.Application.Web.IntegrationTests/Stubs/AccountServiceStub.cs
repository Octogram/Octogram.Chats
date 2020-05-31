using System;
using System.Threading;
using System.Threading.Tasks;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Application.Web.IntegrationTests.Stubs
{
	public class AccountServiceStub : IAccountService
	{
		private readonly Account _account;

		public AccountServiceStub()
		{
			_account = new TestCurrentAccount();
		}

		public AccountServiceStub(Account account)
		{
			_account = account ?? throw new ArgumentNullException(nameof(account));
		}

		/// <inheritdoc />
		public Task<Account> GetCurrentAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(_account);
		}

		private class TestCurrentAccount : Account
		{
			/// <inheritdoc />
			public TestCurrentAccount()
				: base(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
			{
			}
		}
	}
}
