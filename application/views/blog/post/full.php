<?php defined('SYSPATH') or die('No direct script access.'); 

echo '
	<article id="post-', $post->id, '">
', View::factory('blog/post/header')->set('post', $post)->set('config', $config), '
		', $post->content();

// Any tags?
$tag_links = $post->tag_links();
if (!empty($tag_links))
{
	echo '
		<p class="tags"><img src="/res/icons/tag_blue.png" alt="Tags" title="Tags" /> ', $tag_links, '</p>';
}
		
echo '
		<footer>
			Short URL for sharing: ', $post->short_url(), '. This entry was posted on ', date($config->full_date_format, $post->date), ' and is filed under ', $post->category_links(), '. You can <a href="#leave-comment">leave a comment</a> if you\'d like to, or subscribe to the RSS feed to keep up-to-date with all my latest blog posts! 
		</footer>
	</article>';
?>