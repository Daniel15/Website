<?php defined('SYSPATH') or die('No direct script access.'); 

echo '
<h2>', $comment_status, ' comments</h2>
<ul id="comments">';
foreach ($comments as $comment)
{
	echo '
	<li id="comment-', $comment->id, '">
		<strong>Date:</strong> ', date($config->full_date_format, $comment->date), '<br />
		<strong>Author:</strong> ', htmlspecialchars($comment->author), '<br />
		<strong>URL:</strong> ', htmlspecialchars($comment->url), '<br />
		<strong>Email:</strong> ', htmlspecialchars($comment->email), '<br />
		<strong>Content:</strong> ', nl2br(htmlspecialchars($comment->content)), '<br />
		<ul class="actions">';
		
		// Show "Mark as Spam" if it's not already spam
		if ($comment->status != 'spam')
		{
			echo '<li><a href="', Url::site('blogadmin/comments/spam/' . $comment->id), '">Mark as Spam</a></li>';
		}
		else
		{
			echo '<li><a href="', Url::site('blogadmin/comments/ham/' . $comment->id), '">Unmark as spam (and approve)</a></li>';
		}
		
		// Show "Approve" if it's not approved
		if ($comment->status == 'pending')
		{
			echo '<li><a href="', Url::site('blogadmin/comments/approve/' . $comment->id), '">Approve</a></li>';
		}
		else
		{
			echo '<li><a href="', Url::site('blogadmin/comments/unapprove/' . $comment->id), '">Un-approve</a></li>';
		}
			
	echo '
			<li><a href="', Url::site('blogadmin/comments/delete/' . $comment->id), '">Delete</a></li>
		</ul>
	</li>';
}

echo '
</ul>';

echo $pagination;
?>