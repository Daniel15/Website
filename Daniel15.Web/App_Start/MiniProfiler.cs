using Daniel15.Web.Infrastructure;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
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
			// Don't do anything if Mini Profiler isn't enabled
			if (!Ioc.Container.GetInstance<ISiteConfiguration>().EnableProfiling)
				return;

			MiniProfiler.Settings.SqlFormatter = new InlineFormatter();

			// Make sure the MiniProfiler handles BeginRequest and EndRequest
			DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

			// Setup profiler for Controllers via a Global ActionFilter
			GlobalFilters.Filters.Add(new ProfilingActionFilter());

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
		}

		public void Dispose() { }
	}
}

