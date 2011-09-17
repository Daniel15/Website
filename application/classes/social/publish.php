<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Social network that supports automated posting. Used to publish new blog posts.
 * @author Daniel15 <daniel at dan.cx>
 */
interface Social_Publish
{
	/**
	 * Publish a new post to this social network
	 * @param	Model_Blog_Post		The post
	 */
	function new_post(Model_Blog_Post $post);
}
?>