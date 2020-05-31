using System;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows
{
	public class ChatRow
	{
		public Guid Id { get; set; }

		public Guid OwnerId { get; set; }
		
		public AccountRow Owner { get; set; }

		public string Type { get; set; }

		public string Name { get; set; }

		public Guid MemberId { get; set; }
		
		public AccountRow Member { get; set; }
	}
}
