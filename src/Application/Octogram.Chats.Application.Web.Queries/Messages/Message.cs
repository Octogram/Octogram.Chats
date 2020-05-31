using System;

namespace Octogram.Chats.Application.Web.Queries.Messages
{
	public class Message
	{
		public Guid Id { get; set; }

		public string Content { get; set; }

		public string State { get; set; }
		
		public DateTimeOffset SentDate { get; set; }
	}
}
