using System.Web;
using System.Web.Mvc;

namespace Daniel15.Web.Mvc
{
	/// <summary>
	/// Handles serving static files via X-Accel-Redirect header, if supported by the web server.
	/// </summary>
	public class AccelRedirectFileResult : FilePathResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AccelRedirectFileResult" /> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="contentType">Type of the content.</param>
		public AccelRedirectFileResult(string fileName, string contentType) : base(fileName, contentType)
		{
		}

		public override void ExecuteResult(ControllerContext context)
		{
			base.ExecuteResult(context);

			// TODO: Figure out why this is throwing a 404 in Mono - Falling through to StaticFileHandler?
			/*
System.Web.HttpException: Path '/var/www/blah.png' was not found.
  at System.Web.StaticFileHandler.ProcessRequest (System.Web.HttpContext context) [0x00000] in <filename unknown>:0 
  at System.Web.DefaultHttpHandler.BeginProcessRequest (System.Web.HttpContext context, System.AsyncCallback callback, System.Object state) [0x00000] in <filename unknown>:0 
  at System.Web.HttpApplication+<Pipeline>c__Iterator3.MoveNext () [0x00000] in <filename unknown>:0 
  at System.Web.HttpApplication.Tick () [0x00000] in <filename unknown>:0 
*/
			/*if (SupportsAccelRedirect(context.HttpContext.Request))
			{
				context.HttpContext.Response.AddHeader("X-Accel-Redirect", FileName);
			}
			else*/
			{
				// TransmitFile doesn't work properly on Mono - Use WriteFile instead.
				context.HttpContext.Response.WriteFile(FileName);	
			}
		}

		/// <summary>
		/// Writes the file to the response.
		/// </summary>
		/// <param name="response">The response.</param>
		protected override void WriteFile(HttpResponseBase response)
		{
			// No-op as this is handled in ExecuteResult above
		}

		/// <summary>
		/// Determines whether the specified request is coming from a server that supports the 
		/// X-Accel-Redirect header
		/// </summary>
		/// <param name="request">The request</param>
		/// <returns><c>true</c> if the server supports X-Accel-Redirect</returns>
		private bool SupportsAccelRedirect(HttpRequestBase request)
		{
			return (request.ServerVariables["SERVER_SOFTWARE"] ?? string.Empty).Contains("nginx");
		}
	}

	/// <summary>
	/// Helper methods for X-Accel-Redirect file serving
	/// </summary>
	public static class AccelRedirectFileResultExtensions
	{
		/// <summary>
		/// Handles serving static files via X-Accel-Redirect header, if supported by the web server.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <returns>Static file</returns>
		public static AccelRedirectFileResult AccelRedirectFile(this Controller controller, string fileName, string contentType)
		{
			return new AccelRedirectFileResult(fileName, contentType);
		}
	}
}