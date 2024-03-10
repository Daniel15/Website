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

In this post I'll cover the basics of using XHP along with the [Laravel](http://laravel.com/) PHP framework, but most of the information is framework-agnostic and applies to other frameworks too.

## What is XHP and Why Should I Use It?
XHP is a templating syntax originally developed by Facebook and currently in use for all their server-rendered frontend code. It adds an XML-like syntax into PHP itself. XHP comes bundled with HHVM, and is available as an extension for regular PHP 5 too.

The main advantages of XHP include:

* Not just simple language transformations — Every element in XHP is a regular PHP class. This means you have the full power of PHP in your templates, including inheritence. More advanced XHP components can have methods that alter their behaviour
* Typed parameters — You can specify that attributes need to be of a particular type and whether they are mandatory or optional. Most PHP templating languages are weakly-typed.
* Safe by default — All variables are HTML escaped by default.

## Installation
From here on, I'm assuming that you already have a basic Laravel app up and running on your machine. If not, please follow the [Composer](https://getcomposer.org/) and [Laravel](http://laravel.com/docs/quick) quickstart guides before continuing.

If you are running PHP, you will first need to [install the XHP extension](https://github.com/facebook/xhp/blob/master/INSTALL). HHVM comes bundled with XHP so you don't need to worry about the extension if you're using HHVM.

This extension is only one part of XHP and only implements the parsing logic. The actual runtime behaviour of XHP elements is controlled by a few PHP files. These files implement the base XHP classes that your custom tags will extend, in addition to all the basic HTML tags. This means that you can totally customise the way XHP works on a project-by-project basis (although I'd strongly suggest sticking to the default behaviour so you don't introduce incompatibilities). You can install these files via Composer. Edit your composer.json file and add this to the `"require"` section:

```javascript
"facebook/xhp": "dev-master"
```

While in composer.json, also add `"app/views"` to the autoload classmap section. This will tell Composer to handle autoloading your custom XHP classes. XHP elements are compiled down to regular PHP classes, and Composer's autoloader can handle loading them. In the end, your composer.json should look [something like this](https://gist.github.com/Daniel15/081cc25b0ce166646528). If you do not want to use the Composer autoloader (or it does not work for some reason), you can use a [simple custom autoloader](https://gist.github.com/Daniel15/2a1b7d1be7bc40005fc6) instead. I'd only suggest this if you have problems with Composer's autoloader.

## Create Some Views
The first view file we'll create is the basic page layout. Save this as `views/layout/base.php`:

```php
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
```
(side note: if you are using HHVM, you can replace `<?php` with `<?hh` to use [Hack](http://hacklang.org/) instead of vanilla PHP)

This code introduces some core XHP concepts:

* All XHP classes start with a colon (:), and colons are used to denote "namespaces" (note that these are not PHP namespaces). XHP classes can have multiple colons in the name (so `:page:blog:comments` is a valid class name)
* `:x:element` is the base XHP class that all of your XHP templates should extend.
* XHP classes can have attributes. This class has a title attribute that's required. If a required attribute is not specified, an exception will be thrown at runtime. Attributes can use intrinsic types (string, int, bool) as well as complex types (class names, eg. for view models or database models)
* XHP classes have a `render` method that returns the XHP for rendering this component. This can be a mix of regular HTML tags (as shown here) and other XHP components.

Now that we have a layout file, let's also create a simple page that utilises it. Save this as `views/page/home.php`:

```php
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
```

Notice that this component uses `:layout:base` in its render method, passing "Hello Title" as the title attribute. Generally, you should favour composition over inheritance (that is, use other components in your `render` method rather than extending them).

Since we are using Composer's autoloader to load the views, you need to tell it to rebuild its autoloader cache:

```plain
composer dump-autoload
```
This needs to be done every time you add a new view. If you are only editing an existing view, you do not need to do it.

Now that we have a page, let's use it. Using an XHP view from a Laravel route or controller simply involves returning it like you would any other response. In `app/routes.php`, modify the `/` route as follows:

```php
Route::get('/', function() {
  return <page:home name="Daniel" />;
});
```

Save the file and hit your app in your favourite browser. If everything was successful, you should see "Hello Daniel! This is a test" on the screen. Congratulations! You've just created a simple XHP-powered Laravel site!

## Next Steps
So where do you go from here? In general, every reusable component should have its own XHP class. For example, if you were using Bootstrap for your site, each Bootstrap component that you'd like to use belongs in its own XHP class. I'd suggest using at least three separate XHP namespaces:

* `:layout` — Layout pages, the actual header and footer of the site. Different sections of your site may have different header/footers.
* `:page` — Actual website pages
* `:ui` — Reusable UI components

Within each of these namespaces, you can have "sub namespaces". For example, you may have `:page:blog:...` for blog pages

## Further Reading
* [XHP on Github](https://github.com/facebook/xhp)
* [An introduction to XHP](http://codebeforethehorse.tumblr.com/post/3096387855/an-introduction-to-xhp) by Stefan Parker, former UI Engineer at Facebook

