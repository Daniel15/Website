using Microsoft.AspNetCore.Diagnostics;

namespace Daniel15.Web.Services;

/// <summary>
/// Handle exceptions from background tasks
/// </summary>
public interface IExceptionHandler
{
	public void Handle(Exception ex);
}
