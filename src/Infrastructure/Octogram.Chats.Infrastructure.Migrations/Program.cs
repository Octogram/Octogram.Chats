using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octogram.Chats.Infrastructure.Migrations.Settings;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;
using Serilog;

namespace Octogram.Chats.Infrastructure.Migrations
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			try
			{
				Log.Information("Starting up");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureServices((hostContext, services) =>
				{
					var databaseSettings = new DatabaseSettings();
					hostContext.Configuration.Bind("DatabaseSettings", databaseSettings);
					string migrationsAssembly = typeof(Program)
						.GetTypeInfo()
						.Assembly
						.GetName()
						.Name;

					services.AddDbContext<RepositoryDbContext>(options =>
					{
						options.UseNpgsql(databaseSettings.ConnectionString, config =>
						{
							config.MigrationsAssembly(migrationsAssembly);
						});
					});
				});
	}
}
