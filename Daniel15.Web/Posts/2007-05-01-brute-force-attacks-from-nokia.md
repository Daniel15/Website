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

<p>I checked my email inbox this morning, and guess what I found? The firewall (<a href="http://configserver.com/cp/csf.html">ConfigServer Security and Firewall</a>) on a server I help run blocked a brute-force attack from Nokia:</p>
<blockquote><p>
Time:     Tue May  1 02:28:18 2007<br />
IP:       63.97.248.34 (<b>machine34.nokia.com</b>)<br />
Failures: 5 (sshd)<br />
Interval: 135 seconds<br />
Blocked:  Yes</p>
<p>Log entries:</p>
<p>May  1 02:28:08 blue sshd[9363]: Failed password for root from ::ffff:63.97.248.34 port 56057 ssh2<br />
May  1 07:28:08 blue sshd[9364]: Failed password for root from ::ffff:63.97.248.34 port 56057 ssh2<br />
May  1 02:28:11 blue sshd[9368]: Failed password for root from ::ffff:63.97.248.34 port 56436 ssh2<br />
May  1 07:28:11 blue sshd[9369]: Failed password for root from ::ffff:63.97.248.34 port 56436 ssh2<br />
May  1 02:28:13 blue sshd[9370]: Failed password for root from ::ffff:63.97.248.34 port 56591 ssh2
</p></blockquote>
<p>Just thought it was funny :P<br />
(oh yeah, and I will report it to them!)</p>

