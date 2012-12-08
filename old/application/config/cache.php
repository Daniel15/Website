<?php defined('SYSPATH') or die('No direct script access.');
return array
(
	'enabled' => Kohana::$environment < Kohana::TESTING,
	'default' => array
	(
		'driver'             => 'file',
		'cache_dir'          => APPPATH.'cache',
		'default_expire'     => 3600,
	),
	/*'xcache'   => array
	(
		'driver'             => 'xcache',
		'default_expire'     => 3600,
	),*/
);