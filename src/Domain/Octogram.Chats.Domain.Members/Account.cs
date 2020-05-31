using System;
using Octogram.Chats.Domain.Abstractions;

namespace Octogram.Chats.Domain.Members
{
	public class Account : Entity<Guid>, IEquatable<Account>
	{
		protected Account()
		{
		}
		
		public Account(Guid id)
			: base(id)
		{
		}

		/// <inheritdoc />
		public bool Equals(Account other)
		{
			return Id == other?.Id;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((Account)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public static bool operator ==(Account left, Account right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Account left, Account right)
		{
			return !Equals(left, right);
		}
	}
}
