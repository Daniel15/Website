/**
 * Blog administration
 */
var BlogAdmin = 
{

};

BlogAdmin.Posts = 
{
};

BlogAdmin.Posts.Edit = 
{
	init: function()
	{
		this.categorySearchEl = $('category-search');
		this.categoriesEl = $('categories');
		$('Post_Title').addEvent('keyup', this.updateSlug);
		this.categorySearchEl.addEvent('keyup', this.categorySearch.bind(this));
	},
	
	updateSlug: function()
	{
		var slug = $('Post_Title').get('value').toLowerCase()
			.replace(/[^A-Za-z0-9\-]/g, '-')
			.replace(/\s+/g, '-')
			.replace(/\-+/g, '-');
		// Make sure slug doesn't end with hyphen
		if (slug.charAt(slug.length - 1) == '-')
			slug = slug.substring(0, slug.length - 1);
		$('Post_Slug').set('value', slug);
	},
	
	categorySearch: function()
	{
		var categoryEls = this.categoriesEl.getByTag('li'),
			searchTerm = this.categorySearchEl.get('value').toLowerCase();
		for (var i = 0, count = categoryEls.length; i < count; i++)
		{
			var categoryEl = categoryEls[i];
			var containsTerm = categoryEl.get('textContent').toLowerCase().indexOf(searchTerm) !== -1;
			categoryEl.setStyle('display', containsTerm ? 'block' : 'none');
		}
	}
};

// Manual initialisation is necessary since this loads after the core code
if (document.body.id == 'admin-blog-edit')
	BlogAdmin.Posts.Edit.init();
