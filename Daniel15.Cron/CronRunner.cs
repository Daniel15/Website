using System;
using System.IO;
using System.Reflection;
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
				RunProjects();
				RunSocial();
				RunDisqus();
				return;
			}

			var operation = args[0];
			switch (operation)
			{
				case "-disqus":
					RunDisqus();
					break;

				case "-social":
					RunSocial();
					break;

				case "-projects":
					RunProjects();
					break;

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}

		private static void RunDisqus() => _serviceProvider.GetRequiredService<IDisqusComments>().Sync();
		private static void RunSocial() => ActivatorUtilities.CreateInstance<SocialShareUpdater>(_serviceProvider).Run();
		private static void RunProjects() => ActivatorUtilities.CreateInstance<ProjectUpdater>(_serviceProvider).Run();
	}
}
