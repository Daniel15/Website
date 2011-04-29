<?php defined('SYSPATH') or die('No direct script access.');

// Based on http://kohanaframework.org/3.1/guide/kohana/tutorials/error-pages
// TODO: Clean up
class Controller_Error extends Controller_Template
{
	public function before()
	{
		parent::before();

		$this->template->page = URL::site(rawurldecode(Request::$initial->uri()));

		// Internal request only!
		if (Request::$initial !== Request::$current)
		{
			if ($message = rawurldecode($this->request->param('message')))
			{
				// TODO: Move this to after() so 404 isn't loading a second view (performance)
				$this->template->content = View::factory('error')->set('message', $message);
			}
		}
		else
		{
			$this->request->action(404);
		}

		$this->response->status((int) $this->request->action());
	}

	public function action_404()
	{
		$this->template->title = 'File Not Found';
		
		$this->template->content = View::factory('404')->set('page', $this->template->page);

		// Here we check to see if a 404 came from our website. This allows the
		// webmaster to find broken links and update them in a shorter amount of time.
		if (isset ($_SERVER['HTTP_REFERER']) AND strstr($_SERVER['HTTP_REFERER'], $_SERVER['SERVER_NAME']) !== FALSE)
		{
			// Set a local flag so we can display different messages in our template.
			$this->template->local = TRUE;
		}

		// HTTP Status code.
		$this->response->status(404);
	}

	public function action_503()
	{
		$this->template->title = 'Maintenance Mode';
	}

	public function action_500()
	{
		$this->template->title = 'Internal Server Error';
	}
}