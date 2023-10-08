/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2014
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
	init: async function()
	{
		$('email_address').set('innerHTML', '&#100;&#064;&#100;&#046;&#115;&#098;');
		$('gtalk_address').set('innerHTML', '&#100;&#097;&#110;&#105;&#101;&#108;&#064;&#100;&#049;&#053;&#046;&#098;&#105;&#122;');

		await Page.Site.Socialfeed.loadAndInsertItems('/socialfeed.htm?count=10&showDescription=false');
		const loadingPlaceholder = document.querySelector('.minifeed-loading');
		loadingPlaceholder.parentElement.removeChild(loadingPlaceholder);
	},
	/**
	 * Load Google Talk status via AJAX request
	 */
	initGoogleTalk: function(data)
	{
		$('gtalk').removeClass('offline').addClass(data.state.toLowerCase());
		$('gtalk_address').set('title', data.state);
		var status = data.state;
		if (data.statusText && data.statusText.toLowerCase() !== data.state.toLowerCase())
			status += ' (' + data.statusText + ')';
		$('gtalk_status').set('innerHTML', status);
	},
	
	initSkype: function(data)
	{
		// Don't do anything if there's no response
		if (!data.query || data.query.count != 1 || !data.query.results || !data.query.results.result)
			return;

		var skypeStatuses = {
			UNKNOWN: 0,
			OFFLINE: 1,
			ONLINE: 2,
			AWAY: 3,
			DND: 5 // Do Not Disturb
		};

		var result = data.query.results.result,
		    statusCode = parseInt(result.status, 10),
		    status = result.message.en,
			cssClass = 'offline';
		
		switch (statusCode)
		{
			case skypeStatuses.OFFLINE:
				cssClass = 'offline';
				break;
			case skypeStatuses.ONLINE:
				cssClass = 'online';
				break;
			case skypeStatuses.AWAY:
			case skypeStatuses.DND:
				cssClass = 'busy';
				break;
		}

		$('skype').removeClass('offline').addClass(cssClass);
		$('skype_address').set('title', status);
		$('skype_status').set('innerHTML', status);
	}
};

Page.Site.Socialfeed = 
{
	init: function()
	{
		const showMoreLink = document.getElementById('show-more');
		showMoreLink.addEventListener('click', evt => 
		{
			Page.Site.Socialfeed.loadMore(showMoreLink);
			evt.preventDefault();	
		})
	},

	loadMore: async function(showMoreLink)
	{
		showMoreLink.textContent = 'Loading...';

		const url = showMoreLink.getAttribute('href');
		const doc = await Page.Site.Socialfeed.loadAndInsertItems(url);

		// Replace "show more" link with new one containing new URL.
		const newShowMoreLink = doc.querySelector('#show-more');
		showMoreLink.replaceWith(newShowMoreLink);
		Page.Site.Socialfeed.init();
	},

	loadAndInsertItems: async function(url)
	{
		const response = await fetch(`${url}&partial=true`);
		const parser = new DOMParser();
		const doc = parser.parseFromString(await response.text(), 'text/html');
		const items = doc.querySelectorAll('li.feeditem');

		const feed = document.querySelector('ul.socialfeed') || document.querySelector('ul.minifeed');
		for (const item of items) {
			feed.appendChild(item);
		}
		return doc;
	}
};

/**
 * Projects page
 */
Page.Project = {};
Page.Project.Index = 
{
	/**
	 * Initialise the page
	 */
	init: function()
	{
		$('sidebar').addDelegate('click', 'a', null, this.showTech.bind(this));
	},
	
	/**
	 * Currently selected technology
	 */
	currentTech: null,
	
	/**
	 * Show a particular technology
	 */
	showTech: function(e)
	{
		// Get this tech, and the projects using it
		var el = $(e.target.parentNode);
		var id = el.get('id').substring(5);
		var name = e.target.textContent || e.target.innerText;
		
		if (this.currentTech == el)
		{
			this.unselectTech();
			Events.stop(e);
			return;
		}
		
		// Unselect any currently selected item
		if (this.currentTech)
			this.currentTech.removeClass('active');
			
		this.currentTech = el;
		el.addClass('active');
		
		// Show the tech header and some info
		$('intro').setStyle('display', 'none');
		var info = $('tech-info');
		info.setStyle('display', 'block');
		info.firstByTag('h2').set('innerHTML', 'About ' + name);
		// tech_descs is defined in cms/pages/projects.php towards the end
		info.firstByTag('div').set('innerHTML', tech_descs[id]);
		
		// Get all the projects that have used this technology
		var techProjects = $('content').getByClass('uses-' + id);
		$('tech-count').set('innerHTML', techProjects.length);
		
		this.toggleAllProjects(false);
		// Now show the relevant projects
		techProjects.setStyle('display', '').addClass('visible');
		
		// Show or hide the "active projects" section depending on if it actually contains anything.
		var activeProjects = $('active_projects');
		activeProjects.setStyle('display', activeProjects.firstByClass('visible') ? '' : 'none');
		
		Events.stop(e);
	},
	
	/**
	 * Unselect the currently selected technology
	 */
	unselectTech: function(el)
	{
		this.currentTech.removeClass('active');
		this.currentTech = null;
		// Hide the info
		$('intro').setStyle('display', '');
		$('tech-info').setStyle('display', '');
		
		// Show all projects
		this.toggleAllProjects(true);
		$('active_projects').setStyle('display', '');
	},
	
	/**
	 * Hide or show all the projects
	 */
	toggleAllProjects: function(toggle)
	{
		// Toggle all the projects. The "visible" class is just used by the code that
		// hides/shows "Active projects" depending on if any are actually active
		
		var lists = $('content').getByClass('projects');
		for (var i = 0; i < lists.length; i++)
		{
			lists[i].getByTag('li').setStyle('display', toggle ? '' : 'none')[toggle ? 'addClass': 'removeClass']('visible');
		}
	}
};
