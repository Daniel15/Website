<?php defined('SYSPATH') or die('No direct script access.'); 

echo '
<h2>', $comment_status, ' comments</h2>';

if (count($comments) == 0)
	echo 'There are no comments to show.';
else
{
	echo '
<ul id="comments">';

	foreach ($comments as $comment)
	{
		echo '
	<li id="comment-', $comment->id, '">
		<form action="', Url::site('blogadmin/comments/action/' . $comment->id), '" method="post">
			<strong>Date:</strong> ', date($config->full_date_format, $comment->date), '<br />
			<strong>Author:</strong> ', htmlspecialchars($comment->author), '<br />
			<strong>URL:</strong> ', htmlspecialchars($comment->url), '<br />
			<strong>Email:</strong> ', htmlspecialchars($comment->email), '<br />
			<strong>Content:</strong> ', nl2br(htmlspecialchars($comment->content)), '<br />
			<ul class="actions">';
			
		// Show "Mark as Spam" if it's not already spam
		if ($comment->status != 'spam')
		{
			echo '<li><input type="submit" name="spam" value="Mark as Spam" /></li>';
		}
		else
		{
			echo '<li><input type="submit" name="ham" value="Unmark as spam (and approve)" /></li>';
		}
		
		// Show "Approve" if it's not approved
		if ($comment->status == 'pending' || $comment->status = 'hidden')
		{
			echo '<li><input type="submit" name="approve" value="Approve" /></li>';
		}
		else
		{
			echo '<li><input type="submit" name="unapprove" value="Un-approve" /></li>';
		}
				
		echo '
				<li><input type="submit" name="delete" value="Delete" /></li>
			</ul>
		</form>
	</li>';
	}
	
	echo '
</ul>';
}

echo $pagination;
?>