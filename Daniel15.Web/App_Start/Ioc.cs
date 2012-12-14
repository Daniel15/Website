using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Web.Mvc;
using Daniel15.Web.Infrastructure;
using Daniel15.Web.Repositories;
using Daniel15.Web.Services;
using ServiceStack.DataAccess;
using ServiceStack.OrmLite;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of the IoC container.
	/// </summary>
    public static class Ioc
    {
		/// <summary>
		/// IoC container. This should only be used when absolutely necessary - Constructor injection
		/// should be used in most situations.
		/// </summary>
		public static Container Container { get; private set; }

		/// <summary>
		/// Initialises the IoC container
		/// </summary>
        public static void Initialize()
        {
            Container = new Container();
			Container.Options.ConstructorResolutionBehavior =
				new T4MvcControllerConstructorResolutionBehavior(Container.Options.ConstructorResolutionBehavior);

			InitializeContainer(Container);
			Container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			Container.RegisterMvcAttributeFilterProvider();
			Container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
        }

		/// <summary>
		/// Initializes all the components in the IoC container.
		/// </summary>
		/// <param name="container">The container.</param>
        private static void InitializeContainer(Container container)
        {
			// Configuration
			var config = (SiteConfiguration)ConfigurationManager.GetSection("SiteConfiguration");
			container.Register<ISiteConfiguration>(() => config);

			// Services
			container.Register<IUrlShortener, UrlShortener>();

			InitializeDatabase(container, config.EnableProfiling);
        }

		/// <summary>
		/// Initialises the database stuff in the IoC container
		/// </summary>
		/// <param name="container"></param>
		/// <param name="enableProfiling">Whether database profiling is enabled</param>
		private static void InitializeDatabase(Container container, bool enableProfiling)
		{
			// By default, don't add a filter to the DB connection factory
			Func<IDbConnection, IDbConnection> connFilter = conn => conn;
			// Check if profiling is enabled
			if (enableProfiling)
			{
				// Yes, so we need to add a custom filter to the connection factory
				connFilter = conn =>
				{
					var innerConn = ((IHasDbConnection)conn).DbConnection;
					return new ProfiledDbConnection((DbConnection)innerConn, MiniProfiler.Current);
				};
			}

			container.RegisterPerWebRequest<IDbConnectionFactory>(() =>
				new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, 
					                         MySqlDialect.Provider) { ConnectionFilter = connFilter });

			container.RegisterPerWebRequest<IDbConnection>(() => container.GetInstance<IDbConnectionFactory>().OpenDbConnection());

			// Repositories
			container.RegisterPerWebRequest<IBlogRepository, Repositories.OrmLite.BlogRepository>();
			container.RegisterPerWebRequest<IProjectRepository, Repositories.Static.ProjectRepository>();
		}
    }
}