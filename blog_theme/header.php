<?php
/*<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" <?php language_attributes(); ?>>*/

// TODO: Clean this up!
if (Daniel15Blog::is_compression_enabled())
{
	$siteData = Daniel15Blog::get_compression_info();
}
?><!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="ie ie6 ielt8"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="ie ie7 ielt8"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="ie ie8"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="ie ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="en" class="non-ie"> <!--<![endif]-->
<head>
	<!--title><?php bloginfo('name'); ?> <?php if ( is_single() ) { ?> &raquo; Blog Archive <?php } ?> <?php wp_title(); ?></title-->
	<title><?php wp_title('&laquo;', true, 'right'); ?> <?php bloginfo('name'); ?></title>

<?php if (Daniel15Blog::is_compression_enabled()) : ?>
	<link title="dan.cx combined stylesheet" rel="stylesheet" href="/res/<?php echo $siteData->latestCSS ?>" type="text/css" id="default-stylesheet" />
<?php else : ?>
	<!-- Some general stylesheets for d15.biz -->
	<link title="d15.biz stylesheet" rel="stylesheet" href="/res/style_r2.css?v=2.2" type="text/css" media="screen" id="default-stylesheet" />
	<link rel="stylesheet" href="/res/pages.css?v=3.0.1" type="text/css" />
	<link rel="stylesheet" href="/res/sprites-processed.css" type="text/css" />
	<link rel="stylesheet" href="/res/print.css" type="text/css" media="print"  />
	<!--[if lt IE 8]><link rel="stylesheet" href="/res/style-ie7.css" type="text/css" /><![endif]-->
	<!--[if lt IE 7]><link rel="stylesheet" href="/res/style-ie6.css" type="text/css" /><![endif]-->
<?php endif; ?>	
	
	<meta name="generator" content="WordPress <?php bloginfo('version'); ?>" /> <!-- leave this for stats -->
	<link rel="stylesheet" href="<?php bloginfo('stylesheet_url'); ?>" type="text/css" media="screen" />
	<link rel="alternate" type="application/rss+xml" title="<?php bloginfo('name'); ?> RSS Feed" href="<?php bloginfo('rss2_url'); ?>" />
	<link rel="alternate" type="application/atom+xml" title="<?php bloginfo('name'); ?> Atom Feed" href="<?php bloginfo('atom_url'); ?>" />
	<link rel="pingback" href="<?php bloginfo('pingback_url'); ?>" />
<?php if ( is_singular() ) wp_enqueue_script( 'comment-reply' ); ?>
<?php wp_head(); ?>

</head>

<body id="blog" <?php body_class(); ?>>
	<div id="main-container">
		<div id="header">
			<ul id="nav">
				<li><a href="/index.htm"><span>Home</span></a></li>
				<li><a href="/projects.htm"><span>Projects</span></a></li>
				<li<?php if (!is_microblog()) echo ' id="selected"'; ?>><a href="/blog/"><span>Blog</span></a></li>
				<li<?php if (is_microblog()) echo ' id="selected"'; ?>><a href="/blog/category/microblog/"><span>Thoughts</span></a></li>
			</ul>
			
			<h1><a href="/"><?php bloginfo('name'); ?></a></h1>
		</div>
		
		<div id="colmask"><div id="colright">
			<div id="content_wrap"><div id="content">
