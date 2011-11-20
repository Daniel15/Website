<?php defined('SYSPATH') or die('No direct script access.');

/**
 * An album in the photo gallery.
 * @author Daniel15 <daniel at dan.cx>
 */
class Model_Gallery_Picture extends ORM
{
	const RESIZED_SUBDIR = 'resized/';
	
	protected $_belongs_to = array(
		'album' => array(
			'model' => 'Gallery_Album'
		),
	);
}
