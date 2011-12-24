<?php defined('SYSPATH') or die('No direct script access.'); ?>
				<h2>Welcome</h2>
				<p>Hi, I'm Daniel! I'm a 21-year-old guy, living in Melbourne, Australia. I work at <a href="http://www.pageuppeople.com/">PageUp People</a> as a web developer. I enjoy web development, including both frontend (such as HTML5 and CSS3) and backend (such as ASP.NET MVC, PHP and Node.js) development. I love exploring new technologies, and creating websites or applications implementing these technologies.</p>
				<p>This should really have a better introduction on who I am... I'll complete it one day. For now you can <a href="http://facebook.com/daaniel">find me on Facebook</a>, I guess.</p>
				<p>I've also got a list of <a href="projects.htm">projects I've worked on</a>. I'm probably most well known as "the guy that wrote a lot of Simple Machines Forum modifications", or "the guy that wrote the VCE ENTER/ATAR Calculator" :-)</p>
				
				<h2>Contacting me</h2>
				<p>You may contact me via the following methods:</p>
				<ul>
					<!-- Yeah, this is ugly, but it's a really simple method for spambot prevention >_< -->
					<li><strong>Email</strong> &mdash; <span id="email_address"></span></li>
					<li><strong>Windows Live Messenger / MSN Messenger</strong> &mdash; <span id="messenger_address"></span></li>
					<li id="gtalk" class="offline">
						<strong>Google Talk / Jabber</strong> &mdash; <span id="gtalk_address"></span>.
						I'm currently <span id="gtalk_status">Offline</span>.
						<a id="start_gtalk_chat" href="http://www.google.com/talk/service/badge/Start?tk=z01q6amlqaf80ct0iuvnq226055735i723g9omh9525cu7ce7onoqd5vm7quktkdlts0i5d6c8nr113mhh7e06mlu92gmbv1506gcp26fdn3c45cpqlu652rb6ksdsodpjb95s019nqarbqo">Chat with me</a>
					</li>
					<li class="social">
						<strong>Social networking (and other) sites:</strong>
						
						<ul>
							<li class="facebook"><a title="Facebook" href="http://www.facebook.com/daaniel">Facebook</a></li>
							<li class="twitter"><a title="Twitter" href="http://www.twitter.com/daniel15">Twitter</a></li>
							<li class="github"><a title="Github" href="http://github.com/Daniel15">Github</a></li>
							<li class="reddit"><a title="Reddit" href="http://reddit.com/user/daniel15">Reddit</a></li>
							<li class="lastfm"><a title="Last.fm" href="http://last.fm/user/daniel_1515">Last.fm</a></li>
							<li class="linkedin"><a title="LinkedIn" href="http://linkedin.com/in/Daniel15">LinkedIn</a></li>
							<li class="gplus"><a title="Google+" href="http://dl.vc/gplus">Google+</a></li>
						</ul>
					</li>
				</ul>

				<h2><a href="blog">Recent Blog Posts</a></h2>
				<ul id="blog-posts">
<?php
foreach ($blog_posts as $post)
{
	echo '
					<li><time datetime="', date(DATE_W3C, $post['date']), '">', date('jS M Y', $post['date']), '</time> <a href="', $post['url'], '">', $post['title'], '</a></li>';
}
?>
				</ul>
