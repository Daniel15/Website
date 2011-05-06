<?php defined('SYSPATH') or die('No direct script access.');

class Model_Blog_Post extends ORM
{
	protected $_belongs_to = array(
		'maincategory' => array(
			'model' => 'Blog_Category'
		),
	);
	
	protected $_has_many = array(
		'categories' => array(
			'model' => 'Blog_Category',
			'through' => 'blog_post_categories',
			'foreign_key' => 'post_id',
		),
		'tags' => array(
			'model' => 'Blog_Tag',
			'through' => 'blog_post_tags',
			'foreign_key' => 'post_id',
		),
		'comments' => array(
			'model' => 'Blog_Comment',
			'foreign_key' => 'post_id',
		),
	);
	
	/*public function __get($key)
	{
			//const ACCESSOR_PREFIX = 'get_';
			
		// Check if there's an accessor function for this property (eg. get_url(), get_intro())
		// If not, use the default ORM accessor.
		$accessor = self::ACCESSOR_PREFIX . $key;
		if (method_exists($this, $accessor))
			return $this->$accessor();
	
		return parent::__get($key);
	}*/
	
	/**
	 * Get the URL to the blog post
	 */
	public function url()
	{
		return Route::url('blog_view', array(
			'year' => date('Y', $this->date),
			'month' => date('m', $this->date),
			'slug' => $this->slug,
		));
	}
	
	/**
	 * Get the introduction to the blog post. If there's a "more" tag, this will be up to the more
	 * tag. Otherwise, it'll be the whole post.
	 */
	public function intro()
	{
		$content = $this->content();
		if (($more_pos = strpos($content, '<span id="read-more"></span>')) === false)
			return $content;
			
		// Get the content before the "more" tag
		// TODO: Clean up HTML (unclosed tags?)
		$content = substr($content, 0, $more_pos);
		$content .= View::factory('blog/post/read-more-link')->set('url', $this->url() . '#read-more');
		return $content;
	}
	
	public function content()
	{
		$content = str_replace('<!--more-->', '<span id="read-more"></span>', $this->content);
		// HTML encode everything in <pre> tags
		return preg_replace_callback(
			'~<pre([^>]+)>([\S\s]+?)</pre>~',
			function ($match)
			{
				// If "escaped=true" found then just return
				if (strpos($match[1], 'escaped="true"') !== false)
					return str_replace('escaped="true"', '', $match[0]);
					
				return '<pre' . $match[1] . '>' . htmlspecialchars($match[2]) . '</pre>';
			},
			$content
		);
	}
	
	/**
	 * Get the short URL to this blog post
	 */
	public function short_url()
	{
		return 'TODO';
	}
	
	/**
	 * Get links to all the categories this post is a part of
	 */
	public function category_links()
	{
		$category_links = array();
		$categories = $this->categories
			->order_by('title')
			->find_all();
		
		foreach ($categories as $category)
			$category_links[] = '<a href="' . $category->url() . '">' . $category->title . '</a>';
		
		return implode(', ', $category_links);
	}
	
	public function tag_links()
	{
		$tag_links = array();
		$tags = $this->tags
			->order_by('title')
			->find_all();
		
		foreach ($tags as $tag)
		{
			$tag_links[] = '<a href="' . $tag->url() . '">' . $tag->title . '</a>';
		}
		
		return implode(', ', $tag_links);
	}
	
	public function comments()
	{
		return Model_Blog_Comment::for_post($this);
	}
	
	public static function total_count()
	{
		return DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_posts')
			->execute()->get('count');
	}
	
	public function recalculate_comments()
	{
		$count = DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_comments')
			->where('post_id', '=', $this->id)
			->where('status', '=', 'visible')
			->execute()->get('count');
			
		$this->comment_count = $count;
		$this->save();
	}
}
?>