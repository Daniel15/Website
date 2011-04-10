<?php
$this->title = '';
$this->meta = array(
	'Description' => 'Website of Daniel15 (Daniel Lo Nigro), a 20-year-old guy from Melbourne Australia. Here I blog about things important to me, and also link to the various other projects I\'m working on.',
);
$this->pageID = 'home';

//$this->js = 'window.addEvent(\'domready\', Home.init);';
$this->sidebarType = 'right';
$this->sidebar = '
				<h2><a href="feed.htm">What I\'ve Been Doing</a></h2>
				<ul id="minifeed">
					<li class="loading">Loading...</li>
				</ul>
				<ul>
					<li><a href="feed.htm">See more...</a></li>
				</ul>';

?>
				<h2>Welcome</h2>
				<p>At the moment, this site is being redesigned, recoded, rewritten, and generally re-everything'd. Stuff may be broken.</p>
				<p>Hi, I'm Daniel! I'm a 20-year-old web developer, living in Melbourne, Australia. <!--I'm a second-year student at Swinburne University of Technology, studying for a Bachelor of Science (Professional Software Development).-->I'm currently a <a href="http://swin.edu.au/">Swinburne University</a> student, doing my <a href="http://www.swinburne.edu.au/spl/ibl/">Industry Based Learning (IBL)</a> year at <a href="http://www.pageuppeople.com/">PageUp People</a> (as a Junior Developer). I enjoy web development (especially using PHP). I love exploring new technologies, and creating websites or applications implementing these technologies. This should really have a better introduction on who I am... I'll complete it one day. For now you can add me on Facebook, I guess.</p>
				<p>I've also got a list of <a href="projects.htm">projects I've worked on</a>. I'm probably most well known as "the guy that wrote a lot of Simple Machines Forum modifications", or "the guy that wrote the VCE ENTER Calculator" :-)</p>
				<p>I used to use the domain <strong>d15.biz</strong>, but recently changed to <strong>dan.cx</strong>, as it's a nicer domain.</p>
				
				<h2>Contacting me</h2>
				<p>You may contact me via the following methods:</p>
				<ul>
					<!-- Yeah, this is ugly, but it's a really simple method for spambot prevention >_< -->
					<li><strong>Email</strong> &mdash; <span id="email_address"></span></li>
					<!--li>
						<strong>Windows Live Messenger / MSN Messenger</strong> &mdash; <span id="display_name"></span>
						[<span id="messenger_address"></span>].
						I'm currently <img id="status_img" width="16" height="16" src="http://www.wlmessenger.net/static/img/presence/Offline.gif" alt="Messenger Status" title="Messenger Status" /><span id="status">Offline</span>.
						<a id="start_convo" href="http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=135148d074926a0d@apps.messenger.live.com&amp;mkt=en-AU">Start a conversation with me</a>
					</li-->
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
							<li class="reddit"><a title="Reddit" href="http://reddit.com/user/daniel15">Reddit</a></li>
							<li class="lastfm"><a title="Last.fm" href="http://last.fm/user/daniel_1515">Last.fm</a></li>
						</ul>
					</li>
				</ul>

				<h2><a href="/blog/">Recent Blog Posts</a></h2>
				<ul id="blog-posts">
<?php
// Load the recent blog posts data
$posts = unserialize(file_get_contents($this->dir . 'data/blog_posts'));
foreach ($posts as $post)
{
	echo '
					<li title="', $post['desc'], '"><span class="date">', date('jS M Y', $post['date']), '</span> <a href="', $post['url'], '">', $post['title'], '</a></li>';
}
?>
				</ul>