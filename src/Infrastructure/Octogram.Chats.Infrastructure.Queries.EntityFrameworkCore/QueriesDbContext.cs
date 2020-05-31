using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Mappings;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore
{
	public class QueriesDbContext : DbContext
	{
		/// <inheritdoc />
		public QueriesDbContext(DbContextOptions<QueriesDbContext> options)
			: base(options)
		{
		}

		public IQueryable<ChatRow> Chats 
			=> Set<ChatRow>().AsNoTracking();

		public IQueryable<MessageRow> Messages 
			=> Set<MessageRow>().AsNoTracking();

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ChatRowMapping());
			modelBuilder.ApplyConfiguration(new AccountRowMapping());
			modelBuilder.ApplyConfiguration(new MessageRowMapping());
			
			if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
			{
				EnableSqliteDateTimeOffsetConvertible(modelBuilder);
			}
		}

		private static void EnableSqliteDateTimeOffsetConvertible(ModelBuilder modelBuilder)
		{
			foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
			{
				IEnumerable<PropertyInfo> properties = entityType
					.ClrType
					.GetProperties()
					.Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

				foreach (PropertyInfo property in properties)
				{
					modelBuilder
						.Entity(entityType.Name)
						.Property(property.Name)
						.HasConversion(new DateTimeOffsetToBinaryConverter());
				}
			}
		}
	}
}
