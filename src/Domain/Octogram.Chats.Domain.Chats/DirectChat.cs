using System;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public class DirectChat : Chat
	{
		protected DirectChat()
		{
		}
		
		protected DirectChat(string name, DateTimeOffset createDate, Member owned)
			: base(name, createDate, owned)
		{
			
		}
		
		/// <inheritdoc />
		public DirectChat(DateTimeOffset createDate, Member owned, Member member)
			: this(member.Id.ToString(), createDate, owned)
		{
			if (member is null)
			{
				throw new ArgumentNullException();
			}

			Member = member;
		}

		public virtual Member Member { get; private set; }

		public void SetChatName(string name)
		{
			Name = name;
		}
	}
}
