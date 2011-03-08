<?php
error_reporting(E_ALL);

// Custom exceptions
class FileNotFoundException extends Exception {}

class CMS
{
	private $cmsStartTime;
	private $dir;
	public function __construct()
	{
		$this->cmsStartTime = microtime(true);
		$this->dir = dirname(__FILE__) . '/';
	}
	
	public function Run()
	{
		// Do we have a page namE?
		if (!empty($_SERVER['PATH_INFO']) && $_SERVER['PATH_INFO'] != '/')
			$pagename = substr($_SERVER['PATH_INFO'], 1);
		// Default to index
		else
			$pagename = 'index';
			
		// Try to load this page
		try
		{
			$page = new Page($this->cmsStartTime, $this->dir, $pagename);
		}
		// Is it missing? :(
		catch (FileNotFoundException $ex)
		{
			header('HTTP/1.1 404 File Not Found');
			$page = new Page($this->cmsStartTime, $this->dir, '404');
		}
		
		// Render the page.
		$output = $page->Render();
		// Only send this if we actually have it
		if ($page->LastModified() != 0)
			header('Last-Modified: ' . gmdate('D, d M Y H:i:s', $page->LastModified()) . ' GMT');
		// Here's the actual page!
		echo $output;
		echo '
<!-- Not cached -->';
	}
}

class Page
{
	private $dir;
	private $template = 'template';
	private $templatePath;
	private $pagename;
	private $cmsStartTime;
	private $title = 'Untitled Page';
	private $body;
	private $meta = array();
	private $js = '';
	private $lastModified = 0;

	public function __construct($cmsStartTime, $dir, $pagename)
	{
		$this->cmsStartTime = $cmsStartTime;
		$this->pagename = htmlspecialchars($pagename, ENT_QUOTES);
		// Clean up the page name, now
		$pagename = str_replace('.', '', $pagename);
		$this->dir = $dir;
		
		// Check that it exists
		if (!file_exists($this->dir . 'pages/' . $this->pagename . '.php'))
			throw new FileNotFoundException('Page ' . $this->pagename . ' not found');
			
		// Let's get the last modified date
		$this->lastModified = filemtime($this->dir . 'pages/' . $this->pagename . '.php');
	}
	
	public function Render()
	{
		global $cms_start;		
		
		// Let's grab the page itself
		$template_render_start = microtime(true);
		ob_start();
		include('pages/' . $this->pagename . '.php');
		$this->body = ob_get_clean();
		
		// Now let's grab the template
		$render_start = microtime(true);
		ob_start();
		include('templates/' . $this->template . '.php');
		$template = ob_get_clean();
		$now = microtime(true);
		return $template . '
<!-- Loaded in ' . round($now - $this->cmsStartTime, 5) . ' seconds. Template rendered in ' . round($render_start - $template_render_start, 5) . ' seconds. Page rendered in ' . round($now - $render_start, 5) . ' seconds. Used ' . round(memory_get_usage() / 1024, 2) . ' KB memory -->';
	}
	
	public function LastModified()
	{
		return $this->lastModified;	
	}
}
?>