using System;

namespace Octogram.Chats.Application.Web.Queries.Chats
{
	public class Chat
	{
		public Guid Id { get; set; }

		public ChatMember Member { get; set; }
	}
}
