---
id: 332
title: Surrounded by ugly HTML
published: true
publishedDate: 2011-10-13 01:26:00Z
lastModifiedDate: 2011-10-13 01:26:00Z
categories:
- Web Development
- Programming

---

# This post is originally from Daniel15's Blog at https://d.sb/2011/10/surrounded-by-ugly-html

---

Am I the only person that cringes when I see HTML like this?

```html
<div class="h1Title"><div class="spriteicon_img_mini" id="icon-name_mini"></div>Page Title</div>
```
Or like this?

```html
<!--Start Footer-->
<div id="heading-bottom_bg" class="spriteheading_bg footer">
	<ul class="links footer_ul">
		<li class="footer_li"><a class="footer_li_a bottomlink" href="../index.html">Home</a></li>
		<li class="footer_li"><span class="footer" style="font-size:18px;">&#9642;</span></li>
		...
		<li class="footer_li"><a class="footer_li_a bottomlink" href="/logout/">Log out</a></li>
	</ul>
</div>
```
Notice the classes on all those elements. Really? A web developer that doesn't know about the <h1> tag or CSS descendant/child selectors? Why do people feel the need to use <div> tags for everything, when there's other more semantic tags available? It really doesn't make sense to me; some of the first HTML tags I learnt were <h1> and <p>.

For what it's worth, this is how I'd rewrite those two blocks of HTML:

```html
<h1 class="icon-name">Page Title</h1>
```
```html
<!--Start Footer-->
<div id="footer">
	<ul>
		<li><a href="../index.html">Home</a> &#9642;</li>
		...
		<li><a href="/logout/">Log out</a></li>
	</ul>
</div>
```

It's good to keep your HTML and CSS selectors as simple as possible. There's no need for a "footer_li" class when you can just use "#footer li" in your CSS. The "icon-name" CSS class on the <h1> is used for a CSS sprite to display next to the heading. Also, as an alternative, the separator (&#9642;) that was originally in a <span> after all the footer items can easily be added via the :after pseudo selector instead of being in the <li>. 

It's really frustrating that there's so many "web developers" that don't seem to know basic HTML. It's okay if you're just starting to learn, this is fair enough. The HTML I "wrote" when I started web development was horrendous. And by "wrote" I mean "created in FrontPage 98". But it's another thing altogether to be a developer for a number of years and still write ugly HTML like this.

Ugly JavaScript seems to be way more common, though. But that's a rant for another day.
