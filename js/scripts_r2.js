/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * Feel free to use any of this, but please link back to my site
 */
 
/**
 * Object containing all page-specific JavaScript
 */
var Page = {};

/**
 * JavaScript used on every page
 */
Page.Global = 
{
	init: function()
	{
		// TODO: Cheat code
		// TODO: Syntax highlighter
	}
};

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

/**
 * Page showing the lifestream
 */
Page.Site.Socialfeed = 
{
	init: function()
	{
		this.socialfeed = new SocialFeed($('socialfeed'), 
		{
			loadMore: $('loadMore'),
			count: 25,
			showDescription: true,
			loadOnInit: false
		});
	}
};

/**
 * "Social feed" / Lifestream
 */
var SocialFeed = function(container, options)
{
	this.container = $(container);
	this.options = Util.extend(
	{
		feedurl: 'socialfeed/loadjson.php',
		count: 10,
		showDescription: false,
		loadOnInit: false
	}, options);
	
	this.loadMoreButton = this.options.loadMore || null;
	// Attach the Load More button if we have one
	if (this.loadMoreButton)
		this.loadMoreButton.addEvent('click', this.loadMore.bind(this));

	if (this.options.loadOnInit)
		this.load();
};

SocialFeed.prototype =
{
	/**
	 * Load the data for the lifestream
	 */
	load: function(beforeDate)
	{
		if (this.loadMoreButton)
		{
			this.loadMoreButton.set('innerHTML', '<img src="res/icons/spinner.gif" alt="" title="Loading..." /> Loading...');
		}
		
		var data =
		{
			count: this.options.count
		};
		
		if (beforeDate)
			data.before_date = beforeDate;
		
		Ajax.load(this.options.feedurl,
		{
			onSuccess: this.onSuccess,
			context: this,
			abortPrev: false,
			data: data
		});
	},
	
	/**
	 * Called when the AJAX request is successful
	 * @param	Data returned from server
	 */
	onSuccess: function(response)
	{
		// Remove loading indicator
		var loading = this.container.firstByClass('loading');
		if (loading)
			loading.remove();
		
		// Add all the new items
		for (var i = 0, count = response.length; i < count; i++)
		{
			var item = response[i];
			var itemEl = DOM.create('li',
			{
				id: 'feeditem-' + item.id,
				innerHTML: item.text,
				className: 'feeditem source-' + item.type,
			}, true,
			{
				'data-date': item.date
			});
			
			itemEl.prependChild(DOM.create('div', {className: 'icon'}, false));
			
			// Are we showing the description?
			if (this.options.showDescription && item.description)
			{
				itemEl.appendChild(DOM.create('blockquote', {innerHTML: item.description}, false));
			}
			
			var date = new Date(item.date * 1000);
			var meta = DOM.create('ul', {className: 'meta', title: 'Via ' + item.type}, false);
			
			meta.appendChild(DOM.create('li', { innerHTML: item.relativeDate, title: date, 'class': 'date' }, false));
			
			if (item.subtext)
				meta.appendChild(DOM.create('li', { innerHTML: item.subtext, className: 'subtext' }, false));
				
			// Add a link if we have one
			if (item.url)
				meta.appendChild(DOM.create('li', { innerHTML: '<a href="' + item.url + '" target="_blank">View</a>' }, false));
			
			itemEl.appendChild(meta);
			this.container.appendChild(itemEl);
		}
		
		if (this.loadMoreButton)
		{
			this.loadMoreButton.set('innerHTML', 'Show more!');
		}
	},
	
	/**
	 * Called when the "load more" link is clicked
	 */
	loadMore: function(e)
	{
		
		// Get the date of the oldest item
		var children = this.container.element.children;
		var oldest = children[children.length - 1].getAttribute('data-date');
		this.load(oldest);
		
		Events.stop(e);
	}
};