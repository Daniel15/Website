<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller for handling blog sidebar rendering
 * @author Daniel15 <daniel at dan.cx>
 */
class Controller_Blog_Sidebar extends Controller
{
	public function action_index()
	{
		$sidebar = View::factory('blog/sidebar/index')
			->set('years', Model_Blog_Post::get_month_counts())
			->set('month_names', Date::months(Date::MONTHS_LONG))
			->set('categories', ORM::factory('Blog_Category')->order_by('title')->find_all());
		$this->response->body($sidebar);
	}
}
?> 