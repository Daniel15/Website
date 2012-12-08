<?php defined('SYSPATH') or die('No direct script access.');

// Based off http://kohanaframework.org/3.1/guide/kohana/tutorials/error-pages
class ExceptionHandler
{
	public static function handle(Exception $e)
	{
		// If in the development environment, show normal error
		if (Kohana::$environment >= Kohana::TESTING)
			return Kohana_Exception::handler($e);
		
		try
		{
			Kohana::$log->add(Log::ERROR, Kohana_Exception::text($e));
			
			$attributes = array
			(
				'action'  => 500,
				'message' => rawurlencode($e->getMessage())
			);

			if ($e instanceof HTTP_Exception)
			{
				$attributes['action'] = $e->getCode();
			}

			// Error sub-request.
			echo Request::factory(Route::url('error', $attributes))
				->execute()
				->send_headers()
				->body();
		}
		// Exception in the exception handler?!
		catch (Exception $e)
		{
			ob_get_level() and ob_clean();
			echo Kohana_Exception::text($e);
			exit(1);
		}
	}
}
?>