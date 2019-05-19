using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Daniel15.Web.Services
{
	// https://github.com/aspnet/Docs/issues/3352#issuecomment-333593326
	public class BackgroundTaskService : IHostedService
	{
		private readonly ILogger<BackgroundTaskService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly CancellationTokenSource _shutdown = new CancellationTokenSource();
		private Task _backgroundTask;

		public BackgroundTaskService(IBackgroundTaskQueue taskQueue, ILogger<BackgroundTaskService> logger, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
			TaskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
		}

		public IBackgroundTaskQueue TaskQueue { get; }

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_backgroundTask = Task.Run(BackgroundProcessing);
			return Task.CompletedTask;
		}

		private async Task BackgroundProcessing()
		{
			while (!_shutdown.IsCancellationRequested)
			{
				var workItem = await TaskQueue.DequeueAsync(_shutdown.Token);

				try
				{
					_logger.LogInformation("Starting background task");
					using (var scope = _serviceProvider.CreateScope())
					{
						await workItem(scope.ServiceProvider, _shutdown.Token);
					}

					_logger.LogInformation("Completed background task");
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error while running background job");
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_shutdown.Cancel();
			return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
		}
	}
}
