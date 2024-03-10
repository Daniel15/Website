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

TCP sockets in Pascal are generally hard to use; Free Pascal doesn't come with any high-level socket libraries by default, only a relatively low-level `socket` library. Some external libraries are available to make using sockets with Pascal easier, and one of these libraries is **Synapse**. Synapse is an easy-to-use socket library for Pascal, and in this blog post I'll try to show how to use Synapse to connect to a remote server and send/receive data from it. 

<!--more-->
From [the official Synapse site](http://synapse.ararat.cz/):

> This project deals with network communication by means of blocking (synchronous) sockets or with limited non-blocking mode. This project not using asynchronous sockets! The Project contains simple low level non-visual objects for easiest programming without problems. (no required multithread synchronisation, no need for windows message processing,…) Great for command line utilities, visual projects, NT services

## "Installing" it
Firstly, you'll want to [download the stable release of Synapse](http://synapse.ararat.cz/doku.php/download), and place them somewhere. At the time of writing, the latest Synapse version is release number **38**. Once you've downloaded it, extract the files somewhere (it doesn't matter where you extract them to, as long as you remember the directory name. I'd suggest to create a directory for all your Free Pascal library code). Next, we need to edit the config file, so that Free Pascal can find these libraries. Open your Free Pascal configuration file (on Linux, this is at`/etc/fpc.cfg`. On Windows, this *should* be in the directory you installed Free Pascal to). Search for this:  

```plain
# searchpath for libraries
```  

Right before that, add `-Fu` followed by the path to the directory you made earlier. In my case, I added:  

```plain
-Fu/home/daniel/fpc
```

## Using it in your code
In most cases, you'll be using the `TTCPBlockSocket` class. This is included in the `blcksock` unit, so add this unit to the *uses* clause of your application:  

```pascal
uses blcksock;
```

## Example
### Connecting to a server
This is probably the most common way you'd use a socket — Connecting directly to another server. The functionality for this is contained in the `TTCPBlockSocket` class. Firstly, we need to define a variable to store the socket in:  

```pascal
var
	sock: TTCPBlockSocket;
```  

And then we need to actually create the socket:  

```pascal
sock := TTCPBlockSocket.Create;
```  

This creates a socket named **sock** that we're able to use. The next step is to connect to the remote server, using the [Connect](http://synapse.ararat.cz/doc/help/blcksock.TTCPBlockSocket.html#Connect) method:  

```pascal
sock.Connect('66.79.183.71', '80');
// Was there an error?
if sock.LastError <> 0 then
begin
	writeLn('Could not connect to server.');
	halt(1);
end;
```  

At this point, the connection to the remote server has been established, and we may send and receive data.

### Sending Data
Sending data is done via the [SendString](http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#SendString) method. Note that this is slightly different to some other languages; it **does not** add a carriage return and linefeed to the end of the line, you have to add this manually if required.

```pascal
sock.SendString('GET /blog/ HTTP/1.1'#13#10'Host: www.daniel15.com'#13#10#13#10);
```

### Receiving Data
There are several methods for receiving data, but the two main ones are `[RecvString](http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#RecvString)` and `[RecvPacket](http://synapse.ararat.cz/doc/help/blcksock.TBlockSocket.html#RecvPacket)`. RecvString reads a single string (terminated by a carriage return and linefeed) from the socket, and returns this string **without** the carriage return. RecvPacket reads all data waiting to be read, and returns it unmodified (all carriage returns and linefeeds will still be there). Both commands take one parameter: A timeout. If the socket doesn't contain any data within this timeout, it returns a blank string.

```pascal
buffer := sock.RecvPacket(2000);
```

### Putting it all together
Here's an example application that connects to a web server, does a simple HTTP request, and writes the response to the console:  

```pascal
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
```  

The loop is needed because the data may come in multiple packets.

Not that this is **not** really a good example, as there's a HTTP library built-in to Synapse. The in-built HTTP library has several advantages, including the ability to use HTTP proxies. Perhaps I'll cover that in a future blog post :)

