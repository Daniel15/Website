<?php defined('SYSPATH') or die('No direct script access.'); ?>

<h2><?php echo $title ?></h2>

<ul class="photos">
<?php
foreach ($pictures as $picture)
{
	echo '
	<li id="picture-', $picture->id, '">
		<a href="', $picture->url(), '">
			<img src="', $picture->thumbnail_url(), '" alt="', $picture->title, '" />
			', $picture->title, '
		</a>
	</li>';
}
?>
</ul>
