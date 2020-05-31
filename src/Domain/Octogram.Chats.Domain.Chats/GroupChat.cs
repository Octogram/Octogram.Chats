using System;
using System.Collections.Generic;
using Octogram.Chats.Domain.Members;

namespace Messenger.Domain.Chats
{
	public class GroupChat : Chat
	{
		private readonly IList<Member> _members;

		protected GroupChat()
		{
		}
		
		/// <inheritdoc />
		protected GroupChat(string name,DateTimeOffset createDate, Member owned)
			: base(name, createDate, owned)
		{
		}

		public GroupChat(string name, DateTimeOffset createDate, Member owner, ICollection<Member> group)
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
			
			foreach (Member member in group)
			{
				_members.Add(member);
			}
		}

		public virtual IEnumerable<Member> Members => _members;
	}
}
