<?php defined('SYSPATH') or die('No direct script access.');

class Controller_BlogAdmin_Home extends Controller_BlogAdmin
{
	public function action_index()
	{
		$page = View::factory('blog/admin/index')
			->set('pending', Model_Blog_Comment::count_pending_comments())
			->set('spam', Model_Blog_Comment::count_spam_comments());
			
		$this->template
			->set('title', 'Blog Administration')
			->set('content', $page);
	}
	
	public function action_comments($type)
	{
		$comments = ORM::factory('Blog_Comment')
			->where('status', '=', $type)
			->find_all();
	}
}

?>