<?php defined('SYSPATH') or die('No direct script access.');

class Model_Blog_Comment extends ORM
{
	// Used when loading comments for a post
	public $children;
	
	protected $_belongs_to = array(
		'post' => array(
			'model' => 'Blog_Post'
		),
	);
	
	public function avatar_url()
	{
		$md5 = md5(strtolower(trim($this->email)));
		
		return 'http://www.gravatar.com/avatar/' . $md5 . '?s=' . Kohana::config('blog.gravatar_size') . '&d=monsterid&r=PG';
	}
	
	public function author_link()
	{
		if (!empty($this->url))
			return '<a rel="nofollow" href="' . htmlspecialchars($this->url) . '">' . htmlspecialchars($this->author) . '</a>';
			
		return htmlspecialchars($this->author);
	}
	
	public static function for_post(Model_Blog_Post $post)
	{
		$all_comments = array();
		$root_comments = array();
		
		$comments = $post->comments
			->order_by('date')
			->where('status', '=', 'visible')
			->find_all();
			
		if (Kohana::$profiling === TRUE)
			$benchmark = Profiler::start('Blog', 'Building comments hierarchy');
			
		foreach ($comments as $comment)
		{
			// Add it to the "all comments" array
			$all_comments[$comment->id] = $comment;
			$all_comments[$comment->id]->children = array();
			// Does it have a parent?
			if (!empty($comment->parent_comment_id))
				$all_comments[$comment->parent_comment_id]->children[] = &$all_comments[$comment->id];
			else
				$root_comments[] = &$all_comments[$comment->id];
		}
		
		if (isset($benchmark))
			Profiler::stop($benchmark);
		
		return $root_comments;
	}
	
	public static function count_pending_comments()
	{
		return self::count_comments('pending');
	}
	
	public static function count_spam_comments()
	{
		return self::count_comments('spam');
	}
	
	public static function count_hidden_comments()
	{
		return self::count_comments('spam');
	}
	
	public static function count_comments($status = 'active')
	{
		return DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_comments')
			->where('status', '=', $status)
			->execute()->get('count');
	}
	
	/**
	 * Validation rules
	 */
	public function rules()
	{
		return array(
			'author' => array(
				array('not_empty'),
				array('max_length', array(':value', 255)),
			),
			'email' => array(
				array('not_empty'),
				array('max_length', array(':value', 255)),
				array('email'),
			),
			'url' => array(
				array('url'),
				array('max_length', array(':value', 255)),
			),
			'content' => array(
				array('not_empty'),
				array('min_length', array(':value', 4)),
			),
			
		);
	}
}
?>