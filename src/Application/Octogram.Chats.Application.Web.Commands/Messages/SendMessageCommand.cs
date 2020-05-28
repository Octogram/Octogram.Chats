using System;
using MediatR;

namespace Octogram.Chats.Application.Web.Commands.Messages
{
	public class SendMessageCommand : IRequest<bool>
	{
		public Guid AccountId { get; set; }

		public Guid MemberId { get; set; }
		
		public string Content { get; set; }

		public Guid ChatId { get; set; }
	}
}
