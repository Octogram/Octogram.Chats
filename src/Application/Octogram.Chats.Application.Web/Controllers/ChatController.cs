using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octogram.Chats.Application.Web.Commands.Chats;
using Octogram.Chats.Application.Web.Queries;
using Octogram.Chats.Application.Web.Queries.Chats;
using Octogram.Chats.Domain.Members;

namespace Octogram.Chats.Application.Web.Controllers
{
	[Route("chats")]
	public class ChatController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IChatQueries _chatQueries;
		private readonly IAccountService _accountService;

		/// <inheritdoc />
		public ChatController(IMediator mediator, IChatQueries chatQueries, IAccountService accountService)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_chatQueries = chatQueries ?? throw new ArgumentNullException(nameof(chatQueries));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		[HttpGet(Name = "GetChats")]
		[ProducesResponseType(statusCode: 200, type: typeof(List<Chat>))]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);
			
			IEnumerable<Chat> chats = await _chatQueries.GetAsync(account.Id, cancellationToken);
			
			return this.Ok(chats.ToList());
		}

		[HttpGet("{chatId}", Name = "GetChat")]
		[ProducesResponseType(statusCode: 200, type: typeof(Chat))]
		public async Task<IActionResult> Get(Guid chatId, CancellationToken cancellationToken)
		{
			Account account = await _accountService.GetCurrentAsync(cancellationToken);
			
			ChatDetails chat = await _chatQueries.GetDetailsAsync(
				account.Id,
				chatId,
				cancellationToken);
			
			return this.Ok(chat);
		}

		[HttpGet("{chatId}/messages", Name = "GetChatMessages")]
		[ProducesResponseType(statusCode: 200, type: typeof(PagedList<ChatMessage>))]
		public async Task<IActionResult> Get(
			[FromRoute] Guid chatId,
			[FromQuery] int? page,
			int? size,
			CancellationToken cancellationToken)
		{
			int pageValue = page ?? 1;
			int pageSize = size ?? 25;
			
			Account account = await _accountService.GetCurrentAsync(cancellationToken);

			PagedList<ChatMessage> messages = await _chatQueries.GetMessagesAsync(
				account.Id,
				chatId,
				pageValue,
				pageSize,
				cancellationToken);

			return this.Ok(messages);
		}

		[HttpPost(Name = "PostChat")]
		public async Task<IActionResult> Post([FromBody] CreateDirectChatCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			return this.Ok();
		}

		[HttpPut(Name = "PutChat")]
		public async Task<IActionResult> Put([FromBody] ChangeChatInfoCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			return this.Ok();
		}

		[HttpDelete("{chatId}", Name = "DeleteChat")]
		public async Task<IActionResult> Delete([FromRoute] Guid chatId, CancellationToken cancellationToken)
		{
			var command = new DeleteChatCommand
			{
				ChatId = chatId
			};

			await _mediator.Send(command, cancellationToken);

			return this.Ok();
		}
	}
}
