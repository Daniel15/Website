<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Main controller for the command-line interface to the photo gallery
 * @author Daniel15 <daniel at dan.cx>
 */
class Controller_Cli_Gallery extends Controller_Cli
{
	const RESIZE_FILTER = Imagick::FILTER_LANCZOS;
	private static $resize_widths = array(200, 700);
	
	public function before()
	{
		parent::before();
		$this->config = Kohana::$config->load('gallery');
	}
	
	/**
	 * Import pictures into an album specified at the command line.
	 */
	public function action_index()
	{
		$options = CLI::options('album');
		if (empty($options['album']))
		{
			echo "Please specify an album to add via --album=name\n";
			exit(1);
		}
		
		echo 'Checking album... ';
		
		// Check if it exists in the database already
		$album = ORM::factory('Gallery_Album', array('slug' => $options['album']));
		if (!$album->loaded())
		{
			echo 'Not found, creating... ';
			$album->slug = $album->title = $options['album'];
			$album->created_date = time();
			$album->save();
		}
		
		echo 'Album ', $album->id, "\n";
		
		// Actually do the import
		$this->import_pics($album);
	}
	
	/**
	 * Import pictures into an album.
	 * @param    Model_Gallery_Album    Album to import into
	 * @param    String	                File path to import from
	 */
	private function import_pics(Model_Gallery_Album $album)
	{
		$path = $this->config->photo_dir . $album->slug . '/';
		$resized_path = $this->config->photo_dir . Model_Gallery_Picture::RESIZED_SUBDIR . $album->slug . '/';
		
		// Create directory for storing resized versions of photos
		if (!file_exists($resized_path))
			mkdir($resized_path, 0777, true);
		
		echo 'Searching ' . $path . "\n";
		
		// Get all the pictures already in this album
		$existing_pics = $album->pictures->find_all();
		$existing_files = array();
		foreach ($existing_pics as $existing_pic)
		{
			$existing_files[] = $existing_pic->filename;
		}
		
		$files = new DirectoryIterator($path);
		foreach ($files as $file)
		{
			$filename = $file->getFilename();
			// Skip hidden/backup files
			if ($filename[0] === '.' || $filename[strlen($filename)-1] === '~')
				continue;
				
			if ($file->isDir())
			{
				// TODO: Subdirectories - Create a sub-album and import its pics
				echo 'TODO';
			}
			
			// Skip this file if it was already added
			if (in_array($file->getFileName(), $existing_files))
				continue;
			
			$this->import_pic($album, $file, $resized_path);
		}
	}
	
	/**
	 * Import a picture into the album. 
	 * @param    Model_Gallery_Album    Album to import to
	 * @param    SplFileInfo            File to import
	 * @param    String                 Directory to store resized pictures in
	 */
	private function import_pic(Model_Gallery_Album $album, SplFileInfo $file, $resized_path)
	{
		echo $file->getFileName() . ': ';
		
		$image = new IMagick($file->getPathName());
		
		// Create resized versions
		foreach (self::$resize_widths as $width)
		{
			$filename = $resized_path . $width . '_' . $file->getFileName();
			$this->create_resized($image, $width, $filename);
		}
		
		// Add this pic to the database
		$picture = ORM::factory('Gallery_Picture');
		$picture->album = $album;
		$picture->title = $picture->filename = $file->getFileName();
		$picture->width = $image->getImageWidth();
		$picture->height = $image->getImageHeight();
		$picture->upload_date = time();
		$this->add_exif($picture, $file->getPathName());
		$picture->save();
		
		$image->destroy();
		
		echo "Done\n";
	}
	
	/**
	 * Add EXIF information to a picture
	 * @param    Model_Gallery_Picture    The picture to add EXIF data to
	 * @param    string                   The file name to load
	 */
	private function add_exif(Model_Gallery_Picture $picture, $filename)
	{
		// Allowed EXIF fields
		$whitelist = array('camera_make', 'camera_model', 'original_date', 'exposure_time', 'iso', 'aperture');
		
		if (!($exif = Exif::get($filename)))
			return;
		
		foreach ($exif as $name => $value)
		{
			if (in_array($name, $whitelist))
				$picture->$name = $value;
		}
	}
	
	/**
	 * Create a resized version of a picture
	 * @param    IMagick    ImageMagick version of original picture
	 * @param    int        Maximum width of the resized picture
	 * @param    string     File to save picture to
	 */
	private function create_resized(IMagick $original, $max_width, $filename)
	{
		// We don't want to modify the original picture!
		$image = $original->clone();
		
		$image->resizeImage($max_width, 0, self::RESIZE_FILTER, true);
		$image->setImageCompression(Imagick::COMPRESSION_JPEG);
		$image->setImageCompressionQuality(75);
		// Strip out unneeded meta data
		$image->stripImage();
		
		$image->writeImage($filename);
		$image->destroy();
	}
}
?>
