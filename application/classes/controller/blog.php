<?php defined('SYSPATH') or die('No direct script access.');

class Controller_Blog extends Controller_Template
{
	protected $config;
	
	public function before()
	{
		parent::before();
		// Load blog config
		$this->config = Kohana::config('blog');
		$this->template->bind_global('config', $this->config);
		
		$this->template->is_blog = true;
	}
	
	public function after()
	{		
		parent::after();
	}
	
	// Listings
	public function action_index()
	{		
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		// Get the total count of posts
		$count = Model_Blog_post::total_count();
		$posts = ORM::factory('Blog_Post');
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($count, $posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . 'Daniel15\'s Blog')
			->bind('content', $page);
		
	}
	
	public function action_category($slug)
	{
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$category = ORM::factory('Blog_Category', array('slug' => $slug));
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($category->post_count(), $category->posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . 'Latest posts in ' . $category->title . ' &mdash; Daniel15\'s Blog')
			->bind('content', $page);
	}
	
	public function action_tag($slug)
	{
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$tag = ORM::factory('Blog_Tag', array('slug' => $slug));
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($tag->post_count(), $tag->posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . 'Latest posts in ' . $tag->title . ' tag &mdash; Daniel15\'s Blog')
			->bind('content', $page);
	}
	
	protected function listing($total_count, $posts)
	{
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$pagination = Pagination::factory(array(
			'total_items' => $total_count,
			'items_per_page' => $this->config->posts_per_page,
			//'view' => 'includes/pagination',
		));
		
		$posts = $posts
			->with('maincategory')
			->order_by('date', 'desc')
			->limit($pagination->items_per_page)
			->offset($pagination->offset)
			->find_all();
			
		return View::factory('blog/list')
			->bind('posts', $posts)
			->set('pagination', $pagination->render());
	}
	
	// Viewing a post
	public function action_view($year, $month, $slug)
	{
		$post = ORM::factory('Blog_Post', array('slug' => $slug));	

		// Check the URL was actually correct (year and month), redirect if not.
		if ($year != date('Y', $post->date) || $month != date('m', $post->date))
		{
			$this->request->redirect(Route::url('blog_view', array(
				'year' => date('Y', $post->date),
				'month' => date('m', $post->date),
				'slug' => $slug
			)), 301);
		}
		
		$page = View::factory('blog/post')
			->bind('post', $post)
			->set('comments', $post->comments());
			
		$this->template
			->set('title', $post->title)
			->bind('content', $page);
	}
}
?>