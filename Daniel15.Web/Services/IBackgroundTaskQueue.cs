using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daniel15.Web.Services
{
	public interface IBackgroundTaskQueue
	{
		/// <summary>
		/// https://github.com/aspnet/Docs/issues/3352#issuecomment-333593326
		/// https://msdn.microsoft.com/en-us/library/dn636893(v=vs.110).aspx
		/// Differs from a normal ThreadPool work item in that ASP.NET can keep track of how many work items registered through
		/// this API are currently running, and the ASP.NET runtime will try to delay AppDomain shutdown until these work items
		/// have finished executing. This API cannot be called outside of an ASP.NET-managed AppDomain. The provided CancellationToken
		/// will be signaled when the application is shutting down.
		/// </summary>
		/// <param name="workItem"></param>
		void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

		Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
	}
}
