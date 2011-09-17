<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for sharing posts on Reddit.
 */
class Social_Reddit extends Social implements Social_Share
{
	const SHARE_URL = 'http://reddit.com/submit';
	const API_URL = 'http://www.reddit.com/api/info.json';
	
	/**
	 * Get the URL to share this post
	 * @param	Model_Blog_Post		The post
	 */
	public function share_url(Model_Blog_Post $post)
	{
		return self::SHARE_URL . '?' . http_build_query(array(
			'url' => $post->url(true),
			'title' => $post->title,
		));
	}
	
	/**
	 * Get the number of times this URL has been shared
	 * @param	Model_Blog_Post		The post
	 */
	public function share_count(Model_Blog_Post $post)
	{
		$total = 0;
		
		$url = self::API_URL . '?' . http_build_query(array(
			'url' => $post->url(true),
		));
		
		$data = json_decode(file_get_contents($url));
		if (empty($data) || empty($data->data) || empty($data->data->children))
			return 0;
			
		// Need to add up the points in every submission of this URL
		foreach ($data->data->children as $child)
			$total += $child->data->score;
		
		return $total;
	}
}

?>