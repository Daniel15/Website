namespace Daniel15.Web.Models.Shared
{
	/// <summary>
	/// Represents an item in the main menu
	/// </summary>
	public class MenuItemModel
	{
		public string Url { get; set; }
		public string Title { get; set; }
		public bool Active { get; set; }
	}
}