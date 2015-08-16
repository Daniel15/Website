using System.Net;
using Daniel15.Web.ViewModels;
using Daniel15.Web.ViewModels.Error;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Mvc;

namespace Daniel15.Web.Controllers
{
	[Route("/Error/[action]", Order = 1)]
    public class ErrorController : Controller
    {
		/// <summary>
		/// The page that is displayed when some sort of unknown HTTP error occurs.
		/// </summary>
		/// <param name="code">HTTP error code</param>
		[Route("/Error/Status{code:int}", Order = 2)]
		public IActionResult StatusCode(int code)
		{
			return View(new StatusCodeViewModel
			{
				StatusCode = code,
			});
		}

		/// <summary>
		/// The page that is displayed when a File Not Found (404) error occurs.
		/// </summary>
		/// <returns></returns>
		public IActionResult Status404()
		{
			return View(new ViewModelBase());
		}

		/// <summary>
		/// The page that is displayed when an internal server error occurs.
		/// </summary>
		/// <returns>The error page</returns>
		[Route("/Error")]
		public virtual ActionResult Error()
		{
			var view = View(new ErrorViewModel
			{
				Exception = Context.GetFeature<IErrorHandlerFeature>()?.Error,
			});
			view.StatusCode = (int)HttpStatusCode.InternalServerError;
			return view;
		}
	}
}
