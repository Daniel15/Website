<?php defined('SYSPATH') or die('No direct script access.');

class HTML extends Kohana_HTML
{
	public static function link_list($list, $glue = ', ')
	{
		$links = array();
		foreach ($list as $item)
			$links[] = '<a href="' . $item->url() . '">' . $item->title . '</a>';
		
		return implode($glue, $links);
	}
}
?>