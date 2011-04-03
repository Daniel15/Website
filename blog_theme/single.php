<?php get_header(); ?>

	<?php if (have_posts()) : 
		while (have_posts()) :
			the_post();
			$category = get_the_category();
			$category = $category[0];
		?>

		<div class="navigation">
			<div class="alignleft"><?php previous_post_link('&laquo; %link') ?></div>
			<div class="alignright"><?php next_post_link('%link &raquo;') ?></div>
		</div>

		<div <?php post_class() ?> id="post-<?php the_ID(); ?>">
			<h2><?php the_title(); ?></h2>
			
			<ul class="postmetadata">
				<li class="date"><?php the_time('jS F Y') ?></li>
				<li class="category"><a href="/blog/category/<?php echo $category->slug ?>/"><?php echo $category->name ?></a></li>
				<li class="comments"><?php comments_popup_link('No comments', '1 comment', '% comments'); ?></li>
					<li class="permalink" title="Permanent short link to <?php the_title_attribute(); ?>"><a href="<?php echo wp_get_shortlink(get_the_ID()) ?>"><?php echo wp_get_shortlink(get_the_ID()) ?></a></li>
			</ul>

			<div class="entry">
				<?php the_content('<p class="serif">Read the rest of this entry &raquo;</p>'); ?>

				<?php wp_link_pages(array('before' => '<p><strong>Pages:</strong> ', 'after' => '</p>', 'next_or_number' => 'number')); ?>
				<?php the_tags( '<p class="tags"><img src="/res/icons/tag_blue.png" alt="Tags" title="Tags" /> ', ', ', '</p>'); ?>

				<p class="postmetadata alt">
					<small>
						Short URL for sharing: <strong><?php echo wp_get_shortlink(get_the_ID()) ?></strong>.
						This entry was posted
						<?php /* This is commented, because it requires a little adjusting sometimes.
							You'll need to download this plugin, and follow the instructions:
							http://binarybonsai.com/archives/2004/08/17/time-since-plugin/ */
							/* $entry_datetime = abs(strtotime($post->post_date) - (60*120)); echo time_since($entry_datetime); echo ' ago'; */ ?>
						on <?php the_time('l, F jS, Y') ?> at <?php the_time() ?>
						and is filed under <?php the_category(', ') ?>.

						<?php if (('open' == $post-> comment_status) && ('open' == $post->ping_status)) {
							// Both Comments and Pings are open ?>
							You can <a href="#respond">leave a response</a>, or <a href="<?php trackback_url(); ?>" rel="trackback">trackback</a> from your own site.

						<?php } elseif (!('open' == $post-> comment_status) && ('open' == $post->ping_status)) {
							// Only Pings are Open ?>
							Responses are currently closed, but you can <a href="<?php trackback_url(); ?> " rel="trackback">trackback</a> from your own site.

						<?php } elseif (('open' == $post-> comment_status) && !('open' == $post->ping_status)) {
							// Comments are open, Pings are not ?>
							You can skip to the end and leave a response. Pinging is currently not allowed.

						<?php } elseif (!('open' == $post-> comment_status) && !('open' == $post->ping_status)) {
							// Neither Comments, nor Pings are open ?>
							Both comments and pings are currently closed.

						<?php } edit_post_link('Edit this entry','','.'); ?>

						<a href="<?php bloginfo('rss2_url'); ?>">Subscribe to the RSS feed</a> to keep up-to-date with all my latest blog posts!
					</small>
				</p>

			</div>
		</div>

	<?php comments_template('', true); ?>

	<?php endwhile; else: ?>

		<p>Sorry, no posts matched your criteria.</p>

<?php endif; ?>

<?php get_footer(); ?>
