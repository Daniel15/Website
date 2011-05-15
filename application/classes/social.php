<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Social network posting (Twitter, Facebook, etc.)
 */
abstract class Social
{
	abstract public function new_post(Model_Blog_Post $post);
}
?>