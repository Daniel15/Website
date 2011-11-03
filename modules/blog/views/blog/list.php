<?php defined('SYSPATH') or die('No direct script access.'); 

foreach ($posts as $post)
{
	echo View::factory('blog/post/brief')
		->set('post', $post)
		->set('config', $config)
		// TODO: Remove this call from here and move it to the controller! This is ugly :(
		->set('share_links', Social::all_share_urls($post));
}

echo $pagination;
?>