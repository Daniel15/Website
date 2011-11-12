<?php defined('SYSPATH') or die('No direct script access.'); ?><!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="no-js ie ie6 ielt8 ielt9"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="no-js ie ie7 ielt8 ielt9"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="no-js ie ie8 ielt9"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="no-js ie ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="en" class="no-js non-ie"> <!--<![endif]-->
<head>
	<title><?php echo (empty($title) ? '' : $title . ' &mdash; '); ?>Daniel15</title>
	<base href="<?php echo URL::base(TRUE); ?>" />
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta name="Author" content="Daniel Lo Nigro (Daniel15)" />
	<meta property="fb:admins" content="731901032" />
	<meta property="og:country_name" content="Australia" />
	<meta property="og:title" content="<?php echo (empty($title) ? 'Daniel15\'s Site' : $title); ?>" />
<?php if (empty($is_blog)) : ?>
	<meta property="og:type" content="website" />
	<?php echo empty($meta['Description']) ? '' : '<meta property="og:description" content="' . $meta['Description'] . '" />'?>
<?php else : ?>
	<meta property="og:type" content="blog" />
	<meta property="og:site_name" content="Daniel15's Blog" />
<?php endif; ?>
<?php
// Here come the meta tags
foreach ($meta as $metaName => $metaContent)
	echo '
	<meta name="', $metaName, '" content="', $metaContent, '" />';

// Output a last modified date if we have one
if (!empty($lastModified) && $lastModified != 0)
{
	echo '
	<meta name="Date" content="', date('Y-m-d', $lastModified), '" />
	<meta name="DC.Date" content="', date('Y-m-d', $lastModified), '" />';
}

// Are we combining the CSS files?
if ($siteConfig->enableCompression) : ?>

	<link title="dan.cx combined stylesheet" rel="stylesheet" href="res/<?php echo $siteConfig->latestCSS ?>" type="text/css" id="default-stylesheet" />
<?php else : ?>

	<!-- Some general stylesheets for d15.biz -->
	<link title="d15.biz stylesheet" rel="stylesheet" href="res/style_r2.css?v=2.2" type="text/css" media="screen" id="default-stylesheet" />
	<link rel="stylesheet" href="res/pages.css?v=3.0.1" type="text/css" />
	<link rel="stylesheet" href="res/sprites-processed.css" type="text/css" />
	<link rel="stylesheet" href="res/blog.css" type="text/css" />
	<link rel="stylesheet" href="res/print.css" type="text/css" media="print"  />
	<!--[if lt IE 9]><link rel="stylesheet" href="res/style-ie8.css" type="text/css" /><![endif]-->
	<!--[if lt IE 8]><link rel="stylesheet" href="res/style-ie7.css" type="text/css" /><![endif]-->
	<!--[if lt IE 7]><link rel="stylesheet" href="res/style-ie6.css" type="text/css" /><![endif]-->
	
	<!-- Syntax highlighter -->
	<link rel="stylesheet" href="lib/syntaxhighlighter/shCore.css" type="text/css" />
	<link rel="stylesheet" href="lib/syntaxhighlighter/shThemeDefault.css" type="text/css" />
<?php endif; ?>
	
	<!--[if lt IE 9]><script src="lib/IE9.js"></script><![endif]-->

	<!-- Other stuff -->
	<link rel="start" href="/" title="Home" />
	<script>document.documentElement.className = document.documentElement.className.replace('no-js', 'js');</script>
	<?php if (!empty($extraHead)) echo $extraHead; ?>

</head>
<body id="<?php echo $pageID; ?>" class="<?php echo $controller; ?> col-<?php echo $sidebarType; ?>">
	<div id="main-container">
		<header>
<?php echo $menu; ?>

			<h1><a href="/" title="Daniel15 is awesome"><?php echo $h1; ?></a></h1>
		</header>
		<div id="colmask"><div id="colright">
			<div id="content_wrap"><div id="content" role="main">
<?php
if (!empty($top_message))
{
	echo '
				<div id="top_message" class="', empty($top_message_type) ? 'success' : $top_message_type, '">', $top_message, '</div>';
}
echo $content; ?>

			</div></div>
			
<?php
// Do we have a sidebar?
if (!empty($sidebar))
{
	echo '
			<aside id="sidebar">
', $sidebar, '
			</aside>';
}
?>

		</div></div>
		<footer>
			&copy;2008&ndash;2011 <a href="http://dan.cx/">Daniel15 (Daniel Lo Nigro)</a>. <?php
// Output a last modified date if we have one
if (!empty($lastModified) && $lastModified != 0)
	echo '<br />This page last modified ' . date('jS F Y', $lastModified), '.';
?>
		Loaded in {execution_time}<!-- using {memory_usage} memory-->.
		</footer>
	</div>
	<!-- Now for the JS -->
<?php if ($siteConfig->enableCompression) : ?>
	<script src="res/<?php echo $siteConfig->latestJS ?>" type="text/javascript"></script>
<?php else : ?>
	<!-- Uncombined JavaScript files for debugging -->	
	<!-- Framework -->
	<script src="js/framework/core.js"></script>
	<script src="js/framework/ajax.js"></script>
	<script src="js/framework/dom.js"></script>
	<script src="js/framework/events.js"></script>
	<script src="js/framework/storage.js"></script>
	
	<!-- Site scripts -->
	<script src="js/core.js"></script>
	<script src="js/site.js"></script>
	<script src="js/blog.js"></script>
	<script src="js/socialfeed.js"></script>
	<script>Events.initPage();</script>
	
	<!-- Syntax highlighting -->
	<script src="lib/syntaxhighlighter/shCore.js"></script>
	<script src="lib/syntaxhighlighter/shBrushJScript.js"></script>
	<script src="lib/syntaxhighlighter/shBrushPhp.js"></script>
	<script src="lib/syntaxhighlighter/shBrushCSharp.js"></script>
	<script src="lib/syntaxhighlighter/shBrushXml.js"></script>
	<script src="lib/syntaxhighlighter/shBrushDelphi.js"></script>
	<script src="lib/syntaxhighlighter/shBrushPlain.js"></script>
	<script src="js/syntaxhighlighter.js"></script>
<?php endif; ?>

	<!-- Prompt IE 6 and 7 users to install Chrome Frame -->
	<!--[if lt IE 8]>
		<script src="http://ajax.googleapis.com/ajax/libs/chrome-frame/1/CFInstall.min.js"></script>
		<script>window.attachEvent('onload',function(){CFInstall.check({mode:'overlay'})})</script>
	<![endif]-->
<?php

if (!empty($extraFoot))
	echo $extraFoot;

if (Kohana::$environment >= Kohana::TESTING)
	echo View::factory('profiler/stats'); 

?>
</body>
</html>