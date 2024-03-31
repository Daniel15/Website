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

# This post is originally from Daniel15's Blog at https://d.sb/2010/03/javascript-for-modern-sites

---

In this post, I'll discuss some of the techniques that I personally write JavaScript. There's no right or wrong, this is all **my opinion** (still, feel free to flame me if you feel it's necessary :P). This post is aimed at people that understand basic JavaScript and HTML techniques, and want to see how I code my JavaScript. I will talk about the JavaScript of the past, how it's changed, and some techniques used in modern JavaScript development. This will probably be a multi-part series if I ever get around to writing more posts :P <!--more-->

## The Bad Old Days
So where do we begin? In the old days, JavaScript wasn't used very much at all. Browsers didn't really support much JavaScript, so it was only used to add little bits of functionality to sites. The one site might have had a few little procedural functions for the whole site. Global variables were used everywhere, but as the scripts were very small, this wasn't *too *much of an issue. Usability also wasn't considered much of an issue, and there was no proper event model, leading to things like:  

```html
<a href="javascript:DoSomething();">Do something</a>
```  

However, the JavaScript of today is something very different — It is everywhere, and is used for more than just trivial uses. On a normal website, a large amount of JavaScript might be used. Nowadays we even have **web applications** like Gmail and Google Docs which contain heaps of JavaScript for all their core functionality. There are a lot of web applications that are mainly client-side, with a very small server-side component. Because of this use of large scripts, we can no longer rely on unorganised scripts and use of global variables as in the past. Scripts written using these techniques of the past become very unmaintainable very quickly. Additionally, **usability** is a bigger issue today than it used to be, mainly due to the number of users on the Web (many of which have disabilities and require sites to be accessible).

So what can we do to improve our JavaScript? Well, let's first see how we can neatly organise our JavaScript using...

## Object Literal Syntax
You might be thinking, "what the heck is object literal syntax"? Simply put, it's a simple syntax for creating a JavaScript object, added with the release of JavaScript 1.2 in June 1997. Here's an example of an object created with this syntax:  

```javascript
var MyObject =
{
	myVariable: 123,
	hello: 'world'
}
alert(MyObject.myVariable) // alerts "123"
alert(MyObject.hello)      // alerts "world"
```  

See? Pretty simple. It's a lot like a hashtable in other programming languages (such as C#, Perl, and arrays in PHP). One of the great things about JavaScript (and a feature that differentiates it from some other languages) is that you can store** functions inside variables**. We can use this to our advantage:  

```javascript
var MyObject =
{
	myVariable: 123,
	hello: function()
	{
		alert('Hello world ' + this.myVariable);
	}
}

MyObject.hello(); // alerts "Hello world 123"
```  

See what we can do? Instead of having variables functions laying around all over the place, we can **group them together** into objects. This is a similar idea to **namespaces** in other languages, and is probably about the closest equivalent in JavaScript. When coding a site that uses JavaScript, my personal approach is generally to make **one object per unique page that uses JavaScript**. This keeps things nicely organised, and makes each page's functions and variables self-contained. 

Of course, we can also use proper classes to hold reusable components, but this is outside the scope of this post and will probably be covered in a future post :).

Another modern technique is unobtrusive JavaScript...

## So what is unobtrusive JavaScript?
In a nutshell, unobtrusive JavaScript is a coding technique that separates JavaScript from HTML in the same way that CSS is separate from HTML. Basically, with unobtrusive JavaScript, you have **no** JavaScript in the HTML at all, except for the <script> tag that loads the script file, and maybe a <script> tag to run an initialisation function. This means **no **onclick="whatever()" attributes on any of your HTML elements. How do we add JavaScript click actions, then? Easy — Event handlers!

When using unobtrusive JavaScript, the JavaScript initialisation function "connects" the event handlers to all elements that need them. JavaScript event handlers are similar to that in other languages — Stuff throws events (such as click, mouseover, etc.), and functions can be set to run when these events are thrown. Previously, this was a little annoying, as Internet Explorer uses a different syntax to other browsers. However, today, there is no excuse, as JavaScript frameworks (such as MooTools and Prototype) and libraries (such as jQuery) make it really easy.

Another feature of unobtrusive JavaScript is that it involves **progressive enhancement** and **graceful degradation**. If the JavaScript **doesn't** run (ie. if the user has JavaScript disabled), the user's experience will still be acceptable. Maybe not as good as if they had JavaScript enabled, but still usable. We don't rely on JavaScript, but we do take advantage of it if it's available.  This usually involves adding event handlers (as mentioned above) that override the default action for links (that is, go to the URL they link to).

You might be confused by now (seems I ramble a lot in these posts), so let's quickly jump to an example HTML file that shows some of the things we've mentioned so far:  

```html
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<title>JavaScript Test</title>
	<script type="text/javascript" src="mootools-1.2.4-core-yc.js"></script>
	<script type="text/javascript" src="script.js"></script>
	<script type="text/javascript">window.addEvent('domready', UserList.init);</script>
</head>

<body>
	<table>
		<thead>
			<tr>
				<th>Username</th>
				<th>Tools</th>
			</tr>
		</thead>
		<tbody>
		</tbody>
			<tr id="user-1">
				<td>Daniel15</td>
				<td><a href="/user/1/delete" class="delete">Delete</a></td>
			</tr>
			<tr id="user-2">
				<td>Bob</td>
				<td><a href="/user/2/delete" class="delete">Delete</a></td>
			</tr>
			<tr id="user-3">
				<td>Mark</td>
				<td><a href="/user/3/delete" class="delete">Delete</a></td>
			</tr>
		</tbody>
	</table>
</body>
```  

As you can see, absolutely **no inline JavaScript**, just some script loading, and setting the **UserList.init** method to run when the page loads. If JavaScript is disabled, clicking any of those "Delete" links will take the user to the corresponding delete page linked with the <a> tag. What happens if JavaScript is enabled? Let's see! Here's the corresponding JavaScript file:  

```javascript
var UserList =
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
};
```  

This is written using MooTools, but you could achieve a similar thing with another framework/library such as Prototype or jQuery. MooTools is just what I use :)

Notice the init function "connects" the delete links to the delete JavaScript function (which I named "del", "delete" is a reserved word in JavaScript). So, when you have JavaScript enabled, instead of going to the URL specified in the link, the UserList.del method will be run. This example just pops up an alert box, but this could easily be changed to use an AJAX call to delete the user. See? Magic! It's fully functional with JavaScript disabled, but JavaScript being enabled adds a lot of niceness.

And yes, I know, that script is messy and could be made better, it's just an example.

## Conclusion
In this post, I've shown how to use object literal syntax to group JavaScript functions and variables together, and unobtrusive JavaScript techniques to progressively enhance pages. The advantages of doing so include neater and more maintanable code, and graceful degradation if JavaScript is disabled (not possible with <a href="javascript: ... "> links. There might eventually be a follow-up post. :P

Hope this helped you :D

Until next time,  

— Daniel

