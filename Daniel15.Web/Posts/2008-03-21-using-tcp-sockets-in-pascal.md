---
id: 69
title: Using TCP sockets in Pascal, connect to remote servers
published: true
publishedDate: 2008-03-21 03:59:30Z
lastModifiedDate: 2008-03-21 03:59:30Z
categories:
- HIT1301 Portfolio
- pascal
- Programming

---

<p>TCP sockets in Pascal are generally hard to use; Free Pascal doesn't come with any high-level socket libraries by default, only a relatively low-level <tt>socket</tt> library. Some external libraries are available to make using sockets with Pascal easier, and one of these libraries is <strong>Synapse</strong>. Synapse is an easy-to-use socket library for Pascal, and in this blog post I'll try to show how to use Synapse to connect to a remote server and send/receive data from it. </p>
<p><!--more-->From <a href="http://synapse.ararat.cz/">the official Synapse site</a>:</p>
<blockquote><p>
This project deals with network communication by means of blocking (synchronous) sockets or with limited non-blocking mode. This project not using asynchronous sockets! The Project contains simple low level non-visual objects for easiest programming without problems. (no required multithread synchronisation, no need for windows message processing,…) Great for command line utilities, visual projects, NT services
</p></blockquote>
<h3>"Installing" it</h3>
<p>Firstly, you'll want to <a href="http://synapse.ararat.cz/doku.php/download">download the stable release of Synapse</a>, and place them somewhere. At the time of writing, the latest Synapse version is release number <strong>38</strong>. Once you've downloaded it, extract the files somewhere (it doesn't matter where you extract them to, as long as you remember the directory name. I'd suggest to create a directory for all your Free Pascal library code). Next, we need to edit the config file, so that Free Pascal can find these libraries. Open your Free Pascal configuration file (on Linux, this is at<tt>/etc/fpc.cfg</tt>. On Windows, this <em>should</em> be in the directory you installed Free Pascal to). Search for this:<br />
<pre class="brush: plain"># searchpath for libraries</pre><br />
Right before that, add <tt>-Fu</tt> followed by the path to the directory you made earlier. In my case, I added:<br />
<pre class="brush: plain">-Fu/home/daniel/fpc</pre></p>
<h3>Using it in your code</h3>
<p>In most cases, you'll be using the <tt>TTCPBlockSocket</tt> class. This is included in the <tt>blcksock</tt> unit, so add this unit to the <em>uses</em> clause of your application:<br />
<pre class="brush: pascal">uses blcksock;</pre></p>
<h3>Example</h3>
<h4>Connecting to a server</h4>
<p>This is probably the most common way you'd use a socket &mdash; Connecting directly to another server. The functionality for this is contained in the <tt>TTCPBlockSocket</tt> class. Firstly, we need to define a variable to store the socket in:<br />
<pre class="brush: pascal">
var
	sock: TTCPBlockSocket;
</pre><br />
And then we need to actually create the socket:<br />
<pre class="brush: pascal">
sock := TTCPBlockSocket.Create;
</pre><br />
This creates a socket named <strong>sock</strong> that we're able to use. The next step is to connect to the remote server, using the <a href="http://synapse.ararat.cz/doc/help/blcksock.TTCPBlockSocket.html#Connect">Connect</a> method:<br />
<pre class="brush: pascal">
sock.Connect('66.79.183.71', '80');
// Was there an error?
if sock.LastError <> 0 then
begin
	writeLn('Could not connect to server.');
	halt(1);
end;
</pre><br />
At this point, the connection to the remote server has been established, and we may send and receive data.</p>
<h4>Sending Data</h4>
<p>Sending data is done via the <a href="http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#SendString">SendString</a> method. Note that this is slightly different to some other languages; it <strong>does not</strong> add a carriage return and linefeed to the end of the line, you have to add this manually if required.</p>
<p><pre class="brush: pascal">
sock.SendString('GET /blog/ HTTP/1.1'#13#10'Host: www.daniel15.com'#13#10#13#10);
</pre></p>
<h4>Receiving Data</h4>
<p>There are several methods for receiving data, but the two main ones are <tt><a href="http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#RecvString">RecvString</a></tt> and <tt><a href="http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#RecvPacket">RecvPacket</a></tt>. RecvString reads a single string (terminated by a carriage return and linefeed) from the socket, and returns this string <strong>without</strong> the carriage return. RecvPacket reads all data waiting to be read, and returns it unmodified (all carriage returns and linefeeds will still be there). Both commands take one parameter: A timeout. If the socket doesn't contain any data within this timeout, it returns a blank string.</p>
<p><pre class="brush: pascal">
buffer := sock.RecvPacket(2000);
</pre></p>
<h4>Putting it all together</h4>
<p>Here's an example application that connects to a web server, does a simple HTTP request, and writes the response to the console:<br />
<pre class="brush: pascal">
program TestApp;

uses
	blcksock;
	
var
	sock: TTCPBlockSocket;
	
procedure Main();
var
	buffer: String = '';
begin
	sock := TTCPBlockSocket.Create;
	
	sock.Connect('66.79.183.71', '80');
	// Was there an error?
	if sock.LastError <> 0 then
	begin
		writeLn('Could not connect to server.');
		halt(1);
	end;
	// Send a HTTP request
	sock.SendString('GET /blog/ HTTP/1.1'#13#10'Host: www.daniel15.com'#13#10#13#10);
	
	// Keep looping...
	repeat
		buffer := sock.RecvPacket(2000);
		write(buffer);
	// ...until there's no more data.
	until buffer = '';
end;


begin
	Main();
end.
</pre><br />
The loop is needed because the data may come in multiple packets.</p>
<p>Not that this is <strong>not</strong> really a good example, as there's a HTTP library built-in to Synapse. The in-built HTTP library has several advantages, including the ability to use HTTP proxies. Perhaps I'll cover that in a future blog post :)</p>

