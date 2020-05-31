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
		
		protected Chat(string name, DateTimeOffset createDate, Account owner)
		{
			Name = name;
			CreateDate = createDate;
			Owner = owner;
		}

		public string Name { get; protected set; }

		public virtual Account Owner { get; private set; }

		public DateTimeOffset CreateDate { get; private set; }
	}
}
