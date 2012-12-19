using System.Web.Mvc;
using System.Web.Security;
using Daniel15.Web.ViewModels.Account;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles logging in and out
	/// </summary>
	public partial class AccountController : Controller
	{
		/// <summary>
		/// Displays the login page
		/// </summary>
		/// <param name="returnUrl">URL to return to after logging in</param>
		/// <returns></returns>
		public virtual ActionResult Login(string returnUrl)
		{
			return View(Views.Login, new LoginViewModel { ReturnUrl = returnUrl });
		}

		/// <summary>
		/// Actually log the user in.
		/// </summary>
		/// <param name="model">Login model</param>
		/// <param name="returnUrl">URL to return to after logging in</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid && FormsAuthentication.Authenticate(model.UserName, model.Password))
			{
				FormsAuthentication.SetAuthCookie(model.UserName, false);
				return RedirectToLocal(returnUrl);
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "The user name or password provided is incorrect.");
			return View(Views.Login, model);
		}

		/// <summary>
		/// Redirect to the specified URL if it's local to this site. Otherwise, redirect to the index.
		/// </summary>
		/// <param name="returnUrl">URL to redirect to</param>
		/// <returns>Redirect</returns>
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				// TODO: Redirect to admin home
				return RedirectToAction(MVC.Site.Index());
			}
		}
	}
}
