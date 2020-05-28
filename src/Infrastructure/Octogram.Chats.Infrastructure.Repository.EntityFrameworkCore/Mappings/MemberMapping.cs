using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore.Mappings
{
	public class MemberMapping : IEntityTypeConfiguration<Member>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Member> builder)
		{
			builder.ToTable("Members");

			builder.HasKey(p => p.Id);
		}
	}
}
