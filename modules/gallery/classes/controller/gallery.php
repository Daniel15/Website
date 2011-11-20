<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Main controller for the photo gallery
 * @author Daniel15 <daniel at dan.cx>
 */
class Controller_Gallery extends Controller_Template
{
	public function before()
	{
		parent::before();
		Model_Gallery::$config = $this->config = Kohana::$config->load('gallery');
	}
	
	public function action_index()
	{
	}
	
	/**
	 * Viewing an album
	 */
	public function action_album()
	{
		$slug = $this->request->param('album');

		$album = ORM::factory('Gallery_Album', array('slug' => $slug));
		if (!$album->loaded())
			throw new HTTP_Exception_404('Album ":album" not found.', array(':album' => $slug));
			
		// TODO: Pagination
		
		$page = View::factory('gallery/album')
			->set('title', $album->title)
			->set('pictures', $album->pictures->order_by('id')->find_all())
			->set('config', $this->config);
			
		$this->template
			->set('title', $album->title)
			->bind('content', $page);
	}
	
	/**
	 * Viewing a picture
	 */
	public function action_view()
	{
		$album_slug = $this->request->param('album');
		$filename = $this->request->param('filename');
		
		// Check the album exists
		$album = ORM::factory('Gallery_Album', array('slug' => $album_slug));
		if (!$album->loaded())
			throw new HTTP_Exception_404('Album "' . $album_slug . '" not found.');
			
		// Check the picture exists
		$picture = ORM::factory('Gallery_Picture', array('filename' => $filename, 'album_id' => $album));
		if (!$picture->loaded())
			throw new HTTP_Exception_404('Picture ":picture" in album ":album" not found.', array(':picture' => $filename, ':album' => $album_slug));
			
		$page = View::factory('gallery/view')
			->set('picture', $picture)
			->set('after', $picture->after());
			
		$sidebar = View::factory('gallery/sidebar/view')
			->set('picture', $picture)
			->set('after', $picture->after());
			
		$this->template
			->set('title', $picture->title . ' &mdash; ' . $album->title)
			->set('sidebarType', 'right')
			->bind('sidebar', $sidebar)
			->bind('content', $page);
	}
}
?>
