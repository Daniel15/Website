---
id: 42
title: Linux-Vserver on Debian Etch, the easy way
published: true
publishedDate: 2006-11-21 21:31:56Z
lastModifiedDate: 2006-11-21 21:31:56Z
categories:
- Linux

---

<p>In this tutorial, I'll show you how to install Linux-Vserver on Debian Testing (Etch), the easy way. This was the first tutorial I posted to HowtoForge.com, so please tell me if you like it or not. You may find it a bit verbose, as I try to explain things in enough detail so that everyone understands what I mean :-) </p>
<p>What is Linux-Vserver, you ask? It's simple. Basically, Linux-Vserver is an open-source system used to separate a single physical server into multiple virtual servers. From the Linux-Vserver website:</p>
<p>"Linux-VServer allows you to create virtual private servers and security contexts which operate like a normal Linux server, but allow many independent servers to be run simultaneously in one box at full speed. All services, such as ssh, mail, Web, and databases, can be started on such a VPS, without modification, just like on any real server. Each virtual server has its own user account database and root password and doesn't interfere with other virtual servers."</p>
<p><!--more--></p>
<p>Two terms you will need to know are: </p>
<ul>
<li><b>Host System</b>: This is the main system (the physical server) which you install Linux-Vserver onto. </li>
<li><b>Guest System</b>: These are the virtual systems you create using the Linux-Vserver utilities. They run on top of the Host System, and are all isolated from each other. </li>
</ul>
<p>Most tutorials mention that you need to compile your own kernel in order to use Linux-Vserver. However, Debian Etch includes a Linux-Vserver kernel as standard, so you don't need to compile it yourself. This has the advantage of being easier and faster to install, and it's easy to keep up-to-date with security updates. </p>
</p>
<h4> First Steps</h4>
<p>The first thing you'll need to do is have a system with a fresh installation of Debian Etch. To do so, download the Debian Testing CD from <a target="_blank" href="http://www.debian.org/">http://www.debian.org/</a>, and install the base system only. After you've installed the base system, the next step is to make sure that the repositories are enabled. Firstly, make sure you're the superuser (the prompt is a <b>#</b>, not a <b>$</b>). If not, become the superuser (root):</p>
<pre>$ su
#</pre>
<p>Once you're sure you're root, we need to edit <tt>/etc/apt/sources.list</tt>: </p>
<pre># nano /etc/apt/sources.list</pre>
<p>Make sure a line similar to this is in the file:</p>
<p><tt>deb ftp://ftp.au.debian.org/debian etch main contrib non-free </tt></p>
<p>If it's not, add it in. Press CTRL+O and hit ENTER to save the file, and then press CTRL+X to exit.<br />   After this, we need to update the package list, so type the following command:</p>
<p><tt># apt-get update </tt></p>
<h3>The Packages</h3>
<p>Now that we've updated the package list, the next step is to install all the required packages. The packages required by Linux-Vserver are: </p>
<ul>
<li><b>linux-image-vserver-686 </b>- This is the actual kernel</li>
<li><b>util-vserver</b> - These are the utilities used to administer the guests</li>
<li><b>vserver-debiantools </b>- These are special Vserver tools for Debian, used to create and duplicate Debian guest systems.</li>
<li><b>ssh</b> - This is probably already installed, but just in case it isn't </li>
</ul>
<p>So, let's install them:</p>
<p><tt> # apt-get install linux-image-vserver-686 util-vserver vserver-debiantools ssh</tt></p>
<p>Once that's done, we need to reboot:</p>
<p><tt># reboot</tt></p>
<p>When the GRUB menu appears, make sure you choose the vserver kernel. Once your system boots, you'll be running the new kernel. You may check this by running</p>
<pre># uname -r
2.6.16-2-vserver-686</pre>
<p>Notice how the output has 'vserver' in it? This means you're running the VServer kernel.</p>
<p>Guess what? That's the <i>whole</i> installation!  Everything is now setup and ready to use :-). There's one very important thing to do though: Create the guest systems. </p>
</p>
<h4>Creating a virtual server (guest) </h4>
<p>So, now that Linux-Vserver is set up and ready to use, we need to create a guest system. On a Debian system, you may do so using the <tt>newvserver</tt> command. The syntax of this command is:</p>
<pre>newvserver --vsroot /var/lib/vservers/ --hostname [Hostname] \
 --domain [Domain] --ip [IP Address]/[CIDR Range] \
 --dist etch --mirror [Debian Mirror] --interface [Net Interface]</pre>
<p><i>(the backslashes at the end of the lines mean that it continues onto the next line. You may copy it as-is [with the backslashes], or put the whole command on one line [and exclude the backslashes], as I do below) </i></p>
<p>The command line arguments are: </p>
<ul>
<li><b>vsroot</b> - This is where the files for the guests are kept. On a default installation, this will be at /var/lib/vservers. Alternatively, some people create a  separate partition for their vservers. </li>
<li><b>Hostname</b> - The hostname of the system (eg. test1) </li>
<li><b>Domain</b> - The domain of the system. This is usually the same as the domain you chose for the host system (eg. dan-network.local. This doesn't need to be real, it's only used internally.) </li>
<li><b>IP Address</b>- The IP address for the guest system (eg. 10.1.1.7)</li>
<li><b>CIDR Range </b>- The CIDR Range for your local network. For a 10.x.x.x network, this is usually /8. For a 172.16.x.x network, this is usually /16. For a 192.168.x.x network, this is usually /24. If your network is subnetted, this will be different. When in doubt, choose /8 :)</li>
<li><b>Dist</b> -  The distribution to use. For the purposes of this exercise, we use etch.</li>
<li><b>Debian Mirror</b> - The Debian mirror you use (when in doubt, choose ftp://ftp.debian.org/debian) </li>
<li><b>Interface</b> - Your network interface, if it's not eth0 (eg. eth1).</li>
</ul>
<p><span style="color: red">Note (added 5th July 2007): Some people were getting confused here. Usually, if your server is at a data centre and you have multiple public IP addresses, you should use one of your public IP addresses here (not an internal one). If the server is on a local network, use an internal IP address.</span></p>
<p>So, let's make a test server. The settings for our test guest are like so:</p>
<ul>
<li>Hostname: test1</li>
<li>Domain: example.com</li>
<li>IP Address: 10.1.1.7</li>
<li>CIDR Range: /8</li>
<li>Debian Mirror: http://ftp.au.debian.org/debian/</li>
<li>Interface: eth1</li>
</ul>
<p>Let's go!</p>
<p><tt># newvserver --vsroot /var/lib/vservers/ --hostname <i>test1</i> --domain example.com --ip <i>10.1.1.7/8</i> --dist etch --mirror <i>http://ftp.au.debian.org/debian/</i> --interface eth1 </tt></p>
<p>This will begin a Debian net install, which will only take a few minutes. Once the packages are all downloaded and installed, the Debian base setup will come up. This will prompt you for your time zone, and also ask for a root password. Once you've completed this, the VServer will be ready to use. </p>
</p>
<h4>Entering the guest</h4>
<p>OK, so we've set up the guest, now to do anything useful, we need to start the guest, and enter into its context. To do so, we use the 'vserver' command. The basic syntax for it is:</p>
<p><tt>vserver &lt;name&gt; [start | stop | restart | enter] </tt></p>
<p>So, to start and enter the guest we created earlier, type the following:</p>
<p><tt>vserver test1 start; vserver test1 enter </tt></p>
<p>The output will be a bit like:</p>
<pre>root@server1:/home/daniel# vserver test1 start; vserver test1 enter
Starting system log daemon: syslogd.
Starting internet superserver: no services enabled, inetd not started.
Starting periodic command scheduler....
test1:/#</pre>
<p>   We're now 'inside' the virtual server. You can run any command you would normally run. Note that this is a <i>very </i>basic installation, so you should use apt to install whatever you want.</p>
<h4>Other Stuff</h4>
<p>This tutorial doesn't cover everything, it's only meant to be a guide on beginning to use Linux-Vserver. The rest is  up to your imagination... Have fun!</p>

