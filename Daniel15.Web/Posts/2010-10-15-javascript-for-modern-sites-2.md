---
id: 329
title: 'JavaScript for modern sites [Part 2]: The awesomeness of functions'
published: true
publishedDate: 2010-10-15 20:14:20Z
lastModifiedDate: 2010-10-15 20:14:20Z
categories:
- JavaScript

---

# This post is originally from Daniel15's Blog at https://d.sb/2010/10/javascript-for-modern-sites-2

---

In this post, I'll discuss more of my opinions regarding JavaScript development. Please read [the first post in the series](http://dan.cx/blog/2010/03/javascript-for-modern-sites/) if you haven't already. In this post, I'll cover some relatively important language features that don't seem to be covered in a lot of basic JavaScript guides. I'm assuming you have a basic knowledge of JavaScript. Let's begin.

## Functions are variables
In most programming languages, functions are a pretty basic language feature. They're quite nice for structuring your code, but don't really have any built-in awesomeness. Some programming languages have features to dynamically call functions at run-time (usually referred to as *reflection*), but JavaScript has a LOT more power in this area. In JavaScript, functions are known as [first-class objects](http://en.wikipedia.org/wiki/First-class_object). Functions are stored in normal variables, and you can create new ones (known as *anonymous functions*) and edit existing ones on the fly. Functions can also be return values from other functions! This enables a whole range of different programming techniques known as metaprogramming.

Let's take a look at some examples.<!--more-->
 Most people that use JavaScript should know the basic function declaration syntax:  

```javascript
function test()
{
	alert('This is a test function');
}
```

However, because functions are variables, there's also a different syntax you can use:  

```javascript
var test = function()
{
	alert('This is a test function');
}
```

These two examples are exactly identical! Since it's a variable, you can do everything you could do with variables. You can pass it to functions:  

```javascript
function doStuff(fun)
{
	fun('This is fun!'); // Call the passed in function, passing a parameter to it
}

var alertSomething = function(msg)
{
	alert('This is a message: ' + msg);
}
doStuff(alertSomething); // Passes the alertSomething variable to doStuff. Alerts "This is a message: This is fun!"
```

And even overwrite built-in functions:  

```javascript
document.write = function()
{
	alert('NO! document.write is EVIL! Time to learn DOM methods :-)');
}

document.write('Test'); // pwned
```

## Functions are also objects
As we saw above, functions are variables. Functions are also objects! This means you can store variables against functions. Let's see a simple example:  

```javascript
function count()
{
	count.number++;
	alert('Count = ' + count.number);
}

count.number = 0;
count(); // Count = 1
count(); // Count = 2
count(); // Count = 3
```

## No need for global variables
So now that we know we can store variables against functions, you should realise that we should almost never need global variables. For variables that are specific to one function (like a count), you can store it against the function, as shown above. For variables that are relevant to a number of different functions, you should probably group the variables and all the functions into an object literal (see [my previous post](http://dan.cx/blog/2010/03/javascript-for-modern-sites/) for more information on object literals).

## Put object literals into namespaces
But wait... "The example functions all shown so far are globals, but you said to not use globals", I hear you say. Well, that's correct. So I'd say to put all your functions into object literals. Group all your functions into related categories or groups, and make one object literal per group. You can even put object literals inside object literals:  

```javascript
var Site = {};
Site.Home = 
{
	// ... stuff for the home page
};
Site.ContactUs = 
{
	// ... stuff for the contact page
};

var Blog = {};
Blog.Main = 
{
	// ... Stuff for the blog
};

Blog.ViewPost = 
{
	// ... stuff for viewing blog posts
};
```

This keeps your code clean and organised, and ensures you don't pollute the global namespace (you've only made two global objects here - Site and Blog). For any scripts you release publicly, I'd suggest putting them in some sort of namespace object, such as your name or nickname:  

```javascript
var Daniel15 = Daniel15 || {};
Daniel15.AwesomeControl = 
{
	// ... stuff
}
```

The "var Daniel15 = Daniel15 || {};" uses the existing Daniel15 object if it exists, otherwise, it creates a new empty one.

I think that's all for this post. I'd like to post about things like closures and such, but I'll save that for another post :).

Until next time,  

 — Daniel15

