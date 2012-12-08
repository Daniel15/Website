<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Template extends Kohana_Controller_Template
{
	protected $cache;
	
	public function before()
	{
		parent::before();
		
		$this->cache = Cache::instance('default');
		
		// TODO: Fix style of variables (should use underscores not camelcase)
		$this->template->title = 'Untitled Page';
		$this->template->content = null;
		$this->template->sidebarType = 'none';
		$this->template->sidebar = null;
		$this->template->meta = array();
		$this->template->extraHead = '';
		$this->template->extraFoot = '';
		$this->template->menu = $this->get_menu();
		
		$request = Request::current();
		// Is it a request to a subdirectory (eg. blogadmin)?
		if ($request->directory() != '')
			$this->template->controller = $request->directory() . '-' . $request->controller();
		else
			$this->template->controller = $request->controller();
		
		$this->template->pageID = $this->template->controller . '-' . $request->action();
		
		$this->template->siteConfig = $this->siteConfig = Kohana::$config->load('site');
	}
	
	public function after()
	{		
		// Propagate title from template to actual view
		if ($this->template->content instanceof View)
			$this->template->content->title = $this->template->title;
			
		// Send Last-Modified header if available
		if (!empty($this->template->lastModified))
			header('Last-Modified: ' . gmdate('D, d M Y H:i:s', $this->template->lastModified) . ' GMT');
			
		// Set the <h1> tag
		// Is it in the blog?
		if (substr($this->template->controller, 0, 4) == 'blog')
			$this->template->h1 = Kohana::$config->load('blog.name');
		// Otherwise, is there a page title?
		elseif (!empty($this->template->title))
			$this->template->h1 = $this->template->title;
		else
			$this->template->h1 = 'Daniel15';
			
		parent::after();
	}
	
	protected function get_menu()
	{
		$directory = $this->request->directory();
		$controller = $this->request->controller();
		$action = $this->request->action();
		
		// TODO: Should this be in a model?
		$menu_content = array(
			'' => array(
				'title' => 'Home',
				'active' => $controller == 'site' && $action == 'index',
			),
			'projects.htm' => array(
				'title' => 'Projects',
				'active' => $action == 'projects'
			),
			'blog' => array(
				'title' => 'Blog',
				'active' => $controller == 'blog' || $directory == 'blog',
			),
			'http://daaniel.com/' => array(
				'title' => 'Thoughts',
				'active' => false
			),
		);		
		
		return View::factory('includes/menu')
			->set('items', $menu_content);
	}
}
?>
