<?php defined('SYSPATH') or die('No direct script access.'); 

echo '
		<header>
			<h2><a href="', $post->url(), '" rel="bookmark" title="Permanent link to ', $post->title, '">', $post->title, '</a></h2>
			<ul class="postmetadata">
				<li class="date"><time datetime="', date(DATE_W3C, $post->date), '">', date($config->date_format, $post->date), '</time></li>
				<li class="category"><a href="', $post->maincategory->url(), '">', $post->maincategory->title, '</a></li>
				<li class="comments"><a href="', $post->url(), '#comments">', Inflector::plural($post->comment_count . ' comment', $post->comment_count), '</a></li>
				<li class="permalink">', $post->short_url(), '</li>
			</ul>
		</header>';	
?>