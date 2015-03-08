using System.Reflection;
using System.Web.Mvc;
using Daniel15.Infrastructure;
using Daniel15.Web.Infrastructure;
using Daniel15.Web.Mvc;
using Daniel15.Web.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Initialises IoC on the website
	/// </summary>
	public static class IocInitialiser
	{
		/// <summary>
		/// Initialises IoC on the website
		/// </summary>
		public static void Initialise()
		{
			var container = new Container();
			container.Options.ConstructorResolutionBehavior =
				new T4MvcControllerConstructorResolutionBehavior(container.Options.ConstructorResolutionBehavior);

			// Register the MVC controls
			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			container.RegisterMvcIntegratedFilterProvider();
			container.Register<ITempDataProvider, CookieTempDataProvider>();

			// The other MVC stuff
			//container.Register<ITempDataProvider, CookieTempDataProvider>();
			// Register the other ASP.NET MVC stuff
			// TODO: Figure out how to do this properly - http://simpleinjector.codeplex.com/discussions/430939
			//container.RegisterPerWebRequest<RequestContext>(() => HttpContext.Current.Request.RequestContext);
			//container.RegisterPerWebRequest<UrlHelper>(() => new UrlHelper(container.GetInstance<RequestContext>()));

			container.RegisterPerWebRequest<IWebCache>(config => config.WebCacheType);

			// Initialise all the standard stuff
			Ioc.Initialise(container, new WebRequestLifestyle());

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
		}
	}
}