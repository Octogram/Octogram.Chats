using System;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public abstract class Chat
	{
		private Chat()
		{
		}
		
		protected Chat(Guid id, DateTimeOffset createDate, Member owned)
		{
			Id = id;
			CreateDate = createDate;
			Owned = owned;
		}

		public Guid Id { get; private set; }

		public virtual Member Owned { get; private set; }

		public DateTimeOffset CreateDate { get; private set; }
	}
}
