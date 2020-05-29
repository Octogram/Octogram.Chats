using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Octogram.Chats.Application.Web.Queries;
using Octogram.Chats.Infrastructure.Queries.Chats;

namespace Octogram.Chats.Infrastructure.Queries.EntityFrameworkCore
{
	public static class PagingExtensions
	{
		public static async Task<PagedList<T>> ToPagedListAsync<T>(
			this IQueryable<T> query,
			int page,
			int size,
			CancellationToken cancellationToken)
		{
			List<T> items = await query
				.Skip<T>((page - 1) * size)
				.Take<T>(size)
				.ToListAsync(cancellationToken);

			int total = await query
				.CountAsync(cancellationToken);
			
			return new PagedList<T>(page, size, total, items);
		}
	}
}
