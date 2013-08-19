using System.Collections.Generic;
using Daniel15.Web.Areas.Gallery.Models.Screenshot;
using Daniel15.Web.ViewModels;

namespace Daniel15.Web.Areas.Gallery.ViewModels.Screenshot
{
	/// <summary>
	/// Represents data required to render a screenshot listing
	/// </summary>
	public class IndexViewModel : ViewModelBase
	{
		/// <summary>
		/// Path to the screenshot listing
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// All the files to be rendered
		/// </summary>
		public IEnumerable<ScreenshotFileModel> Files { get; set; }
	}
}