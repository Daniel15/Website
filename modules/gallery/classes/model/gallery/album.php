<?php defined('SYSPATH') or die('No direct script access.');

/**
 * An album in the photo gallery.
 * @author Daniel15 <daniel at dan.cx>
 */
class Model_Gallery_Album extends ORM
{
	protected $_has_many = array(
		'pictures' => array(
			'model' => 'Gallery_Picture',
			'foreign_key' => 'album_id',
		),
	);
}
