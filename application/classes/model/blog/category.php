<?php defined('SYSPATH') or die('No direct script access.');

class Model_Blog_Category extends ORM
{
	protected $_has_many = array(
		'posts' => array(
			'model' => 'Blog_Post',
			'through' => 'blog_post_categories',
			'foreign_key' => 'category_id',
		),
	);

	/**
	 * Get the URL for this category
	 */
	public function url()
	{
		return Route::url('blog_category', array(
			'slug' => $this->slug
		));
	}
	
	/**
	 * Get the number of posts in this category
	 */
	public function post_count()
	{
		return DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_post_categories')
			->where('category_id', '=', $this->id)
			->execute()->get('count');
	}
	
	/**
	 * Get a list of categories. Returns an associative array of id => category
	 */
	public static function aslist()
	{
		$categories = array();
		$rows = ORM::factory('Blog_Category')
			->order_by('title')
			->find_all();
			
		foreach ($rows as $row)
			$categories[$row->id] = $row->title;
			
		return $categories;
	}
}
?>