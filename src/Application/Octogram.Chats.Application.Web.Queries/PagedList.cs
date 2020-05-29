using System;
using System.Collections.Generic;

namespace Octogram.Chats.Application.Web.Queries
{
	public class PagedList<T>
	{
		public PagedList(int page, int size, int total, IList<T> items)
		{
			Page = page;
			Size = size;
			Total = total;
			Items = items ?? throw new ArgumentNullException(nameof(items));
		}

		public int Page { get; private set; }

		public int Size { get; private set; }

		public int Total { get; private set; }

		public IList<T> Items { get; private set; }
	}
}
