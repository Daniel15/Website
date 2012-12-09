using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Daniel15.Web.Infrastructure;
using Daniel15.Web.Repositories;
using ServiceStack.OrmLite;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of the IoC container.
	/// </summary>
    public static class IocConfig
    {
        public static void Initialize()
        {
            var container = new Container();
	        container.Options.ConstructorResolutionBehavior =
		        new T4MvcControllerConstructorResolutionBehavior(container.Options.ConstructorResolutionBehavior);
            
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();
            container.Verify();
           
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

		/// <summary>
		/// Initializes all the components in the IoC container.
		/// </summary>
		/// <param name="container">The container.</param>
        private static void InitializeContainer(Container container)
        {
			InitializeDatabase(container);
        }

		/// <summary>
		/// Initialises the database stuff in the IoC container
		/// </summary>
		/// <param name="container"></param>
		private static void InitializeDatabase(Container container)
		{
			// Connections
			container.RegisterPerWebRequest<IDbConnectionFactory>(() =>
				new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString,
				                             MySqlDialect.Provider));
			container.RegisterPerWebRequest<IDbConnection>(() => container.GetInstance<IDbConnectionFactory>().OpenDbConnection());

			// Repositories
			container.RegisterPerWebRequest<IBlogPostRepository, Repositories.OrmLite.BlogRepository>();
			container.RegisterPerWebRequest<IProjectRepository, Repositories.Static.ProjectRepository>();
		}
    }
}