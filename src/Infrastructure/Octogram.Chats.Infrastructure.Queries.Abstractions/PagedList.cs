using System;
using System.Collections.Generic;

namespace Octogram.Chats.Infrastructure.Queries.Abstractions
{
	public class PagedList<T>
	{
		public PagedList(PageSize pageSize, int total, IList<T> items)
		{
			Page = pageSize.Page;
			Size = pageSize.Size;
			Total = total;
			Items = items ?? throw new ArgumentNullException(nameof(items));
		}

		public int Page { get; private set; }

		public int Size { get; private set; }

		public int Total { get; private set; }

		public IList<T> Items { get; private set; }
	}
}
