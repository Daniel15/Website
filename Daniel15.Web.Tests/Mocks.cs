using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Daniel15.Web.App_Start;
using Daniel15.Web.Areas.Admin;
using Moq;

namespace Daniel15.Web.Tests
{
	class Mocks
	{
		public HttpContextBase Context { get { return ContextMock.Object; } }
		public HttpRequestBase Request { get { return RequestMock.Object; } }
		public HttpResponseBase Response { get { return ResponseMock.Object; } }		
		public HttpServerUtilityBase Server { get { return ServerMock.Object; } }
		public UrlHelper UrlHelper { get; private set; }

		public Mock<HttpContextBase> ContextMock { get; private set; }
		public Mock<HttpRequestBase> RequestMock { get; private set; }
		public Mock<HttpResponseBase> ResponseMock { get; private set; }
		public Mock<HttpServerUtilityBase> ServerMock { get; private set; }

		public Mocks()
		{
			ContextMock = new Mock<HttpContextBase>();
			RequestMock = new Mock<HttpRequestBase>();
			ResponseMock = new Mock<HttpResponseBase>();
			ServerMock = new Mock<HttpServerUtilityBase>();

			ContextMock.Setup(ctx => ctx.Request).Returns(Request);
			ContextMock.Setup(ctx => ctx.Response).Returns(Response);
			ContextMock.Setup(ctx => ctx.Server).Returns(Server);

			RequestMock.Setup(x => x.ApplicationPath).Returns("/");
			RequestMock.Setup(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
			RequestMock.Setup(x => x.ServerVariables).Returns(new NameValueCollection());

			ResponseMock.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

			var routes = new RouteCollection();
			//AreaRegistration.RegisterAllAreas();
			var areaRegistration = new AreaRegistrationContext("Admin", routes);
			new AdminAreaRegistration().RegisterArea(areaRegistration);
			RouteConfig.RegisterRoutes(routes);

			var requestContext = new RequestContext(Context, new RouteData());
			UrlHelper = new UrlHelper(requestContext, routes);
		}
	}
}
