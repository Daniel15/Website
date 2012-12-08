<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Account extends Controller_Template
{
	protected $auth;
	
	public function before()
	{
		parent::before();
		$this->auth = Auth::instance();
		
	}
	
	public function action_login()
	{
		// Logging in?
		if ($_POST)
		{
			// Try to log in 
			if ($this->auth->login($_POST['username'], $_POST['password']))
			{
				$this->request->redirect('blogadmin/');
			}
			
			// Login failed
			// TODO: Block brute force attacks
			sleep(10);
			$this->template
				->set('top_message', 'Login failed. Please try again')
				->set('top_message_type', 'error');
		}
		
		$page = View::factory('account/login');
		
		$this->template
			->set('title', 'Log In')
			->bind('content', $page);
	}
	
	public function action_logout()
	{
		$this->auth->logout();
		$this->request->redirect('/');
	}
	
	// TODO: Remove
	public function action_hash()
	{
		echo $this->auth->hash($_GET['password']);
		die();
	}
}
?>