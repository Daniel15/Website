using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Configuration;
using Daniel15.Data.Repositories;
using Daniel15.Data.Repositories.OrmLite;
using Daniel15.Infrastructure.Extensions;
using ServiceStack.DataAccess;
using ServiceStack.OrmLite;
using SimpleInjector;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using ProjectRepository = Daniel15.Data.Repositories.OrmLite.ProjectRepository;

namespace Daniel15.Infrastructure
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
		/// Some IoC-registered components might require settings during initialisation (eg. to know
		/// which component to register). This is ugly - Need to think of a better way :(
		/// </summary>
		private static readonly SiteConfiguration _siteConfig;

		static Ioc()
		{
			// This is ugly - Clean it up!
			_siteConfig = (SiteConfiguration)ConfigurationManager.GetSection("SiteConfiguration");
			_siteConfig.ApiKeys = (ApiKeysConfiguration)ConfigurationManager.GetSection("ApiKeys");
		}

		/// <summary>
		/// Initializes all the components in the IoC container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="dbLifestyle">Lifestyle to use for database connections</param>
		public static void Initialise(Container container = null, Lifestyle dbLifestyle = null)
		{
			Container = container ?? new Container();
			dbLifestyle = dbLifestyle ?? Lifestyle.Transient;

			// Configuration
			Container.Register<ISiteConfiguration>(() => _siteConfig);
			Container.Register<IGalleryConfiguration>(() => (GalleryConfiguration)ConfigurationManager.GetSection("Gallery"));

			// Services
			Container.Register<IUrlShortener, UrlShortener>();
			Container.Register<ISocialManager, SocialManager>();
			Container.Register<IDisqusComments, DisqusComments>();
			Container.Register<IMarkdownProcessor, MarkdownProcessor>();

			InitializeDatabase(Container, dbLifestyle);
		}

		/// <summary>
		/// Initialises the database stuff in the IoC container
		/// </summary>
		/// <param name="container"></param>
		/// <param name="lifestyle">Lifestyle to use when registering components</param>
		private static void InitializeDatabase(Container container, Lifestyle lifestyle)
		{
			const bool ENABLE_PROFILING = false;

			// By default, don't add a filter to the DB connection factory
			Func<IDbConnection, IDbConnection> connFilter = conn => conn;
			// Check if profiling is enabled
			if (ENABLE_PROFILING)
			{
				// Yes, so we need to add a custom filter to the connection factory
				connFilter = conn =>
				{
					var innerConn = ((IHasDbConnection)conn).DbConnection;
					return new ProfiledDbConnection((DbConnection)innerConn, MiniProfiler.Current);
				};
			}

			// TODO: Move to Daniel15.Data
			container.Register<IDbConnectionFactory>(() =>
				new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, 
					                         MySqlDialect.Provider) { ConnectionFilter = connFilter }, lifestyle);

			container.Register<IDbConnection>(() => container.GetInstance<IDbConnectionFactory>().OpenDbConnection(), lifestyle);

			// Repositories
			container.Register<IBlogRepository, BlogRepository>(lifestyle);
			container.Register<IDisqusCommentRepository, DisqusCommentRepository>(lifestyle);
			container.Register<IProjectRepository, ProjectRepository>(lifestyle);
			container.Register<IMicroblogRepository, MicroblogRepository>(lifestyle);
		}

		/// <summary>
		/// Registers that one instance of the type specified by the config value <paramref name="configValue"/> 
		/// will be returned for every web request every time a <typeparamref name="TService"/> is 
		/// requested and ensures that -if  <paramref name="configValue"/> implements <see cref="IDisposable"/>
		/// - this instance will get disposed on the end of the web request.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
		/// <param name="container">The container to make the registrations in.</param>
		/// <param name="configValue">Configuration value that specifies concrete type.</param>
		public static void RegisterPerWebRequest<TService>(this Container container, Func<ISiteConfiguration, Type> configValue) where TService : class
		{
			container.RegisterPerWebRequest<TService>(configValue(_siteConfig));
		}
	}
}