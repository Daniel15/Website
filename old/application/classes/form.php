<?php defined('SYSPATH') or die('No direct script access.');

class Form extends Kohana_Form
{
	// public static function checkbox($name, $value = NULL, $checked = FALSE, array $attributes = NULL)
	// public static function select($name, array $options = NULL, $selected = NULL, array $attributes = NULL)
	/**
	 * Get a list of checkboxes
	 *
	 * @param   string   Input name
	 * @param   array    Available options
	 * @param   string   Selected option
	 * @param   array    HTML attributes
	 * @return  string   HTML string
	 */
	public static function checkbox_list($name, array $options, array $selected_items = array())
	{
		$x = 0;
		$html = '
	<ul>';
		
		foreach ($options as $value => $text)
		{
			$selected = in_array($value, $selected_items);
			$id = $name . '_' . ++$x;
			$html .= '
		<li>' . self::checkbox($name . '[]', $value, $selected, array('id' => $id)) . ' <label for="' . $id . '">' . $text . '</label></li>';
		}
		
		$html .= '
	</ul>';
	
		return $html;
	}
}
?>