<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Support for posting blog posts to Twitter
 */
class Social_Twitter extends Social
{
	const TWEET_LENGTH = 115;
	const UPDATE_URL = 'https://twitter.com/statuses/update.json';
	
	public function new_post(Model_Blog_Post $post)
	{
		$config = Kohana::config('social.twitter');
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
}
?>