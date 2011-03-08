<?php
if (isset($_GET['ver']) && $_GET['ver'] == 2)
{
	require('lib/shortener.php');
	$id = Shortener::alias_to_id($_GET['id']);
}
else
{
	$id = base_convert($_GET['id'], 36, 10);
}

// Include Wordpress 
define('WP_USE_THEMES', false);
require('blog/wp-load.php');

header('HTTP/1.1 301 Moved Permanently');
header('Location: ' . get_permalink($id));
?>