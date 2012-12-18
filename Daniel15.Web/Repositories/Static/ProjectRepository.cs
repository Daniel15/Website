using System;
using System.Collections.Generic;
using Daniel15.Web.Models.Home;
using System.Linq;

namespace Daniel15.Web.Repositories.Static
{
	/// <summary>
	/// A project repository using a set of static data. 
	/// TODO: Move it all into the database
	/// </summary>
	public class ProjectRepository : IProjectRepository
	{
		private static List<ProjectTechnologyModel> _technologies; 
		private static List<ProjectModel> _projects;

		public ProjectRepository()
		{
			#region Static data
			_technologies = new List<ProjectTechnologyModel>
			{
				// TODO: Move this all into the database!!!
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="C#",
					Alias = "csharp",
					Url = "http://msdn.microsoft.com/en-us/vcsharp/default.aspx",
					Desc = "<p>C# is a programming language created by Microsoft. It is very popular these days due to its ease of use. I love it, it's one of my favourite programming languages. More information about C# is available on Microsoft's developer site, MSDN: <a href=\"http://msdn.microsoft.com/en-us/vcsharp/default.aspx\">http://msdn.microsoft.com/en-us/vcsharp/default.aspx</a></p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="CMS Made Simple",
					Alias = "cmsms",
					Url = "http://www.cmsmadesimple.org/",
					Desc = "<p><a href=\"http://cmsmadesimple.org/\">CMS Made Simple</a> is a reasonably simple CMS (Content Management System) that works very well for small to medium websites. I've used it in the past for a few of my sites, and also for people I've created websites for.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="CSS",
					Alias = "css",
					Url = "http://en.wikipedia.org/wiki/CSS",
					Desc = "<p><a href=\"http://en.wikipedia.org/wiki/CSS\">CSS</a> is used to give style to web pages on the Internet. It is used for everything from creating the base of the actual layout, all the way down to styling the text. Writing CSS that works perfectly across all browsers (including Internet Explorer) is somewhat of a challenge, a skill that is gained with time.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="CodeIgniter PHP Framework",
					Alias = "codeigniter",
					Url = "http://www.codeigniter.com/",
					Desc = "<p><a href=\"http://www.codeigniter.com/\">CodeIgniter</a> is a lightweight, easy to learn and easy to understand PHP framework. It is designed to be a small framework with a small footprint, to help you make awesome PHP applications in a fraction of the time it would normally take.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="HTML",
					Alias = "html",
					Url = "http://en.wikipedia.org/wiki/HTML",
					Desc = "<p><a href=\"http://en.wikipedia.org/wiki/HTML\">HTML (Hypertext Markup Language)</a> is what is used to create web pages.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="JavaScript",
					Alias = "js",
					Url = "http://en.wikipedia.org/wiki/JavaScript",
					Desc = "<p><a href=\"http://en.wikipedia.org/wiki/JavaScript\">JavaScript</a> is a programming language used to add interactivity to web pages. A large majority of web pages on the internet these days have at least a little bit of JavaScript. In fact, you loaded this bit of text using JavaScript :-)</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="Kohana PHP Framework",
					Alias = "kohana",
					Url = "http://kohanaphp.com/",
					Desc = "<p><a href=\"http://kohanaphp.com/\">Kohana</a>, like CodeIgniter, is a lightweight PHP framework designed to help you build awesome PHP applications. Originally it was based off CodeIgniter, but as of the latest releases, it has been completely rewritten.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="MediaWiki",
					Alias = "mediawiki",
					Url = "http://mediawiki.org/",
					Desc = "<p><a href=\"http://mediawiki.org/\">MediaWiki</a> is a web application used to power wikis such as Wikipedia. It is in very widespread use, and is the most popular PHP-based wiki software.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="MooTools JS framework",
					Alias = "mootools",
					Url = "http://www.mootools.net/",
					Desc = "<p>In my opinion, <a href=\"http://www.mootools.net/\">MooTools</a> is the best JavaScript framework, quite a bit better than jQuery (the <a href=\"http://jqueryvsmootools.com/\">jQuery vs MooTools</a> website mentions some of the points I like about MooTools</a>). I use it for most of my JavaScript development &mdash; Including this very site.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="MySQL DBMS",
					Alias = "mysql",
					Url = "http://www.mysql.com/",
					Desc = "<p><a href=\"http://www.mysql.com/\">MySQL</a> is one of the world's most popular database engines. It is used in the backend of a massive number of sites, and by a large number of web applications. My experience with MySQL includes simple things as well as advanced queries, stored procedures, etc.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="Node.js",
					Alias = "nodejs",
					Url = "http://nodejs.org/",
					Desc = "<p>Node.js is a software system designed for writing highly-scalable internet applications, using event-driven, asynchronous I/O to minimize overhead and maximize scalability. It lets you write your server applications in JavaScript.</p>"
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="PHP",
					Alias = "php",
					Url = "http://www.php.net/",
					Desc = "<p><a href=\"http://php.net/\">PHP</a> is the world's most popular web scripting language. It is used to create dynamic websites and web applications. I'm well experienced in PHP, and have used it for quite a few websites. I use Object-Oriented techniques for PHP coding. It's a pretty good programming language.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="SQLite DBMS",
					Alias = "sqlite",
					Url = "http://sqlite.org/",
					Desc = "<p><a href=\"http://sqlite.org/\">SQLite</a> is, as the name suggests, a lightweight database engine.</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="Simple Machines Forum",
					Alias = "smf",
					Url = "http://www.simplemachines.org/",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="Visual Basic .NET",
					Alias = "vb",
					Url = "http://msdn.microsoft.com/en-us/vbasic/default.aspx"
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="Visual Basic 6",
					Alias = "vb6",
					Url = "http://en.wikipedia.org/wiki/Visual_Basic",
					Desc = "<p>Visual Basic 6 is ugly :(</p>",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="WordPress Blog",
					Alias = "wordpress",
					Url = "http://www.wordpress.org",
				},
				new ProjectTechnologyModel {
					IsPrimary = true,
					Name ="C++/CLI",
					Alias = "cppcli",
					Url = "http://en.wikipedia.org/wiki/C%2B%2B/CLI"
				},

				// Secondary technologies
				new ProjectTechnologyModel
				{
					Alias = "kohana_orm",
					Url = "http://kohanaphp.com/",
					Name = "Kohana ORM"
				},
				new ProjectTechnologyModel
				{
					Alias = "xmlrpc",
					Name = "XMLRPC",
					Url = "http://en.wikipedia.org/wiki/XMLRPC",
				},
				new ProjectTechnologyModel
				{
					Alias = "ajax",
					Name = "AJAX (XMLHttpRequest)",
					Url = "http://en.wikipedia.org/wiki/XMLHttpRequest",
				},
				new ProjectTechnologyModel
				{
					Alias = "gd",
					Name = "PHP GD (image generation) library"
				},
				new ProjectTechnologyModel
				{
					Alias = "powerdns",
					Name = "PowerDNS",
					Url = "http://www.powerdns.com/"
				},
				new ProjectTechnologyModel
				{
					Alias = "highcharts",
					Name = "Highcharts",
					Url = "http://www.highcharts.com/"
				},
			};

			var techsByAlias = _technologies.ToDictionary(x => x.Alias);

			_projects = new List<ProjectModel>
			{
				new ProjectModel {
					IsCurrent = true,
					Name = "dan.cx (this site!)",
					Url = "http://dan.cx/",
					Thumbnail = "daniel15net",
					ProjectType = ProjectType.Website,
					Description = "A personal site about me, listing all the current and previous projects I've worked on, as well as contact information. Site was designed and coded by me, using the Kohana PHP framework, and the MooTools JavaScript framework. The source code is <a href=\"http://github.com/Daniel15/Website\">available on Github</a>. It is <em>always</em> under construction, I keep changing it. :P",
					Date = "January 2012",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["kohana"], techsByAlias["html"], techsByAlias["css"], techsByAlias["js"], techsByAlias["kohana_orm"] },
				},
			
				new ProjectModel {
					IsCurrent = true,
					Name = "Daniel15 JavaScript Framework",
					Url = "http://dl.vc/jsframework",
					ProjectType = ProjectType.Library,
					Description = "This is a simple JavaScript framework I started writing for when I don't need a big framework like MooTools or jQuery. It's currently in use on this site as well as a few other little sites.",
					Date = "December 2011",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["js"] },
				},
			
				new ProjectModel {
					IsCurrent = true,
					Name = "zURL",
					Url = "http://zurl.ws/",
					Thumbnail = "zurl",
					ProjectType = ProjectType.Website,
					Description = "zURL is a URL shortening service. Basically, it lets you enter loooong URLs, and make them into nice short ones. Due to the large number of shortening services available these days, I wasn't going to update it much. However, I totally redid the site in February 2010, mainly as an exercise to learn Kohana 3.0. I may eventually open-source the site and its code.",
					Date = "April 2007, major revision February/March 2010",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["kohana"], techsByAlias["php"], techsByAlias["html"], techsByAlias["css"], techsByAlias["js"], techsByAlias["mootools"], techsByAlias["kohana_orm"], techsByAlias["xmlrpc"], techsByAlias["ajax"], },
				},
			
				new ProjectModel {
					IsCurrent = true,
					Name = "VCE ATAR Calculator",
					Url = "http://atarcalc.com/",
					Thumbnail = "entercalc",
					ThumbnailHeight = 141,
					ProjectType = ProjectType.Website,
					Description = "The VCE ATAR calculator allows you to estimate what your ATAR would be, based on your study score estimates. The previous year's scaling report is used to estimate what these study scores would scale to, and then the aggregate to ATAR table is used to estimate the ATAR. This is one of my most popular sites to date. As shown in the screenshot, the VCE ATAR calculator uses the same design as Syte.",
					Date = "November 2007, updated February 2010",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["mysql"], techsByAlias["mootools"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], },
				},

				/**********************************************************************************
				 * Previous projects
				 **********************************************************************************/
				new ProjectModel {
					IsCurrent = false,
					Name = "NodeWhiteboard",
					Url = "https://github.com/Daniel15/NodeWhiteboard",
					Thumbnail = "nodewhiteboard",
					ThumbnailHeight = 190,
					ProjectType = ProjectType.WebApplication,
					Description = "NodeWhiteboard is a simple whiteboard app using Node.js, SVG (via Raphaël) and Socket.io. It allows users to draw stuff and see updates in real-time.",
					Date = "January 2012",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["nodejs"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], techsByAlias["ajax"],  },
				},
				new ProjectModel {
					IsCurrent = false,
					Name = "pmacct-frontend",
					Url = "https://github.com/Daniel15/NodeWhiteboard",
					Thumbnail = "pmacct",
					ThumbnailHeight = 131,
					ProjectType = ProjectType.WebApplication,
					Description = "Pmacct-frontend is a quick and ugly statistics frontend for <a href=\"http://www.pmacct.net/\">pmacct</a> for my personal use.",
					Date = "October 2011",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["mysql"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], techsByAlias["highcharts"],  },
				},
				new ProjectModel {
					IsCurrent = false,
					Name = "rTorrentWeb",
					Url = "http://rtorrentweb.com/",
					Thumbnail = "rtorrentweb",
					ThumbnailHeight = 129,
					ProjectType = ProjectType.WebApplication,
					Description = "A web-based frontend for the high-performance command-line torrent client rTorrent. The interface is inspired by uTorrent.",
					Date = "Pre-beta available from December 2009.",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["kohana"], techsByAlias["sqlite"], techsByAlias["mootools"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], techsByAlias["kohana_orm"], techsByAlias["xmlrpc"], techsByAlias["ajax"],  },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "Sharpamp",
					Url = "http://code.google.com/p/sharpamp/",
					Thumbnail = "no-pic",
					ProjectType = ProjectType.Library,
					Description = "Sharpamp allows you to easily write Winamp plugins in C#. It provides a library for access to the Winamp API, and a Visual Studio template for creating Winamp plugins. It is open-source, licenced under the GNU Lesser General Public License.",
					Date = "November 2009",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["csharp"], techsByAlias["cppcli"] },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "Syte",
					Url = "http://syte.cc/",
					Thumbnail = "syte",
					ProjectType = ProjectType.Website,
					Description = "Syte is a website that allows you to get a free <em>whatever</em>.syte.cc subdomain. The design for this site was coded by me, and I have reused it on a few other sites (instead of just using a plain layout).",
					Date = "April 2009",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["mysql"], techsByAlias["mootools"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], techsByAlias["powerdns"],  },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "ObviousSpoilers.com",
					Url = "http://obviousspoilers.com/",
					Thumbnail = "no-pic",
					ProjectType = ProjectType.Website,
					Description = "ObviousSpoilers.com is a site that lists a large number of blatantly obvious movie, game and TV show spoilers. All content is user-submitted, and users are able to vote submissions up and down, and favourite the submissions they like the most.",
					Date = "May 2009",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["codeigniter"], techsByAlias["mootools"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "DanSoft Australia",
					Url = "http://www.dansoftaustralia.net/",
					Thumbnail = "dansoft",
					ProjectType = ProjectType.Website,
					Description = "DanSoft Australia was a website that hosted all the applications I used to develop when I got bored. It is one of my oldest projects that's still live on the internet today. Unfortunately due to time constraints, I have stopped updating it as much as I used to, and instead moved on to developing websites and webapps instead.", 
					Date = "2003 to 2008",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["vb"], techsByAlias["vb6"], techsByAlias["cmsms"], techsByAlias["html"], techsByAlias["css"], },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "Daniel15.com",
					Thumbnail = "daniel15",
					ProjectType = ProjectType.Website,
					Description = "Daniel15.com once contained my blog, and a support forum for SMFShop as well as the other SMF modifications I used to develop. Since then, my blog has moved to this site, and the SMFShop support forum has moved to <em>smfshop.com</em>. Daniel15.com no longer exists, and redirects to this site now. The design I made for Daniel15.com has been updated and used on this site.",
					Date = "2007 to 2009",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["wordpress"], techsByAlias["smf"], techsByAlias["html"], techsByAlias["css"],  },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "DNSTools.ws",
					Url = "http://dnstools.ws/",
					Thumbnail = "dnstools",
					ProjectType = ProjectType.Website,
					Description = "DNSTools is a site that contains a lot of DNS utilities, including DNS Lookup, whois, traceroute, ping, and reverse DNS. It is very handy for server administrators who want to check their servers are working correctly.",
					Date = "December 2007",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["php"], techsByAlias["html"], techsByAlias["css"]},
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "PicHost",
					Url = "http://pichost.ws/",
					ProjectType = ProjectType.Website,
					Thumbnail = "no-pic",
					Description = "PicHost is a picture hosting website that I developed in my free time. It was never opened publically, and remains forever a beta test.",
					Date = "2008",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["codeigniter"], techsByAlias["php"], techsByAlias["mootools"], techsByAlias["js"], techsByAlias["html"], techsByAlias["css"], },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "Dynamic-Sig",
					Url = "http://www.dynamic-sig.com/",
					Thumbnail = "dynamic",
					ProjectType = ProjectType.Website,
					Description = "Dynamic-Sig is a website that allows you to get a dynamic signature image, based on certain aspects of your computer system, as well as things like local time, and when you were last online. It runs a program on your computer, which sends updated data to the server at a regular interval. The server would then use this data to generate a unique signature image for the user. I've lost interest in this project, so probably won't update it.",
					Date = "2006 - 2007",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["vb"], techsByAlias["smf"], techsByAlias["php"], techsByAlias["html"], techsByAlias["gd"],  },
				},
			
				new ProjectModel {
					IsCurrent = false,
					Name = "SMF Modifications Wiki",
					Url = "http://www.smfmods.org/",
					Thumbnail = "smfmods",
					ProjectType = ProjectType.Website,
					Description = "The SMF Modifications Wiki is a MediaWiki-powered wiki containing information for SMF (Simple Machines Forum) modification developers. It never really caught on, and now lays abandoned. ",
					Date = "2007",
					Technologies = new List<ProjectTechnologyModel> { techsByAlias["smf"], techsByAlias["mediawiki"] },
				},
			};

			#endregion
		}
		
		/// <summary>
		/// Gets all the entities in the database
		/// </summary>
		/// <returns></returns>
		public List<ProjectModel> All()
		{
			return _projects;
		}

		/// <summary>
		/// Gets a particular entity from the database
		/// </summary>
		/// <param name="id">ID of the entity</param>
		/// <returns>The entity</returns>
		public ProjectModel Get(int id)
		{
			return _projects[id];
		}
		
		/// <summary>
		/// Saves this entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		public void Save(ProjectModel entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get the total number of records in this table
		/// </summary>
		/// <returns>Total number of records</returns>
		public int Count()
		{
			return _projects.Count;
		}

		/// <summary>
		/// Gets a list of the main technologies used to build my sites
		/// </summary>
		/// <returns>A list of technologies</returns>
		public IList<ProjectTechnologyModel> PrimaryTechnologies()
		{
			return _technologies.Where(x => x.IsPrimary).ToList();
		}
	}
}