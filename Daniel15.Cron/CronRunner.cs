using System;
using System.IO;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace Daniel15.Cron
{
	public class Program
	{
		private readonly IApplicationEnvironment _appEnv;
		private readonly IServiceCollection _serviceCollection = new ServiceCollection();
		private IConfiguration _config;
		private IServiceProvider _serviceProvider;

		public Program(IApplicationEnvironment appEnv)
		{
			_appEnv = appEnv;
		}

		public void Main(string[] args)
		{
			_serviceCollection.AddDaniel15();
			var builder = new ConfigurationBuilder(Path.Combine(_appEnv.ApplicationBasePath, "..\\Daniel15.Web\\"))
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
			_serviceCollection.AddDaniel15Config(builder.Build());
			_serviceCollection.AddOptions();
			_serviceProvider = _serviceCollection.BuildServiceProvider();

			var operation = args[0];
			switch (operation)
			{
				case "-disqus":
					_serviceProvider.GetRequiredService<IDisqusComments>().Sync();
					break;

				case "-social":
					ActivatorUtilities.CreateInstance<SocialShareUpdater>(_serviceProvider).Run();
					break;

				case "-projects":
					ActivatorUtilities.CreateInstance<ProjectUpdater>(_serviceProvider).Run();
					break;

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}
	}
}
