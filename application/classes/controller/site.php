<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Site extends Controller_Template
{
	/**
	 * Index page for the website
	 */
	public function action_index()
	{
		// Load the most recent blog posts (from cache if available)
		if (!Kohana::$config->load('cache.enabled') || !($posts = $this->cache->get('daniel15-recent-posts-summary')))
		{
			// No cache available, so load data from database
			$posts = array();
			$posts_full = ORM::factory('Blog_Post')
				->order_by('date', 'desc')
				->limit(10)
				->find_all();
				
			// Pull the ORM objects into a very simple object array (better for caching)
			foreach ($posts_full as $post)
			{
				// The post is stored in an array as XCache doesn't support caching objects
				// TODO: Switch to another caching system (eg. APC) on live?
				$posts[] = array(
					'title' => $post->title,
					'date' => $post->date,
					'url' => $post->url()
				);
			}
				
			$this->cache->set('daniel15-recent-posts-summary', $posts, 86400);
		}
		
		$page = View::factory('index')
			->bind('blog_posts', $posts);
		
		$this->template
			->set('title', '')
			->set('lastModified', filemtime(Kohana::find_file('views', 'index')))
			->set('sidebarType', 'right')
			->bind('content', $page)
			->bind('sidebar', new View('sidebars/index'));
		
		$this->template->meta['Description'] = 'Website of Daniel15 (Daniel Lo Nigro), a 21-year-old guy from Melbourne Australia. Here I blog about things important to me, and also link to the various other projects I\'m working on.';
		
		// Extra <head> stuff
		// TODO: This should probably go elsewhere
		$this->template->extraHead = '
	<!-- OpenID -->
	<link rel="openid.server" href="https://www.startssl.com/id.ssl" />
	<link rel="openid.delegate" href="https://daniel15.startssl.com/" />
	<link rel="openid2.provider" href="https://www.startssl.com/id.ssl" />
	<link rel="openid2.local_id" href="https://daniel15.startssl.com/" />
	<meta http-equiv="X-XRDS-Location" content="https://daniel15.startssl.com/xrds/" />
';
	}
	
	/**
	 * Projects page for the website
	 */
	public function action_projects()
	{
		$model = Model::factory('projects');
		$techs = $model->get_techs();
		$techs2 = $model->get_other_techs();
		
		$this->template
			->set('title', 'Projects')
			->set('sidebarType', 'right');
			
		$projects = View::factory('includes/projectlist')
			->set('projects', $model->get_projects())
			->set('techs', $techs)
			->set('techs2', $techs2);
			
		$prev_projects = View::factory('includes/projectlist')
			->set('projects', $model->get_prev_projects())
			->set('techs', $techs)
			->set('techs2', $techs2);
			
		$this->template->content = View::factory('projects')
			->bind('techs', $techs)
			->bind('projects', $projects)
			->bind('prevProjects', $prev_projects)
			->set('techDescs', $model->get_tech_descs());
			
		$this->template->sidebar = View::factory('sidebars/projects')
			->bind('techs', $techs);
			
		$this->template->meta['Description'] = 'A listing of projects that I\'m currently working on (including this site, the VCE ATAR Calculator, rTorrentWeb, Sharpamp, ObviousSpoilers.com, DanSoft Australia, and more)';
		
		// Since the data file changes this page too, the last modified date is the maximum
		// of either this page itself, or the data file.
		$this->template->lastModified = max(filemtime(Kohana::find_file('views', 'projects')), filemtime(Kohana::find_file('classes/model', 'projects')));
	}
	
	/**
	 * "Social Feed" - Latest Twitter, YouTube, Facebook, etc. posts
	 */
	public function action_socialfeed()
	{
		// TODO: Convert this to a Kohana module. This method of loading the page is a little ugly!
		$content = Request::factory(URL::site('socialfeed/loadhtml.php', true))->execute()->body();
		$this->template
			->set('title', 'What I\'ve Been Doing')
			->set('content', View::factory('socialfeed')->set('content', $content));
	}
	
	/**
	 * Search page. Uses Google Custom Search
	 */
	public function action_search()
	{
		$this->template
			->set('title', 'Search Results')
			->set('content', View::factory('search'));
	}
	
	// Old URLs
	public function action_feed()
	{
		$this->request->redirect('socialfeed.htm' . ((!empty($_SERVER['QUERY_STRING'])) ? '?' . $_SERVER['QUERY_STRING'] : ''), 301);
	}
}
