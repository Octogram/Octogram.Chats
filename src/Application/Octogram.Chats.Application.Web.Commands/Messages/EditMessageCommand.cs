using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Messages
{
	public class EditMessageCommand : IRequest<bool>
	{
		public Guid MessageId { get; set; }

		public string Content { get; set; }
	}
}
