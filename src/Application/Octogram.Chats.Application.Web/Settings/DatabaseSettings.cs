using Octogram.Chats.Infrastructure.IoC;

namespace Messenger.Web.Settings
{
	public class DatabaseSettings : IDatabaseSettings
	{
		/// <inheritdoc />
		public string ConnectionString { get; set; } 
	}
}
