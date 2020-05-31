using System;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public class DirectChat : Chat
	{
		protected DirectChat()
		{
		}
		
		protected DirectChat(string name, DateTimeOffset createDate, Account owner)
			: base(name, createDate, owner)
		{
			
		}
		
		/// <inheritdoc />
		public DirectChat(DateTimeOffset createDate, Account owner, Account member)
			: this(member.Id.ToString(), createDate, owner)
		{
			if (member is null)
			{
				throw new ArgumentNullException();
			}

			Member = member;
		}

		public virtual Account Member { get; private set; }

		public void SetChatName(string name)
		{
			Name = name;
		}
	}
}
