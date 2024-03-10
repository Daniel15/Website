---
id: 347
title: Netdata causing load increase every ~20 minutes
published: true
publishedDate: 2019-01-20 22:55:23Z
lastModifiedDate: 2019-01-20 22:55:23Z
categories:
- Linux

---

<p>So I recently encountering a strange issue on two of my servers. I noticed that the load average was increasing approximately every 20 minutes:</p>
<p><img src="/blog-content/2019/high-load-avg.png" alt="Load average graph" /></p>
<p>I suspected a cronjob, but I don't have any cronjobs that run every 20 mins. Also, CPU usage doesn't actually increase during that period:</p>
<p><img src="/blog-content/2019/low-cpu.png" alt="Low CPU usage graph" /></p>
<p>I did some digging and it took a long time to work out what was happening.</p>
<h3>So What Is Load Average Anyway?</h3>
<p>"Load average" is a term used to describe a measure of how "busy" a system is. Unix-like systems (including Linux) show a load average as three numbers, representing the system load over the previous one minute, five minutes, and fifteen minutes. These numbers represent the number of processes that are using the CPU right now, waiting to use the CPU, or waiting for disk I/O. <a href="https://en.wikipedia.org/wiki/Load_(computing)">The Wikipedia article</a> has more details.</p>
<p>Linux updates the load average every 5 seconds. In fact, it actually updates every 5 seconds plus one "tick". The reason for this is to <a href="https://github.com/torvalds/linux/commit/0c2043abefacac97b6d01129c1123a466c95b7c1">avoid coinciding with other five-second timers</a>:</p>
<blockquote>It turns out that there are a few other five-second timers in the
kernel, and if the timers get in sync, the load-average can get artificially inflated by events that just happen to coincide. So just offset the load average calculation it by a timer tick.</blockquote>
<p>From the Linux kernel code:</p>
<p><a href="https://github.com/torvalds/linux/blob/v4.18/include/linux/sched/loadavg.h#L20">sched/loadavg.h</a>:
<pre class="brush: plain">
#define LOAD_FREQ	(5*HZ+1) /* 5 sec intervals */
</pre>
<p><a href="https://github.com/torvalds/linux/blob/345671ea0f9258f410eb057b9ced9cefbbe5dc78/kernel/sched/loadavg.c#L12-L26">sched/loadavg.c</a></p>
<pre class="brush: plain">
 * The global load average is an exponentially decaying average of nr_running +
 * nr_uninterruptible.
 *
 * Once every LOAD_FREQ:
 *
 *   nr_active = 0;
 *   for_each_possible_cpu(cpu)
 *	nr_active += cpu_of(cpu)->nr_running + cpu_of(cpu)->nr_uninterruptible;
 *
 *   avenrun[n] = avenrun[0] * exp_n + nr_active * (1 - exp_n)
</pre>
<tt>HZ</tt> is the kernel timer frequency, which is defined when compiling the kernel. On my system, it's <tt>250</tt>:
<pre class="brush: plain">
% grep "CONFIG_HZ=" /boot/config-$(uname -r)
CONFIG_HZ=250
</pre>
<p>This means that every 5.004 seconds (5 + 1/250), Linux calculates the load average. It checks how many processes are actively running plus how many processes are in uninterruptable wait (eg. waiting for disk IO) states, and uses that to compute the load average, smoothing it exponentially over time.</p>
<p>Say you have a process that starts a bunch of subprocesses every second. For example, Netdata collecting data from some apps. Normally, the process will be very fast and won't overlap with the load average check, so everything is fine. However, every 1251 seconds (5.004 * 250), the load average update interval will be an exact multiple of one second (that is, 1251 is the least common multiple of 5.004 and 1). 1251 seconds is 20.85 minutes, which is exactly the interval I was seeing the load average increase. My educated guess here is that every 20.85 minutes, Linux is checking the load average at the exact time that several processes are being started and are in the queue to run.</p>
<p>I confirmed this by disabling netdata and manually watching the load average:</p>
<pre class="brush: plain">
while true; do uptime; sleep 5; done
</pre>
<p>After 1.5 hours, I did not see <strong>any</strong> similar spikes. The spikes <strong>only</strong> occur when Netdata is running.</p>
<p>It turns out other people have hit similar issues in the past, albeit with different intervals. The following posts were extremely helpful:</p>
<ul>
<li><a href="https://blog.avast.com/investigation-of-regular-high-load-on-unused-machines-every-7-hours">Investigation of regular high load on unused machines every 7 hours</a></li>
<li><a href="https://mackerel.io/blog/entry/tech/high-loadavg-every-7-hours">Understanding why the Linux loadavg rises every 7 hours</a></li>
<li><a href="https://github.com/influxdata/telegraf/issues/3465">Telegraf - high load average every 1h 45m</a></li>
<li><a href="https://github.com/torvalds/linux/commit/0c2043abefacac97b6d01129c1123a466c95b7c1">Linux commit that changed load average calculation to be every 5 seconds + 1 tick, instead of exactly every 5 seconds</a></li>
</ul>

<p>In the end, I'm not sure if I'd call this a bug, but perhaps netdata could implement some jitter so that it doesn't perform checks every one second exactly. I posted <a href="https://github.com/netdata/netdata/issues/5234">a GitHub issue</a> so their developers can take a look.</p>
