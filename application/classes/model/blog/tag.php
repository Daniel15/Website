<?php defined('SYSPATH') or die('No direct script access.');

class Model_Blog_Tag extends ORM
{
	protected $_has_many = array(
		'posts' => array(
			'model' => 'Blog_Post',
			'through' => 'blog_post_tags',
			'foreign_key' => 'tag_id',
		),
	);
	
	/**
	 * Get the URL to this tag
	 */
	public function url()
	{
		return Route::url('blog_tag', array(
			'slug' => $this->slug
		));
	}
	
	/**
	 * Get the number of posts tagged with this tag
	 */
	public function post_count()
	{
		return DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_post_tags')
			->where('tag_id', '=', $this->id)
			->execute()->get('count');
	}
	
	/**
	 * Get a list of tags. Returns an associative array of id => tag title
	 */
	public static function aslist()
	{
		$categories = array();
		$rows = ORM::factory('Blog_Tag')
			->order_by('title')
			->find_all();
			
		foreach ($rows as $row)
			$categories[$row->id] = $row->title;
			
		return $categories;
	}
}
?>