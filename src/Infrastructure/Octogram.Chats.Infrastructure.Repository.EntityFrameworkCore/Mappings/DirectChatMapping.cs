using Messenger.Domain.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings
{
	public class DirectChatMapping : IEntityTypeConfiguration<DirectChat>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<DirectChat> builder)
		{
			builder.HasOne(p => p.Member)
				.WithOne()
				.HasForeignKey<DirectChat>("MemberId");
		}
	}
}
