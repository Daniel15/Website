---
id: 343
title: Fixing GPG "Inappropriate ioctl for device" errors
published: true
publishedDate: 2016-11-06 16:49:00Z
lastModifiedDate: 2016-11-06 16:49:00Z
categories:
- Linux

---

<p>Recently I moved all my sites onto a new server. I use Duplicity and Backupninja to perform weekly backups of my server. While configuring backups on the new server, I kept encountering a strange error:</p>
<pre class="brush: plain">Error: gpg: using "D5673F3E" as default secret key for signing
Error: gpg: signing failed: Inappropriate ioctl for device
Error: gpg: [stdin]: sign+encrypt failed: Inappropriate ioctl for device</pre>

<p>It turns out this error is due to changes in GnuPG 2.1, which only recently landed in Debian Testing. The error occurs because GnuPG 2.1 by default ignores passphrases passed in via environment variables or stdin, and is trying to show a pinentry prompt. "Inappropriate ioctl for device" is thrown because the Backupninja script is not running through a TTY, so there's no way to actually render the prompt.</p>
<p>To solve the problem, you need to enable <tt>loopback</tt> pinentry mode. Add this to <tt>~/.gnupg/gpg.conf</tt>:</p>
<pre class="brush: plain">use-agent
pinentry-mode loopback</pre>
<p>And add this to <tt>~/.gnupg/gpg-agent.conf</tt>, creating the file if it doesn't already exist:</p>
<pre class="brush: plain">allow-loopback-pinentry</pre>
<p>Then restart the agent with <tt>echo RELOADAGENT | gpg-connect-agent</tt> and you should be good to go!</p>
