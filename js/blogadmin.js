/**
 * Blog administration
 */
var BlogAdmin = 
{

};

BlogAdmin.Posts = 
{
}

BlogAdmin.Posts.Edit = 
{
	init: function()
	{
		$('title').addEvent('keyup', this.updateSlug);
	},
	
	updateSlug: function()
	{
		var slug = $('title').get('value').toLowerCase()
			.replace(/[^A-Za-z0-9\-]/g, '-')
			.replace(/\s+/g, '-')
			.replace(/\-+/g, '-');
		// Make sure slug doesn't end with hyphen
		if (slug.charAt(slug.length - 1) == '-')
			slug = slug.substring(0, slug.length - 1);
		$('slug').set('value', slug);
	}
};

// Manual initialisation is necessary since this loads after the core code
if (document.body.id == 'blogadmin-posts-edit')
	BlogAdmin.Posts.Edit.init();
