﻿using System.Collections.Generic;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Represents a post and some additional data related to it.
	/// </summary>
	public class PostViewModel : ViewModelBase
	{
		/// <summary>
		/// The actual post.
		/// </summary>
		public PostModel Post { get; set; }

		/// <summary>
		/// Short URL to the post, for sharing.
		/// </summary>
		public string ShortUrl { get; set; }

		/// <summary>
		/// List of all the categories this post is included in.
		/// </summary>
		public IList<CategoryModel> PostCategories { get; set; }
	}
}