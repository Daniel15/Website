<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Class for handling EXIF data included in photos
 * @author Daniel15 <daniel at dan.cx>
 */
class Exif
{
	// Useful EXIF sections - These are what will be used
	private static $sections = array(
		'camera_make' => array('IFD0', 'Make'),
		'camera_model' => array('IFD0', 'Model'),
		'original_date' => array('EXIF', 'DateTimeOriginal'),
		'exposure_time' => array('EXIF', 'ExposureTime'),
		'iso' => array('EXIF', 'ISOSpeedRatings'),
		'aperture' => array('COMPUTED', 'ApertureFNumber'),	
	);

	/**
	 * Get the EXIF data in the specified file
	 * @param    string    File name to get data from
	 * @return   Array of exif data, or null if the image has none
	 */
	public static function get($file)
	{
		if (!($exif = @exif_read_data($file, 'ANY_TAG', true)))
			return null;
			
		$result = array();

		foreach (Exif::$sections as $section_name => $section)
		{
			if (!empty($exif[$section[0]][$section[1]]))
				$result[$section_name] = $exif[$section[0]][$section[1]];
		}

		// If there's a date, parse it
		if (!empty($result['original_date']))
			$result['original_date'] = strtotime($result['original_date'] . ' UTC');
			
		// Format the exposure time
		if (!empty($result['exposure_time']))
			$result['exposure_time'] = Exif::format_number($result['exposure_time']);
			
		return $result;
	}
	
	/**
	 * Parse a number like "33000/1000000" and return 0.033
	 * @param    string    The number to parse
	 * @return   The number
	 */
	private static function format_number($number)
	{
		// Get the fraction.
		if (!preg_match('~([0-9]+)/([0-9]+)~', $number, $matches))	
			// If it can't be done, return the original
			return $number;
		
		return $matches[1] / $matches[2];
	}
}
?>
