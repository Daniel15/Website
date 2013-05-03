using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Daniel15.Web.App_Start;
using Glimpse.Ado;
using Glimpse.Ado.AlternateType;

namespace Daniel15.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			Glimpse.Settings.Initialize.Ado();
			AreaRegistration.RegisterAllAreas();
			IocInitialiser.Initialise();
			MiniProfilerInitialiser.Init();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}