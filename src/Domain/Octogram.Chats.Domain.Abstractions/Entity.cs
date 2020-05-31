using System;

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
	}
}
