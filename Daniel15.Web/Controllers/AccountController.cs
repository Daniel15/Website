using System.Threading.Tasks;
using Daniel15.SimpleIdentity;
using Daniel15.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles logging in and out
	/// </summary>
	public partial class AccountController : Controller
	{
		private readonly SignInManager<SimpleIdentityUser> _signInManager;

		public AccountController(SignInManager<SimpleIdentityUser> signInManager)
		{
			_signInManager = signInManager;
		}

		/// <summary>
		/// Displays the login page
		/// </summary>
		/// <param name="returnUrl">URL to return to after logging in</param>
		/// <returns></returns>
		public virtual ActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		/// <summary>
		/// Actually log the user in.
		/// </summary>
		/// <param name="model">Login model</param>
		/// <param name="returnUrl">URL to return to after logging in</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return RedirectToLocal(returnUrl);
				}
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
			return View(model);
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
				return RedirectToAction("Index", "Site");
			}
		}
	}
}
