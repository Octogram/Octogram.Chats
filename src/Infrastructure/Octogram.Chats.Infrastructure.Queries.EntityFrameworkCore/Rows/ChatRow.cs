using System;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows
{
	public class ChatRow
	{
		public Guid Id { get; set; }

		public Guid OwnedId { get; set; }
		
		public MemberRow Owner { get; set; }

		public string Type { get; set; }

		public Guid MemberId { get; set; }
		
		public MemberRow Member { get; set; }
	}
}
