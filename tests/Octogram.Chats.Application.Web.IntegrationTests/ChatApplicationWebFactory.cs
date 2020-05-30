using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Octogram.Chats.Application.Web.Commands;
using Octogram.Chats.Application.Web.IntegrationTests.Extensions;
using Octogram.Chats.Application.Web.IntegrationTests.Stubs;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;
using Serilog;

namespace Octogram.Chats.Application.Web.IntegrationTests
{
	public class ChatApplicationWebFactory : WebApplicationFactory<Startup>
	{
		/// <inheritdoc />
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				DbConnection connection = CreateSqliteConnection();
				
				services
					.ReplaceDbContextOnSqlite<RepositoryDbContext>(connection)
					.ReplaceDbContextOnSqlite<QueriesDbContext>(connection);

				services.AddSingleton<IAccountService, AccountServiceStub>();
				services.AddSingleton<ICommandsBus, SilentCommandsBusStub>();
			});

			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.MinimumLevel.Information()
				.CreateLogger();
		}

		private static DbConnection CreateSqliteConnection()
		{
			var connection = new SqliteConnection("Filename=:memory:");

			return connection;
		}
	}
}
