---
id: 239
title: 'JavaScript for modern sites [Part 1]: Unobtrusive JavaScript and object literal syntax'
published: true
publishedDate: 2010-03-28 04:07:55Z
lastModifiedDate: 2010-03-28 04:07:55Z
categories:
- JavaScript
- Programming

---

<p>In this post, I'll discuss some of the techniques that I personally write JavaScript. There's no right or wrong, this is all <strong>my opinion</strong> (still, feel free to flame me if you feel it's necessary :P). This post is aimed at people that understand basic JavaScript and HTML techniques, and want to see how I code my JavaScript. I will talk about the JavaScript of the past, how it's changed, and some techniques used in modern JavaScript development. This will probably be a multi-part series if I ever get around to writing more posts :P <!--more--></p>
<h3>The Bad Old Days</h3>
<p>So where do we begin? In the old days, JavaScript wasn't used very much at all. Browsers didn't really support much JavaScript, so it was only used to add little bits of functionality to sites. The one site might have had a few little procedural functions for the whole site. Global variables were used everywhere, but as the scripts were very small, this wasn't <em>too </em>much of an issue. Usability also wasn't considered much of an issue, and there was no proper event model, leading to things like:<br />
<pre class="brush: html" escaped="true">&lt;a href="javascript:DoSomething();"&gt;Do something&lt;/a&gt;</pre><br />
However, the JavaScript of today is something very different — It is everywhere, and is used for more than just trivial uses. On a normal website, a large amount of JavaScript might be used. Nowadays we even have <strong>web applications</strong> like Gmail and Google Docs which contain heaps of JavaScript for all their core functionality. There are a lot of web applications that are mainly client-side, with a very small server-side component. Because of this use of large scripts, we can no longer rely on unorganised scripts and use of global variables as in the past. Scripts written using these techniques of the past become very unmaintainable very quickly. Additionally, <strong>usability</strong> is a bigger issue today than it used to be, mainly due to the number of users on the Web (many of which have disabilities and require sites to be accessible).</p>
<p>So what can we do to improve our JavaScript? Well, let's first see how we can neatly organise our JavaScript using...</p>
<h3>Object Literal Syntax</h3>
<p>You might be thinking, "what the heck is object literal syntax"? Simply put, it's a simple syntax for creating a JavaScript object, added with the release of JavaScript 1.2 in June 1997. Here's an example of an object created with this syntax:<br />
<pre class="brush: javascript">var MyObject =
{
	myVariable: 123,
	hello: 'world'
}
alert(MyObject.myVariable) // alerts "123"
alert(MyObject.hello)      // alerts "world"</pre><br />
See? Pretty simple. It's a lot like a hashtable in other programming languages (such as C#, Perl, and arrays in PHP). One of the great things about JavaScript (and a feature that differentiates it from some other languages) is that you can store<strong> functions inside variables</strong>. We can use this to our advantage:<br />
<pre class="brush: javascript">var MyObject =
{
	myVariable: 123,
	hello: function()
	{
		alert('Hello world ' + this.myVariable);
	}
}

MyObject.hello(); // alerts "Hello world 123"</pre><br />
See what we can do? Instead of having variables functions laying around all over the place, we can <strong>group them together</strong> into objects. This is a similar idea to <strong>namespaces</strong> in other languages, and is probably about the closest equivalent in JavaScript. When coding a site that uses JavaScript, my personal approach is generally to make <strong>one object per unique page that uses JavaScript</strong>. This keeps things nicely organised, and makes each page's functions and variables self-contained. </p>
<p>Of course, we can also use proper classes to hold reusable components, but this is outside the scope of this post and will probably be covered in a future post :).</p>
<p>Another modern technique is unobtrusive JavaScript...</p>
<h3>So what is unobtrusive JavaScript?</h3>
<p>In a nutshell, unobtrusive JavaScript is a coding technique that separates JavaScript from HTML in the same way that CSS is separate from HTML. Basically, with unobtrusive JavaScript, you have <strong>no</strong> JavaScript in the HTML at all, except for the &lt;script&gt; tag that loads the script file, and maybe a &lt;script&gt; tag to run an initialisation function. This means <strong>no </strong>onclick="whatever()" attributes on any of your HTML elements. How do we add JavaScript click actions, then? Easy — Event handlers!</p>
<p>When using unobtrusive JavaScript, the JavaScript initialisation function "connects" the event handlers to all elements that need them. JavaScript event handlers are similar to that in other languages — Stuff throws events (such as click, mouseover, etc.), and functions can be set to run when these events are thrown. Previously, this was a little annoying, as Internet Explorer uses a different syntax to other browsers. However, today, there is no excuse, as JavaScript frameworks (such as MooTools and Prototype) and libraries (such as jQuery) make it really easy.</p>
<p>Another feature of unobtrusive JavaScript is that it involves <strong>progressive enhancement</strong> and <strong>graceful degradation</strong>. If the JavaScript <strong>doesn't</strong> run (ie. if the user has JavaScript disabled), the user's experience will still be acceptable. Maybe not as good as if they had JavaScript enabled, but still usable. We don't rely on JavaScript, but we do take advantage of it if it's available.  This usually involves adding event handlers (as mentioned above) that override the default action for links (that is, go to the URL they link to).</p>
<p>You might be confused by now (seems I ramble a lot in these posts), so let's quickly jump to an example HTML file that shows some of the things we've mentioned so far:<br />
<pre class="brush: html" escaped="true">&lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"&gt;
&lt;html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en"&gt;
&lt;head&gt;
	&lt;title&gt;JavaScript Test&lt;/title&gt;
	&lt;script type="text/javascript" src="mootools-1.2.4-core-yc.js"&gt;&lt;/script&gt;
	&lt;script type="text/javascript" src="script.js"&gt;&lt;/script&gt;
	&lt;script type="text/javascript"&gt;window.addEvent('domready', UserList.init);&lt;/script&gt;
&lt;/head&gt;

&lt;body&gt;
	&lt;table&gt;
		&lt;thead&gt;
			&lt;tr&gt;
				&lt;th&gt;Username&lt;/th&gt;
				&lt;th&gt;Tools&lt;/th&gt;
			&lt;/tr&gt;
		&lt;/thead&gt;
		&lt;tbody&gt;
		&lt;/tbody&gt;
			&lt;tr id="user-1"&gt;
				&lt;td&gt;Daniel15&lt;/td&gt;
				&lt;td&gt;&lt;a href="/user/1/delete" class="delete"&gt;Delete&lt;/a&gt;&lt;/td&gt;
			&lt;/tr&gt;
			&lt;tr id="user-2"&gt;
				&lt;td&gt;Bob&lt;/td&gt;
				&lt;td&gt;&lt;a href="/user/2/delete" class="delete"&gt;Delete&lt;/a&gt;&lt;/td&gt;
			&lt;/tr&gt;
			&lt;tr id="user-3"&gt;
				&lt;td&gt;Mark&lt;/td&gt;
				&lt;td&gt;&lt;a href="/user/3/delete" class="delete"&gt;Delete&lt;/a&gt;&lt;/td&gt;
			&lt;/tr&gt;
		&lt;/tbody&gt;
	&lt;/table&gt;
&lt;/body&gt;</pre><br />
As you can see, absolutely <strong>no inline JavaScript</strong>, just some script loading, and setting the <strong>UserList.init</strong> method to run when the page loads. If JavaScript is disabled, clicking any of those "Delete" links will take the user to the corresponding delete page linked with the &lt;a&gt; tag. What happens if JavaScript is enabled? Let's see! Here's the corresponding JavaScript file:<br />
<pre class="brush: javascript">var UserList =
{
	init: function()
	{
		// Connect up all the delete links
		$$('a.delete').addEvent('click', UserList.del);
	},

	del: function()
	{
		var user_id = this.getParent().getParent().id.substring(5);
		alert('Delete user ' + user_id);
		// Make sure the link doesn't go anywhere.
		return false;
	}
};</pre><br />
This is written using MooTools, but you could achieve a similar thing with another framework/library such as Prototype or jQuery. MooTools is just what I use :)</p>
<p>Notice the init function "connects" the delete links to the delete JavaScript function (which I named "del", "delete" is a reserved word in JavaScript). So, when you have JavaScript enabled, instead of going to the URL specified in the link, the UserList.del method will be run. This example just pops up an alert box, but this could easily be changed to use an AJAX call to delete the user. See? Magic! It's fully functional with JavaScript disabled, but JavaScript being enabled adds a lot of niceness.</p>
<p>And yes, I know, that script is messy and could be made better, it's just an example.</p>
<h3>Conclusion</h3>
<p>In this post, I've shown how to use object literal syntax to group JavaScript functions and variables together, and unobtrusive JavaScript techniques to progressively enhance pages. The advantages of doing so include neater and more maintanable code, and graceful degradation if JavaScript is disabled (not possible with &lt;a href="javascript: ... "&gt; links. There might eventually be a follow-up post. :P</p>
<p>Hope this helped you :D</p>
<p>Until next time,<br />
— Daniel</p>

