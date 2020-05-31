using System;
using Octogram.Chats.Domain.Abstractions;

namespace Octogram.Chats.Domain.Members
{
	public class Account : Entity<Guid>
	{
		protected Account()
		{
		}
		
		public Account(string username, string usernameId, string name, string email)
			: this()
		{
			Username = username;
			UsernameId = usernameId;
			Name = name;
			Email = email;
		}

		public string Username { get; private set; }
		
		public string UsernameId { get; private set; }

		public string Name { get; private set; }

		public string Email { get; private set; }
	}
}
