using System.Web.Mvc;

namespace Daniel15.Web.Areas.Api
{
	public class ApiAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "API";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				name: "API_Posts",
				url: "api/posts/{postId}/{action}",
				defaults: new { controller = "PostsApi", action = "Index" }
			);

			context.MapRoute(
				name: "Api_default",
				url: "api/{controller}/{action}/{id}",
				defaults: new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
