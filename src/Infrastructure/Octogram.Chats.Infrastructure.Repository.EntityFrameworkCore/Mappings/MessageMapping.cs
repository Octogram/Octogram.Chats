using System;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings
{
	public class MessageMapping : IEntityTypeConfiguration<Message>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			builder.ToTable("Messages");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id);

			builder.Property(p => p.Content)
				.HasColumnName("Content")
				.HasMaxLength(1000);

			builder.Property(p => p.SentDate)
				.HasColumnType("timestamptz")
				.HasColumnName("SentDate");

			builder.Property(p => p.State)
				.HasColumnName("State")
				.HasConversion(
					to => to.ToString("G"),
					from => (MessageState)Enum.Parse(typeof(MessageState), from))
				.HasDefaultValue(MessageState.Sending);

			builder.HasOne(p => p.Chat)
				.WithMany()
				.HasForeignKey("ChatId");
		}
	}
}
