using System.Collections.Generic;
using Daniel15.Web.Areas.Gallery.Models;
using Daniel15.Web.ViewModels;

namespace Daniel15.Web.Areas.Gallery.ViewModels
{
	/// <summary>
	/// Represents data required to render a gallery listing
	/// </summary>
	public class IndexViewModel : ViewModelBase
	{
		/// <summary>
		/// Details on the gallery
		/// </summary>
		public Shared.Configuration.Gallery Gallery { get; set; }
		/// <summary>
		/// Path to the gallery listing
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// All the files to be rendered
		/// </summary>
		public IEnumerable<GalleryFileModel> Files { get; set; }
	}
}
