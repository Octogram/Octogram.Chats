using System.Linq;
using Microsoft.EntityFrameworkCore;
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
			modelBuilder.ApplyConfiguration(new MemberTableMapping());
			modelBuilder.ApplyConfiguration(new MessageTableMapping());
		}
	}
}
