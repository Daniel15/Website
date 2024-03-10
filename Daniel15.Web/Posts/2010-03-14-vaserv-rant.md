---
id: 231
title: VAServ rant
published: true
publishedDate: 2010-03-14 01:55:25Z
lastModifiedDate: 2010-03-14 01:55:25Z
categories:
- Rants

---

A while ago, I used to use a VPS from a company called [FSCKVPS](http://fsckvps.com){rel=nofollow}, mainly for storing backups offsite (in case something bad happens to my server one day), and secondary DNS (so in case my server is ever down, I can still get emails, as my emails are hosted using Google Apps). In June 2009, their parent company VAServ had a massive hack attack, with news websites reporting that [as many as 100,000 websites were wiped out by the hack](http://www.theregister.co.uk/2009/06/08/webhost_attack/){title="TheRegister.co.uk article about VAServ / FSCKVPS hack" rel=nofollow}, and the [WebHostingTalk thread about the outage](http://www.webhostingtalk.com/showthread.php?t=867100){rel=nofollow} ended up being one of the longest ones I've seen, at 177 pages long. Some people's VPSes survived, but [mine was one of the ones that was totally lost](http://www.webhostingtalk.com/showpost.php?p=6223507&postcount=562) (luckily, as it was only for backups, it didn't have anything too important in it). They offered [two months free as compensation](http://www.webhostingtalk.com/showpost.php?p=6227951&postcount=1792), so I waited patiently for them to provision me a new VPS, and lived without offsite backups for a while.

After the two free months, they were still having issues — My VPS kept breaking, and they still hadn't given me a secondary IP address as I had requested. I kept giving them the benefit of the doubt, but eventually I decided that enough was enough (two months should have been enough to sort out things), so I moved to another provider. When a company can't even work out how to spell its own company name (sometimes they write "VAServ", other times they write "VAServ**e**"), it's probably time to give up on them. I asked them to politely remove me from their mailing list so I'd no longer get any emails from them. I thought this'd be the end of it, but last week, I received the following email from them:

> What goes up won’t go down.{title="That's what she said"}
> 
> At Poundhost/VAServ we know that if your site is not up, your profits go down. Which is why we recently migrated your website onto a more secure Linux server platform.
> 
> However, threats are always evolving. To ensure that you are provided with the very best platform that’s reliable, secure, easier to manage, with greater interoperability and a substantially lower total cost of ownership, we recommend that you consider switching to Microsoft’s Hyper-V hosting platform.
> 
> Running on next generation virtualisation technologies Microsoft’s Hyper-V stores your data on a cluster of servers rather than one. So if a server is attacked, or goes down, the system automatically switches to the others. Thereby guaranteeing 100% uptime.
> 
> Migration is so simple you can do it yourself. However some of you may need to tweak or re-code your data beforehand to enable it to run on a Microsoft platform. Should you have any queries, call our contact centre on 01628 67 31 31.
> 
> Don’t delay though because we are prepared to offer a 10% discount to all those who migrate before 31st March 2009 using the coupon vdsmigrate.
> 
> See http://vds.poundhost.com for more information!

Ugh. Where do I begin?

* I migrated away from their services in September 2009, so they certainly did NOT "migrate [my] website"
* I wasn't actually even hosting a website with them to begin with, so they wouldn't have migrated a website at all. Not everyone uses VPSes only for websites, you know?
* How are Hyper-V virtual machines more secure and reliable than Linux equivalents? They're not even comparable with things like OpenVZ or Linux-VServer as they're totally different products for totally different requirements.
* What does Hyper-V have to do with "threats always evolving"? I'm absolutely certain that Windows is attacked at least as much (if not a lot more) than Linux is.
* 100% uptime is not guaranteed if all servers in the cluster go down (as happened when they were hacked last year)
* If people got a Linux virtual server originally, why would they want to spend significantly more on a Windows VPS? In my case, I used rsync to transfer backups and cPanel DNSONLY for hosting the DNS, things that don't work on Windows

I'm sure [I'm not the only former customer that got this email](http://blog.ergatides.com/linux/vaserve-at-it-again/). Did they just send it to everyone, regardless of whether they're a current customer or not? I asked them to remove my personal information when I left, so I'd consider this spam. I replied to the email asking them to remove my details from their system, and they replied saying they had done so, so we'll see. At least they could spell "guaranteed" correctly this time around, the FSCKVPS site had misspellings of it from their launch, and a lot of people told them about it, they still didn't fix them.

For what it's worth, I'm currently using a Core 2 Duo server at [HiVelocity](http://hivelocity.net/) for hosting all my sites, and the backup VPS is now at [PhotonVPS](http://photonvps.com/). I'd definitely recommend both companies :)

Yes yes, this isn't really a proper blog post. One will come eventually :D

Until next time,  

— Daniel

*[VPS]: Virtual Private Server
