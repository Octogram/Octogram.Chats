using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Mappings
{
	public class ChatRowMapping : IEntityTypeConfiguration<ChatRow>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ChatRow> builder)
		{
			builder.ToTable("Chats");

			builder.HasKey(p => p.Id);
			
			builder.Property(p => p.Type)
				.HasColumnName("Type");

			builder.Property(p => p.Name)
				.HasColumnName("Name");

			builder.HasOne(p => p.Owner)
				.WithOne()
				.HasForeignKey<ChatRow>(p => p.OwnedId);

			builder.HasOne(p => p.Member)
				.WithOne()
				.HasForeignKey<ChatRow>(p => p.MemberId);
		}
	}
}
