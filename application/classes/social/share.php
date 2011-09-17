<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Social network that supports sharing of items. Used to obtain share links for blog posts
 * @author Daniel15 <daniel at dan.cx>
 */
interface Social_Share
{
	/**
	 * Get the URL to share this post
	 * @param	Model_Blog_Post		The post
	 */
	function share_url(Model_Blog_Post $post);
	/**
	 * Get the number of times this URL has been shared
	 * @param	Model_Blog_Post		The post
	 */
	function share_count(Model_Blog_Post $post);
}
?>