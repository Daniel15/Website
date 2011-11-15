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
		(new Ajax('chatstatus.php', 
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
		})).send();
		
		// Make the "start conversation" link open in a new window
		$('start_gtalk_chat').addEvent('click', function(e)
		{
			Events.stop(e);
			window.open(this.href, '_blank', 'height=500px,width=300px');
		});
	}
};

/**
 * Projects page
 */
Page.Site.Projects = 
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
		for (var i = 0; i < techProjects.length; i++)
		{
			techProjects[i].setStyle('display', '').addClass('visible');
		}
		
		// Show or hide the "active projects" section depending on if it actually contains anything.
		var activeProjects = $('active_projects');
		activeProjects.setStyle('display', activeProjects.firstByClass('.visible') ? '' : 'none');
		
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
		
		// TODO in the framework: Make some sort of custom element list that can call setStyle 
		// on all items (MooTools and jQuery style).
		
		
		var lists = $('content').getByClass('projects');
		for (var i = 0; i < lists.length; i++)
		{
			var projects = lists[i].getByTag('li');
			for (var j = 0; j < projects.length; j++)
			{
				projects[j].setStyle('display', toggle ? '' : 'none')[toggle ? 'addClass': 'removeClass']('visible');
			}
		}
	}
};