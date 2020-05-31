using System;

namespace Octogram.Chats.Application.Web.Queries.Chats
{
	public class ChatMessage
	{
		public Guid Id { get; set; }

		public string Content { get; set; }

		public string State { get; set; }
		
		public DateTimeOffset SentDate { get; set; }
	}
}
