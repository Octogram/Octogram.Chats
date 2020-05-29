using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Application.Web.Services
{
	public class HttpContextAccountService : IAccountService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextAccountService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		/// <inheritdoc />
		public async Task<Account> GetCurrentAsync(CancellationToken cancellationToken)
		{
			if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				return null;
			}
			
			Claim subject = _httpContextAccessor
				.HttpContext
				.User
				.Claims
				.FirstOrDefault(c => c.Type == "sub");

			if (subject != null)
			{
				return new Account(Guid.Parse(subject.Value));
			}

			return null;
		}
	}
}
