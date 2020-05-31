using System.Threading.Tasks;
using MassTransit;
using Octogram.Contracts.Events;

namespace Octogram.Chats.Application.Web.Consumers
{
	public class MessageSentConsumer : IConsumer<IMessageSentEvent>
	{
		/// <inheritdoc />
		public async Task Consume(ConsumeContext<IMessageSentEvent> context)
		{
			// TODO: Тут по факту получения сообщения, нужно создать новый direct чат для получателя
		}
	}
}
