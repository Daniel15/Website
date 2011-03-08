/**
 * dan.cx JavaScript - By Daniel15, 2010
 * Feel free to use any of this. I'd like you to link to my site, but it's not needed if you don't want to :)
 */
 
/**
 * Stuff used on every page
 */
var Page = 
{
	init: function()
	{
		// Inject the spans used for the gradient over the headings
		$$('h2, h3').each(function(heading)
		{
			// Check if this heading has a link. The span has to be *inside* the link.
			var link = heading.getElement('a');
			if (link)
				link.grab(new Element('span'), 'top');
			else
				heading.grab(new Element('span'), 'top');
		});
	}
};

window.addEvent('domready', Page.init);
 
/**
 * JavaScript for the Projects page
 */
var Projects =
{
	/**
	 * Initialise the page. Called on domready
	 */
	init: function()
	{
		var techs = $$('ul.projects li a.tech');
		Projects.tips = new Tips(techs);		
		// These should open in a new tab, but "target" is invalid XHTML. I dunno
		// if this is "legal" or not. :P
		techs.set('target', '_blank');
		
		// Attach all the sidebar click listeners
		$$('div#sidebar li a').addEvent('click', Projects.show_tech);
	},
	
	/**
	 * Currently selected techology
	 */
	current_tech: null,
	
	/**
	 * Click handler for technologies sidebar on Projects page
	 */
	show_tech: function()
	{
		// Get this tech, and the projects using it
		var parent = this.getParent();
		var tech = parent.id.substring(5);
		var tech_friendly = this.get('text');
		// If they're clicking the currently selected one, just unselect.
		if (Projects.current_tech == parent)
		{
			Projects.current_tech = null;
			parent.removeClass('active');
			$('intro').setStyle('display', '');
			$('tech-info').setStyle('display', '');
			// Show them all
			$$('ul.projects li').setStyle('display', '').addClass('visible');
			$('active_projects').setStyle('display', '');
			return false;
		}
		
		var projects = $$('ul.projects li.uses-' + tech);
		// Mark this tech as active
		if (Projects.current_tech != null)
			Projects.current_tech.removeClass('active');

		Projects.current_tech = parent;
		parent.addClass('active');
		
		// Show the tech header and some info
		$('intro').setStyle('display', 'none');
		var info = $('tech-info');
		info.setStyle('display', 'block');
		info.getElement('h2').set('html', 'About ' + tech_friendly);
		$('tech-count').set('html', projects.length);
		// tech_descs is defined in cms/pages/projects.php towards the end
		info.getElement('div').set('html', tech_descs[tech]);
		
		// Hide all the projects. The "visible" class is just used by the very last bit that
		// hides/shows "Active projects"
		$$('ul.projects li').setStyle('display', 'none').removeClass('visible');
		// Show projects using this technology
		projects.setStyle('display', '').addClass('visible');
		
		// Show or hide the "active projects" section depending on if it actually contains anything.
		var active_projects = $('active_projects');
		active_projects.setStyle('display', $('active_projects').getElement('li.visible') ? '' : 'none');

		return false;
	}
};

/**
 * JS for the Home page
 * TODO: 
 *  - Convert this to use a class for the sidebar stuff, as currently the code is duplicated
 *  - Abstract the common stuff out to classes
 */
var Home = 
{
	/**
	 * Initialise the page. Called on domready
	 */
	init: function()
	{
		Home.tips = new Tips('li.social a', {fixed: true});
		// Make the "start conversation" link open in a new window
		$('start_convo').addEvent('click', function()
		{
			window.open(this.href, '_blank', 'height=400px,width=300px');
			// Cancel the event
			return false;
		});
		// Get our awesome stuff
		Home.Twitter.get();
		Home.LastFm.get();
		Home.WLM.get();
	},	
	
	/**
	 * Twitter stuff for the homepage
	 */
	Twitter: 
	{
		// Twitter username
		username: 'Daniel15',
		// Number of tweets to get
		count: 6,
		/**
		 * Get the latest tweets for the user
		 */	
		get: function()
		{
			new Request.JSONP({
				// TODO: URL should be a "constant" variable, not hard-coded?
				url: 'http://twitter.com/status/user_timeline/' + Home.Twitter.username + '.json',
				data: {
					'count': Home.Twitter.count
				},
				onComplete: Home.Twitter.callback
			}).send();
		},
		
		/**
		 * Called when data is returned from Twitter
		 */
		callback: function(data)
		{
			var tweets = $('tweets');
			// Delete the "loading" tweet
			tweets.getElement('li.loading').dispose();
			
			data.each(function(item)
			{
				var tweet = new Element('li',
				{
					html: item.text.linkify().linkifyTwitter() + '<br />',
					// Do our little hover things
					events:
					{
						'mouseover': function()
						{
							this.getElement('span').tween('height', null, 20);
						},
						'mouseout': function()
						{
							this.getElement('span').tween('height', null, 0);
						}
					}
				}).inject(tweets);
				// Username link
				new Element('a', {'href': 'http://www.twitter.com/' + Home.Twitter.username + '/status/' + item.id, 'html': Home.Twitter.username + ': '}).inject(tweet, 'top');
				// Date
				var date = Date.parse(item.created_at);
				new Element('span',
				{
					html: date.timeDiffInWords() + ' via ' + item.source,
					title: date
				}).inject(tweet);
			});
		}
	},
	
	/**
	 * Last.fm stuff
	 */
	LastFm:
	{
		// Last.fm username
		username: 'daniel_1515',
		// Number of items to retrieve
		count: 10,
		/**
		 * Get the last last.fm songs for this user
		 */
		get: function()
		{
			new Request.JSONP({
				// TODO: Should not be hard coded, I guess?
				url: 'http://ws.audioscrobbler.com/2.0/',
				data: {
					// TODO: Change this from Daniel15's key if you use this on your own site!
					'api_key': 'bde07c17f9a125f564e9fc369af79e49',
					'method': 'user.getRecentTracks',
					'format': 'json',
					'user': Home.LastFm.username,
					'count': Home.LastFm.count
				},
				onComplete: Home.LastFm.callback
			}).send();
		},
		/**
		 * Called when data is returned from last.fm
		 */
		callback: function(data)
		{
			var lastfm = $('lastfm');
			lastfm.getElement('li.loading').dispose();
			
			data.recenttracks.track.each(function(item)
			{
				var song = new Element('li',
				{
					html: ' by ' + item.artist['#text'] + '<br />',
					events:
					{
						'mouseover': function()
						{
							this.getElement('span').tween('height', null, 20);
						},
						'mouseout': function()
						{
							this.getElement('span').tween('height', null, 0);
						}
					}
				}).inject(lastfm);
				new Element('a', {'href': item.url, 'html': item.name}).inject(song, 'top');
				// Artist name
				//new Element('span', {'html': item.artist['#text']}).inject(song);
				
				// Date
				// If we don't have a date, we're listening now.
				var date;
				if ($defined(item.date))
				{
					var playDate = new Date();
					playDate.setTime(item.date.uts * 1000);
					date = playDate.timeDiffInWords();
				}
				else
				{
					date = 'Listening right now';
				}
				
				new Element('span', {'html': date}).inject(song);
				//new Element('span', {'html': Date.parse(item.date['#text'] + ' GMT').timeDiffInWords()}).inject(song);
			});
		}
	},
	
	/**
	 * Windows Live Messenger stuff
	 */
	WLM:
	{
		// User ID
		id: '135148d074926a0d@apps.messenger.live.com',
		/**
		 * Get the user's WLM status
		 */
		get: function()
		{
			new Request.JSONP({
				url: 'http://messenger.services.live.com/users/' + Home.WLM.id + '/presence',
				//callbackKey: 'cb',
				data: {
					'dt': '',
					'mkt': 'en-AU',
					// Ugly hack below
					'cb': 'wlm_cb'
				},
				onComplete: Home.WLM.callback
			}).send();
		},
		/**
		 * called when WLM returns data
		 */
		callback: function(data)
		{
			$('status').set('html', data.statusText);
			$('status_img').set('src', data.icon.url);
			$('display_name').set('html', data.displayName);
			// Hide the "start conversation" link if offline
			if (data.status == 'Offline')
				$('start_convo').setStyle('display', 'none');
		}
	}
}

/**
 * This is an ugly hack for WLM - It seems noobish and doesn't allow dots in the
 * callback function name >___<. Only Microsoft would be stupid and do that...
 */
function wlm_cb(data)
{
	Home.WLM.callback(data);
}

// Stuff for Twitter
String.implement(
{
	linkify: function()
	{
		return this.replace(/(http:\/\/\S+)/gi, '<a href="$1">$1</a>');
	},
	
	linkifyTwitter: function()
	{
		return this.replace(/@([A-Z0-9]+)/gi, '<a href="http://www.twitter.com/$1" title="Twitter profile for $1">@$1</a>').replace(/#(\S+)/g, '<a href="http://search.twitter.com/search?q=%23$1" title="Twitter hashtag \'$1\'">#$1</a>');
	}
});