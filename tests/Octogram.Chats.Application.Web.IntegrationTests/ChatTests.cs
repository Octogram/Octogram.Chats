using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Messenger.Domain.Chats;
using Messenger.Domain.Messages;
using NUnit.Framework;
using Octogram.Chats.Application.Web.Commands.Chats;
using Octogram.Chats.Application.Web.IntegrationTests.Extensions;
using Octogram.Chats.Application.Web.Queries;
using Octogram.Chats.Application.Web.Queries.Chats;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore.Rows;
using Shouldly;
using Chat = Octogram.Chats.Application.Web.Queries.Chats.Chat;

namespace Octogram.Chats.Application.Web.IntegrationTests
{
	[Parallelizable]
	public class ChatTests : ChatApplicationTestsFixture
	{
		private static readonly CancellationToken CancellationToken = CancellationToken.None;
		
		[Test]
		public async Task Can_has_chats()
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
			
			await RepositoryContext.Chats.AddAsync(directChat);
			await RepositoryContext.SaveChangesAsync(CancellationToken);
			
			// Act
			HttpResponseMessage response = await HttpClient.GetAsync("chats");
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			var actual = await response.Content.As<List<Chat>>();
			actual.ShouldNotBeNull();
			Chat item = actual.ShouldHaveSingleItem();
			item.ShouldNotBeNull();
			item.Member.ShouldNotBeNull();
			item.Member.Id.ShouldBe(member.Id);
		}
		
		[Test]
		public async Task Should_returns_only_empty_chats()
		{
			// Act
			HttpResponseMessage response = await HttpClient.GetAsync("chats");
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			var actual = await response.Content.As<List<Chat>>();
			actual.ShouldBeEmpty();
		}
		
		[Test]
		public async Task Can_get_chat_by_id()
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
			
			await RepositoryContext.Chats.AddAsync(directChat);
			await RepositoryContext.SaveChangesAsync(CancellationToken);
			
			// Act
			HttpResponseMessage response = await HttpClient.GetAsync($"chats/{directChat.Id}");
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			var actual = await response.Content.As<Chat>();
			actual.ShouldNotBeNull();
			actual.Id.ShouldBe(directChat.Id);
			actual.Member.ShouldNotBeNull();
			actual.Member.Id.ShouldBe(member.Id);
		}
		
		[Test]
		public async Task Can_get_null_chat_by_id()
		{
			// Act
			HttpResponseMessage response = await HttpClient.GetAsync($"chats/{Guid.NewGuid()}");
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
			var actual = await response.Content<Chat>();
			actual.ShouldBeNull();
		}
		
		[Test]
		public async Task Can_get_chat_messages()
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
			var messages = new[]
			{
				new Message(DateTimeOffset.Parse("2020-01-01T00:00:00+00:00"), directChat, Guid.NewGuid().ToString()),
				new Message(DateTimeOffset.Parse("2020-01-01T00:00:10+00:00"), directChat, Guid.NewGuid().ToString()),
				new Message(DateTimeOffset.Parse("2020-01-01T00:00:20+00:00"), directChat, Guid.NewGuid().ToString()),
				new Message(DateTimeOffset.Parse("2020-01-01T00:00:30+00:00"), directChat, Guid.NewGuid().ToString())
			};
			const int page = 1;
			const int size = 25;
			
			await RepositoryContext.Chats.AddAsync(directChat);
			await RepositoryContext.Messages.AddRangeAsync(messages);
			await RepositoryContext.SaveChangesAsync(CancellationToken);
			
			// Act
			HttpResponseMessage response = await HttpClient.GetAsync($"chats/{directChat.Id}/messages?page={page}&size={size}");
			
			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			var actual = await response.Content.As<PagedList<ChatMessage>>();
			actual.ShouldNotBeNull();
			actual.Page.ShouldBe(page);
			actual.Size.ShouldBe(size);
			actual.Items.Count.ShouldBeLessThanOrEqualTo(25);
			actual.Items
				.Select(m => m.Id)
				.SequenceEqual(messages.OrderByDescending(m => m.SentDate).Select(m => m.Id))
				.ShouldBeTrue();
		}

		[Test]
		public async Task Can_create_chat()
		{
			// Arrange
			var member = new Account(
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString());
			await RepositoryContext.Accounts.AddAsync(member);
			await RepositoryContext.SaveChangesAsync(CancellationToken);

			// Act
			var command = new CreateDirectChatCommand
			{
				To = member.Id
			};
			string chatName = command.To.ToString();

			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PostAsync("chats", payload, CancellationToken);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			ChatRow expected = QueriesContext.Chats.FirstOrDefault(ch => ch.Name == chatName);
			expected.ShouldNotBeNull();
			expected.Name.ShouldBe(chatName);
			expected.MemberId.ShouldBe(command.To);
		}
		
		[Test]
		public async Task Can_change_chat_name()
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
			
			await RepositoryContext.Chats.AddAsync(directChat, CancellationToken);
			await RepositoryContext.SaveChangesAsync(CancellationToken);

			// Act
			string newChatName = Guid.NewGuid().ToString();
			var command = new ChangeChatInfoCommand
			{
				Name = newChatName,
				ChatId = directChat.Id
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PutAsync("chats", payload, CancellationToken);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			ChatRow expected = QueriesContext.Chats.FirstOrDefault(ch => ch.Name == newChatName);
			expected.ShouldNotBeNull();
			expected.Name.ShouldBe(newChatName);
		}
		
		[Test]
		public async Task Can_change_chat_name_but_chat_not_found()
		{
			// Arrange
			var notFoundChatId = Guid.NewGuid();
			
			// Act
			string newChatName = Guid.NewGuid().ToString();
			var command = new ChangeChatInfoCommand
			{
				Name = newChatName,
				ChatId = notFoundChatId
			};
			StringContent payload = command.ToJsonContent();
			HttpResponseMessage response = await HttpClient.PutAsync("chats", payload, CancellationToken);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}
