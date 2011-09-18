<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Social network posting (Twitter, Facebook, etc.)
 */
abstract class Social
{
	/**
	 * List of all social networks that allow sharing
	 * TODO: This is really ugly. Remove it.
	 */
	private static $can_share_with = array('facebook', 'twitter', 'reddit');
	
	/**
	 * Get all share counts (from all available social networks) for the specified post
	 */
	public static function all_share_counts(Model_Blog_Post $post)
	{
		$result = array();
		
		foreach (self::$can_share_with as $name)
		{
			try
			{
				$network = self::factory($name);
				$result[$name] = $network->share_count($post);
			}
			catch (Exception $e)
			{
				// If an error occured, just set the count to 0 and log it.
				$result[$name] = 0;
				Kohana::$log->add(Log::WARNING, 'Couldn\'t get social share count for ' . $post->url() . ': ' . Kohana_Exception::text($e));
			}
		}
		
		return $result;
	}
	
	/**
	 * Get all share counts (from all available social networks) for the specified post
	 */
	public static function all_share_urls(Model_Blog_Post $post)
	{
		$result = array();
		
		foreach (self::$can_share_with as $name)
		{
			$network = self::factory($name);
			$result[$name] = $network->share_url($post);
		}
		
		return $result;
	}
	
	/**
	 * Create a new instance of the specified social network class
	 * @param	string		Name of the social network
	 * @return Instance of the specified social network
	 */
	public static function factory($name)
	{
		$class_name = 'Social_' . ucfirst($name);
		return new $class_name;
	}
}
?>