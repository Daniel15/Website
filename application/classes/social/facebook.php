<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for posting blog posts to Twitter
 */
class Social_Facebook extends Social
{
	const POST_LENGTH = 300;
	const POST_URL = 'https://graph.facebook.com/me/feed';
	
	public function new_post(Model_Blog_Post $post)
	{
		$config = Kohana::config('social.facebook');
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
}
?>