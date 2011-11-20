<?php defined('SYSPATH') or die('No direct script access.'); ?>

<h2><?php echo $title ?></h2>

<ul class="pictures">
<?php
foreach ($pictures as $picture)
{
	echo '
	<li id="picture-', $picture->id, '">
		<a href="', $picture->url(), '">
			<img src="', $picture->thumbnail_url(), '" alt="', $picture->title, '" />
			<p>', $picture->title, '</p>
		</a>
	</li>';
}
?>
</ul>
