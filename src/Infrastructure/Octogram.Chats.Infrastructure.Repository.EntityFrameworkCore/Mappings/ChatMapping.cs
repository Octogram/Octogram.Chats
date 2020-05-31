using Messenger.Domain.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings
{
	public class ChatMapping : IEntityTypeConfiguration<Chat>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Chat> builder)
		{
			builder.ToTable("Chats");

			builder.HasKey(p => p.Id);

			builder.HasDiscriminator<string>("Type")
				.HasValue<DirectChat>(nameof(DirectChat));

			builder.Property(p => p.Name)
				.HasColumnName("Name");
			
			builder.Property(p => p.CreateDate)
				.HasColumnType("timestamptz")
				.HasColumnName("CreateDate");

			builder.HasOne(p => p.Owner)
				.WithOne()
				.HasForeignKey<Chat>("OwnerId");
		}
	}
}
