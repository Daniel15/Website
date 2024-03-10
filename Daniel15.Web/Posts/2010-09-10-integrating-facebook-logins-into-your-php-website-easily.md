---
id: 316
title: Integrating Facebook logins into your PHP website easily
published: true
publishedDate: 2010-09-10 02:01:36Z
lastModifiedDate: 2010-09-10 02:01:36Z
categories:
- Facebook
- PHP
- Programming
- Web Development

---

<p>A lot of sites now use OpenID. This is great, as you can use the one account on multiple sites. Unfortunately, Facebook accounts can not yet be used as OpenIDs :(. However, using Facebook logins isn't too hard, as they support using OAuth 2.0. OpenID and OAuth are fundamentally for different things (OpenID is authentication, OAuth is authorization), but it still works well in this situation. </p>
<p>Firstly, sign up for a Facebook application at the <a href="http://facebook.com/developer/">Facebook developer website</a>.  You'll have to correctly set the site URL and site domain. Copy the application ID and application secret as shown on the Web Site section of the settings, as you will need them later. <!--more--><br />
<img src="http://ss.dan.cx/2010/09/05-16.15.54.png" alt="Facebook application details" border="1" /></p>
<p>Now we're ready to begin. Here's a very simple class for logging in via Facebook. It doesn't have much error checking, but should work okay: <a href="http://dan.cx/blog/wp-content/uploads/2010/09/facebook.txt">Download the class (Facebook.php)</a>. Here's some code that uses that class:<br />
<pre class="brush: php">
$facebook = new FacebookLogin('100929283281389', '8*******************************1');
$user = $facebook->doLogin();
echo 'User\'s URL: ', $user->link, '<br />';
echo 'User\'s name: ', $user->name, '<br />';
echo 'Full details:<br /><pre>', print_r($user, true), '</ pre>';
</pre></p>
<p>The first number in the constructor is the application ID, and the second one is the application secret (remember these from earlier? Here's where you use them!). Stick both the class and the little code snippet above into a .php file, and access it. If everything works correctly, you'll be able to hit that file to log in via Facebook, and get the user's details after logging in. <a href="http://stuff.dan.cx/facebook/login_test/">Here's a demo to show you how it works</a>. The idea now is you save the file as something like "FacebookLogin.php", and add a "Log in using Facebook" link on your site that goes to it :).</p>
<p>The class I've provided here is just a base class that you can base your own code on. What you do now is up to you. Here's some suggestions:</p>
<ul>
<li>Move the application ID and secret into a config file, instead of hard-coded like above</li>
<li>If you're using this to log in to a site, I'd store some of the user's details (like name, URL and Facebook ID) in session variables. </li>
<li>Maybe do things like load the user's profile picture. The access token retrieved at this line: <strong>$this->access_token = $result_array['access_token'];</strong> can be used to access pretty much anything on Facebook, as long as the user has given permission. Take a look at <a href="http://stuff.dan.cx/facebook/login_test/">the demo</a> to see what info you can get by default</li>
</ul>
<p>Good luck! :)</p>

