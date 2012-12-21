using System.Web.Mvc;

namespace Daniel15.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				name: "BlogAdminPostsPublished",
				url: "blog/admin/posts/published",
				defaults: new { controller = "Blog", action = "Posts", published = true }
			);

			context.MapRoute(
				name: "BlogAdminPostsUnpublished",
				url: "blog/admin/posts/unpublished",
				defaults: new { controller = "Blog", action = "Posts", published = false }
			);

			// Editing an existing blog post
			context.MapRoute(
				name: "BlogEdit",
				url: "blog/{year}/{month}/{slug}/edit",
				defaults: MVC.Admin.Blog.Edit(),
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// Creating a new blog post
			context.MapRoute(
				name: "BlogNew",
				url: "blog/admin/new",
				defaults: MVC.Admin.Blog.Edit(null)
			);

			context.MapRoute(
				name: "BlogAdminDefault",
				url: "blog/admin/{action}",
				defaults: new { controller = "Blog", action = "Index" }
			);

			context.MapRoute(
				"Admin_default",
				"Admin/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
