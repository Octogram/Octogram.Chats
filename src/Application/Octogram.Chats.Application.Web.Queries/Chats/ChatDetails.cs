using System;

namespace Octogram.Chats.Application.Web.Queries.Chats
{
	public class ChatDetails
	{
		public Guid Id { get; set; }

		public ChatDetailsMember Member { get; set; }
	}
}
