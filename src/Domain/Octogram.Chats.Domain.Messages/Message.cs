using System;
using Messenger.Domain.Chats;
using Octogram.Chats.Domain.Abstractions;

namespace Messenger.Domain.Messages
{
	public class Message : Entity<Guid>
	{
		protected Message()
		{
		}
		
		public Message(DateTimeOffset sentDate, Chat chat, string content)
		{
			SentDate = sentDate;
			Chat = chat;
			Content = content;

			State = MessageState.Sending;
		}

		public DateTimeOffset SentDate { get; private set; }
		
		public virtual Chat Chat { get; private set; }

		public MessageState State { get; private set; }
		
		public string Content { get; private set; }

		public void EditContent(string content)
		{
			Content = content;
		}
	}
}
