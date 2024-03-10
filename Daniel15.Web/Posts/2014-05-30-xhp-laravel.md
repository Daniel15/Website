---
id: 341
title: Getting Started with XHP in Laravel
published: true
publishedDate: 2014-05-30 21:53:31Z
lastModifiedDate: 2014-05-30 21:53:31Z
categories:
- PHP
- Programming

---

<p>In this post I'll cover the basics of using XHP along with the <a href="http://laravel.com/">Laravel</a> PHP framework, but most of the information is framework-agnostic and applies to other frameworks too.</p>

<h3>What is XHP and Why Should I Use It?</h3>
<p>XHP is a templating syntax originally developed by Facebook and currently in use for all their server-rendered frontend code. It adds an XML-like syntax into PHP itself. XHP comes bundled with HHVM, and is available as an extension for regular PHP 5 too.</p>

<p>The main advantages of XHP include:</p>
<ul>
  <li>Not just simple language transformations &mdash; Every element in XHP is a regular PHP class. This means you have the full power of PHP in your templates, including inheritence. More advanced XHP components can have methods that alter their behaviour</li>
  <li>Typed parameters &mdash; You can specify that attributes need to be of a particular type and whether they are mandatory or optional. Most PHP templating languages are weakly-typed.</li>
  <li>Safe by default &mdash; All variables are HTML escaped by default.</li>
</ul>

<h3>Installation</h3>
<p>From here on, I'm assuming that you already have a basic Laravel app up and running on your machine. If not, please follow the <a href="https://getcomposer.org/">Composer</a> and <a href="http://laravel.com/docs/quick">Laravel</a> quickstart guides before continuing.</p>

<p>If you are running PHP, you will first need to <a href="https://github.com/facebook/xhp/blob/master/INSTALL">install the XHP extension</a>. HHVM comes bundled with XHP so you don't need to worry about the extension if you're using HHVM.</p>

<p>This extension is only one part of XHP and only implements the parsing logic. The actual runtime behaviour of XHP elements is controlled by a few PHP files. These files implement the base XHP classes that your custom tags will extend, in addition to all the basic HTML tags. This means that you can totally customise the way XHP works on a project-by-project basis (although I'd strongly suggest sticking to the default behaviour so you don't introduce incompatibilities). You can install these files via Composer. Edit your composer.json file and add this to the <code>"require"</code> section:</p>

<pre class="brush: javascript">
"facebook/xhp": "dev-master"
</pre>

<p>While in composer.json, also add <code>"app/views"</code> to the autoload classmap section. This will tell Composer to handle autoloading your custom XHP classes. XHP elements are compiled down to regular PHP classes, and Composer's autoloader can handle loading them. In the end, your composer.json should look <a href="https://gist.github.com/Daniel15/081cc25b0ce166646528">something like this</a>. If you do not want to use the Composer autoloader (or it does not work for some reason), you can use a <a href="https://gist.github.com/Daniel15/2a1b7d1be7bc40005fc6">simple custom autoloader</a> instead. I'd only suggest this if you have problems with Composer's autoloader.</p>

<h3>Create Some Views</h3>
<p>The first view file we'll create is the basic page layout. Save this as <code>views/layout/base.php</code>:</p>
<pre class="brush: php">
<?php
class :layout:base extends :x:element {
  attribute
    string title @required;

  public function render() {
    return
      <x:doctype>
        <html>
          <head>
            <title>{$this->getAttribute('title')}</title>
          </head>
          <body>
            {$this->getChildren()}
          </body>
        </html>
      </x:doctype>;
  }
}
</pre>
<p>(side note: if you are using HHVM, you can replace <code>&lt;?php</code> with <code>&lt;?hh</code> to use <a href="http://hacklang.org/">Hack</a> instead of vanilla PHP)</p>

<p>This code introduces some core XHP concepts:</p>
<ul>
  <li>All XHP classes start with a colon (:), and colons are used to denote "namespaces" (note that these are not PHP namespaces). XHP classes can have multiple colons in the name (so <code>:page:blog:comments</code> is a valid class name)</li>
  <li><code>:x:element</code> is the base XHP class that all of your XHP templates should extend.</li>
  <li>XHP classes can have attributes. This class has a title attribute that's required. If a required attribute is not specified, an exception will be thrown at runtime. Attributes can use intrinsic types (string, int, bool) as well as complex types (class names, eg. for view models or database models)</li>
  <li>XHP classes have a <code>render</code> method that returns the XHP for rendering this component. This can be a mix of regular HTML tags (as shown here) and other XHP components.</li>
</ul>

<p>Now that we have a layout file, let's also create a simple page that utilises it. Save this as <code>views/page/home.php</code>:</p>
<pre class="brush: php">
<?php
class :page:home extends :x:element {
  attribute
    string name @required;

  protected function render() {
    return
      <layout:base title="Hello Title">
        Hello {$this->getAttribute('name')}!
        <strong>This is a test</strong>
      </layout:base>;
  }
}
</pre>

<p>Notice that this component uses <code>:layout:base</code> in its render method, passing "Hello Title" as the title attribute. Generally, you should favour composition over inheritance (that is, use other components in your <code>render</code> method rather than extending them).</p>

<p>Since we are using Composer's autoloader to load the views, you need to tell it to rebuild its autoloader cache:</p>
<pre class="brush: plain">
composer dump-autoload
</pre>
<p>This needs to be done every time you add a new view. If you are only editing an existing view, you do not need to do it.</p>

<p>Now that we have a page, let's use it. Using an XHP view from a Laravel route or controller simply involves returning it like you would any other response. In <code>app/routes.php</code>, modify the <code>/</code> route as follows:</p>
<pre class="brush: php">
Route::get('/', function() {
  return <page:home name="Daniel" />;
});
</pre>

<p>Save the file and hit your app in your favourite browser. If everything was successful, you should see "Hello Daniel! This is a test" on the screen. Congratulations! You've just created a simple XHP-powered Laravel site!</p>

<h3>Next Steps</h3>
<p>So where do you go from here? In general, every reusable component should have its own XHP class. For example, if you were using Bootstrap for your site, each Bootstrap component that you'd like to use belongs in its own XHP class. I'd suggest using at least three separate XHP namespaces:</p>
<ul>
  <li><code>:layout</code> &mdash; Layout pages, the actual header and footer of the site. Different sections of your site may have different header/footers.</li>
  <li><code>:page</code> &mdash; Actual website pages</li>
  <li><code>:ui</code> &mdash; Reusable UI components</li>
</ul>
<p>Within each of these namespaces, you can have "sub namespaces". For example, you may have <code>:page:blog:...</code> for blog pages</p>

<h3>Further Reading</h3>
<ul>
  <li><a href="https://github.com/facebook/xhp">XHP on Github</a></li>
  <li><a href="http://codebeforethehorse.tumblr.com/post/3096387855/an-introduction-to-xhp">An introduction to XHP</a> by Stefan Parker, former UI Engineer at Facebook</li>
</ul>

