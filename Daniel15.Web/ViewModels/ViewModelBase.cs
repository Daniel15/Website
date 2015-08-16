using System;

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
