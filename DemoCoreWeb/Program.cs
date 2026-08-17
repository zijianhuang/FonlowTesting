using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DemoCoreWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Web API launching...");
			var builder = WebApplication.CreateBuilder(args);

			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();// so other logger like those in system can use
			builder.Services.AddControllers(options =>
			{
#if DEBUG
				options.Conventions.Add(new Fonlow.CodeDom.Web.ApiExplorerVisibilityEnabledConvention());//To make ApiExplorer be visible to WebApiClientGen
#endif
			});

			using var loggerFactory = LoggerFactory.Create(logging =>
			{
				logging.AddSimpleConsole();
			});

			var logger = loggerFactory.CreateLogger("Startup");

			logger.LogInformation("Application startup begins");

			var app = builder.Build();

			logger.LogInformation("Application built successfully");

			app.MapControllers();
			app.Run();
		}

	}
}
