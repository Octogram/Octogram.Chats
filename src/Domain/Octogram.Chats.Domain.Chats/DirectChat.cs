using System;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public class DirectChat : Chat
	{
		protected DirectChat(DateTimeOffset createDate, Member owned)
			: base(default, createDate, owned)
		{
			
		}
		
		protected DirectChat()
			: this(default, default)
		{
		}
		
		/// <inheritdoc />
		public DirectChat(DateTimeOffset createDate, Member owned, Member member)
			: this(createDate, owned)
		{
			if (member is null)
			{
				throw new ArgumentNullException();
			}

			Member = member;
		}

		public virtual Member Member { get; private set; }
	}
}
