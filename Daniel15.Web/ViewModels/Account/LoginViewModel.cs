using System.ComponentModel.DataAnnotations;

namespace Daniel15.Web.ViewModels.Account
{
	public class LoginViewModel : ViewModelBase
	{
		public string ReturnUrl { get; set; }

		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }
	}
}