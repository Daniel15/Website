<?php


Interface Dispatcher_Dispatchable
{

	/**
	 * Registers a listener
	 *
	 * @param string $event     Event to listen for
	 * @param mixed  $callback  Callback to trigger
	 */
	public function register_listener($event, $callback);

	/**
	 * Triggers an event
	 *
	 * @param string           $event_name       Event to trigger
	 * @param Dispatcher_Event $event            Event object
	 * @param boolean          $halt_on_success  Should the loop stop if a callback returns TRUE
	 */
	public function trigger_event($event_name, Dispatcher_Event $event, $halt_on_success = FALSE);
}