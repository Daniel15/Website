---
id: 336
title: iOS 6 Safari caches AJAX POST requests
published: true
publishedDate: 2012-09-20 20:13:00Z
lastModifiedDate: 2012-09-20 20:13:00Z
categories:
- Web Development
- JavaScript

---

Back when IE 9 came out, it was the first major browser to start caching redirects to improve performance. The IE team [wrote a detailed blog post about it](http://blogs.msdn.com/b/ie/archive/2010/07/14/caching-improvements-in-internet-explorer-9.aspx), but they still got some backlash (mainly from people that didn't set correct no-cache headers on redirects with side effects, like login pages).

The recently released version of Safari for iOS 6 has [started caching AJAX POST requests](http://stackoverflow.com/questions/12506897/ios6-safari-caching-ajax-results), with no notification to developers at all. Not only is this unexpected, but it goes against [the HTTP 1.1 standard](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html), which states:

> 9.5 POST  
> 
> ...  
> 
> Responses to this method **are not cacheable**, unless the response includes appropriate Cache-Control or Expires header fields.

This means that if you have something like an edit page that uses AJAX to post the form, the first edit will work, but subsequent edits will return a cached "Success" message instead of sending the request to the server!

This is somewhat similar to Internet Explorer caching AJAX GET requests, except caching GET requests is nowhere near as dangerous. While GET requests are allowed to be cached, POST requests are not idempotent (they can have side effects) so they should **never** be cached. Sure, IE caches AJAX GET requests more heavily than other browsers, but this is allowed in the HTTP specs:

> The response to a GET request is cacheable if and only if it meets the requirements for HTTP caching described in section 13.

Caching POST requests is horrible. How many times have you added no-cache headers to your POST pages? I'm going to have a guess and say never, since no other browser in the history of the World Wide Web has cached POST requests. This move has essentially broken the functionality of some AJAX-based edit forms, and developers might not notice the breakage initially. I hope this is a bug and not expected functionality, and it gets fixed at some point. Safari used to be a horrible browser with many DOM issues... I hope it's not heading this way again.
