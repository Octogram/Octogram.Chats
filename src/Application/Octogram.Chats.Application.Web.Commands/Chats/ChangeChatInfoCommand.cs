using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Chats
{
	public class ChangeChatInfoCommand : IRequest<bool>
	{
		public Guid ChatId { get; set; }

		public string Name { get; set; }
	}
}
