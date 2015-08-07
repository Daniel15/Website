using System;

namespace Daniel15.Web.ViewModels.Feed
{
    public class SitemapUrlModel
    {
		public string Url { get; set; }
		public DateTime? LastModified { get; set; }
		public string ChangeFrequency { get; set; }
		public double? Priority { get; set; }
    }
}
