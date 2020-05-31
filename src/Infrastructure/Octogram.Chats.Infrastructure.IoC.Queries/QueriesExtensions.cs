using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Octogram.Chats.Application.Web.Queries.Chats;
using Octogram.Chats.Infrastructure.Queries.Chats;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore;

namespace Octogram.Chats.Infrastructure.IoC.Queries
{
	public static class QueriesExtensions
	{
		public static IServiceCollection AddDatabaseQueries(
			this IServiceCollection services,
			IDatabaseSettings settings)
		{
			services.AddDbContext<QueriesDbContext>(options =>
			{
				options.UseNpgsql(settings.ConnectionString);
			});

			services.AddScoped<IChatQueries, ChatQueries>();

			return services;
		}
	}
}
