<?php defined('SYSPATH') or die('No direct script access.'); 

/**
 * Configuration for the gallery
 */
return array(
	// Directory the gallery photos are stored in
	'image_dir' => '/var/www/gallerypics/',
	// URL corresponding to the above directory
	'image_url' => 'http://localhost/gallerypics/',
	// Subdirectory for resized images
	'resized_subdir' => 'resized/',
	
	// Widths
	'thumbnail_width' => 200,
	'normal_width' => 700,
);
?>
