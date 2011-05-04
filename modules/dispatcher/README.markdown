# Dispatcher

Inspired by the Symfony [Event Dispatcher Component](http://components.symfony-project.org/event-dispatcher/), this module
allows you to create event driven applications.

It differs from the K2 Event class in a number of ways:

* It's not a singleton, which makes it a lot easier to test the system and create multiple dispatchers
* Event data is encapsulated, allowing you can trigger events within events

## Unit Testing

At time of writing this module has 95.65% code coverage, ensuring the reliability of the API through development.

To run the tests yourself you should install the official [unittest](http://github.com/kohana/unittest) module and 
run the `modules.dispatcher` group.

## Examples

If you look in the unit tests you'll find lots of examples on how it works, but here are a quick few:

	// Here we're using the "global" dispatcher, so anything can add events
	Dispatcher::instance()
		->register_listener('post.pre_publish', array('Misc_Class', 'callback'));

	// Later on...
	Dispatcher::instance()
		// Calls Misc_Class::callback();
		->trigger_event('post.pre_publish', Dispatcher::event(array('argument_name' => 'value'));


Inside the callback:

	Class Misc_Class
	{
		// First argument passed is the name of the triggered event, and the second
		// is a Dispatcher_Event object which encapsulates any arguments
		function callback($event_name, Dispatcher_Event $event)
		{
			// You can access arguments in two different ways:

			// Through __get()
			$var = $event->arguments['argument_name'];

			// Or via ArrayAccess
			$var = $event['argument_name'];
		}
	}

The latter method is reccomended & preferred.