using Messenger.Domain.Chats;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
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
		}
	}
}
