var Home = 
{
	'init': function()
	{
		// Add the click handler to each element, and then hide the inner DIV.
		$$('div.section').each(function(el)
		{
			var theHeading = el.getElement('h2');
			theHeading.addEvent('click', Home.toggle_section);
			theHeading.setStyle('cursor', 'pointer');
			theHeading.innerHTML += ' (click to expand)';
			el.getElement('div.section_inner').slide('hide');
		});
	},
	
	'toggle_section': function(e)
	{
		this.parentNode.getElement('div.section_inner').slide('toggle');
	}
};

window.addEvent('domready', Home.init);