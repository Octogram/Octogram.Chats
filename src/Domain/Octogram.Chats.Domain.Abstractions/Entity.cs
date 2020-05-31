using System;
using System.Collections.Generic;

namespace Octogram.Chats.Domain.Abstractions
{
	public abstract class Entity<TKey> where TKey : IEquatable<TKey>
	{
		protected Entity()
		{
			Id = default;
		}
		
		protected Entity(TKey id)
			: this()
		{
			Id = id;
		}

		public TKey Id { get; private set; }

		protected bool IsTransient()
		{
			return EqualityComparer<TKey>.Default.Equals(Id, default);
		}
		
		protected bool Equals(Entity<TKey> other)
		{
			return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
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

			return Equals((Entity<TKey>)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return EqualityComparer<TKey>.Default.GetHashCode(Id);
		}

		public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
		{
			return !Equals(left, right);
		}
	}
}
