using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Chats
{
	public class CreateDirectChatCommand : IRequest<bool>
	{
		public Guid To { get; set; }
	}
}
