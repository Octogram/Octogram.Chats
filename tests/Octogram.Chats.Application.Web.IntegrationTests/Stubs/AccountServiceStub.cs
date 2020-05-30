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
				: base(Guid.Parse("9BBAE6B2-B9A1-4D15-9F05-66F9FDCB9F29"))
			{
			}
		}
	}
}
