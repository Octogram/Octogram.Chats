using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings
{
	public class AccountMapping : IEntityTypeConfiguration<Account>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.ToTable("Accounts");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Username)
				.HasColumnName("Username")
				.HasMaxLength(255);

			builder.Property(p => p.UsernameId)
				.HasColumnName("UsernameId")
				.HasMaxLength(64);

			builder.Property(p => p.Name)
				.HasColumnName("Name")
				.HasMaxLength(500);
			
			builder.Property(p => p.Email)
				.HasColumnName("Email")
				.HasMaxLength(320);
		}
	}
}
