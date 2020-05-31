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
			Account account = await AccountService.GetCurrentAsync(CancellationToken);
			var owned = new Member(account.Id);
			var member = new Member(Guid.Parse("03C2781A-6CB1-4E27-8AF7-725FB5159244"));
			var directChat = new DirectChat(
				createDate: DateTimeOffset.Now,
				owned: owned,
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
		public async Task Can_edit_message()
		{
			// Arrange
			Account account = await AccountService.GetCurrentAsync(CancellationToken);
			var owned = new Member(account.Id);
			var member = new Member(Guid.Parse("03C2781A-6CB1-4E27-8AF7-725FB5159244"));
			var directChat = new DirectChat(
				createDate: DateTimeOffset.Now,
				owned: owned,
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
		public async Task Can_edit_message_but_not_found()
		{
			// Arrange
			Account account = await AccountService.GetCurrentAsync(CancellationToken);
			var owned = new Member(account.Id);
			var member = new Member(Guid.Parse("03C2781A-6CB1-4E27-8AF7-725FB5159244"));
			var directChat = new DirectChat(
				createDate: DateTimeOffset.Now,
				owned: owned,
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
	}
}
