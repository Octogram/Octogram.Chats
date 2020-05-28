using System;

namespace Octogram.Chats.Domain.Members
{
	public class Member
	{
		protected Member()
		{
		}

		public Member(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; private set; }
	}
}
