<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Template extends Kohana_Controller_Template
{
	public function before()
	{
		parent::before();
		$this->template->title = 'Untitled Page';
		$this->template->content = null;
		$this->template->sidebarType = 'none';
		$this->template->sidebar = null;
		$this->template->meta = array();
		$this->template->menu = $this->getMenu();
		
		$request = Request::current();
		$this->template->pageID = $request->controller() . '-' . $request->action();
		$this->template->controller = $request->controller();
		
		$this->template->siteConfig = $this->siteConfig = Kohana::config('site');
	}
	
	public function after()
	{		
		// Propagate title from template to actual view
		if ($this->template->content instanceof View)
			$this->template->content->title = $this->template->title;
			
		// Send Last-Modified header if available
		if (!empty($this->template->lastModified))
			header('Last-Modified: ' . gmdate('D, d M Y H:i:s', $this->template->lastModified) . ' GMT');
			
		parent::after();
	}
	
	protected function getMenu()
	{
		$action = Request::current()->action();
		
		// TODO: Should this be in a model?
		$menu_content = array(
			'' => array(
				'title' => 'Home',
				'active' => $action == 'home'
			),
			'projects.htm' => array(
				'title' => 'Projects',
				'active' => $action == 'projects'
			),
			'blog/' => array(
				'title' => 'Blog',
				'active' => false
			),
			'blog/category/microblog/' => array(
				'title' => 'Thoughts',
				'active' => false
			),
		);		
		
		return View::factory('includes/menu')
			->set('items', $menu_content);
	}
}
?>