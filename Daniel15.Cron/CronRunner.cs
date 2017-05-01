using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Daniel15.Cron
{
	public class Program
	{
		private static IServiceCollection _serviceCollection = new ServiceCollection();
		private static IServiceProvider _serviceProvider;

		public static void Main(string[] args)
		{
			MainAsync(args).GetAwaiter().GetResult();
		}

		private static async Task MainAsync(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
			var config = builder.Build();
			_serviceCollection.AddDaniel15(config);
			_serviceCollection.AddDaniel15Config(config);
			_serviceCollection.AddOptions();
			_serviceProvider = _serviceCollection.BuildServiceProvider();

			if (args.Length == 0)
			{
				// No argument, so run everything
				await RunProjectsAsync();
				await RunSocialAsync();
				await RunDisqusAsync();
				return;
			}

			var operation = args[0];
			switch (operation)
			{
				case "-disqus":
					await RunDisqusAsync();
					break;

				case "-social":
					await RunSocialAsync();
					break;

				case "-projects":
					await RunProjectsAsync();
					break;

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}

		private static Task RunDisqusAsync() => _serviceProvider.GetRequiredService<IDisqusComments>().SyncAsync();
		private static Task RunSocialAsync() => ActivatorUtilities.CreateInstance<SocialShareUpdater>(_serviceProvider).RunAsync();
		private static Task RunProjectsAsync() => ActivatorUtilities.CreateInstance<ProjectUpdater>(_serviceProvider).RunAsync();
	}
}
