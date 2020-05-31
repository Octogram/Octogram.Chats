using System;
using Octogram.Chats.Domain.Abstractions;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public abstract class Chat : Entity<Guid>
	{
		protected Chat()
		{
		}
		
		protected Chat(string name, DateTimeOffset createDate, Member owned)
		{
			Name = name;
			CreateDate = createDate;
			Owned = owned;
		}

		public string Name { get; protected set; }

		public virtual Member Owned { get; private set; }

		public DateTimeOffset CreateDate { get; private set; }
	}
}
