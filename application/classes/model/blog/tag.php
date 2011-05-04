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
	
	public function url()
	{
		return Route::url('blog_tag', array(
			'slug' => $this->slug
		));
	}
	
	public function post_count()
	{
		return DB::select(DB::expr('COUNT(*) AS count'))
			->from('blog_post_tags')
			->where('tag_id', '=', $this->id)
			->execute()->get('count');
	}
}
?>