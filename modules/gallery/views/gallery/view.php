<?php defined('SYSPATH') or die('No direct script access.'); 

echo '

<h2>', $picture->title, '</h2>
<a href="', $picture->full_url(), '"><img src="', $picture->normal_url(), '" alt="', $picture->title, '" /></a>';
?>
