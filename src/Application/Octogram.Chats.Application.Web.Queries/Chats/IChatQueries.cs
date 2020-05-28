using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Octogram.Chats.Application.Web.Queries.Chats
{
	public interface IChatQueries
	{
		Task<IEnumerable<Chat>> GetAsync(Guid accountId, CancellationToken cancellationToken);

		Task<ChatDetails> GetDetailsAsync(Guid accountId, Guid chatId, CancellationToken cancellationToken);

		Task<IEnumerable<ChatMessage>> GetMessagesAsync(Guid accountId, Guid chatId, int page, int size, CancellationToken cancellationToken);
	}
}
