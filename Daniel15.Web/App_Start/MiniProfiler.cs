using Daniel15.Web.Infrastructure;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using StackExchange.Profiling;
using StackExchange.Profiling.MVCHelpers;
using StackExchange.Profiling.SqlFormatters;

namespace Daniel15.Web.App_Start 
{
	/// <summary>
	/// Helper class to initialise MiniProfiler
	/// </summary>
	public static class MiniProfilerInitialiser
	{
		/// <summary>
		/// Initialises MiniProfiler
		/// </summary>
		public static void Init()
		{
			MiniProfiler.Settings.SqlFormatter = new InlineFormatter();

			// Setup profiler for Controllers via a Global ActionFilter
			GlobalFilters.Filters.Add(new ProfilingActionFilter());

			MiniProfiler.Settings.Results_Authorize = MiniProfiler.Settings.Results_List_Authorize = request => request.IsAuthenticated;

			// Intercept ViewEngines to profile all partial views and regular views.
			// If you prefer to insert your profiling blocks manually you can comment this out
			var copy = ViewEngines.Engines.ToList();
			ViewEngines.Engines.Clear();
			foreach (var item in copy)
			{
				ViewEngines.Engines.Add(new ProfilingViewEngine(item));
			}
		}
	}

	/// <summary>
	/// Module to attach MiniProfiler to requests
	/// </summary>
	public class MiniProfilerStartupModule : IHttpModule
	{
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			// Attach MiniProfiler to requests
			context.BeginRequest += (sender, e) => MiniProfiler.Start();
			context.EndRequest += (sender, e) => MiniProfiler.Stop();

			context.AuthenticateRequest += (sender, e) =>
			{
				// Stop profiling if user isn't logged in.
				if (!context.Request.IsAuthenticated)
					MiniProfiler.Stop(discardResults: true);
			};
		}

		public void Dispose() { }
	}
}

