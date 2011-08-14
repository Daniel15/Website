<?php defined('SYSPATH') or die('No direct script access.');

class Model_Projects extends Model
{
	public function get_techs()
	{
		return array(
			'csharp' => array(
				'name' => 'C#',
				'icon' => 'csharp',
				'url' => 'http://msdn.microsoft.com/en-us/vcsharp/default.aspx',
				'desc' => '<p>C# is a programming language created by Microsoft. It is very popular these days due to its ease of use. I love it, it\'s one of my favourite programming languages. More information about C# is available on Microsoft\'s developer site, MSDN: <a href="http://msdn.microsoft.com/en-us/vcsharp/default.aspx">http://msdn.microsoft.com/en-us/vcsharp/default.aspx</a></p>',
			),
			'cmsms' => array(
				'name' => 'CMS Made Simple',
				'icon' => 'cmsms',
				'url' => 'http://www.cmsmadesimple.org/',
				'desc' => '<p><a href="http://cmsmadesimple.org/">CMS Made Simple</a> is a reasonably simple CMS (Content Management System) that works very well for small to medium websites. I\'ve used it in the past for a few of my sites, and also for people I\'ve created websites for.</p>',
			),
			'css' => array(
				'name' => 'CSS',
				'icon' => 'css',
				'url' => 'http://en.wikipedia.org/wiki/CSS',
				'desc' => '<p><a href="http://en.wikipedia.org/wiki/CSS">CSS</a> is used to give style to web pages on the Internet. It is used for everything from creating the base of the actual layout, all the way down to styling the text. Writing CSS that works perfectly across all browsers (including Internet Explorer) is somewhat of a challenge, a skill that is gained with time.</p>',
			),
			'codeigniter' => array(
				'name' => 'CodeIgniter PHP Framework',
				'icon' => 'codeigniter',
				'url' => 'http://www.codeigniter.com/',
				'desc' => '<p><a href="http://www.codeigniter.com/">CodeIgniter</a> is a lightweight, easy to learn and easy to understand PHP framework. It is designed to be a small framework with a small footprint, to help you make awesome PHP applications in a fraction of the time it would normally take.</p>',
			),
			'html' => array(
				'name' => 'HTML',
				'icon' => 'html',
				'url' => 'http://en.wikipedia.org/wiki/HTML',
				'desc' => '<p><a href="http://en.wikipedia.org/wiki/HTML">HTML (Hypertext Markup Language)</a> is what is used to create web pages.</p>',
			),
			'js' => array(
				'name' => 'JavaScript',
				'icon' => 'js',
				'url' => 'http://en.wikipedia.org/wiki/JavaScript',
				'desc' => '<p><a href="http://en.wikipedia.org/wiki/JavaScript">JavaScript</a> is a programming language used to add interactivity to web pages. A large majority of web pages on the internet these days have at least a little bit of JavaScript. In fact, you loaded this bit of text using JavaScript :-)</p>',
			),
			'kohana' => array(
				'name' => 'Kohana PHP Framework',
				'icon' => 'kohana',
				'url' => 'http://kohanaphp.com/',
				'desc' => '<p><a href="http://kohanaphp.com/">Kohana</a>, like CodeIgniter, is a lightweight PHP framework designed to help you build awesome PHP applications. Originally it was based off CodeIgniter, but as of the latest releases, it has been completely rewritten.</p>',
			),
			'mediawiki' => array(
				'name' => 'MediaWiki',
				'icon' => 'mediawiki',
				'url' => 'http://mediawiki.org/',
				'desc' => '<p><a href="http://mediawiki.org/">MediaWiki</a> is a web application used to power wikis such as Wikipedia. It is in very widespread use, and is the most popular PHP-based wiki software.</p>',
			),
			'mootools' => array(
				'name' => 'MooTools JS framework',
				'icon' => 'mootools',
				'url' => 'http://www.mootools.net/',
				'desc' => '<p>In my opinion, <a href="http://www.mootools.net/">MooTools</a> is the best JavaScript framework, quite a bit better than jQuery (the <a href="http://jqueryvsmootools.com/">jQuery vs MooTools</a> website mentions some of the points I like about MooTools</a>). I use it for most of my JavaScript development &mdash; Including this very site.</p>',
			),
			'mysql' => array(
				'name' => 'MySQL DBMS',
				'icon' => 'mysql',
				'url' => 'http://www.mysql.com/',
				'desc' => '<p><a href="http://www.mysql.com/">MySQL</a> is one of the world\'s most popular database engines. It is used in the backend of a massive number of sites, and by a large number of web applications. My experience with MySQL includes simple things as well as advanced queries, stored procedures, etc.</p>',
			),
			'php' => array(
				'name' => 'PHP',
				'icon' => 'php',
				'url' => 'http://www.php.net/',
				'desc' => '<p><a href="http://php.net/">PHP</a> is the world\'s most popular web scripting language. It is used to create dynamic websites and web applications. I\'m well experienced in PHP, and have used it for quite a few websites. I use Object-Oriented techniques for PHP coding. It\'s a pretty good programming language.</p>',
			),
			'sqlite' => array(
				'name' => 'SQLite DBMS',
				'icon' => 'sqlite',
				'url' => 'http://sqlite.org/',
				'desc' => '<p><a href="http://sqlite.org/">SQLite</a> is, as the name suggests, a lightweight database engine.</p>',
			),
			'smf' => array(
				'name' => 'Simple Machines Forum',
				'icon' => 'smf',
				'url' => 'http://www.simplemachines.org/',
			),
			'vb' => array(
				'name' => 'Visual Basic .NET',
				'icon' => 'vb',
				'url' => 'http://msdn.microsoft.com/en-us/vbasic/default.aspx'
			),
			'vb6' => array(
				'name' => 'Visual Basic 6',
				'icon' => 'vb6',
				'url' => 'http://en.wikipedia.org/wiki/Visual_Basic',
				'desc' => '<p>Visual Basic 6 is ugly :(</p>',
			),
			'wordpress' => array(
				'name' => 'WordPress Blog',
				'icon' => 'wordpress',
				'url' => 'http://www.wordpress.org',
			),
			'cppcli' => array(
				'name' => 'C++/CLI',
				'icon' => 'cplusplus',
				'url' => 'http://en.wikipedia.org/wiki/C%2B%2B/CLI'
			),
		);
	}
	
	public function get_tech_descs()
	{
		$techs = $this->get_techs();
		$output = array();
		foreach ($techs as $key => $tech)
		{
			$output[$key] = isset($tech['desc']) ? $tech['desc'] : '';
		}
		
		return $output;
	}
	
	public function get_other_techs()
	{
		return array(
			'kohana_orm' => '<a href="http://kohanaphp.com/">Kohana ORM</a>',
			'xmlrpc' => '<a href="http://en.wikipedia.org/wiki/XMLRPC">XMLRPC</a>',
			'ajax' => '<a href="http://en.wikipedia.org/wiki/XMLHttpRequest">AJAX (XMLHttpRequest)</a>',
			'gd' => 'PHP GD (image generation) library',
			'powerdns' => '<a href="http://www.powerdns.com/">PowerDNS</a>',
		);
	}
	
	public function get_projects()
	{
		return array(
			array(
				'name' => 'dan.cx (this site!)',
				'url' => 'http://dan.cx/',
				'thumb' => 'daniel15net',
				'type' => 'Website',
				'description' => 'A personal site about me, listing all the current and previous projects I\'ve worked on, as well as contact information. Site was designed and coded by me, using the Kohana PHP framework, and the MooTools JavaScript framework. The source code is <a href="http://github.com/Daniel15/Website">available on Github</a>. It is <em>always</em> under construction, I keep changing it. :P',
				'date' => 'August 2011',
				'tech' => array('php', 'kohana', 'html', 'css', 'mootools', 'js'),
				'tech2' => array('kohana_orm'),
			),
			
			array(
				'name' => 'zURL',
				'url' => 'http://zurl.ws/',
				'thumb' => 'zurl',
				'type' => 'Website',
				'description' => 'zURL is a URL shortening service. Basically, it lets you enter loooong URLs, and make them into nice short ones. Due to the large number of shortening services available these days, I wasn\'t going to update it much. However, I totally redid the site in February 2010, mainly as an exercise to learn Kohana 3.0. I may eventually open-source the site and its code.',
				'date' => 'April 2007, major revision February/March 2010',
				'tech' => array('kohana', 'php', 'html', 'css', 'js', 'mootools'),
				'tech2' => array('kohana_orm', 'xmlrpc', 'ajax'),
			),
			
			array(
				'name' => 'VCE ATAR Calculator',
				'url' => 'http://atarcalc.com/',
				'thumb' => 'entercalc',
				'thumb_height' => 141,
				'type' => 'Website',
				'description' => 'The VCE ATAR calculator allows you to estimate what your ATAR would be, based on your study score estimates. The previous year\'s scaling report is used to estimate what these study scores would scale to, and then the aggregate to ATAR table is used to estimate the ATAR. This is one of my most popular sites to date. As shown in the screenshot, the VCE ATAR calculator uses the same design as Syte.',
				'date' => 'November 2007, updated February 2010',
				'tech' => array('php', 'mysql', 'mootools', 'js', 'html', 'css'),
			),
		);
		
	}
	
	public function get_prev_projects()
	{
		return array(
			array(
				'name' => 'rTorrentWeb',
				'url' => 'http://rtorrentweb.com/',
				'thumb' => 'rtorrentweb',
				'thumb_height' => 129,
				'type' => 'Web Application',
				'description' => 'A web-based frontend for the high-performance command-line torrent client rTorrent. The interface is inspired by uTorrent.',
				'date' => 'Pre-beta available from December 2009.',
				'tech' => array('php', 'kohana', 'sqlite', 'mootools', 'js', 'html', 'css'),
				'tech2' => array('kohana_orm', 'xmlrpc', 'ajax'),
			),
			
			array(
				'name' => 'Sharpamp',
					'url' => 'http://code.google.com/p/sharpamp/',
				'thumb' => 'no-pic',
				'type' => 'Library',
				'description' => 'Sharpamp allows you to easily write Winamp plugins in C#. It provides a library for access to the Winamp API, and a Visual Studio template for creating Winamp plugins. It is open-source, licenced under the GNU Lesser General Public License.',
				'date' => 'November 2009',
				'tech' => array('csharp', 'cppcli'),
			),
			
			array(
				'name' => 'Syte',
				'url' => 'http://syte.cc/',
				'thumb' => 'syte',
				'type' => 'Website',
				'description' => 'Syte is a website that allows you to get a free <em>whatever</em>.syte.cc subdomain. The design for this site was coded by me, and I have reused it on a few other sites (instead of just using a plain layout).',
				'date' => 'April 2009',
				'tech' => array('php', 'mysql', 'mootools', 'js', 'html', 'css'),
				'tech2' => array('powerdns'),
			),
			
			array(
				'name' => 'ObviousSpoilers.com',
				'url' => 'http://obviousspoilers.com/',
				'thumb' => 'no-pic',
				'type' => 'Website',
				'description' => 'ObviousSpoilers.com is a site that lists a large number of blatantly obvious movie, game and TV show spoilers. All content is user-submitted, and users are able to vote submissions up and down, and favourite the submissions they like the most.',
				'date' => 'May 2009',
				'tech' => array('php', 'codeigniter', 'mootools', 'js', 'html', 'css'),
				'tech2' => array('ajax'),
			),
			
			array(
				'name' => 'DanSoft Australia',
				'url' => 'http://www.dansoftaustralia.net/',
				'thumb' => 'dansoft',
				'type' => 'Website',
				'description' => 'DanSoft Australia was a website that hosted all the applications I used to develop when I got bored. It is one of my oldest projects that\'s still live on the internet today. Unfortunately due to time constraints, I have stopped updating it as much as I used to, and instead moved on to developing websites and webapps instead.', 
				'date' => '2003 to 2008',
				'tech' => array('vb', 'vb6', 'cmsms', 'html', 'css'),
			),
			
			array(
				'name' => 'Daniel15.com',
				'thumb' => 'daniel15',
				'type' => 'Website',
				'description' => 'Daniel15.com once contained my blog, and a support forum for SMFShop as well as the other SMF modifications I used to develop. Since then, my blog has moved to this site, and the SMFShop support forum has moved to <em>smfshop.com</em>. Daniel15.com no longer exists, and redirects to this site now. The design I made for Daniel15.com has been updated and used on this site.',
				'date' => '2007 to 2009',
				'tech' => array('php', 'wordpress', 'smf', 'html', 'css')
			),
			
			array(
				'name' => 'DNSTools.ws',
				'url' => 'http://dnstools.ws/',
				'thumb' => 'dnstools',
				'type' => 'Website',
				'description' => 'DNSTools is a site that contains a lot of DNS utilities, including DNS Lookup, whois, traceroute, ping, and reverse DNS. It is very handy for server administrators who want to check their servers are working correctly.',
				'date' => 'December 2007',
				'tech' => array('php', 'html', 'css'),
			),
			
			array(
				'name' => 'PicHost',
				'url' => 'http://pichost.ws/',
				'type' => 'Website',
				'thumb' => 'no-pic',
				'description' => 'PicHost is a picture hosting website that I developed in my free time. It was never opened publically, and remains forever a beta test.',
				'date' => '2008',
				'tech' => array('codeigniter', 'php', 'mootools', 'js', 'html', 'css'),
			),
			
			array(
				'name' => 'Dynamic-Sig',
				'url' => 'http://www.dynamic-sig.com/',
				'thumb' => 'dynamic',
				'type' => 'Website',
				'description' => 'Dynamic-Sig is a website that allows you to get a dynamic signature image, based on certain aspects of your computer system, as well as things like local time, and when you were last online. It runs a program on your computer, which sends updated data to the server at a regular interval. The server would then use this data to generate a unique signature image for the user. I\'ve lost interest in this project, so probably won\'t update it.',
				'date' => '2006 - 2007',
				'tech' => array('vb', 'smf', 'php', 'html'),
				'tech2' => array('gd'),
			),
			
			array(
				'name' => 'SMF Modifications Wiki',
				'url' => 'http://www.smfmods.org/',
				'thumb' => 'smfmods',
				'type' => 'Website',
				'description' => 'The SMF Modifications Wiki is a MediaWiki-powered wiki containing information for SMF (Simple Machines Forum) modification developers. It never really caught on, and now lays abandoned. ',
				'date' => '2007',
				'tech' => array('smf', 'mediawiki'),
			),
		);
	}
}
?>