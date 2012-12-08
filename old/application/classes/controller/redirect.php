<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller to handle various redirects on the site.
 */
class Controller_Redirect extends Controller
{
	/**
	 * Redirect to the latest CSS or JS file
	 */
	public function action_latest_res()
	{
		$name = $this->request->param('name');
		$type = $this->request->param('type');
		
		if ($type != 'js' && $type != 'css')
			throw new HTTP_Exception_404('Invalid type specified for latest_res: ' . $type);
			
		$filename = Kohana::$config->load('site.' . $name . strtoupper($type));
		
		if (empty($filename))
			throw new HTTP_Exception_404('Invalid name in latest_res: ' . $name);
		
		$this->request->redirect($filename);
	}
}
?>