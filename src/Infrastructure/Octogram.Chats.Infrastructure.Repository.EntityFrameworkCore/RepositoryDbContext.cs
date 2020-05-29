using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Messenger.Domain.Chats;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore
{
	public class RepositoryDbContext : DbContext
	{
		/// <inheritdoc />
		public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
			: base(options)
		{
		}

		public DbSet<Message> Messages { get; set; }
		
		public DbSet<Chat> Chats { get; set; }
		
		public DbSet<Member> Interlocutors { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new MessageMapping());
			modelBuilder.ApplyConfiguration(new ChatMapping());
			modelBuilder.ApplyConfiguration(new DirectChatMapping());
			modelBuilder.ApplyConfiguration(new MemberMapping());
			
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
