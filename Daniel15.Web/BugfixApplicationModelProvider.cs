using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace Daniel15.Web
{
	/// <summary>
	/// Temporary hack to work around https://github.com/aspnet/Routing/issues/391
	/// </summary>
	public class BugfixApplicationModelProvider : DefaultApplicationModelProvider
	{
		public BugfixApplicationModelProvider(IOptions<MvcOptions> mvcOptionsAccessor) : base(mvcOptionsAccessor) {}

		protected override bool IsAction(TypeInfo typeInfo, MethodInfo methodInfo)
		{
			return methodInfo.Name != "Dispose" && base.IsAction(typeInfo, methodInfo);
		}
	}
}
