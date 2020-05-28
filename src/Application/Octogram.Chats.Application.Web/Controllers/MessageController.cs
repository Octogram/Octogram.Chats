using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octogram.Chats.Application.Web.Commands.Messages;
using Octogram.Chats.Domain.Members;

namespace Messenger.Web.Controllers
{
	[Route("messages")]
	public class MessageController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IAccountService _accountService;

		/// <inheritdoc />
		public MessageController(IMediator mediator, IAccountService accountService)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		[HttpPost(Name = "PostMessage")]
		public async Task<IActionResult> Post([FromBody] SendMessageCommand command, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);

			command.AccountId = account.Id;
			
			await _mediator.Send(command, cancellationToken);

			return Ok();
		}

		[HttpPut(Name = "PutMessage")]
		public async Task<IActionResult> Put([FromBody] EditMessageCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			return Ok();
		}

		[HttpDelete("{messageId}", Name = "DeleteMessage")]
		public async Task<IActionResult> Delete([FromRoute] Guid messageId, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);
			
			var command = new DeleteMessageCommand
			{
				AccountId = account.Id,
				MessageId = messageId
			};

			await _mediator.Send(command, cancellationToken);

			return Ok();
		}
	}
}
