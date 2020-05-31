using Octogram.Chats.Infrastructure.IoC;

namespace Octogram.Chats.Infrastructure.Migrations.Settings
{
	public class DatabaseSettings : IDatabaseSettings
	{
		/// <inheritdoc />
		public string ConnectionString { get; set; }
	}
}
