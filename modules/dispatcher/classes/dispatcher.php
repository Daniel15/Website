<?php

/**
 * Dispatcher
 *
 * The main dispatcher class for dispatching events
 * Unlike K2 events are totally isolated from the rest of the system, allowing you
 * to trigger events while handling an event.
 *
 * You can also create individual instances of the dispatcher for private event
 * handling
 *
 * Usage:
 *
 * <code>
 *	$dispatcher = Dispatcher::factory();
 *
 *  $dispatcher->register_listener('project.pre_create', array('class', 'callback');
 *
 *  $dispatcher->trigger_event('project.pre_create' [, array(..arguments..)]);
 * </code>
 *
 * @author Matt Button <matthew@sigswitch.com?
 */
Class Dispatcher Implements Dispatcher_Dispatchable
{
	/**
	 * Gets the global dispatcher instance
	 * 
	 * @return Dispatcher
	 */
	static function instance()
	{
		static $instance = NULL;

		if($instance === NULL)
		{
			$instance = new Dispatcher();
		}

		return $instance;
	}

	/**
	 * Returns a factory instance of Dispatcher
	 * 
	 * @return Dispatcher
	 */
	static function factory()
	{
		return new Dispatcher();
	}

	/**
	 * Factory method for generating event instances
	 *
	 * @param array $arguments
	 * @return Dispatcher_Event
	 */
	static function event(array $arguments = array())
	{
		return new Dispatcher_Event($arguments);
	}

	/**
	 * Listeners for events
	 *
	 * @var array
	 */
	protected $listeners = array();

	/**
	 * Magic PHP Getter
	 *
	 * @param string $var Variable to get
	 */
	public function __get($var)
	{
		return $this->$var;
	}

	/**
	 * Sets up a listener for the $event event
	 *
	 * Callback must be a valid callback, i.e.:
	 *
	 * <ul>
	 *		<li>'function_name'</li>
	 *		<li>array('class', 'static_function)</li>
	 *		<li>array($instance_of_class, 'function')</li>
	 * </ul>
	 *
	 * Callback must also accept exactly 2 parameters:
	 *
	 * <dl>
	 *		<dt>$event_name</dt>
	 *			<dd>The name of the event that it's receiving</dd>
	 *		<dt>$event</dt>
	 *			<dd>The event object</dd>
	 * </dl>
	 * 
	 * @param string $event     Name of event to listen for
	 * @param mixed  $callback  Callback for this event
	 */
	public function register_listener($event, $callback)
	{
		if( ! isset($this->listeners[$event]))
		{
			$this->listeners[$event] = array();
		}

		if(empty($callback))
		{
			throw new Kohana_Exception('Invalid callback provided');
		}

		$this->listeners[$event][] = $callback;
	}

	/**
	 * Triggers event
	 * 
	 * @param  string            $event_name      Event to trigger
	 * @param  Dispatcher_Event  $event           Event Object
	 * @param  boolean           $halt_on_success Stop processing the event if a callback returns TRUE?
	 * @return Dispatcher_Event                   The Event object, after modification
	 */
	public function trigger_event($event_name, Dispatcher_Event $event, $halt_on_success = FALSE)
	{
		if(isset($this->listeners[$event_name]))
		{
			foreach($this->listeners[$event_name] as $callback)
			{
				if(call_user_func($callback, $event_name, $event) AND $halt_on_success)
				{
					break;
				}
			}
		}
		
		return $event;
	}
}