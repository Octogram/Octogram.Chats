using Octogram.Chats.Infrastructure.IoC;

namespace Octogram.Chats.Application.Web.Settings
{
	public class DatabaseSettings : IDatabaseSettings
	{
		/// <inheritdoc />
		public string ConnectionString { get; set; } 
	}
}
