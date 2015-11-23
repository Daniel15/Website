using System;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Daniel15.Cron
{
	public class Program
	{
		private readonly IServiceCollection _serviceCollection = new ServiceCollection();
		private IServiceProvider _serviceProvider;

		public void Main(string[] args)
		{
			_serviceCollection.AddDaniel15();
			var builder = new ConfigurationBuilder()
				// This is extremely ugly, but the paths differ in dev vs in prod. 
				// Need to figure out a nicer way of doing this.
				.AddJsonFile("..\\Daniel15.Web\\config.Development.json", optional: true)
				.AddJsonFile("../../../../../../site/approot/packages/Daniel15.Web/1.0.0/root/config.Production.json", optional: true)
				.AddEnvironmentVariables();
			_serviceCollection.AddDaniel15Config(builder.Build());
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

		private void RunDisqus() => _serviceProvider.GetRequiredService<IDisqusComments>().Sync();
		private void RunSocial() => ActivatorUtilities.CreateInstance<SocialShareUpdater>(_serviceProvider).Run();
		private void RunProjects() => ActivatorUtilities.CreateInstance<ProjectUpdater>(_serviceProvider).Run();
	}
}
