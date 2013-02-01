using System;
using Daniel15.Data.Entities.Blog;
using Daniel15.Web.Extensions;
using NUnit.Framework;

namespace Daniel15.Web.Tests.Extensions
{
	[TestFixture]
	public class BlogUrlHelperExtensionsTests
	{
		[Test]
		[Ignore("Not working under Mono yet")]
		public void BlogPosts()
		{
			var post = new PostSummaryModel
			{
				Id = 123,
				Date = new DateTime(2012, 1, 1),
				Title = "Test Post",
				Slug = "test-post",
				Published = true,
			};

			var mocks = new Mocks();
			Assert.AreEqual("/blog/2012/01/test-post", BlogUrlHelperExtensions.BlogPost(mocks.UrlHelper, post));
			Assert.AreEqual("/blog/2012/01/test-post/edit", BlogUrlHelperExtensions.BlogPostEdit(mocks.UrlHelper, post));
			Assert.AreEqual("http://localhost/blog/2012/01/test-post", BlogUrlHelperExtensions.BlogPostAbsolute(mocks.UrlHelper, post));
		}

		[Test]
		[Ignore("Not working under Mono yet")]
		public void BlogIndex()
		{
			var mocks = new Mocks();
			Assert.AreEqual("/blog", BlogUrlHelperExtensions.BlogIndex(mocks.UrlHelper, 1));
			Assert.AreEqual("/blog/page-2", BlogUrlHelperExtensions.BlogIndex(mocks.UrlHelper, 2));
		}

		[Test]
		[Ignore("Not working under Mono yet")]
		public void BlogCategory()
		{
			var category = new CategoryModel { Id = 1, Title = "Test Category", Slug = "test-category" };
			var mocks = new Mocks();
			Assert.AreEqual("/blog/category/test-category", BlogUrlHelperExtensions.BlogCategory(mocks.UrlHelper, category, 1));
			Assert.AreEqual("/blog/category/test-category/page-2", BlogUrlHelperExtensions.BlogCategory(mocks.UrlHelper, category, 2));
		}
	}
}
