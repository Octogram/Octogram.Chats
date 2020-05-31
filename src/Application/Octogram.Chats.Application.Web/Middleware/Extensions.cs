using Microsoft.AspNetCore.Builder;

namespace Octogram.Chats.Application.Web.Middleware
{
	internal static class Extensions
	{
		public static IApplicationBuilder UseExceptionsHandling(this IApplicationBuilder builder)
		{
			builder.UseMiddleware<ExceptionHandlerMiddleware>();
			return builder;
		}
	}
}
