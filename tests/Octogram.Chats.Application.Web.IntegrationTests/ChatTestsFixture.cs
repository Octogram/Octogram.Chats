using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.IntegrationTests
{
	[TestFixture]
	public abstract class ChatTestsFixture
	{
		private readonly ChatApplicationWebFactory _factory;
		private IServiceScope _scope;

		protected ChatTestsFixture()
		{
			_factory = new ChatApplicationWebFactory();
		}

		[SetUp]
		public void Setup()
		{
			_scope = _factory.Services.CreateScope();
			QueriesContext = _scope.ServiceProvider.GetRequiredService<QueriesDbContext>();
			RepositoryContext = _scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
			AccountService = _scope.ServiceProvider.GetService<IAccountService>();

			HttpClient = _factory.CreateClient();
			
			RepositoryContext.Database.OpenConnection();
			RepositoryContext.Database.EnsureCreated();
		}

		[TearDown]
		public void Down()
		{
			RepositoryContext.Database.EnsureDeleted();
			RepositoryContext.Database.CloseConnection();
			_scope.Dispose();
		}
		
		protected HttpClient HttpClient { get; private set; }
		
		protected QueriesDbContext QueriesContext { get; private set; }
		
		protected RepositoryDbContext RepositoryContext { get; private set; }
		
		protected IAccountService AccountService { get; private set; }
	}
}
