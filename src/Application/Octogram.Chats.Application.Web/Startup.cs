using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Octogram.Chats.Application.Web.Behaviors;
using Octogram.Chats.Application.Web.Commands;
using Octogram.Chats.Application.Web.Commands.Chats;
using Octogram.Chats.Application.Web.Middleware;
using Octogram.Chats.Application.Web.Services;
using Octogram.Chats.Application.Web.Settings;
using Octogram.Chats.Domain.Members;
using Octogram.Chats.Infrastructure.CommandsBus.MassTransit;
using Octogram.Chats.Infrastructure.IoC.Queries;
using Octogram.Chats.Infrastructure.IoC.Repositories;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;
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
			services.AddScoped<IAccountService, HttpContextAccountService>();

			services.AddMediatR(typeof(CreateDirectChatCommand));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

			services.AddSingleton<ICommandsBus, MassTransitCommandsBus>();
			
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			// Add Cookie settings
			services.AddAuthentication(options =>
			{
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
			{
				options.LoginPath = "/account/login";
				options.LogoutPath = "/account/logout";
				options.SlidingExpiration = true;
			})
			// Add GitHub authentication
			.AddGitHub("Github", options =>
			{
				options.ClientId = _configuration["GitHub:OAuth:ClientId"]; // client id from registering github app
				options.ClientSecret = _configuration["GitHub:OAuth:ClientSecret"]; // client secret from registering github app
				options.Scope.Add("user:email"); // add additional scope to obtain email address
				options.Events = new OAuthEvents
				{
					OnCreatingTicket = OnCreatingGitHubTicket()
				}; // Event to capture when the authentication ticket is being created
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseExceptionsHandling();
			
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint(
					url: "/swagger/v1/swagger.json",
					name: "Octogram API v1");
				options.RoutePrefix = "docs";
			});
			
			app.UseCookiePolicy();
			app.UseAuthentication();
			
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
		
		private static Func<OAuthCreatingTicketContext, Task> OnCreatingGitHubTicket()
		{
			return async context =>
			{
				await using (var dbContext = new RepositoryDbContext())
				{
					Account account = CreateAccountFromIdentity(context.Principal);
					await dbContext.AddAsync(account);
					await dbContext.SaveChangesAsync();
				}

				await Task.FromResult(true);
			};
		}
		
		private static Account CreateAccountFromIdentity(ClaimsPrincipal claimsPrincipal)
		{
			Claim usernameId = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			
			Claim username = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.Name);
			
			Claim email = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.Email);
			
			Claim name  = claimsPrincipal
				.Claims
				.FirstOrDefault(c => c.Type == "urn:github:name");
			
			return new Account(
				username?.Value,
				usernameId?.Value,
				name?.Value,
				email?.Value);
		}
	}
}
