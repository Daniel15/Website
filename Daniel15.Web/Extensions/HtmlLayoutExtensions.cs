using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Daniel15.Web.Controllers;
using Daniel15.Web.Models.Shared;
using Daniel15.Web.ViewModels;
using System.Web.Mvc.Html;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// <see cref="HtmlHelper"/> extensions for rendering of the page
	/// </summary>
	public static class HtmlLayoutExtensions
	{
		/// <summary>
		/// Gets the value to use for the id attribute on the body tag
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <returns>The ID</returns>
		public static string BodyId(this HtmlHelper htmlHelper)
		{
			var routeData = htmlHelper.ViewContext.RouteData;
			return routeData.GetRequiredString("controller").ToLower() + "-" + routeData.GetRequiredString("action").ToLower();
		}

		/// <summary>
		/// Gets the value to use for the class attribute on the body tag
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <returns>The class</returns>
		public static string BodyClass(this HtmlHelper<ViewModelBase> htmlHelper)
		{
			var classes = new List<string>
			{
				htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToLower(),
				"col-" + htmlHelper.ViewData.Model.SidebarType.ToString().ToLower()
			};
			return string.Join(" ", classes);
		}

		/// <summary>
		/// Renders the top menu
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <returns>Text for the top menu</returns>
		public static MvcHtmlString Menu(this HtmlHelper htmlHelper)
		{
			var controller = htmlHelper.ViewContext.Controller;
			var action = htmlHelper.ViewContext.RouteData.GetRequiredString("action").ToLower();

			// TODO: Where should these actually be? Probably in a model.
			// TODO: Should these be using UrlHelper?
			var menuItems = new List<MenuItemModel>
			{
				new MenuItemModel
				{
					Url = "", 
					Title = "Home", 
					Active = controller is SiteController && action == "index"
				},
				new MenuItemModel
				{
					Url = "projects.htm", 
					Title = "Projects", 
					Active = action == "projects"
				},
				new MenuItemModel
				{
					Url = "blog", 
					Title = "Blog", 
					Active = controller is BlogController
				},
				new MenuItemModel
				{
					Url = "http://daaniel.com/", 
					Title = "Thoughts", 
					Active = false
				},
			};

			return htmlHelper.Partial(MVC.Shared.Views._Menu, menuItems);
		}

		/// <summary>
		/// Retrieves the content of the blog sidebar
		/// </summary>
		/// <param name="htmlHelper">HTML helper</param>
		/// <returns>HTML for the blog sidebar</returns>
		public static MvcHtmlString BlogSidebar(this HtmlHelper htmlHelper)
		{
			return htmlHelper.ViewContext.HttpContext.Cache.GetOrInsert("BlogSidebar", DateTime.UtcNow.AddDays(1), null,
			                                                            () => htmlHelper.Action(MVC.BlogPartials.Sidebar()));
		}
	}
}