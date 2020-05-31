using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octogram.Chats.Domain.Abstractions.Exceptions;

namespace Octogram.Chats.Application.Web.Middleware
{
	internal class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (EntityNotFoundException e)
			{
				await HandleEnitityNotFoundExceptionAsync(context, e);
			}
		}

		private static Task HandleEnitityNotFoundExceptionAsync(HttpContext context, EntityNotFoundException exception)
		{
			const int statusCode = 404;
			var error = new ProblemDetails
			{
				Title = exception.Message,
				Status = statusCode
			};

			string result = JsonConvert.SerializeObject(error);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;
			return context.Response.WriteAsync(result);
		}
	}
}
