<?php defined('SYSPATH') or die('No direct script access.');

abstract class Controller_Admin extends Controller_Template
{
	protected $auth;
	
	public function before()
	{
		parent::before();
		$this->auth = Auth::instance();
		if (!$this->auth->logged_in())
		{
			echo 'Please log in first';
			die();
		}
	}
}
?>