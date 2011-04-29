<?php defined('SYSPATH') or die('No direct script access.'); ?><!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="ie ie6 ielt8"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="ie ie7 ielt8"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="ie ie8"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="ie ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="en" class="non-ie"> <!--<![endif]-->
<head>
	<title><?php echo (empty($title) ? '' : $title . ' &mdash; '); ?>Daniel15</title>
	<base href="<?php echo URL::base(TRUE); ?>" />
	<meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
	<meta name="Author" content="Daniel Lo Nigro (Daniel15)" />
	<meta property="fb:admins" content="731901032" />
	<meta property="og:type" content="website" />
	<meta property="og:title" content="<?php echo (empty($title) ? 'Daniel15\'s Site' : $title); ?>" />
	<?php echo empty($meta['Description']) ? '' : '<meta property="og:description" content="' . $meta['Description'] . '" />'?>
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
	<link rel="stylesheet" href="res/print.css" type="text/css" media="print"  />
	<!--[if lt IE 8]><link rel="stylesheet" href="res/style-ie7.css" type="text/css" /><![endif]-->
	<!--[if lt IE 7]><link rel="stylesheet" href="res/style-ie6.css" type="text/css" /><![endif]-->
<?php endif; ?>

	<!-- Other stuff -->
	<link rel="start" href="/" title="Home" />
	<?php if (!empty($extraHead)) echo $extraHead; ?>
</head>
<body id="<?php echo $pageID; ?>" class="<?php echo $controller; ?> col-<?php echo $sidebarType; ?> no-js">
	<div id="main-container">
		<div id="header">
<?php echo $menu; ?>

			<h1><a href="/" title="Daniel15 is awesome"><?php echo empty($title) ? 'Daniel15' : $title; ?></a></h1>
		</div>
		<div id="colmask"><div id="colright">
			<div id="content_wrap"><div id="content">
<?php echo $content; ?>

			</div></div>
			
<?php
// Do we have a sidebar?
if (!empty($sidebar))
{
	echo '
			<div id="sidebar">
', $sidebar, '
			</div>';
}
?>

		</div></div>
		<div id="footer">
			&copy;2008&ndash;2011 <a href="http://dan.cx/">Daniel15 (Daniel Lo Nigro)</a>. <?php
// Output a last modified date if we have one
if (!empty($lastModified) && $lastModified != 0)
	echo '<br />This page last modified ' . date('jS F Y', $lastModified), '.';
?>
		Loaded in {execution_time}<!-- using {memory_usage} memory-->.
		</div>
	</div>
	<!-- Now for the JS -->
	<script src="http://ajax.googleapis.com/ajax/libs/mootools/1.3.0/mootools-yui-compressed.js" type="text/javascript"></script>
<?php if ($siteConfig->enableCompression) : ?>
	<script src="res/<?php echo $siteConfig->latestJS ?>" type="text/javascript"></script>
<?php else : ?>
	<script src="res/mootools-more-1.3.0.1.js" type="text/javascript"></script>
	<script src="res/scripts_r1.js" type="text/javascript"></script>
<?php endif; ?>
	<?php //echo View::factory('profiler/stats'); ?>
</body>
</html>