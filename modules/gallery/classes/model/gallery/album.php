<?php defined('SYSPATH') or die('No direct script access.');

/**
 * An album in the photo gallery.
 * @author Daniel15 <daniel at dan.cx>
 */
class Model_Gallery_Album extends Model_Gallery
{
	protected $_has_many = array(
		'pictures' => array(
			'model' => 'Gallery_Picture',
			'foreign_key' => 'album_id',
		),
	);
	
	/**
	 * Get the URL that all resized images should use
	 * @reutrn The URL
	 */
	public function resized_url()
	{
		return self::$config->image_url . self::$config->resized_subdir . $this->slug . '/';
	}
	
	/**
	 * Get the URL that all images live in
	 * @return The URL
	 */
	public function full_url()
	{
		return self::$config->image_url . $this->slug . '/';
	}
}
