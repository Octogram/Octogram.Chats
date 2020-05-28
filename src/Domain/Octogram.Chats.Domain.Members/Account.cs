using System;

namespace Octogram.Chats.Domain.Members
{
	public class Account
	{
		public Account(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; private set; }
	}
}
