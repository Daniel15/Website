/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * "Social feed" / lifestream
 * Feel free to use any of this, but please link back to my site
 */
 
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
		
		// TODO: Reuse the one AJAX object
		(new Ajax(this.options.feedurl,
		{
			onSuccess: this.onSuccess,
			context: this,
			data: data
		})).send();
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
				className: 'feeditem source-' + item.type
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
		var children = this.container.get('children');
		var oldest = children[children.length - 1].getAttribute('data-date');
		this.load(oldest);
		
		Events.stop(e);
	}
};