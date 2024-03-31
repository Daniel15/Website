---
id: 54
title: Brute-force attacks from Nokia
published: true
publishedDate: 2007-05-01 23:15:35Z
lastModifiedDate: 2007-05-01 23:15:35Z
categories:
- Internet
- Linux

---

# This post is originally from Daniel15's Blog at https://d.sb/2007/05/brute-force-attacks-from-nokia

---

I checked my email inbox this morning, and guess what I found? The firewall ([ConfigServer Security and Firewall](http://configserver.com/cp/csf.html)) on a server I help run blocked a brute-force attack from Nokia:

> Time:     Tue May  1 02:28:18 2007  
> 
> IP:       63.97.248.34 (**machine34.nokia.com**)  
> 
> Failures: 5 (sshd)  
> 
> Interval: 135 seconds  
> 
> Blocked:  Yes
> 
> Log entries:
> 
> May  1 02:28:08 blue sshd[9363]: Failed password for root from ::ffff:63.97.248.34 port 56057 ssh2  
> 
> May  1 07:28:08 blue sshd[9364]: Failed password for root from ::ffff:63.97.248.34 port 56057 ssh2  
> 
> May  1 02:28:11 blue sshd[9368]: Failed password for root from ::ffff:63.97.248.34 port 56436 ssh2  
> 
> May  1 07:28:11 blue sshd[9369]: Failed password for root from ::ffff:63.97.248.34 port 56436 ssh2  
> 
> May  1 02:28:13 blue sshd[9370]: Failed password for root from ::ffff:63.97.248.34 port 56591 ssh2

Just thought it was funny :P  

(oh yeah, and I will report it to them!)

