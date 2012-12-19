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

The database schema is in the schema.sql file. Blog administration is at /blogadmin. The default 
username and password are "admin" and "p@ssw0rd" respectively. To change the password, go to 
/account/hash?password=your_new_password (where your_new_password is the password you want) to get a
hash, and then copy this into the application/config/auth.php file. You should also change the hash
key for maximum security. 

Licence
=======
Please feel free to use any bits of this code in your own work, but please:

1. Link to my site somewhere
2. Don't steal my site design, use your own. :)
