<?php defined('SYSPATH') or die('No direct script access.'); ?>

<div id="comments">
	<div id="disqus_thread"></div>
</div>

<script type="text/javascript">
	var disqus_shortname = <?php echo json_encode($config->disqus_shortname); ?>,
		disqus_title = <?php echo json_encode($post->title); ?>,
		disqus_identifier = <?php echo $post->id; ?>,
		disqus_url = <?php echo json_encode($post->url(true)); ?>;

    (function() {
		var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
		dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
		(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
    })();
</script>
<noscript>Please enable JavaScript to view the <a href="http://disqus.com/?ref_noscript">comments powered by Disqus.</a></noscript>
<a href="http://disqus.com" class="dsq-brlink">comments powered by <span class="logo-disqus">Disqus</span></a>


<!--<div id="leave-comment">
	<h3>Leave a comment</h3>
	<p id="cancel-reply"><a href="<?php echo $post->url(); ?>">Cancel reply</a></p>
	<form id="leave-comment-form" method="post" action="<?php echo $post->url(); ?>">
		<p>
			<label for="author">Name:</label>
			<input type="text" name="author" id="author" value="<?php echo htmlspecialchars(Arr::get($_POST, 'author')); ?>" size="22" tabindex="1" aria-required="true" />
			<small>(required)</small>
		</p>

		<p>
			<label for="email">E-mail:</label>
			<input type="text" name="email" id="email" value="<?php echo htmlspecialchars(Arr::get($_POST, 'email')); ?>" size="22" tabindex="2" aria-required="true" />
			<small>(required, but will not be published)</small>
		</p>

		<p>
			<label for="url">Website:</label>
			<input type="text" name="url" id="url" value="<?php echo htmlspecialchars(Arr::get($_POST, 'website')); ?>" size="22" tabindex="3" />
		</p>
		<p id="subject-field">
			<label for="subject">Ple&#65279;ase leave this field bla&#65279;nk</label>
			<input type="text" name="subject" id="subject" size="22" tabindex="999" />
		</p>
		
		<p>
			<textarea name="content" id="content" cols="100%" rows="10" tabindex="4"><?php echo htmlspecialchars(Arr::get($_POST, 'content')); ?></textarea>
		</p>
		
		<p>
			<input type="checkbox" name="subscribe" id="subscribe"<?php echo isset($_POST['subscribe']) ? ' checked="checked"' : '' ?>>
			<label for="subscribe">Subscribe to new comments &mdash; You'll get an email every time a new comment is posted</label>
		</p>
		
		<p>
			<input type="hidden" name="parent_comment_id" id="parent_comment_id" value="<?php echo htmlspecialchars(Arr::get($_POST, 'parent_comment_id')); ?>" />
			<input name="submit" type="submit" id="submit" tabindex="5" value="Submit Comment" />
		</p>
	</form>
</div>-->
