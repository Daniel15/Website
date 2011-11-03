<?php defined('SYSPATH') or die('No direct script access.');

echo View::factory('blog/post/full')
	->set('post', $post)
	->set('categories', $categories)
	->set('tags', $tags)
	->set('share_links', $share_links);
	
echo View::factory('blog/post/comments')
	->set('post', $post)
	->set('comments', $comments);
?>
