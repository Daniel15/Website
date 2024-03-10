---
id: 331
title: Twitter autoreply bot - DBZNappa
published: true
publishedDate: 2011-06-11 05:15:06Z
lastModifiedDate: 2011-06-11 05:15:06Z
categories:
- PHP
- Programming

---

So I thought I'd finally write a little blog post about a [Twitter bot](http://twitter.com/#!/DBZNappa) I made a while ago. A few people emailed me asking for the source code, so I had previously [posted about it on webDevRefinery](http://webdevrefinery.com/forums/topic/5039-ideas-for-a-twitter-bot/page__view__findpost__p__42777), but never on my own blog. Basically all the bot does is search for whenever people mention "[over 9000](http://ohinternet.com/Over_9000){rel=nofollow}" or "over nine thousand", and replies with "WHAT, NINE THOUSAND?!". Pretty simple, but I wanted to learn about using the Twitter API. It seems to have inspired the creation of other Twitter bots, like [AnnoyingNavi](http://davidcurado.com.br/projects/annoyingnavi/) and [The Spacesphere](http://aqua3.bplaced.net/2011/06/the-spacesphere/), which I think is pretty cool. :).

The source code is [available as a Gist on Github](https://gist.github.com/820281). It is written in PHP and requires the PECL OAuth extension to be installed. I think it's a pretty good example of a simple "search and reply" Twitter bot, that could easily be extended to do more useful things.

Until next time,  

— Daniel
