---
id: 342
title: Using Entity Framework 6 and MySQL on ASP.NET 5 (vNext)
published: true
publishedDate: 2015-08-01 17:38:33Z
lastModifiedDate: 2015-08-01 17:38:33Z
categories:
- C#
- Programming

---

<p>Visual Studio 2015 was recently released, and with it came a newer beta of ASP.NET 5 (formerly referred to as "ASP.NET vNext"). ASP.NET 5 is a complete rewrite of ASP.NET, focusing on being lightweight, composible, and cross-platform. It also includes an alpha version of Entity Framework 7. However, EF7 is not yet production-ready and does not support all features of EF6. One feature that is missing from EF6 is support for other database providers - Only SQL Server and SQLite are supported at this time.</p>

</p>I wanted to transition a site over to ASP.NET 5, but needed to continue using MySQL as a data source. This meant getting Entity Framework 6 running on ASP.NET 5, which is pretty much undocumented right now. All the documentation and tutorials for EF6 heavily relies on configuration in Web.config, which no longer exists in ASP.NET 5. In this post I'll discuss the steps I needed to take to get it running. An example project containing all the code in this post can be found at <a href="https://github.com/Daniel15/EFExample">https://github.com/Daniel15/EFExample</a>.</p>

<p>Since EF6 does not support .NET Core, we need to remove .NET Core support (delete <code>"dnxcore50": { }</code> from <code>project.json</code>). Once that's done, install the <em>EntityFramework</em> and <em>MySql.Data.Entity</em> packages, and add references to <em>System.Data</em> and <em>System.Configuration</em>. For this post, I'll be using this basic model and <code>DbContext</code>, and assume you've already created your database in MySQL:</p>
<pre class="brush: csharp">
public class MyContext : DbContext
{
	public virtual DbSet<Post> Posts { get; set; }
}

public class Post
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }
}
</pre>

<p>Entity Framework 6 relies on the provider and connection string being configured in <em>Web.config</em>. Since Web.config is no longer used with ASP.NET 5, we need to use <a href="https://msdn.microsoft.com/en-us/data/jj680699.aspx"  target="_blank">code-based configuration</a> to configure it instead. To do so, create a new class that inherits from <code>DbConfiguration</code>:</p>

<pre class="brush: csharp">
public class MyDbConfiguration : DbConfiguration
{
	public MyDbConfiguration()
	{
		// Attempt to register ADO.NET provider
		try {
			var dataSet = (DataSet)ConfigurationManager.GetSection("system.data");
			dataSet.Tables[0].Rows.Add(
				"MySQL Data Provider",
				".Net Framework Data Provider for MySQL",
				"MySql.Data.MySqlClient",
				typeof(MySqlClientFactory).AssemblyQualifiedName
			);
		}
		catch (ConstraintException)
		{
			// MySQL provider is already installed, just ignore the exception
		}

		// Register Entity Framework provider
		SetProviderServices("MySql.Data.MySqlClient", new MySqlProviderServices());
		SetDefaultConnectionFactory(new MySqlConnectionFactory());
	}
}
</pre>

<p>The first part of the configuration is a hack to register the ADO.NET provider at runtime, by dynamically adding a new configuration entry to the <code>system.data</code> section. The second part registers the Entity Framework provider. We also need to modify the configuration file to include the connection string. You can use any configuration provider supported by ASP.NET 5, I'm using <code>config.json</code> here because it's the default provider.
</p>

<pre class="brush: js">
{
  "Data": {
    "DefaultConnection": {
      "ConnectionString": "Server=localhost; Database=test; Uid=vmdev; Pwd=password;"
    }
  }
}
</pre>
<p></p>
<p>Now that we have the configuration, we need to modify the context to use it:</p>

<pre class="brush: csharp">
[DbConfigurationType(typeof(MyDbConfiguration))]
public class MyContext : DbContext
{
	public MyContext(IConfiguration config)
		: base(config.Get("Data:DefaultConnection:ConnectionString"))
	{
	}
	// ...
}
</pre>

<p>An instance of <code>IConfiguration</code> will be automatically passed in by <a href="http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx" target="_blank">ASP.NET 5's dependency injection system</a>. The final step is to register <code>MyContext</code> in the dependency injection container, which is done in your <code>Startup.cs</code> file:</p>

<pre class="brush: csharp">
public void ConfigureServices(IServiceCollection services)
{
	// ...
	services.AddScoped<MyContext>();
}
</pre>

<p><code>AddScoped</code> specifies that one context should be created per request, and the context will automatically be disposed once the request ends. Now that all the configuration is complete, we can use <code>MyContext</code> like we normally would:</p>
<pre class="brush: csharp">
public class HomeController : Controller
{
    private readonly MyContext _context;

    public HomeController(MyContext context)
    {
	    _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Posts);
    }
}
</pre>
<p>Hope you find this useful!</p>
<p>Until next time, <br />&mdash; Daniel</p>
