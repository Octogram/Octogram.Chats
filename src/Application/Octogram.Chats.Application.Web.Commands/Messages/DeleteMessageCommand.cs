using System;

namespace Messenger.Web.Controllers
{
	public class DeleteMessageCommand
	{
		public Guid AccountId { get; set; }

		public Guid MessageId { get; set; }
	}
}