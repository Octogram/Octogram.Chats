using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Infrastructure.IoC.Repositories
{
	public static class RepositoriesExtensions
	{
		public static IServiceCollection AddDatabaseRepositories(
			this IServiceCollection services,
			IDatabaseSettings settings)
		{
			services.AddDbContext<RepositoryDbContext>(options =>
			{
				options.UseNpgsql(settings.ConnectionString);
				options.UseLazyLoadingProxies();
			});

			return services;
		}
	}
}
