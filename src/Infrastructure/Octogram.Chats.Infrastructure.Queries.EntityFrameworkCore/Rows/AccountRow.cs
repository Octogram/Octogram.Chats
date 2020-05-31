using System;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows
{
	public class AccountRow
	{
		public Guid Id { get; set; }
		
		public string Username { get; set; }
		
		public string UsernameId { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }
	}
}
