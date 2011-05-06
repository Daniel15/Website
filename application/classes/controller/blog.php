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
			->set('comments', $post->comments())
			->set('categories', $post->categories->order_by('title')->find_all())
			->set('tags', $post->tags->order_by('title')->find_all());
		
		// If the page was POSTed, it's a comment
		if ($_POST)
		{
			$this->comment($post);
		}
			
		$this->template
			->set('title', $post->title)
			->bind('content', $page);
	}
	
	protected function comment($post)
	{
		try
		{	
			$comment = ORM::factory('Blog_Comment');
			$comment->post = $post;
			$comment->author = Arr::get($_POST, 'author');
			$comment->url = Arr::get($_POST, 'url');
			$comment->email = Arr::get($_POST, 'email');
			$comment->content = Arr::get($_POST, 'content');
			$comment->ip = $_SERVER['REMOTE_ADDR'];
			$comment->ip2 = Arr::get($_SERVER, 'HTTP_X_FORWARDED_FOR');
			$comment->date = time();
			$comment->parent_comment_id = Arr::get($_POST, 'parent_comment_id');
			$comment->user_agent = Arr::get($_SERVER, 'HTTP_USER_AGENT');
			$comment->status = 'pending';
			// Check if it's all valid even before the Akismet check, so we don't call Akismet unnecessarily
			$comment->check();
			
			// If we're here, we need to do a spam check
			if ($this->check_for_spam($post))
				// What an evil person! D:
				$comment->status = 'spam';
			else
			{
				// TODO: Send email notification
			}
		
			$comment->save();
			$this->template
				->set('top_message', 'Your comment has been added and will appear on the site as soon as it is approved')
				->set('top_message_type', 'success');
		}
		catch (ORM_Validation_Exception $ex)
		{
			//$page->set('errors', $ex->errors('models'));
			$errors = $ex->errors('models');
			$this->template
				->set('top_message_type', 'error')
				->set('top_message', 'There were some errors with your comment. Please <a href="' . $post->url() . '#leave-comment">correct them</a> and try again:
<ul>
<li>' . implode('</li>
<li>', $errors) . '</li>
</ul>');			
		}
	}
	
	protected function check_for_spam($post)
	{
		return Akismet::factory(array(
			'user_ip' => $_SERVER['REMOTE_ADDR'],
			'permalink' => $post->url(),
			'comment_type' => 'comment',
			'comment_author' => Arr::get($_POST, 'author'),
			'comment_author_email' => Arr::get($_POST, 'email'),
			'comment_author_url' => Arr::get($_POST, 'url'),
			'comment_content' => Arr::get($_POST, 'content')
		))->is_spam();
	}
}
?>