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
			$network = self::factory($name);
			$result[$name] = $network->share_count($post);
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