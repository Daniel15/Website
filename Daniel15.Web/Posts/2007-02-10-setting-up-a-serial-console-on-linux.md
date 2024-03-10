---
id: 47
title: Setting up a serial console on Linux
published: true
publishedDate: 2007-02-10 21:45:11Z
lastModifiedDate: 2007-02-10 21:45:11Z
categories:
- Linux

---

<p>This tutorial will show you how to set up a serial console on a Linux system, and connect to it via a null modem cable. This is quite useful if your Linux server is in a headless configuration (no keyboard or monitor), as it allows you to easily get a console on the system if there are any problems with it (especially network problems, when SSH is not available). In the end, the GRUB menu will appear over the serial link, as will the bootup messages (output when booting the system). I'm using Debian Etch on the server and Ubuntu Edgy on my client, although this should work on any Linux distribution. </p>
<p><!--more--></p>
<h4>First steps</h4>
<p>One of the most important things we need to check that you do actually have a serial port on the server :). Take a look at the back of your server, and see if it has a 9-pin serial port. Most motherboards have either one or two serial ports. On the system, check to see that Linux is recognising the serial ports:</p>
<pre>root@server:~#  dmesg | grep tty
serial8250: ttyS0 at I/O 0x3f8 (irq = 4) is a 16550A
00:08: ttyS0 at I/O 0x3f8 (irq = 4) is a 16550A</pre>
<p>This shows that my system has one serial port, <tt>ttyS0</tt> (remember this for later).</p>
<h4>GRUB configuration </h4>
<p>The next step is to edit the GRUB configuration, so it sends its messages to the serial console. One of the most important things is to set a password, otherwise anyone can connect a serial cable, edit the GRUB configuration line while the system is booting (via the "e" key), and get root access. When a password is set, interactive menu editing will be disabled, unless the correct password is entered. To set the password, we first need to get the encrypted version of it.</p>
<p>Run <tt>grub</tt>, and use the "md5crypt" command to encrypt the password:</p>
<pre>grub&gt; md5crypt

Password: ********
Encrypted: $1$AlfMq1$FxRolxW5XvSLAOksiC7MD1</pre>
<p>Copy the encrypted version of the password (we need it for the next step), and then type <tt>quit</tt> to exit. </p>
<p>Now, we need to edit the GRUB configuration. Edit the /boot/grub/menu.lst file (by typing <tt>nano /boot/grub/menu.lst</tt>), and find this section:</p>
<pre>## password ['--md5'] passwd
   # If used in the first section of a menu file, disable all interactive editing
   # control (menu entry editor and command-line)  and entries protected by the
   # command 'lock'
   # e.g. password topsecret
   #      password --md5 $1$gLhU0/$aW78kHK1QfV3P2b2znUoe/
   # password topsecret
</pre>
<p>Below that, add:</p>
<pre>password --md5 $1$AlfMq1$FxRolxW5XvSLAOksiC7MD1
serial --unit=0 --speed=38400 --word=8 --parity=no --stop=1
terminal --timeout=10 serial console</pre>
<p>Replace <i>$1$AlfMq1$FxRolxW5XvSLAOksiC7MD1</i> with the encrypted form of <b>your</b> password. The second line tells GRUB to initialise the serial port at 38,400 bps (same speed as the standard console), 8 data bits, no parity, and 1 stop bit (basically, the standard settings). Note that the <b>--unit=0</b> means that it will use the <b>first</b> serial port (ttyS0). If you're using the <b>second</b> serial port (ttyS1), change it to<b> --unit=1</b>. The last line tells GRUB to show its menu on both the serial line <i>and</i> the console (monitor).</p>
<p>Now, we also need to edit the kernel sections, so that they output messages to the serial console. At the end of every kernel line, add <tt>console=tty0 console=ttyS0,38400n8</tt> (replace <tt>ttyS0</tt> with the correct serial port). In my case, it ended up looking like:</p>
<pre>title           Debian GNU/Linux, kernel 2.6.18-4-vserver-686
   root            (hd0,1)
   kernel          /vmlinuz-2.6.18-4-vserver-686 root=/dev/hda3 ro console=tty0 console=ttyS0,38400n8
   initrd          /initrd.img-2.6.18-4-vserver-686
   savedefault
title           Debian GNU/Linux, kernel 2.6.18-4-vserver-686 (single-user mode)
   root            (hd0,1)
   kernel          /vmlinuz-2.6.18-4-vserver-686 root=/dev/hda3 ro single console=tty0 console=ttyS0,38400n8
   initrd          /initrd.img-2.6.18-4-vserver-686
   savedefault
title           Debian GNU/Linux, kernel 2.6.18-3-686
   root            (hd0,1)
   kernel          /vmlinuz-2.6.18-3-686 root=/dev/hda3 ro console=tty0 console=ttyS0,38400n8
   initrd          /initrd.img-2.6.18-3-686
   savedefault
title           Debian GNU/Linux, kernel 2.6.18-3-686 (single-user mode)
   root            (hd0,1)
   kernel          /vmlinuz-2.6.18-3-686 root=/dev/hda3 ro single console=tty0 console=ttyS0,38400n8
   initrd          /initrd.img-2.6.18-3-686   savedefault&nbsp;</pre>
<p>Save and exit, by pressing CTRL+O (to "output", or save the file), Enter (to accept the file name) and CTRL+X (to actually exit).</p>
<h4>Allow logins over Serial Console  </h4>
<p>Now, the GRUB menu will appear over the serial connection, but we still aren't listening for logins over it (there's no "getty" running on it yet). Edit the <tt>/etc/inittab</tt><tt> file, and find this section:</tt></p>
<pre># Example how to put a getty on a serial line (for a terminal)
#
#T0:23:respawn:/sbin/getty -L ttyS0 9600 vt100
#T1:23:respawn:/sbin/getty -L ttyS1 9600 vt100
</pre>
<p>Below that (I don't like editing the default lines :P), add:</p>
<pre>T0:2345:respawn:/sbin/getty -L ttyS0 38400 vt100</pre>
<p>And that's all there is to it. Your server will now show the GRUB menu over the serial console, and also allow logons (once it has finished booting). </p>
<h4>Let's test it!</h4>
<p>Now that that's all done, we need to configure our client. I'm using GtkTerm on my laptop, although any terminal program should work (as long as it can use a serial port. On Windows, HyperTerminal should work). My laptop doesn't have a serial port, so I'm using a USB to Serial adapter I bought off eBay (it creates a <tt>ttyUSB0</tt> device). Set your terminal program to these settings:</p>
<ul>
<li><b>Port (Linux):</b> <tt>ttyS0</tt> or <tt>ttyS1</tt> (if your system has a serial port), or <tt>ttyUSB0</tt> (if you're using a USB to Serial converter).</li>
<li><b>Port (Windows):</b> COM1 or COM2 </li>
<li><b>Bits per second:</b> 38400</li>
<li><b>Data bits:</b> 8  </li>
<li><b>Parity: </b>None</li>
<li><b>Stop bits:</b> 1</li>
<li><b>Flow control:</b> None, although hardware (RTS/CTS) should work properly </li>
</ul>
<p>Restart the server (probably from a SSH connection, or however you edited the GRUB config above), and then connect the null modem cable as it's starting (ie. at the BIOS screen). Press any key when prompted, and you'll get something like: </p>
<p><img src='http://www.daniel15.com/blog/wp-content/uploads/2007/02/grub-resized.jpg' alt='Serial console howto - GRUB menu' /><br />
This means that GRUB is working fine :). Press enter, and it should boot, showing all messages in the terminal window. Once it boots, it will look something like:</p>
<p><img src='http://www.daniel15.com/blog/wp-content/uploads/2007/02/booted-resized.jpg' alt='Serial console howto - booted' /><br />
Finally, log in, and check that it works fine:</p>
<p><img src='http://www.daniel15.com/blog/wp-content/uploads/2007/02/loggedin-resized.jpg' alt='Serial console howto - Logged in' /><br />
 </p>
<p>Congratulations, everything is set up and working fine.</p>
<p>Hope you enjoyed this tutorial! :) </p>

