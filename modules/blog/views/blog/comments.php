<?php defined('SYSPATH') or die('No direct script access.'); ?>

<ol>
<?php
foreach ($comments as $comment)
{

	// $comment->author_link() is already escaped

	echo '
	<li id="comment-', $comment->id, '">
		<article>
			<header>
				<img class="avatar" src="', $comment->avatar_url(), '" alt="Avatar for " />
				<time datetime="', date(DATE_W3C, $comment->date), '">', date($config->full_date_format, $comment->date), '</time>
				', $comment->author_link(), ' said:
			</header>
			', nl2br(htmlspecialchars($comment->content)), '
			<footer>
				<p><a href="', $post->url(), '?parent_comment_id=', $comment->id, '#leave-comment" class="reply-to">Reply</a></p>
			</footer>
		</article>';
		
	// Any children?
	if (!empty($comment->children))
	{
		echo View::factory('blog/comments')->bind('comments', $comment->children)->bind('post', $post);
	}
	echo '
	</li>
	';
}
?>	
</ol>