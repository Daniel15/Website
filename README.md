Daniel15
========

This is the code that powers [my personal website](http://dan.cx/) and blog. It is in the process of
being rewritten in .NET (old version was in PHP) so some of this is out-of-date.

Blog
====
The blog is a very simple blog. It doesn't really have many fancy features. Things that ARE
supported are:

 - Browsing monthly archives
 - Writing comments (now handled with Disqus)
 - Writing new posts
   - Posting new posts to Facebook and Twitter
   
As it's just for my personal use, I haven't put too much work into polishing the blog administration
section. 

The database schema is in the schema.sql file. Blog administration is at /blog/admin. The default 
username and password are "example" and "password" respectively. To change the password, SHA1 has the
password you want to use, and then modify Authentication.config.

Licence
=======
Please feel free to use any bits of this code in your own work, but please:

1. Link to my site somewhere
2. Don't steal my site design, use your own. :)
