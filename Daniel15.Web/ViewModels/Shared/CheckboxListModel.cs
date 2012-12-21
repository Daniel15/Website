using System.Collections.Generic;

namespace Daniel15.Web.ViewModels.Shared
{
	public class CheckboxListModel
	{
		public string Name { get; set; }
		public IEnumerable<Item> Items { get; set; }

		public class Item
		{
			public dynamic Id { get; set; }
			public string Label { get; set; }
			public bool Selected { get; set; }
		}
	}
}