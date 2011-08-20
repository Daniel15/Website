<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Helper class for sending emails
 */
class Email
{
	/**
	 * Send a notification email to the administrator
	 * @param	string		Email subject
	 * @param	string		Message to send
	 */
	public static function admin_notification($subject, $message)
	{
		return self::send(
			Kohana::$config->load('email.from_name'), Kohana::$config->load('email.from'), Kohana::$config->load('email.to'),
			'[' . Kohana::$config->load('blog.name') . '] ' . $subject, $message);
	}
	
	/**
	 * Send a notification email to a user
	 * @param	string		Email address to send to
	 * @param	string		Email subject
	 * @param	string		Message to send
	 */
	public static function notification($to, $subject, $message)
	{
		return self::send(
			Kohana::$config->load('email.from_name'), Kohana::$config->load('email.from'), $to,
			'[' . Kohana::$config->load('blog.name') . '] ' . $subject, $message);
	}
	
	/**
	 * Actually send an email
	 * @param	string		Name to show email as being from
	 * @param	string		Email address to send FROM
	 * @param	string		Email address to send TO
	 * @param	string		Email subject
	 * @param	string		Message to send
	 */
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