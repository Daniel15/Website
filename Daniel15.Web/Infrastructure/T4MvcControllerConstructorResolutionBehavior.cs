using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SimpleInjector.Advanced;

namespace Daniel15.Web.Infrastructure
{
	/// <summary>
	/// An implementation of <see cref="IConstructorResolutionBehavior"/> that ignores the default
	/// constructor created by T4MVC for MVC controllers.
	/// </summary>
	/// <remarks>See http://simpleinjector.codeplex.com/wikipage?title=T4MVC%20Integration</remarks>
	public class T4MvcControllerConstructorResolutionBehavior : IConstructorResolutionBehavior
	{
		private readonly IConstructorResolutionBehavior _defaultBehavior;

		public T4MvcControllerConstructorResolutionBehavior(IConstructorResolutionBehavior defaultBehavior)
		{
			_defaultBehavior = defaultBehavior;
		}

		[DebuggerStepThrough]
		public ConstructorInfo GetConstructor(Type serviceType, Type impType)
		{
			if (typeof(IController).IsAssignableFrom(impType))
			{
				var nonDefaultConstructor = impType.GetConstructors().Where(x => x.GetParameters().Length > 0).ToList();

				if (nonDefaultConstructor.Count == 1)
				{
					return nonDefaultConstructor.Single();
				}
			}

			// fall back to the container's default behavior.
			return _defaultBehavior.GetConstructor(serviceType, impType);
		}

	}
}