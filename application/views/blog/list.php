<?php defined('SYSPATH') or die('No direct script access.'); 

foreach ($posts as $post)
{
	echo View::factory('blog/post/brief')
		->set('post', $post)
		->set('config', $config);
}

echo $pagination;
?>