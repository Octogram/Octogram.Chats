using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Octogram.Chats.Application.Web.Commands
{
	public interface ICommandsBus
	{
		IEnumerable<object> Commands { get; }

		void Enqueue<T>(object command) where T : class;

		Task SendAllAsync(CancellationToken cancellationToken);
	}
}
