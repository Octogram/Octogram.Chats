using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Chats
{
	public class CreateChatCommand : IRequest<bool>
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public Guid To { get; set; }
	}
}
