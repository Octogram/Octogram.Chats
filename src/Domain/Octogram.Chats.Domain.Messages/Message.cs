using System;
using Messenger.Domain.Chats;

namespace Messenger.Domain.Messages
{
	public class Message
	{
		protected Message()
		{
			Id = default;
		}
		
		public Message(DateTimeOffset sentDate, Chat chat, string content)
			: this()
		{
			SentDate = sentDate;
			Chat = chat;
			Content = content;

			State = MessageState.Sending;
		}

		public Guid Id { get; private set; }
		
		public DateTimeOffset SentDate { get; private set; }
		
		public virtual Chat Chat { get; private set; }

		public MessageState State { get; private set; }
		
		public string Content { get; private set; }
	}

	public enum MessageState
	{
		Sending,
		Sent,
		Read
	}
}
