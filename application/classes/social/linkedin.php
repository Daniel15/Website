<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for sharing posts on LinkedIn.
 */
class Social_Linkedin extends Social implements Social_Share
{
	const SHARE_URL = 'http://www.linkedin.com/shareArticle';
	const API_URL = 'http://www.linkedin.com/countserv/count/share';
	
	/**
	 * Get the URL to share this post
	 * @param	Model_Blog_Post		The post
	 */
	public function share_url(Model_Blog_Post $post)
	{
		return self::SHARE_URL . '?' . http_build_query(array(
			'mini' => 'true',
			'url' => $post->url(true),
			'title' => $post->title,
			'source' => 'Daniel15',
		));
	}
	
	/**
	 * Get the number of times this URL has been shared
	 * @param	Model_Blog_Post		The post
	 */
	public function share_count(Model_Blog_Post $post)
	{
		$url = self::API_URL . '?' . http_build_query(array(
			'url' => $post->url(true),
		));
		
		$data = file_get_contents($url);
		// Ugly hack to get JSON data from the JavaScript method call
		$data = str_replace(array('IN.Tags.Share.handleCount(', ');'), '', $data);
		$data = json_decode($data);
		
		if (empty($data))
			return 0;
			
		return $data->count;
	}
}
?>
