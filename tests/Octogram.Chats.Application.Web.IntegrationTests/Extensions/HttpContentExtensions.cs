using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Octogram.Chats.Application.Web.IntegrationTests.Extensions
{
	internal static class HttpContentExtensions
	{
		public static async Task<T> Content<T>(this HttpResponseMessage responseMessage)
		{
			string content = await responseMessage.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(content);
		}

		public static async Task<T> As<T>(this HttpContent content)
		{
			string stringContent = await content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(stringContent);
		}

		public static StringContent ToJsonContent(this object obj)
		{
			string content = JsonConvert.SerializeObject(obj);
			return new StringContent(content, Encoding.UTF8, "application/json");
		}
	}
}
