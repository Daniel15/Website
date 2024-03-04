namespace Daniel15.Web.Services;

/// <summary>
/// Handle exceptions from background tasks
/// </summary>
public class ExceptionHandler(IHostEnvironment env) : IExceptionHandler
{
	public void Handle(Exception ex)
	{
		SentrySdk.CaptureException(ex);
		if (env.IsDevelopment())
		{
			throw new Exception("Error occurred in background task", ex);
		}
	}
}
