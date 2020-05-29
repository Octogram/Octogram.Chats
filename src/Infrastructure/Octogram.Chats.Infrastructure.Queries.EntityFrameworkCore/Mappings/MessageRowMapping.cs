using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Mappings
{
	public class MessageRowMapping : IEntityTypeConfiguration<MessageRow>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<MessageRow> builder)
		{
			builder.ToTable("Messages");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Content)
				.HasColumnName("Content");

			builder.Property(p => p.State)
				.HasColumnName("State");

			builder.Property(p => p.SentDate)
				.HasColumnName("SentDate");

			builder.HasOne(p => p.Chat)
				.WithOne()
				.HasForeignKey<MessageRow>("ChatId");
		}
	}
}
