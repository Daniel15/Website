Daniel15
========

This is the code that powers [my personal website](http://dan.cx/) and blog

Blog
====
The blog is a very simple blog. It doesn't really have many fancy features. Things that ARE
supported are:

 - Browsing monthly archives
 - Writing comments
   - Spam protection via Akismet
   - Subscribing to comment replies (receive emails when new comments are made)
 - Writing new posts
   - Posting new posts to Facebook and Twitter
   
As it's just for my personal use, I haven't put too much work into polishing the blog administration
section. 

The database schema is in the schema.sql file. Blog administration is at /blogadmin. The default 
username and password are "admin" and "p@ssw0rd" respectively. To change the password, go to 
/account/hash?password=your_new_password (where your_new_password is the password you want) to get a
hash, and then copy this into the application/config/auth.php file. You should also change the hash
key for maximum security. 

CSS and JavaScript combining 
============================
The CSS and JavaScript files can be combined and compressed to save download time. To do this, first
run tools/compress.php at the command line. This combines and compresses the CSS and JS files, saves
the result into res/combined, and saves the filenames to application/config/site.php. Once this is
done, make sure "enableCompression" is set to "true" in application/config/site.php.

Licence
=======
Please feel free to use any bits of this code in your own work, but please:

1. Link to my site somewhere
2. Don't steal my site design, use your own. :)