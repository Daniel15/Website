<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Handles social network sharing links
 * @author Daniel15 <daniel at dan.cx>
 */
class Controller_Social extends Controller
{
	const CACHE_TIME = 1800; // seconds, = 30 minutes
	
	/**
	 * Social network sharing links for a blog post
	 * Example URL: /social/blogpost/333 (where 333 is blog post ID)
	 */
	public function action_blogpost()
	{
		$id = $this->request->param('id');
		$post = ORM::factory('Blog_Post', $id);
		if (!$post->loaded())
			throw new HTTP_Exception_404('Invalid post ID passed to social/blogpost');
		
		// Check if it's cached first!
		$cache = Cache::instance('default');
		$cache_key = 'daniel15-blogshare-' . $post->id;
		if (!Kohana::$config->load('cache.enabled') || !($counts = $cache->get($cache_key)))
		{
			$counts = Social::all_share_counts($post);
			$cache->set($cache_key, $counts, self::CACHE_TIME);
		}
		
		echo json_encode($counts);
	}
}
?>