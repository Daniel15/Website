using System;
using System.Configuration;

namespace Daniel15.Web.ViewModels
{
	/// <summary>
	/// Base class that all view models should inherit from
	/// </summary>
	public class ViewModelBase
	{
		/// <summary>
		/// Title (used in title tag)
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Heading (used in heading at top of the page)
		/// </summary>
		public string Heading { get; set; }
		/// <summary>
		/// Type of sidebar to use
		/// </summary>
		public SidebarType SidebarType { get; set; }
		/// <summary>
		/// Description to use in the meta tag
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Last modified date of the page
		/// </summary>
		public DateTime? LastModified { get; set; }

		// TODO: Move this elsewhere?
		#region Configuration
		/// <summary>
		/// Gets the Google Analytics account number
		/// </summary>
		public string GoogleAnalyticsAccount
		{
			get { return ConfigurationManager.AppSettings["GoogleAnalyticsAccount"]; }
		}

		/// <summary>
		/// Gets the name of the blog
		/// </summary>
		public string BlogName
		{
			get { return ConfigurationManager.AppSettings["BlogName"]; }
		}

		/// <summary>
		/// Gets the URL to the blog's FeedBurner feed
		/// </summary>
		public string FeedBurnerUrl
		{
			get { return ConfigurationManager.AppSettings["FeedBurnerUrl"]; }
		}

		/// <summary>
		/// Gets the Disqus shortname to use for blog comments
		/// </summary>
		public string DisqusShortname
		{
			get { return ConfigurationManager.AppSettings["DisqusShortname"]; }
		}
		#endregion

		public ViewModelBase()
		{
			SidebarType = SidebarType.None;
		}
	}

	public enum SidebarType
	{
		/// <summary>
		/// No sidebar is displayed
		/// </summary>
		None,
		/// <summary>
		/// Sidebar is displayed on the left
		/// </summary>
		Left,
		/// <summary>
		/// Sidebar is displayed on the right
		/// </summary>
		Right
	}
}