using System;
using System.Reflection;
using SimpleInjector;
using System.Linq;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for SimpleInjector.
	/// </summary>
	public static class SimpleInjectorExtensions
	{
		/// <summary>
		/// RegisterPerWebRequest method.
		/// </summary>
		private static readonly MethodInfo _registerPerWebRequest;
		/// <summary>
		/// Transient registration method.
		/// </summary>
		private static readonly MethodInfo _register;

		/// <summary>
		/// Initializes the <see cref="SimpleInjectorExtensions" /> class. Finds the required methods
		/// via reflection.
		/// </summary>
		static SimpleInjectorExtensions()
		{
			// Find the Register<TService, TImplementation>() method
			_register = typeof(Container).GetMethods()
				.First(method => 
					method.Name == "Register" 
					&& method.GetGenericArguments().Count() == 2 
					&& !method.GetParameters().Any()
				);

			_registerPerWebRequest = typeof(SimpleInjectorWebExtensions).GetMethods()
				// Find the RegisterPerWebRequest<TService, TImplementation>(container) method
				.Where(method => method.Name == "RegisterPerWebRequest")
				.Where(method =>
				{
					var methodParams = method.GetParameters();
					return
						method.GetGenericArguments().Count() == 2
						&& methodParams.Count() == 1
						&& methodParams[0].ParameterType == typeof(Container);
				})
				.First();
		}

		/// <summary>
		/// Registers that one instance of <paramref name="implementationType"/> will be returned for every 
		/// web request every time a <typeparamref name="TService"/> is requested and ensures that -if 
		/// <paramref name="implementationType"/> implements <see cref="IDisposable"/>- this instance 
		/// will get disposed on the end of the web request.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
		/// <param name="container">The container to make the registrations in.</param>
		/// <param name="implementationType">The concrete type that will be registered.</param>
		public static void RegisterPerWebRequest<TService>(this Container container, Type implementationType) where TService : class
		{
			_registerPerWebRequest
				.MakeGenericMethod(new[] { typeof(TService), implementationType })
				.Invoke(null, new object[] { container });	
		}

		/// <summary>
		/// Registers that a new instance of <paramref name="implementationType"/> will be returned every time a
		/// <typeparamref name="TService"/> is requested.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
		/// <param name="container">The container to make the registrations in.</param>
		/// <param name="implementationType">The concrete type that will be registered.</param>
		public static void Register<TService>(this Container container, Type implementationType) where TService : class
		{
			_register
				.MakeGenericMethod(new[] { typeof(TService), implementationType })
				.Invoke(container, null);
		}
	}
}