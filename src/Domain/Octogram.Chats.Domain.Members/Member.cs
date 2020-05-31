using System;
using Octogram.Chats.Domain.Abstractions;

namespace Octogram.Chats.Domain.Members
{
	public class Member : Entity<Guid>
	{
		protected Member()
		{
		}
		
		public Member(Guid accountId)
			: base(accountId)
		{
		}
	}
}
