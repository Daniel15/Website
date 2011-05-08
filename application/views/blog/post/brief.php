<?php defined('SYSPATH') or die('No direct script access.');

echo '
	<article id="post-', $post->id, '">
', View::factory('blog/post/header')->set('post', $post), '
		', $post->intro(), '
		<footer>
', View::factory('blog/post/share-links')->set('post', $post), '
			<a href="', $post->url(), '#comments">', ($post->comment_count == 0 ? 'No comments' : Inflector::plural($post->comment_count . ' Comment', $post->comment_count)), ' &raquo;</a></p>
		</footer>
	</article>';
?>