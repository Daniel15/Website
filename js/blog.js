/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * Blog scripts
 * Feel free to use any of this, but please link back to my site
 */

Page.Blog =
{
	/**
	 * All the posts on the current page
	 */
	posts: [],
	/**
	 * Initialise the blog (called on all blog pages)
	 */
	init: function()
	{
		this.initSidebar();
		this.initPosts();
	},
	
	/**
	 * Initialise the blog sidebar
	 */
	initSidebar: function()
	{
		var archives = $('sidebar-archives');
		if (!archives)
			return;
			
		var years = archives.children();
		for (var i = 0, count = years.length; i < count; i++)
		{
			var year = years[i];
			var inner = year.firstByTag('ul');
			// Store the height for later (for animating)
			inner.originalHeight = inner.get('offsetHeight');
			inner.setStyle('height', '0');
			
			year.addEvent('click', this.toggleYear.bind(this));
		}
	},
	
	/**
	 * Toggle displaying of a year in the sidebar
	 */
	toggleYear: function(e)
	{
		var inner = $(e.target.parentNode).firstByTag('ul');
		// Animation is handled by the CSS
		inner.setStyle('height', (inner.get('offsetHeight') == 0 ? inner.originalHeight : 0) + 'px');
		Events.stop(e);
	},
	
	/**
	 * Initialise the posts on the page
	 */
	initPosts: function()
	{
		var posts = $('content').getByTag('article', false);
		for (var i = 0, count = posts.length; i < count; i++)
		{
			// Ensure it's a post
			// TODO: Use descendant selector with querySelectorAll instead of this check
			if (posts[i].id.slice(0, 5) == 'post-')
				this.posts.push(new Blog.Post(DOM.get(posts[i])));
		}
	}
};

/**
 * Viewing a blog post
 */
Page.Blog.View = 
{
	/**
	 * Initialise viewing a blog post
	 */
	init: function()
	{
		$('comments').addDelegate('click', 'a', 'reply-to', this.replyToComment);
		
		// TODO: Store commenter data
		// TODO: Placeholders
	},
	
	/**
	 * Called when a reply link is clicked
	 * @param	Event data
	 */
	replyToComment: function(e)
	{
		Events.stop(e);
		console.log(e);
		console.log(this);
		alert('TODO');
	}
};

/**
 * Classes used in the blog
 */
var Blog = {};

/**
 * Represents a single post in the blog. Handles the social network sharing links
 */
Blog.Post = function(post)
{
	this.post = post;
	this.id = this.post.get('id').slice(5);
	var socialCountUrl = 'social/blogpost/' + this.id;
	
	this.socialNetworks = {};
	var socialNetworks = this.post.firstByClass('share').getByTag('li');
	for (var i = 0, count = socialNetworks.length; i < count; i++)
	{
		this.socialNetworks[socialNetworks[i].get('className')] = socialNetworks[i];
	}
	
	this.addPopupHandler('facebook', 500, 400);
	this.addPopupHandler('twitter', 550, 420);
	
	// Initialise data for network sharing links
	Ajax.load(socialCountUrl, 
	{
		method: 'get',
		onSuccess: this.updateSocialCounts,
		context: this,
		abortPrev: false
	});
};

Blog.Post.prototype = 
{
	/**
	 * Add a popup click handler to a social sharing button
	 */
	addPopupHandler: function(name, width, height)
	{
		this.socialNetworks[name].firstByTag('a').addEvent('click', function(e)
		{
			// "this" refers to the link here!
			window.open(this.href, '_blank', 'height=' + height + 'px,width=' + width + 'px');
			Events.stop(e);
		});
	},
	
	/**
	 * Update the social sharing counts for a post (called via AJAX response)
	 */
	updateSocialCounts: function(data)
	{
		for (var name in data)
		{
			this.socialNetworks[name].firstByClass('count').set('innerHTML', data[name]);
		}
	}
};