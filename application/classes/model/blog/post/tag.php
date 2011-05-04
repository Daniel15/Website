<?php defined('SYSPATH') or die('No direct script access.');

class Model_Blog_Post_Category extends ORM
{
	protected $_belongs_to = array(
		'post' => array(
		),
		'tag' => array(
		)
	);
}
?>