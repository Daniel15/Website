/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * Feel free to use any of this, but please link back to my site
 */

/**
 * JavaScript used on the website itself
 */
Page.Site = {};

/**
 * The home page
 */
Page.Site.Index = 
{
	/**
	 * Initialise the index page
	 */
	init: function()
	{
		$('email_address').set('innerHTML', '&#100;&#097;&#110;&#105;&#101;&#108;&#049;&#053;&#115;&#105;&#116;&#101;&#064;&#100;&#097;&#110;&#046;&#099;&#120;');
		$('messenger_address').set('innerHTML', '&#x6D;&#x73;&#x6E;&#64;&#100;&#x61;&#110;&#x69;&#x65;&#x6C;&#49;&#53;&#x2E;&#x63;&#111;&#x6D;');
		$('gtalk_address').set('innerHTML', '&#100;&#097;&#110;&#105;&#101;&#108;&#064;&#100;&#049;&#053;&#046;&#098;&#105;&#122;');
		
		// TODO: Tips
		this.socialfeed = new SocialFeed($('minifeed'), 
		{
			count: 10,
			loadOnInit: true
		});
		this.initGoogleTalk();
		// TODO: Hover thingies
	},
	/**
	 * Load Google Talk status via AJAX request
	 */
	initGoogleTalk: function()
	{
		Ajax.load('chatstatus.php', 
		{
			onSuccess: function(data)
			{
				$('gtalk').removeClass('offline').addClass(data.status.toLowerCase());
				$('gtalk_address').set('title', data.status);
				var status = data.status;
				if (data.statusText != data.status)
					status += ' (' + data.statusText + ')';
				$('gtalk_status').set('innerHTML', status);
				
				// If status is not Online, we can't start a new conversation (even Busy!)
				if (data.status == 'Online')
					$('start_gtalk_chat').setStyle('display', 'inline');
			},
			context: this,
			abortPrev: false
		});
		
		// Make the "start conversation" link open in a new window
		$('start_gtalk_chat').addEvent('click', function(e)
		{
			Events.stop(e);
			window.open(this.href, '_blank', 'height=500px,width=300px');
		});
	}
};
