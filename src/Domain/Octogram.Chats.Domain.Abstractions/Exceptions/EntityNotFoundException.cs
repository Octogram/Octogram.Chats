using System;

namespace Octogram.Chats.Domain.Abstractions.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public Type EntityType { get; }

		/// <inheritdoc />
		public EntityNotFoundException(Type entityType, string message)
			: base(message)
		{
			EntityType = entityType;
		}
	}
}
