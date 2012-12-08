<?php defined('SYSPATH') or die('No direct script access.');

echo '
	<article id="post-', $post->id, '" itemscope itemtype="http://schema.org/BlogPosting">
', View::factory('blog/post/header')->set('post', $post), '
		', $post->intro(), '
		</div>
		<footer>
', View::factory('blog/post/share-links')->set('post', $post)->set('share_links', $share_links), '
			<a href="', $post->url(), '#comments" itemprop="discussionUrl">', ($post->comment_count == 0 ? 'No comments' : Inflector::plural($post->comment_count . ' Comment', $post->comment_count)), ' &raquo;</a></p>
		</footer>
	</article>';
?>
