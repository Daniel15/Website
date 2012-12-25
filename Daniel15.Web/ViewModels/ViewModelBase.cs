using System;
using System.Configuration;
using System.Web;

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
		/// <summary>
		/// Gets the version string to use in the footer of the site
		/// </summary>
		public IHtmlString Version
		{
			get
			{
				var gitRevision = ConfigurationManager.AppSettings["GitRevision"];
				var gitRevisionUrl = ConfigurationManager.AppSettings["GitRevisionUrl"];

				// Don't display anything if there's no version in the Web.config
				if (string.IsNullOrEmpty(gitRevision))
					return null;

				return new HtmlString(string.Format("Running revision <a href=\"{1}\">{0}</a>", gitRevision, gitRevisionUrl));
			}
		}

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