using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.Services
{
	public class HttpContextAccountService : IAccountService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly RepositoryDbContext _dbContext;

		public HttpContextAccountService(IHttpContextAccessor httpContextAccessor, RepositoryDbContext dbContext)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		/// <inheritdoc />
		public async Task<Account> GetCurrentAsync(CancellationToken cancellationToken)
		{
			if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				return null;
			}
			
			Claim usernameClaim = _httpContextAccessor.HttpContext.User
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.Name);

			if (usernameClaim is null)
			{
				throw new InvalidOperationException($"User identity hasn't username claim.");
			}

			Account account = await _dbContext
				.Accounts
				.FirstOrDefaultAsync(a => a.Username == usernameClaim.Value, cancellationToken);

			return account;
		}

		private static Account CreateAccountFromIdentity(ClaimsPrincipal claimsPrincipal)
		{
			Claim usernameId = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			
			Claim username = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.Name);
			
			Claim email = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.Email);
			
			Claim name  = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == "urn:github:name");
			
			return new Account(
				username?.Value,
				usernameId?.Value,
				name?.Value,
				email?.Value);
		}
	}
}
