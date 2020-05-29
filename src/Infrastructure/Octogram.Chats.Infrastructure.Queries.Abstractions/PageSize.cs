using System.ComponentModel;

namespace Octogram.Chats.Infrastructure.Queries.Abstractions
{
	public class PageSize
	{
		public PageSize()
		{
			
		}
		
		public PageSize(int page, int size)
		{
			Page = page;
			Size = size;
		}

		[DefaultValue("1")]
		public int Page { get; private set; } = 1;

		[DefaultValue("25")]
		public int Size { get; private set; } = 25;
	}
}
