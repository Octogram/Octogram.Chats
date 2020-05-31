using System;
using System.Collections.Generic;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public class GroupChat : Chat
	{
		private readonly IList<Account> _members;

		protected GroupChat()
		{
		}
		
		/// <inheritdoc />
		protected GroupChat(string name,DateTimeOffset createDate, Account owner)
			: base(name, createDate, owner)
		{
		}

		public GroupChat(string name, DateTimeOffset createDate, Account owner, ICollection<Account> group)
			: this(name, createDate, owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException(nameof(owner));
			}

			if (group == null)
			{
				throw new ArgumentNullException(nameof(group));
			}

			_members.Add(owner);
			
			foreach (Account member in group)
			{
				_members.Add(member);
			}
		}

		public virtual IEnumerable<Account> Members => _members;
	}
}
