<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Helper class for sending emails
 */
class Email
{
	/**
	 * Send an notification email to the administrator
	 */
	public static function admin_notification($subject, $message)
	{
		return self::send(
			Kohana::config('email.from_name'), Kohana::config('email.from'), Kohana::config('email.to'),
			'[' . Kohana::config('blog.name') . '] ' . $subject, $message);
	}
	
	public static function send($from_name, $from, $to, $subject, $message)
	{
		// Wrap at 70 characters
		$message = wordwrap($message, 70);
		
		mail($to, $subject, $message, implode("\r\n", array(
			'From: ' . $from_name . ' <' . $from . '>',
			'X-Mailer: Daniel15/1.0'
		)), '-f' . $from);
	}
}
?>