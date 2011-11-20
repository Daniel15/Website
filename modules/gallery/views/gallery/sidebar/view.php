<?php defined('SYSPATH') or die('No direct script access.'); 

if (empty($after))
{
	echo 'At the end';	
}
else
{
	echo 'Next: 
<a href="', $after->url(), '"><img src="', $after->thumbnail_url(), '" alt="', $after->title, '" /></a>';
}
?>
