<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Base controller for all CLI (Command Line Interface) controllers. Ensures that the controller is
 * only accessed via the command line.
 * @author Daniel15 <daniel at dan.cx>
 */
abstract class Controller_Cli extends Controller
{
	/**
	 * Called before the controller action is run
	 */	
	public function before()
	{
		parent::before();
		
		// Ensure the request is done via the command line
		if (!Kohana::$is_cli)
			throw new HTTP_Exception_403('This can only be accessed via the command line.');
			
		// Output buffering is not very useful in CLI mode...
		ob_end_flush();
	}
}
?>
