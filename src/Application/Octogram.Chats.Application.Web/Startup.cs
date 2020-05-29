using System;
using System.IO;
using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Octogram.Chats.Application.Web.Behaviors;
using Octogram.Chats.Application.Web.Commands;
using Octogram.Chats.Application.Web.Commands.Chats;
using Octogram.Chats.Application.Web.Services;
using Octogram.Chats.Application.Web.Settings;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.CommandsBus.MassTransit;
using Octogram.Chats.Infrastructure.IoC.Queries;
using Octogram.Chats.Infrastructure.IoC.Repositories;
using Octogram.Contracts.Commands;

namespace Octogram.Chats.Application.Web
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddControllers();

			AddSwagger(services);
			
			services.AddCors(option =>
			{
				option.AddPolicy("CorsPolicy", builder =>
					builder.WithOrigins("http://localhost:4200")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});
			
			services.AddMassTransit(configurator =>
			{
				var host = new Uri("rabbitmq://localhost");

				configurator.AddBus(serviceProvider => Bus.Factory.CreateUsingRabbitMq(configure =>
				{
					configure.Host(host);
					
					configure.ReceiveEndpoint("Octogram.Chats", endpoint =>
					{
					});
				}));
				
				var sagaUri = new Uri(host, "/Octogram.Saga");
				EndpointConvention.Map<IMessageSendCommand>(sagaUri);
			});
			
			var databaseSettings = new DatabaseSettings();
			_configuration.Bind("DatabaseSettings", databaseSettings);
			
			services.AddDatabaseRepositories(databaseSettings);
			services.AddDatabaseQueries(databaseSettings);

			services.AddHttpContextAccessor();
			services.AddSingleton<IAccountService, HttpContextAccountService>();

			services.AddMediatR(typeof(CreateChatCommand));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

			services.AddSingleton<ICommandsBus, MassTransitCommandsBus>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint(
					url: "/swagger/v1/swagger.json",
					name: "Octogram API v1");
				options.RoutePrefix = "docs";
			});
			
			app.UseCors("CorsPolicy");
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
		
		private static void AddSwagger(IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Octogram API",
					Version = "v1"
				});

				// Set the comments path for the Swagger JSON and UI.
				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});
		}
	}
}
