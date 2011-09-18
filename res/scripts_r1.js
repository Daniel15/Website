/**
 * dan.cx JavaScript (revision 1) - By Daniel15, 2010
 * Feel free to use any of this, but please link back to my site
 */
 
/*
 * D15 - Daniel15 framework stuff :-)
 */
 
// Various string functions
String.implement(
{
	/**
	 * Uppercase the first letter of the string
	 */
	ucfirst: function()
	{
		return this.charAt(0).toUpperCase() + this.slice(1);
	}
});

var D15 = D15 || {};
/**
 * Onload handler for Daniel15 framework. Calls onload functions depending on the current page.
 * Splits the body ID by hyphen (-), uses first piece as the main object, and other pieces as 
 * sub-objects. Non-existant init methods are ignored (no error is thrown).
 *
 * Example: An ID of "site-projects" would call Site.init() and Site.Projects.init().
 * site-projects-foo would call Site.init(), Site.Projects.init() and Site.Foo.init().
 * "blog" would call Blog.init().
 *
 */
D15.onload = function()
{
	var id_pieces = document.body.id.split('-');
	// First piece is the main object
	var obj_name = id_pieces.shift().ucfirst();
	var obj = Page[obj_name];
	
	// If we don't have the object, just die
	if (obj === undefined)
		return;
	
	// Call the object initialisation function
	obj.init && obj.init();
	// Go through each piece, and call the initialisation on it.
	id_pieces.each(function(piece)
	{
		piece = piece.ucfirst();
		obj[piece] && obj[piece].init && obj[piece].init();
	});
};

////////////////////////////////////////////////////////////////////////////////
// Begin website stuff

/**
 * JavaScript used on every page
 */
var Global =
{
	init: function()
	{
		// Inject the spans used for the gradient over the headings
		// TODO: Use psudeo-elements instead of this.
		$$('h2, h3').each(function(heading)
		{
			// Check if this heading has a link. The span has to be *inside* the link.
			var link = heading.getElement('a');
			if (link)
				link.grab(new Element('span'), 'top');
			else
				heading.grab(new Element('span'), 'top');
		});
		
		CheatCode.init();
		// If we've got SyntaxHighlighter, initialise it
		if (SyntaxHighlighter && SyntaxHighlighter.all)
		{
			SyntaxHighlighter.defaults['toolbar'] = false;
			//SyntaxHighlighter.all();
			// .delay() to run in another "thread"
			SyntaxHighlighter.highlight.bind(SyntaxHighlighter).delay(0);
		}
	}
};

/**
 * Object containing all page-specific JavaScript
 */
var Page = {};

/**
 * JavaScript used on the website itself
 */
Page.Site = {};

/**
 * JS for the Home page
 * TODO: 
 *  - Abstract the common stuff out to classes
 */
Page.Site.Index = 
{
	/**
	 * Initialise the homepage
	 */
	init: function()
	{
		// Put the email addresses into the spans. This is done here for spam prevention.
		$('email_address').set('html', '&#100;&#097;&#110;&#105;&#101;&#108;&#049;&#053;&#115;&#105;&#116;&#101;&#064;&#100;&#097;&#110;&#046;&#099;&#120;');
		$('messenger_address').set('html', '&#x6D;&#x73;&#x6E;&#64;&#100;&#x61;&#110;&#x69;&#x65;&#x6C;&#49;&#53;&#x2E;&#x63;&#111;&#x6D;');
		$('gtalk_address').set('html', '&#100;&#097;&#110;&#105;&#101;&#108;&#064;&#100;&#049;&#053;&#046;&#098;&#105;&#122;');
		
		this.tips = new Tips('li.social a', {fixed: true});
		// Make the "start conversation" link open in a new window
		// Get our awesome stuff
		this.socialfeed = new SocialFeed($('minifeed'), 
		{
			count: 10,
			loadOnInit: true
		});
		this.initGoogleTalk();
		
		// Add the social feed hover thingies						
		$('sidebar').addEvents(
		{
			'mouseover:relay(ul#minifeed > li)': function(e, el)
			{
				el.getElement('ul.meta').tween('height', null, 20);
			},
			'mouseout:relay(ul#minifeed > li)': function(e, el)
			{
				el.getElement('ul.meta').tween('height', null, 0);
			}
		});
	},
	
	initGoogleTalk: function()
	{
		new Request.JSON({
			url: 'chatstatus.php',
			onRequest: function()
			{
				// TODO: Loading indicator
			},
			onSuccess: function(data)
			{
				$('gtalk').removeClass('offline').addClass(data.status.toLowerCase());
				$('gtalk_address').set('title', data.status);
				
				var status = data.status;
				if (data.statusText != data.status)
					status += ' (' + data.statusText + ')';
				$('gtalk_status').set('html', status);
				
				// If status is not Online, we can't start a new conversation (even Busy!)
				if (data.status == 'Online')
					$('start_gtalk_chat').setStyle('display', 'inline');
				
			}
		}).send();
		
		// Make the "start conversation" link open in a new window
		$('start_gtalk_chat').addEvent('click', function()
		{
			window.open(this.href, '_blank', 'height=500px,width=300px');
			// Cancel the event
			return false;
		});
	}
};

/**
 * Projects page
 */
Page.Site.Projects = 
{
	/**
	 * Initialise the page. Called on domready
	 */
	init: function()
	{
		var techs = $$('ul.projects li a.tech');
		this.tips = new Tips(techs);		
		// These should open in a new tab, but "target" is invalid XHTML. I dunno
		// if this is "legal" or not. :P
		techs.set('target', '_blank');
		
		// Attach all the sidebar click listeners
		$$('#sidebar li a').addEvent('click', this.show_tech.bind(this));
	},
	
	/**
	 * Currently selected techology
	 */
	current_tech: null,
	
	/**
	 * Click handler for technologies sidebar on Projects page
	 */
	show_tech: function(e)
	{
		// Get this tech, and the projects using it
		var parent = e.target.getParent();
		var tech = parent.id.substring(5);
		var tech_friendly = e.target.get('text');
		// If they're clicking the currently selected one, just unselect.
		if (Page.Site.Projects.current_tech == parent)
		{
			Page.Site.Projects.current_tech = null;
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
		if (this.current_tech != null)
			this.current_tech.removeClass('active');

		this.current_tech = parent;
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
 * JS for the blog
 */
Page.Blog = 
{
	posts: [],
	init: function()
	{
		this.initSidebar();
		this.initPosts();
	},
	
	initSidebar: function()
	{
		var archives = $('sidebar-archives');
		if (!archives)
			return;
			
		var years = archives.getElements('> li');
		years.each(function(year)
		{
			var inner = year.getElement('ul');
			// Store the height for later
			var height = inner.getHeight();
			inner.store('height', height);
			inner.setStyle('height', '0');
			
			year.addEvent('click', this.toggleYear.bind(this));
		}, this);
		
	},
	
	toggleYear: function(e)
	{
		var inner = $(e.target).getParent().getElement('ul');
		inner.tween('height', null, inner.getHeight() == '0' ? inner.retrieve('height') : 0);
		return false;
	},
	
	initPosts: function()
	{
		$$('#content > article').each(function(post)
		{
			this.posts.push(new Blog.Post(post));
		}, this);
	}
};

Page.Blog.View = 
{
	placeholderFields: ['author', 'email', 'url', 'subject'],
	
	init: function()
	{	
		// Attach "reply to comment" links
		$$('#comments .reply-to').addEvent('click', this.replyToComment.bind(this));
		$('cancel-reply').addEvent('click', this.cancelReply.bind(this));
		
		// Remembering of comment user details
		this.storage = new LocalStorage();
		$('leave-comment-form').addEvent('submit', this.saveCommentDetails.bind(this));
		this.loadCommentDetails();
		
		// Do the placeholders
		this.initPlaceholders();
	},
	
	initPlaceholders: function()
	{
		var comment_form = $('leave-comment-form');
		this.placeholderFields.each(function(field_name)
		{
			var field = $(field_name);				
			var label = comment_form.getElement('label[for=' + field_name + ']');
			if (!field || !label)
				return;
				
			var label_value = label.get('html').replace(':', '');
			// Check if there's any extra info after the field
			var extra_info = $$('#' + field_name + ' + small');
			if (extra_info && extra_info[0])
				label_value += ' ' + extra_info[0].get('html');
			
			field.addEvent('focus', this.fieldFocus);
			field.addEvent('blur', this.fieldBlur);
			field.store('placeholder', label_value);
			label.setStyle('display', 'none');
			
			this.fieldBlur.apply(field);
		}, this);
		// On submit, remove placeholders
		comment_form.addEvent('submit', this.removePlaceholders.bind(this));
		this.checkForPlaceholderText()
	},
	
	/**
	 * Save details about the commenter (name, email, url) into local storage
	 */
	saveCommentDetails: function()
	{
		try
		{
			this.storage.set('comment-info',
			{
				'author': $('author').value,
				'email': $('email').value,
				'url': $('url').value
			});
		}
		catch (ex) {}
		
	},
	
	/**
	 * Retrieve the commenter details from local storage
	 */
	loadCommentDetails: function()
	{
		try
		{
			var info = this.storage.get('comment-info');
			if (!info)
				return;
			
			$('author').value = $('author').value || info.author || '';
			$('email').value = $('email').value || info.email || '';
			$('url').value = $('url').value || info.url || '';
		}
		catch (ex) {}
	},
	
	replyToComment: function(e)
	{
		// Find the footer and stick the comments form in it
		// IE didn't seem to like getParent('footer') for some reason
		//var comment_footer = $(e.target).getParent('footer');
		var comment_footer = $(e.target).getParent().getParent().getParent();
		var comment_id = $(e.target).getParent('li').id.split('-')[1];
		var comment_form = $('leave-comment');
		comment_footer.grab(comment_form, 'bottom');
		$('cancel-reply').setStyle('display', 'block');
		$('parent_comment_id').value = comment_id;
		return false;
	},
	
	cancelReply: function()
	{
		// Put the comment form back where it belongs
		$('content').grab('leave-comment');
		$('cancel-reply').setStyle('display', 'none');
		$('parent_comment_id').value = '';
		return false;
	},
	
	fieldFocus: function()
	{
		// If we're showing the placeholder, hide it
		if (this.value == this.retrieve('placeholder'))
		{
			this.value = '';
			this.removeClass('placeholder');
		}
	},
	
	fieldBlur: function()
	{
		// If the field is blank, show the placeholder
		if (this.value == '')
		{
			this.value = this.retrieve('placeholder');
			this.addClass('placeholder');
		}
		else
			this.removeClass('placeholder');
	},
	
	removePlaceholders: function()
	{
		Page.Blog.View.placeholderFields.each(function(field_name)
		{
			var field = $(field_name);
			if (field.value == field.retrieve('placeholder'))
			{
				field.value = '';
			}
		});
	},
	
	checkForPlaceholderText: function()
	{
		this.placeholderFields.each(function(field_name)
		{
			var field = $(field_name);
			if (field.value == field.retrieve('placeholder'))
			{
				field.addClass('placeholder');
			}
		});
	}
}

/**
 * Classes used in the blog
 */
var Blog = {};

/**
 * Represents a single post in the blog. Handles the social network sharing links
 */
Blog.Post = new Class(
{
	/**
	 * Initialise the blog post. Load all the social network sharing links and counts
	 */
	initialize: function(post)
	{
		this.post = post;
		this.id = this.post.id.slice(5);
		var socialCountUrl = 'social/blogpost/' + this.id;
		
		// Set all social network counts to "..." while they load
		this.socialNetworks = this.post.getElements('.share ul');
		this.socialNetworks.getElements('li').each(function(socialNetwork)
		{
			socialNetwork.getElement('.count').set('html', '&hellip;');
		});
		
		this.addPopupHandler('facebook', 500, 400);
		this.addPopupHandler('twitter', 550, 420);
		
		// Initialise data for network sharing links
		new Request.JSON(
		{
			method: 'get',
			url: socialCountUrl,
			onSuccess: this.updateSocialCounts.bind(this)
		}).send();
	},
	
	addPopupHandler: function(name, width, height)
	{
		this.socialNetworks.getElement('li.' + name + ' a').addEvent('click', function()
		{
			// "this" refers to the link here!
			window.open(this.href, '_blank', 'height=' + height + 'px,width=' + width + 'px');
			return false;
		});
	},
	
	/**
	 * Update the social network sharing counts
	 */
	updateSocialCounts: function(data)
	{
		$H(data).each(function(count, name)
		{
			this.socialNetworks.getElement('li.' + name + ' .count').set('html', count);
		}, this);
	}
});

/**
 * Social feed
 */
var SocialFeed = new Class(
{
	Implements: [Options, Events],
	options:
	{
		feedurl: 'socialfeed/loadjson.php',
		count: 10,
		showDescription: false,
		loadOnInit: false
	},
	
	initialize: function(container, options)
	{
		this.setOptions(options);
		this.container = $(container);
		this.loadMoreButton = this.options.loadMore || null;
		
		// Attach the Load More button if we have one
		if (this.loadMoreButton)
		{
			this.loadMoreButton.addEvent('click', this.loadMore.bind(this));
		}
		
		// Start it up!
		this.request = new Request.JSON(
		{
			url: this.options.feedurl,
			data:
			{
				count: this.options.count
			},
			onRequest: this.onRequest.bind(this),
			onSuccess: this.onSuccess.bind(this)
		});
		
		if (this.options.loadOnInit)
			this.request.send();
	},
	
	onRequest: function()
	{
		if (this.loadMoreButton)
		{
			this.loadMoreButton.set('html', '<img src="res/icons/spinner.gif" alt="" title="Loading..." /> Loading...');
		}
	},
	
	onSuccess: function(response)
	{
		var loading = this.container.getElement('li.loading')
		if (loading)
			loading.dispose();
			
		response.each(function(item)
		{
			var itemEl = new Element('li',
			{
				id: 'feeditem-' + item.id,
				html: item.text,
				'class': 'feeditem source-' + item.type,
				'data-date': item.date
			});
			
			//itemEl.store('date', item.date);
			
			new Element('div', {'class': 'icon'}).inject(itemEl, 'top');
			
			// Are we showing the description?
			if (this.options.showDescription && item.description)
			{
				(new Element('blockquote', {html: item.description})).inject(itemEl);
			}
			
			var date = new Date(item.date * 1000);
			var meta = new Element('ul', {'class': 'meta', title: 'Via ' + item.type.ucfirst()});
			
			new Element('li', { html: date.timeDiffInWords(), title: date, 'class': 'date' }).inject(meta);
			if (item.subtext)
				new Element('li', { html: item.subtext, 'class': 'subtext' }).inject(meta);
			// Add a link if we have one
			if (item.url)
				new Element('li', { html: '<a href="' + item.url + '" target="_blank">View</a>' }).inject(meta);
			
			meta.inject(itemEl);			
			itemEl.inject(this.container);
		}, this);
		
		if (this.loadMoreButton)
		{
			this.loadMoreButton.set('html', 'Show more!');
		}
	},
	
	loadMore: function()
	{
		// Check the date of the oldest item
		//var oldest = this.container.getElement('li.feeditem:last-child').retrieve('date');
		var oldest = this.container.getElement('li.feeditem:last-child').getAttribute('data-date');
		
		this.request.send(
		{
			data:
			{
				count: this.options.count,
				before_date: oldest
			}
		});
		
		return false;
	}
});

/**
 * This class does not exist.
 */
var CheatCode = 
{
	keys: ['up', 'up', 'down', 'down', 'left', 'right', 'left', 'right', 'b', 'a', 'enter'],
	step: 0,
	
	/**
	 * This function does not exist.
	 */
	init: function()
	{
		window.addEvent('keydown', CheatCode.keypress.bind(this));
	},
	
	keypress: function(e)
	{		
		// If incorrect key is pressed, start again!
		if (e.key != this.keys[this.step])
		{
			this.step = 0;
			return;
		}
		
		// All entered correctly?!
		if (++this.step == this.keys.length)
		{
			// Magic!
			$('default-stylesheet').disabled = true;
			// Ssshhhh, you cannot see this :o
			new Element('link',
			{
				rel: 'stylesheet',
				href: 'not_an_easter_egg/zero.css',
				type: 'text/css'
			}).inject(document.head || $$('head')[0]);
		}
	}
}

// Fire it up!
//window.addEvent('domready', D15.onload);
//window.addEvent('domready', Global.init);
// Since the script is at the very bottom of the page, this should be okay.
$(document.body).removeClass('no-js').addClass('js');
D15.onload();
Global.init();