<?php defined('SYSPATH') or die('No direct script access.');

/**
 * An album in the photo gallery.
 * @author Daniel15 <daniel at dan.cx>
 */
class Model_Gallery_Picture extends Model_Gallery
{	
	protected $_belongs_to = array(
		'album' => array(
			'model' => 'Gallery_Album'
		),
	);
	
	/**
	 * Get the thumbnail image for this picture
	 * @return URL to the thumbnail
	 */
	public function thumbnail_url()
	{
		// TODO: Is this the best place for this? :/
		return $this->album->resized_url() . self::$config->thumbnail_width . '_' . $this->filename;
	}
	
	/**
	 * Get the normal URL image for this picture
	 * @return URL to the image
	 */
	public function normal_url()
	{
		return $this->album->resized_url() . self::$config->normal_width . '_' . $this->filename;
	}
	
	/**
	 * Get the full image URL for this image
	 * @return URL to the image
	 */
	public function full_url()
	{
		return $this->album->full_url() . $this->filename;
	}
	
	/**
	 * Get the URL to the image page
	 * @return The URL
	 */
	public function url()
	{
		return Route::url('gallery_view', array(
			'album' => $this->album->slug,
			'filename' => $this->filename
		));
	}
	
	/**
	 * Get the picture after this one
	 */
	public function after()
	{
		$after = ORM::factory('Gallery_Picture')
			->where('id', '>', $this->id)
			->where('album_id', '=', $this->album)
			->order_by('id')
			->find();
			
		return $after->loaded() ? $after : null;
	}
}
