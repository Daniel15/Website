<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Main controller for the blog
 * @author Daniel15 <daniel at dan.cx>
 * @todo Split all comment-related functionality into a separate controller.
 */
class Controller_Blog extends Controller_Template
{
	protected $config;
	
	/**
	 * Called before any controller action is performed
	 */
	public function before()
	{
		parent::before();
		// Load blog config
		$this->config = Kohana::$config->load('blog');
		$this->template->bind_global('config', $this->config);
		$this->template->is_blog = true;
		$this->template->extraHead .= '
	<link rel="alternate" type="application/rss+xml" title="' . $this->config->name . ' - RSS Feed" href="' . $this->config->feedburner_url . '" />
	<link rel="index" title="' . $this->config->name . '" href="' . Url::site('blog', true) . '" />';
	}
	
	public function after()
	{
		$this->template->sidebarType = 'right';
		
		// Load sidebar from cache, if available
		if (!Kohana::$config->load('cache.enabled') || !($this->template->sidebar = $this->cache->get('daniel15-blog-sidebar')))
		{
			// No cache, so load via HMVC request
			$this->template->sidebar = Request::factory('blog/sidebar')->execute()->body();
			$this->cache->set('daniel15-blog-sidebar', $this->template->sidebar);
		}
		
		parent::after();
	}
	
	/**
	 * Main blog listing
	 */
	public function action_index()
	{		
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		// Get the total count of posts
		$count = Model_Blog_post::count_posts();
		$posts = ORM::factory('Blog_Post');
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($count, $posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . $this->config->name)
			->bind('content', $page);
		
	}
	
	/**
	 * Blog "archive" - Listing for a particular month
	 * @param	int		Year of listing
	 * @param	int		Month of listing
	 */
	public function action_archive()
	{
		$year = $this->request->param('year');
		$month = $this->request->param('month');
		
		$month_name = strftime('%B', mktime(0, 0, 0, $month, 1));
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$posts = Model_Blog_Post::posts_for_month($year, $month);
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing(Model_Blog_Post::count_for_month($year, $month), $posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . $month_name . ' ' . $year . ' &mdash; ' . $this->config->name)
			->bind('content', $page);
	}
	
	/**
	 * Viewing a listing of all posts in a specific category
	 * @param	string	Slug (URL alias) of the category
	 */
	public function action_category()
	{
		$slug = $this->request->param('slug');
		
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$category = ORM::factory('Blog_Category', array('slug' => $slug));
		
		// Throw a 404 if the category doesn't exist
		if (!$category->loaded())
			throw new HTTP_Exception_404('Category "' . $slug . '" not found.');
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($category->post_count(), $category->posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . 'Latest posts in ' . $category->title . ' &mdash; ' . $this->config->name)
			->bind('content', $page);
	}
	
	/**
	 * Viewing a listing of all posts in a specific tag
	 * @param	string	Slug (URL alias) of the tag
	 */
	public function action_tag()
	{
		$slug = $this->request->param('slug');
		
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$tag = ORM::factory('Blog_Tag', array('slug' => $slug));
		
		// Throw a 404 if the tag doesn't exist
		if (!$tag->loaded())
			throw new HTTP_Exception_404('Tag "' . $slug . '" not found.');
		
		$page = View::factory('blog/index')
			->set('posts', $this->listing($tag->post_count(), $tag->posts));
			
		$this->template
			->set('title', ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . 'Latest posts in ' . $tag->title . ' tag &mdash; ' . $this->config->name)
			->bind('content', $page);
	}
	
	/**
	 * Core listing code shared by all blog listing actions
	 * @param	int		Total record count for all posts in this specific category/tag/listing
	 * @param	array	Posts that are currently visible
	 */
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
	
	/**
	 * Viewing a post itself
	 * URL: /blog/[year]/[month]/[slug (URL alias)]
	 */
	public function action_view()
	{
		$year = $this->request->param('year');
		$month = $this->request->param('month');
		$slug = $this->request->param('slug');
		
		$post = ORM::factory('Blog_Post', array('slug' => $slug));
		// Throw a 404 if the post doesn't exist
		if (!$post->loaded())
			throw new HTTP_Exception_404('Blog post "' . $slug . '" not found.');

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
			->set('tags', $post->tags->order_by('title')->find_all())
			->set('share_links', Social::all_share_urls($post));
		
		// If the page was POSTed, it's a comment
		if ($_POST)
		{
			$this->comment($post);
		}
			
		$this->template
			->set('title', $post->title)
			->bind('content', $page);
			
		// Set the meta tags for Facebook
		$this->template->extraHead .= '
	<meta property="og:title" content="' .  htmlspecialchars($post->title) . '" />
	<meta property="og:url" content="' . $post->url(true) . '" />
	<link rel="canonical" href="' . $post->url(true) . '" />
	<link rel="shortlink" href="' . $post->short_url() . '" />';
	}
	
	/**
	 * Short URL redirect
	 * @param	alias		Base-64 numeric post ID
	 */
	public function action_short_url()
	{
		$alias = $this->request->param('alias');
		
		$id = Shortener::alias_to_id($alias);
		$post = ORM::factory('Blog_Post', $id);
		$this->request->redirect($post->url(), 301);
	}
	
	/**
	 * Unsubscribe from comments to a post
	 */
	public function action_unsub()
	{
		$year = $this->request->param('year');
		$month = $this->request->param('month');
		$slug = $this->request->param('slug');
		$email = $this->request->param('email');
		
		// Load the post this subscription is for
		$post = ORM::factory('Blog_Post', array('slug' => $slug));
		if (!$post->loaded())
			throw new HTTP_Exception_404('Post "' . $slug . '" not found.');
			
		// Remove the subscription
		$sub = ORM::factory('Blog_Subscription', array('email' => $email, 'post_id' => $post->id));
		if (!$sub->loaded())
			throw new HTTP_Exception_500('Subscription for email ' . $email . ' on post ' . $slug . ' not found!');
			
		$sub->delete();
		$this->template
			->set('top_message', 'You have been unsubscribed from new comments to this post.')
			->set('top_message_type', 'success');
		return $this->action_view();
	}
	
	/**
	 * Adding a post to a comment
	 * @param	Model_Blog_Post		Post the comment is being added to
	 */
	protected function comment(Model_Blog_Post $post)
	{
		// Simple spambot check - this field is hidden via CSS and clearly labelled as "Do not fill in"
		// for non-visual browsers.
		if (!empty($_POST['subject']))
			die('Not today, spambot.');
			
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
			if (empty($comment->parent_comment_id))
				$comment->parent_comment_id = null;
			$comment->user_agent = Arr::get($_SERVER, 'HTTP_USER_AGENT');
			$comment->status = 'pending';
			// Check if it's all valid even before the Akismet check, so we don't call Akismet unnecessarily
			$comment->check();
			
			// If we're here, we need to do a spam check
			if ($this->check_for_spam($post))
				// What an evil person! D:
				$comment->status = 'spam';
		
			$comment->save();
			
			// Adding comment was successful, do post-processing (if it's not spam)
			if ($comment->status != 'spam')
				$this->handle_new_comment($comment, $post);
			
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
	
	/**
	 * Do all required processing when a new (valid) comment is added
	 * @param	Model_Blog_Comment	The comment
	 * @param	Model_Blog_Post		The post
	 */
	protected function handle_new_comment(Model_Blog_Comment $comment, Model_Blog_Post $post)
	{
		// Send an email notification to the admin
		$email = View::factory('email/admin/new_comment')
			->set('comment', $comment);
		Email::admin_notification('New comment on "' . $post->title . '"', $email);
		
		// Are they not subscribing to replies? We're all done!
		if (!isset($_POST['subscribe']))
			return;
			
		// Make sure they're not already subscribed
		$sub = ORM::factory('Blog_Subscription')
			->where('email', '=', $comment->email)
			->where('post_id', '=', $post->id)
			->find();
		if ($sub->loaded())
			return;
			
		// Add their subscription
		$sub = ORM::factory('Blog_Subscription');
		$sub->post = $post;
		$sub->email = $comment->email;
		$sub->save();
	}
	
	/**
	 * Check if a comment is a spam post, via Akismet
	 * @param	Model_Blog_Post		Post the comment is for
	 */
	protected function check_for_spam(Model_Blog_Post $post)
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