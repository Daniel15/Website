using System.Web.Mvc;
using Daniel15.Web.App_Start;

namespace Daniel15.Web.Infrastructure
{
	/// <summary>
	/// Base class for Razor views with no model
	/// </summary>
	public abstract class ViewBase : WebViewPage
	{
		/// <summary>
		/// Gets the site configuration
		/// </summary>
		public ISiteConfiguration Config { get; set; }

		public ViewBase()
		{
			Ioc.Container.InjectProperties(this);
		}
	}

	/// <summary>
	/// Base class for Razor views
	/// </summary>
	/// <typeparam name="T">Type of the view</typeparam>
	public abstract class ViewBase<T> : WebViewPage<T>
	{
		/// <summary>
		/// Gets the site configuration
		/// </summary>
		public ISiteConfiguration Config { get; set; }

		public ViewBase()
		{
			Ioc.Container.InjectProperties(this);
		}
	}
}