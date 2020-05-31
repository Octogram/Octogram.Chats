using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Mappings
{
	public class AccountRowMapping : IEntityTypeConfiguration<AccountRow>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<AccountRow> builder)
		{
			builder.ToTable("Accounts");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Username)
				.HasColumnName("Username");

			builder.Property(p => p.UsernameId)
				.HasColumnName("UsernameId");

			builder.Property(p => p.Name)
				.HasColumnName("Name");
			
			builder.Property(p => p.Email)
				.HasColumnName("Email");
		}
	}
}
