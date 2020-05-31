using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Messenger.Domain.Chats;
using Messenger.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Octogram.Chats.Application.Web.Commands.Messages;
using Octogram.Chats.Application.Web.IntegrationTests.Extensions;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;
using Shouldly;

namespace Octogram.Chats.Application.Web.IntegrationTests
{
	public class MessagesTests : ChatApplicationTestsFixture
	{
		private static readonly CancellationToken CancellationToken = CancellationToken.None;

		[Test]
		public async Task Can_send_message_to_direct_chat()
		{
			// Arrange
			Account owner = await AccountService.GetCurrentAsync(CancellationToken);
			var member = new Account(
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString());
			var directChat = new DirectChat(
				createDate: DateTimeOffset.Now,
				owner: owner,
				member: member);

			string content = Guid.NewGuid().ToString();

			await RepositoryContext.Chats.AddAsync(directChat, CancellationToken);
			await RepositoryContext.SaveChangesAsync(CancellationToken);
			
			// Act
			var command = new SendMessageCommand
			{
				ChatId = directChat.Id,
				MemberId = member.Id,
				Content = content
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PostAsync("messages", payload, CancellationToken);
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			MessageRow message = await QueriesContext
				.Messages
				.FirstOrDefaultAsync(m => m.ChatId == directChat.Id);
			message.ShouldNotBeNull();
			message.Content.ShouldBe(content);
		}
		
		[Test]
		public async Task Cannot_send_message_direct_chat_not_found()
		{
			// Arrange
			var notFoundChatId = Guid.NewGuid();
			var member = new Account(
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString());
			string content = Guid.NewGuid().ToString();
			
			// Act
			var command = new SendMessageCommand
			{
				ChatId = notFoundChatId,
				MemberId = member.Id,
				Content = content
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PostAsync("messages", payload, CancellationToken);
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
		
		[Test]
		public async Task Can_edit_message()
		{
			// Arrange
			Account owner = await AccountService.GetCurrentAsync(CancellationToken);
			var member = new Account(
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString());
			var directChat = new DirectChat(
				createDate: DateTimeOffset.Now,
				owner: owner,
				member: member);

			string existedContent = Guid.NewGuid().ToString();
			var existedMessage = new Message(DateTimeOffset.UtcNow, directChat, existedContent);

			await RepositoryContext.Chats.AddAsync(directChat, CancellationToken);
			await RepositoryContext.Messages.AddAsync(existedMessage, CancellationToken);
			await RepositoryContext.SaveChangesAsync(CancellationToken);
			
			// Act
			string newContent = Guid.NewGuid().ToString();
			var command = new EditMessageCommand
			{
				Content = newContent,
				MessageId = existedMessage.Id
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PutAsync("messages", payload, CancellationToken);
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			MessageRow message = await QueriesContext
				.Messages
				.FirstOrDefaultAsync(m => m.Id == existedMessage.Id);
			message.ShouldNotBeNull();
			message.Content.ShouldBe(newContent);
		}
		
		[Test]
		public async Task Cannot_edit_message_not_found()
		{
			// Act
			string newContent = Guid.NewGuid().ToString();
			var command = new EditMessageCommand
			{
				Content = newContent,
				MessageId = Guid.NewGuid()
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PutAsync("messages", payload, CancellationToken);
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}
