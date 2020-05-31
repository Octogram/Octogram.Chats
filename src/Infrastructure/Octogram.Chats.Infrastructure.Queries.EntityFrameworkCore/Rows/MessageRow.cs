using System;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows
{
	public class MessageRow
	{
		public Guid Id { get; set; }

		public Guid ChatId { get; set; }
		
		public ChatRow Chat { get; set; }

		public string State { get; set; }

		public string Content { get; set; }
		
		public DateTimeOffset SentDate { get; set; }
	}
}
