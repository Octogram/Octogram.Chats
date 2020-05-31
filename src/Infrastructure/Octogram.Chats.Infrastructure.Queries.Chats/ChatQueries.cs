﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Application.Web.Queries;
using Octogram.Chats.Application.Web.Queries.Chats;
using Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore;

namespace Octogram.Chats.Infrastructure.Queries.Chats
{
	public class ChatQueries : IChatQueries
	{
		private readonly QueriesDbContext _queriesDbContext;

		public ChatQueries(QueriesDbContext queriesDbContext)
		{
			_queriesDbContext = queriesDbContext ?? throw new ArgumentNullException(nameof(queriesDbContext));
		}

		/// <inheritdoc />
		public async Task<IEnumerable<Chat>> GetAsync(Guid accountId, CancellationToken cancellationToken)
		{
			List<Chat> chats = await _queriesDbContext
				.Chats
				.Where(ch => ch.OwnerId == accountId)
				.Select(ch => new Chat
				{
					Id = ch.Id,
					Member = new ChatMember
					{
						Id = ch.Member.Id
					}
				})
				.ToListAsync(cancellationToken);

			return chats;
		}

		/// <inheritdoc />
		public async Task<ChatDetails> GetDetailsAsync(Guid accountId, Guid chatId, CancellationToken cancellationToken)
		{
			ChatDetails chat = await _queriesDbContext
				.Chats
				.Where(ch => ch.OwnerId == accountId && ch.Id == chatId)
				.Select(ch => new ChatDetails
				{
					Id = ch.Id,
					Member = new ChatDetailsMember
					{
						Id = ch.MemberId
					}
				})
				.SingleOrDefaultAsync(cancellationToken);

			return chat;
		}

		/// <inheritdoc />
		public async Task<PagedList<ChatMessage>> GetMessagesAsync(
			Guid accountId,
			Guid chatId,
			int page,
			int size,
			CancellationToken cancellationToken)
		{
			PagedList<ChatMessage> messages = await _queriesDbContext
				.Messages
				.OrderByDescending(m => m.SentDate)
				.Where(m => m.ChatId == chatId)
				.Select(m => new ChatMessage
				{
					Id = m.Id,
					Content = m.Content,
					State = m.State,
					SentDate = m.SentDate
				})
				.ToPagedListAsync(page, size, cancellationToken);

			return messages;
		}
	}
}
