---
id: 348
title: WireGuard on OpenVZ/LXC
published: true
publishedDate: 2019-07-02 22:33:58Z
lastModifiedDate: 2019-07-02 22:33:58Z
categories:
- Linux

---

# This post is originally from Daniel15's Blog at https://d.sb/2019/07/wireguard-on-openvz-lxc

---

[WireGuard](https://www.wireguard.com/){rel=nofollow} is an exciting, new, extremely simple VPN system that uses state-of-the-art cryptography. Its Linux implementation runs in the kernel, which provides a significant performance boost compared to traditional userspace VPN implementations

The WireGuard kernel module is great, but sometimes you might not be able to install new kernel modules. One example scenario is on a VPS that uses OpenVZ or LXC. For these cases, we can use [wireguard-go](https://git.zx2c4.com/wireguard-go/about/){rel=nofollow}, a userspace implementation of WireGuard. This is the same implementation used on MacOS, Windows, and the WireGuard mobile apps. This implementation is slower than the kernel module, but still plenty fast.

This post focuses on Debian, however the instructions should mostly work on other Linux distros too.

## Install WireGuard Tools
We need to install the WireGuard tools (`wg-quick`). On Debian, you can run this as root:

```plain
echo "deb http://deb.debian.org/debian/ unstable main" > /etc/apt/sources.list.d/unstable.list
printf 'Package: *\nPin: release a=unstable\nPin-Priority: 90\n' > /etc/apt/preferences.d/limit-unstable
apt update
apt install wireguard-tools --no-install-recommends
```
(see [the WireGuard site](https://www.wireguard.com/install/){rel=nofollow} for instructions if you're not on Debian)

## Install Go
Unfortunately, since wireguard-go is not packaged for Debian, we need to compile it ourselves. To compile it, we first need to install the latest version of the Go programming language (currently version 1.13.4):

```plain
cd /tmp
wget https://dl.google.com/go/go1.13.4.linux-amd64.tar.gz
tar zvxf go1.13.4.linux-amd64.tar.gz
sudo mv go /opt/go1.13.4
sudo ln -s /opt/go1.13.4/bin/go /usr/local/bin/go
```
Now, running `go version` should show the version number.

## Compile wireguard-go
Now that we've got Go, we can download and compile wireguard-go. Download the [latest release version](https://git.zx2c4.com/wireguard-go/refs/tags):

```plain
cd /usr/local/src
wget https://git.zx2c4.com/wireguard-go/snapshot/wireguard-go-0.0.20191012.tar.xz
tar xvf wireguard-go-0.0.20191012.tar.xz
cd wireguard-go-0.0.20191012
```
If you are on a system with limited RAM (such as a 256 MB or lower "LowEndSpirit" VPS), you will need to do a small tweak to the wireguard-go code to make it use less RAM. Open `device/queueconstants_default.go` and replace this:

```plain
MaxSegmentSize             = (1 << 16) - 1 // largest possible UDP datagram
	PreallocatedBuffersPerPool = 0 // Disable and allow for infinite memory growth
```

With these values (taken from `device/queueconstants_ios.go`):

```plain
MaxSegmentSize             = 1700
	PreallocatedBuffersPerPool = 1024
```

This will make it use a fixed amount of RAM (~20 MB max), rather than allowing memory usage to grow infinitely.

Now we can compile it:

```plain
make
# "Install" it
sudo cp wireguard-go /usr/local/bin
```
Running `wireguard-go --version` should work and show the version number.

If you have multiple VPSes that use the same OS version and architecture (eg. Debian 10, 64-bit), you can compile it on one of them and then just copy the `wireguard-go` binary to all the others.

## Configuration
### wg0.conf
You'll need to configure `/etc/wireguard/wg0.conf` to contain the configuration for your peer. This post won't go into significant detail about this; please refer to another general WireGuard guide ([like this one](https://www.stavros.io/posts/how-to-configure-wireguard/){rel=nofollow}) for more details. The basic jist is that you need to run:

```plain
wg genkey | tee privatekey | wg pubkey > publickey
```
to generate a public/private key pair for each peer, then configure the `[Interface]` with the private key for the peer, and a `[Peer]` section for each peer that can connect to it.

Your `wg0.conf` should end up looking something like:

```plain
[Interface]
Address = 10.123.0.2
PrivateKey = 12345678912345678912345678912345678912345678
ListenPort = 51820

[Peer]
PublicKey = 987654321987654321987654321987654321987654321
AllowedIPs = 10.123.0.1/32
Endpoint = 198.51.100.1:51820
```

### systemd
We need to modify the systemd unit to pass the `WG_I_PREFER_BUGGY_USERSPACE_TO_POLISHED_KMOD` flag to wireguard-go, to allow it to run on Linux. Open `/lib/systemd/system/wg-quick@.service`, find:

```plain
Environment=WG_ENDPOINT_RESOLUTION_RETRIES=infinity
```
and add this line directly below:

```plain
Environment=WG_I_PREFER_BUGGY_USERSPACE_TO_POLISHED_KMOD=1
```

Finally, enable and start the systemd service:

```plain
systemctl enable wg-quick@wg0
systemctl start wg-quick@wg0
```
Enabling the systemd service will connect the VPN on boot, and starting the systemd service will connect it right now.

## You're Done
Now, everything should be working! You can check the status of `wg-quick` by running `systemctl status wg-quick@wg0`, which should return something like:

```plain
● wg-quick@wg0.service - WireGuard via wg-quick(8) for wg0
   Loaded: loaded (/lib/systemd/system/wg-quick@.service; enabled; vendor preset: enabled)
   Active: active (exited) since Mon 2019-07-01 06:30:30 UTC; 1 day 22h ago
```
Running `wg` will give you a list of all the peers, and some details about them:

```plain
interface: wg0
  public key: 987654321987654321987654321987654321987654321
  private key: (hidden)
  listening port: 38917

peer: 987654321987654321987654321987654321987654321
  endpoint: 198.51.100.1:51820
  allowed ips: 10.123.0.1/32
  latest handshake: 1 day, 22 hours, 59 minutes, 34 seconds ago
  transfer: 2.75 KiB received, 2.83 KiB sent
```