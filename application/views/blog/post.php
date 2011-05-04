<?php defined('SYSPATH') or die('No direct script access.');

echo View::factory('blog/post/full')->set('post', $post)->set('config', $config);
?>
<div id="comments">
<?php if ($post->comment_count == 0): ?>
	<h3>Comments</h3>
	<p>This post currently has no comments. You can be the first!</p>
<?php else: ?>
	<h3><?php echo $post->comment_count, ' ', Inflector::plural('comment', $post->comment_count); ?></h3>
	<?php echo View::factory('blog/comments')->set('comments', $comments); ?>
<?php endif; ?>
</div>

<div id="leave-comment">
	<h3>Leave a comment</h3>
	<form id="leave-comment-form" method="post" action="">
		<p>
			<label for="author">Name:</label>
			<input type="text" name="author" id="author" value="" size="22" tabindex="1" aria-required="true" />
			<small>(required)</small>
		</p>

		<p>
			<label for="email">E-mail:</label>
			<input type="text" name="email" id="email" value="" size="22" tabindex="2" aria-required="true" />
			<small>(required, but will not be published)</small>
		</p>

		<p>
			<label for="url">Website:</label>
			<input type="text" name="url" id="url" value="" size="22" tabindex="3" />
		</p>
		
		<p>
			<textarea name="comment" id="comment" cols="100%" rows="10" tabindex="4"></textarea>
		</p>
		
		<p>
			<input name="submit" type="submit" id="submit" tabindex="5" value="Submit Comment" />
		</p>
	</form>
</div>