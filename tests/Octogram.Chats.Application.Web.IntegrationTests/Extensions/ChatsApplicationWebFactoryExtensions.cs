using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Octogram.Chats.Application.Web.IntegrationTests.Extensions
{
	internal static class ChatsApplicationWebFactoryExtensions
	{
		public static IServiceCollection ReplaceDbContextOnSqlite<T>(this IServiceCollection services, DbConnection connection)
			where T : DbContext
		{
			ServiceDescriptor descriptor = services
				.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<T>));

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}
			
			services.AddDbContext<T>(options =>
			{
				options.UseSqlite(connection);
			});

			return services;
		}
	}
}
