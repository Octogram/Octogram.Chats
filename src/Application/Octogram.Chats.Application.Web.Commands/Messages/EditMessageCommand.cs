using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Messages
{
	public class EditMessageCommand : IRequest<bool>
	{
		public Guid AccountId { get; set; }
		
		public Guid MessageId { get; set; }

		public Guid Content { get; set; }
	}
}
