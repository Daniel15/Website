<?php defined('SYSPATH') or die('No direct script access.'); ?>

<ol>
<?php
foreach ($comments as $comment)
{

	// $comment->author_link() is already escaped

	echo '
	<li id="comment-', $comment->id, '" itemprop="comment" itemscope itemtype="http://schema.org/UserComments">
		<article>
			<header>
				<img class="avatar" src="', $comment->avatar_url(), '" alt="Avatar for " itemprop="image" />
				<time datetime="', date(DATE_W3C, $comment->date), '" itemprop="commentTime">', date($config->full_date_format, $comment->date), '</time>
				', $comment->author_link(), ' said:
			</header>
			<div itemprop="commentText">
				', nl2br(htmlspecialchars($comment->content)), '
			</div>
			<footer>
				<p><a href="', $post->url(), '?parent_comment_id=', $comment->id, '#leave-comment" class="reply-to" itemprop="replyToUrl">Reply</a></p>
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
