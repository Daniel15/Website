<?php

if (!function_exists('get_header'))
	die('Go away Tobias.');
	
get_header();

?>

	<?php if (have_posts()) : ?>

		<?php
			while (have_posts()) :
				the_post();
				$category = get_the_category();
				$category = $category[0];
		?>

			<div <?php post_class() ?> id="post-<?php the_ID(); ?>">
				<h2><a href="<?php the_permalink() ?>" rel="bookmark" title="Permanent Link to <?php the_title_attribute(); ?>"><?php the_title(); ?></a></h2>
				<ul class="postmetadata">
					<li class="date"><?php the_time('jS F Y') ?></li>
					<li class="category"><a href="/blog/category/<?php echo $category->slug ?>/"><?php echo $category->name ?></a></li>
					<li class="comments"><?php comments_popup_link('No comments', '1 comment', '% comments'); ?></li>
					<li class="permalink" title="Permanent short link to <?php the_title_attribute(); ?>"><a href="<?php echo wp_get_shortlink(get_the_ID()) ?>"><?php echo wp_get_shortlink(get_the_ID()) ?></a></li>
				</ul>

				<div class="entry">
					<?php the_content('Read the rest of this entry &raquo;'); ?>
				</div>

				<p class="postmetadata">
					<span class="category">
						<?php the_tags('<img src="/res/icons/tag_blue.png" alt="Tags" title="Tags" /> ', ', ', ' | '); ?>
						Posted in <?php the_category(', ') ?> | 
					</span>
					<img src="/res/icons/comments.png" alt="Comments" title="Comments" /> <?php comments_popup_link('No Comments &#187;', '1 Comment &#187;', '% Comments &#187;'); ?>
				</p>
			</div>

		<?php endwhile; ?>

		<div class="navigation">
			<div class="alignleft"><?php next_posts_link('&laquo; Older Entries') ?></div>
			<div class="alignright"><?php previous_posts_link('Newer Entries &raquo;') ?></div>
		</div>

	<?php else : ?>

		<h2 class="center">Not Found</h2>
		<p class="center">Sorry, but you are looking for something that isn't here.</p>
		<?php get_search_form(); ?>

	<?php endif; ?>



<?php get_footer(); ?>


