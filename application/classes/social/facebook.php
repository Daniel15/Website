<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for posting blog posts to Twitter
 */
class Social_Facebook extends Social implements Social_Publish, Social_Share
{
	const POST_LENGTH = 300;
	const POST_URL = 'https://graph.facebook.com/me/feed';
	const SHARE_URL = 'https://www.facebook.com/sharer.php';
	const QUERY_URL = 'http://api.facebook.com/method/fql.query';
	const LINK_QUERY = 'SELECT share_count, like_count, comment_count, total_count FROM link_stat WHERE url="%s"';
	
	/**
	 * Publish a new post to Facebook
	 * @param	Model_Blog_Post		The post
	 */
	public function new_post(Model_Blog_Post $post)
	{
		$config = Kohana::$config->load('social.facebook');
		// No configuration?
		if (empty($config) || empty($config['access_token']))
			return;
			
		$message = strip_tags($post->content());
		
		if (strlen($message) > self::POST_LENGTH)
			$message = substr($message, 0, self::POST_LENGTH) . '...';
			
		$data = array(
			'access_token' => $config['access_token'],
			'message' => $post->title, // Next to the user's name
			'link' => $post->url(true),
			'picture' => 'http://example.com/',	// TODO
			'name' => $post->title,
			//'caption' => 'Caption',	// Right under the title
			'description' => $message,
		);
		
		$data = http_build_query($data);
		$context = stream_context_create(array(
			'http' => array(
				'method' => 'POST',
				'header' => implode("\r\n", array(
					'Content-Type: application/x-www-form-urlencoded',
					'Content-Length: ' . strlen($data),
					''
				)),
				'user_agent' => 'Daniel15 Blog FacebookPost +dan.cx',
				'content' => $data,
				'ignore_errors' => true,
			)
		));
		
		return file_get_contents(self::POST_URL, false, $context);
	}
	
	/**
	 * Get the URL to share this post
	 * @param	Model_Blog_Post		The post
	 */
	public function share_url(Model_Blog_Post $post)
	{
		return self::SHARE_URL . '?' . http_build_query(array(
			'u' => $post->url(true),
			't' => $post->title,
		));
	}
	
	/**
	 * Get the number of times this URL has been shared
	 * @param	Model_Blog_Post		The post
	 */
	public function share_count(Model_Blog_Post $post)
	{
		// Get the count using FQL. Returns *both* like count and share count.
		$query = sprintf(self::LINK_QUERY, $post->url(true));
		$url = self::QUERY_URL . '?' . http_build_query(array(
			'query' => $query,
		));
		
		$data = simplexml_load_file($url);
		if (empty($data) || empty($data->link_stat) || empty($data->link_stat->total_count))
			return 0;
			
		return $data->link_stat->total_count;
	}
}
?>