Daniel15
========

This is the code that powers [my personal website](https://dan.cx/) and blog. It uses C# and the ASP.NET Core MVC framework, with some older bits in PHP.

The live site is currently running on .NET Core 2.0

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

1. Link to my site s
2. Don't steal my site design, use your own. :)

(The MIT licence)

Copyright (C) 2012 Daniel Lo Nigro (Daniel15)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
