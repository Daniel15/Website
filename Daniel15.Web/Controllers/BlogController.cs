﻿using System;
using System.Web.Mvc;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Blog;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the main blog pages
	/// </summary>
	public partial class BlogController : Controller
    {
	    private readonly IBlogPostRepository _blogPostRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogPostRepository">The blog post repository.</param>
		public BlogController(IBlogPostRepository blogPostRepository)
		{
			_blogPostRepository = blogPostRepository;
		}

		/// <summary>
		/// Index page of the blog
		/// </summary>
		public virtual ActionResult Index()
        {
			var posts = _blogPostRepository.LatestPosts();

			throw new NotImplementedException();
        }

		/// <summary>
		/// Viewing a blog post
		/// </summary>
		/// <param name="month">The month of the post</param>
		/// <param name="year">The year of the post</param>
		/// <param name="slug">The slug.</param>
		/// <returns>Blog post page</returns>
		public virtual ActionResult View(int month, int year, string slug)
		{
			throw new NotImplementedException("TODO: View " + slug);
		}
    }
}