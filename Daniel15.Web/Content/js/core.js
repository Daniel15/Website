/** @preserve
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * Feel free to use any of this, but please link back to my site
 */
 
/*
 * Still to be done:
 * - Blog admin
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
		// up up down down left right left right b a enter
		// up = 38, down = 40, left = 37, right = 39, b = 66, a = 65, enter = 13
		var cheatCode = new CheatCode([38, 38, 40, 40, 37, 39, 37, 39, 66, 65]);
	}
};

/**
 * This class does not exist
 */
var CheatCode = function(keys)
{
	this.keys = keys;
	this.step = 0;
	Events.add(window, 'keydown', this.keypress.bind(this));
}

CheatCode.prototype = 
{
	/**
	 * This function does not exist
	 */
	keypress: function(e)
	{
		// If incorrect key is pressed, start again!
		if (e.keyCode != this.keys[this.step])
		{
			this.step = 0;
			return;
		}
		
		// All entered correctly?!
		if (++this.step == this.keys.length)
		{
			var head = document.head || document.getElementsByTagName('head')[0];
			head.getElementsByTagName('link')[0].disabled = true;
			head.appendChild(DOM.create('link', 
			{
				rel: 'stylesheet',
				href: 'Content/not_an_easter_egg/zero.css',
				type: 'text/css'
			}, false));
		}
	}
};