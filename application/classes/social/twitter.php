<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for posting blog posts to Twitter
 */
class Social_Twitter extends Social implements Social_Publish, Social_Share
{
	const TWEET_LENGTH = 115;
	const UPDATE_URL = 'https://twitter.com/statuses/update.json';
	const SHARE_URL = 'https://twitter.com/intent/tweet';
	const COUNT_URL = 'http://urls.api.twitter.com/1/urls/count.json';
	
	/**
	 * Publish a new post to Twitter
	 * @param	Model_Blog_Post		The post
	 */
	public function new_post(Model_Blog_Post $post)
	{
		$config = Kohana::$config->load('social.twitter');
		// No configuration?
		if (empty($config) || empty($config['consumer_key']))
			return;
			
		$status = '';
		if (strlen($post->title) > 0)
			$status = $post->title . ' - ';
		$status .= strip_tags($post->content());
		
		if (strlen($status) > self::TWEET_LENGTH)
			$status = substr($status, 0, self::TWEET_LENGTH) . '...';
			
		$status .= ' ' . $post->short_url();
		
		$oauth = new OAuth($config['consumer_key'], $config['consumer_secret'], OAUTH_SIG_METHOD_HMACSHA1, OAUTH_AUTH_TYPE_URI);
		$oauth->setToken($config['access_token'], $config['access_token_secret']);
		$oauth->fetch(self::UPDATE_URL, array(
			'status' => $status,
		), OAUTH_HTTP_METHOD_POST, array('User-Agent' => 'Daniel15 Blog TwitterPost'));
	}
	
	/**
	 * Get the URL to share this post
	 * @param	Model_Blog_Post		The post
	 */
	public function share_url(Model_Blog_Post $post)
	{
		return self::SHARE_URL . '?' . http_build_query(array(
			'text' => $post->title,
			'original_referer' => $post->url(true),
			'url' => $post->short_url(true),
			'via' => 'Daniel15',
			'related' => 'Daniel15',
			));
	}
	
	/**
	 * Get the number of times this URL has been shared
	 * @param	Model_Blog_Post		The post
	 */
	public function share_count(Model_Blog_Post $post)
	{
		$url = self::COUNT_URL . '?' . http_build_query(array(
			'url' => $post->url(true),
		));
		$data = json_decode(file_get_contents($url));
		
		if (empty($data) || empty($data->count))
			return 0;
			
		return $data->count;
	}
}
?>