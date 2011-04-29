<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Site extends Controller_Template
{	
	public function action_home()
	{
		// Load the most recent blog posts
		$posts = unserialize(file_get_contents('cms/data/blog_posts'));
		
		$page = View::factory('index')
			->bind('blogPosts', $posts);
		
		$this->template
			->set('title', '')
			->set('lastModified', filemtime(Kohana::find_file('views', 'index')))
			->set('sidebarType', 'right')
			->bind('content', $page)
			->bind('sidebar', new View('sidebars/index'));
		
		$this->template->meta['Description'] = 'Website of Daniel15 (Daniel Lo Nigro), a 20-year-old guy from Melbourne Australia. Here I blog about things important to me, and also link to the various other projects I\'m working on.';
		
		// Extra <head> stuff
		$this->template->extraHead = '
	<!-- OpenID -->
	<link rel="openid.server" href="https://www.startssl.com/id.ssl" />
	<link rel="openid.delegate" href="https://daniel15.startssl.com/" />
	<link rel="openid2.provider" href="https://www.startssl.com/id.ssl" />
	<link rel="openid2.local_id" href="https://daniel15.startssl.com/" />
	<meta http-equiv="X-XRDS-Location" content="https://daniel15.startssl.com/xrds/" />
';
	}
	
	public function action_projects()
	{
		$model = Model::factory('projects');
		$techs = $model->getTechs();
		$techs2 = $model->getOtherTechs();
		
		$this->template
			->set('title', 'Projects')
			->set('sidebarType', 'right');
			
		$projects = View::factory('includes/projectList')
			->set('projects', $model->getProjects())
			->set('techs', $techs)
			->set('techs2', $techs2);
			
		$prevProjects = View::factory('includes/projectList')
			->set('projects', $model->getPrevProjects())
			->set('techs', $techs)
			->set('techs2', $techs2);
			
		$this->template->content = View::factory('projects')
			->bind('techs', $techs)
			->bind('projects', $projects)
			->bind('prevProjects', $prevProjects)
			->set('techDescs', $model->getTechDescs());
			
		$this->template->sidebar = View::factory('sidebars/projects')
			->bind('techs', $techs);
			
		$this->template->meta['Description'] = 'A listing of projects that I\'m currently working on (including this site, the VCE ENTER Calculator, rTorrentWeb, Sharpamp, ObviousSpoilers.com, DanSoft Australia, and more)';
		
		// Since the data file changes this page too, the last modified date is the maximum
		// of either this page itself, or the data file.
		$this->template->lastModified = max(filemtime(Kohana::find_file('views', 'projects')), filemtime(Kohana::find_file('classes/model', 'projects')));
		
		// TODO: Set last modified date
	}
	
	public function action_socialfeed()
	{
		// TODO: Convert this to a Kohana module. This method of loading the page is a little ugly!
		$content = Request::factory(URL::site('socialfeed/loadhtml.php', true))->execute()->body();
		$this->template
			->set('title', 'What I\'ve Been Doing')
			->set('content', View::factory('socialfeed')->set('content', $content));
	}
	
	// Old URLs
	public function action_feed()
	{
		$this->request->redirect('socialfeed.htm', 301);
	}
}
