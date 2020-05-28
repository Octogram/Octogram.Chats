using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Mappings
{
	public class MemberTableMapping : IEntityTypeConfiguration<MemberRow>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<MemberRow> builder)
		{
			builder.ToTable("Members");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				.HasColumnName("Id");
		}
	}
}
