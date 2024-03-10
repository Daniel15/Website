---
id: 348
title: WireGuard on OpenVZ/LXC
published: true
publishedDate: 2019-07-02 22:33:58Z
lastModifiedDate: 2019-07-02 22:33:58Z
categories:
- Linux

---

<p><a href="https://www.wireguard.com/" rel="nofollow">WireGuard</a> is an exciting, new, extremely simple VPN system that uses state-of-the-art cryptography. Its Linux implementation runs in the kernel, which provides a significant performance boost compared to traditional userspace VPN implementations</p>
<p>The WireGuard kernel module is great, but sometimes you might not be able to install new kernel modules. One example scenario is on a VPS that uses OpenVZ or LXC. For these cases, we can use <a href="https://git.zx2c4.com/wireguard-go/about/" rel="nofollow">wireguard-go</a>, a userspace implementation of WireGuard. This is the same implementation used on MacOS, Windows, and the WireGuard mobile apps. This implementation is slower than the kernel module, but still plenty fast.</p>
<p>This post focuses on Debian, however the instructions should mostly work on other Linux distros too.</p>

<h3>Install WireGuard Tools</h3>
<p>We need to install the WireGuard tools (<code>wg-quick</code>). On Debian, you can run this as root:</p>
<pre class="brush: plain">
echo "deb http://deb.debian.org/debian/ unstable main" > /etc/apt/sources.list.d/unstable.list
printf 'Package: *\nPin: release a=unstable\nPin-Priority: 90\n' > /etc/apt/preferences.d/limit-unstable
apt update
apt install wireguard-tools --no-install-recommends
</pre>
<p>(see <a href="https://www.wireguard.com/install/" rel="nofollow">the WireGuard site</a> for instructions if you're not on Debian)</p>

<h3>Install Go</h3>
<p>Unfortunately, since wireguard-go is not packaged for Debian, we need to compile it ourselves. To compile it, we first need to install the latest version of the Go programming language (currently version 1.13.4):
<pre class="brush: plain">
cd /tmp
wget https://dl.google.com/go/go1.13.4.linux-amd64.tar.gz
tar zvxf go1.13.4.linux-amd64.tar.gz
sudo mv go /opt/go1.13.4
sudo ln -s /opt/go1.13.4/bin/go /usr/local/bin/go
</pre>
<p>Now, running <code>go version</code> should show the version number.</p>

<h3>Compile wireguard-go</h3>
<p>Now that we've got Go, we can download and compile wireguard-go. Download the <a href="https://git.zx2c4.com/wireguard-go/refs/tags">latest release version</a>:</p>
<pre class="brush: plain">
cd /usr/local/src
wget https://git.zx2c4.com/wireguard-go/snapshot/wireguard-go-0.0.20191012.tar.xz
tar xvf wireguard-go-0.0.20191012.tar.xz
cd wireguard-go-0.0.20191012
</pre>
<p>If you are on a system with limited RAM (such as a 256 MB or lower "LowEndSpirit" VPS), you will need to do a small tweak to the wireguard-go code to make it use less RAM. Open <code>device/queueconstants_default.go</code> and replace this:</p>
<pre class="brush: plain">
	MaxSegmentSize             = (1 << 16) - 1 // largest possible UDP datagram
	PreallocatedBuffersPerPool = 0 // Disable and allow for infinite memory growth
</pre>

<p>With these values (taken from <code>device/queueconstants_ios.go</code>):</p>
<pre class="brush: plain">
	MaxSegmentSize             = 1700
	PreallocatedBuffersPerPool = 1024
</pre>

<p>This will make it use a fixed amount of RAM (~20 MB max), rather than allowing memory usage to grow infinitely.</p>
<p>Now we can compile it:</p>

<pre class="brush: plain">
make
# "Install" it
sudo cp wireguard-go /usr/local/bin
</pre>
<p>Running <code>wireguard-go --version</code> should work and show the version number.</p>
<p>If you have multiple VPSes that use the same OS version and architecture (eg. Debian 10, 64-bit), you can compile it on one of them and then just copy the `wireguard-go` binary to all the others.</p>

<h3>Configuration</h3>
<h4>wg0.conf</h4>
<p>You'll need to configure <code>/etc/wireguard/wg0.conf</code> to contain the configuration for your peer. This post won't go into significant detail about this; please refer to another general WireGuard guide (<a href="https://www.stavros.io/posts/how-to-configure-wireguard/" rel="nofollow">like this one</a>) for more details. The basic jist is that you need to run:</p>
<pre class="brush: plain">
wg genkey | tee privatekey | wg pubkey > publickey
</pre>
<p>to generate a public/private key pair for each peer, then configure the <code>[Interface]</code> with the private key for the peer, and a <code>[Peer]</code> section for each peer that can connect to it.</p>

<p>Your <code>wg0.conf</code> should end up looking something like:</p>
<pre class="brush: plain">
[Interface]
Address = 10.123.0.2
PrivateKey = 12345678912345678912345678912345678912345678
ListenPort = 51820

[Peer]
PublicKey = 987654321987654321987654321987654321987654321
AllowedIPs = 10.123.0.1/32
Endpoint = 198.51.100.1:51820
</pre>

<h4>systemd</h4>
<p>We need to modify the systemd unit to pass the <code>WG_I_PREFER_BUGGY_USERSPACE_TO_POLISHED_KMOD</code> flag to wireguard-go, to allow it to run on Linux. Open <code>/lib/systemd/system/wg-quick@.service</code>, find:
<pre class="brush: plain">
Environment=WG_ENDPOINT_RESOLUTION_RETRIES=infinity
</pre>
<p>and add this line directly below:</p>
<pre class="brush: plain">
Environment=WG_I_PREFER_BUGGY_USERSPACE_TO_POLISHED_KMOD=1
</pre>

<p>Finally, enable and start the systemd service:</p>
<pre class="brush: plain">
systemctl enable wg-quick@wg0
systemctl start wg-quick@wg0
</pre>
<p>Enabling the systemd service will connect the VPN on boot, and starting the systemd service will connect it right now.</p>

<h3>You're Done</h3>
<p>Now, everything should be working! You can check the status of <code>wg-quick</code> by running <code>systemctl status wg-quick@wg0</code>, which should return something like:</p>
<pre class="brush: plain">
● wg-quick@wg0.service - WireGuard via wg-quick(8) for wg0
   Loaded: loaded (/lib/systemd/system/wg-quick@.service; enabled; vendor preset: enabled)
   Active: active (exited) since Mon 2019-07-01 06:30:30 UTC; 1 day 22h ago
</pre>
<p>Running <code>wg</code> will give you a list of all the peers, and some details about them:</p>
<pre class="brush: plain">
interface: wg0
  public key: 987654321987654321987654321987654321987654321
  private key: (hidden)
  listening port: 38917

peer: 987654321987654321987654321987654321987654321
  endpoint: 198.51.100.1:51820
  allowed ips: 10.123.0.1/32
  latest handshake: 1 day, 22 hours, 59 minutes, 34 seconds ago
  transfer: 2.75 KiB received, 2.83 KiB sent
</pre>
