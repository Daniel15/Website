<?php
if (empty($this->pageID))
	$this->pageID = 'unknown';

// Do we have a column type?
if (empty($this->sidebarType))
	$this->sidebarType = 'none';
	
// Pages we have on the site (for the nav menu)
$menu = array(
	'index.htm' => 'Home',
	'projects.htm' => 'Projects',
	'blog/' => 'Blog',
	'blog/category/microblog/' => 'Thoughts',
);
$dirname = dirname($_SERVER['SCRIPT_NAME']);
//<html xmlns="http://www.w3.org/1999/xhtml" xmlns:og="http://opengraphprotocol.org/schema/" xmlns:fb="http://www.facebook.com/2008/fbml" xml:lang="en" lang="en">
?><!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="ie ie6 ielt8"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="ie ie7 ielt8"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="ie ie8"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="ie ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="en" class="non-ie"> <!--<![endif]-->
<head>
	<title><?php echo (empty($this->title) ? '' : $this->title . ' &mdash; '); ?>Daniel15</title>
	<base href="http://<?php echo $_SERVER['HTTP_HOST']; ?><?php echo $dirname, $dirname == '/' ? '' : '/'; ?>" />
	<meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
	<meta name="Author" content="Daniel Lo Nigro (Daniel15)" />
	<meta property="fb:admins" content="731901032" />
	<meta property="og:type" content="website" />
	<meta property="og:title" content="<?php echo (empty($this->title) ? 'Daniel15\'s Site' : $this->title); ?>" />
	<?php echo empty($this->meta['Description']) ? '' : '<meta property="og:description" content="' . $this->meta['Description'] . '" />'?>
<?php
// Here come the meta tags
foreach ($this->meta as $name => $content)
	echo '
	<meta name="', $name, '" content="', $content, '" />';

// Output a last modified date if we have one
if ($this->lastModified != 0)
{
	echo '
	<meta name="Date" content="', date('Y-m-d', $this->lastModified), '" />
	<meta name="DC.Date" content="', date('Y-m-d', $this->lastModified), '" />';
}
?>
	
<?php if ($this->enableCompression) : ?>
	<link title="dan.cx combined stylesheet" rel="stylesheet" href="res/<?php echo $this->cssFile ?>" type="text/css" id="default-stylesheet" />
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
<?php
// Only show OpenID stuff on homepage
/* Old OpenID:
	<link rel="openid.server" href="http://www.myopenid.com/server" />
	<link rel="openid.delegate" href="http://daniel15.myopenid.com/" />
	<link rel="openid2.local_id" href="http://daniel15.myopenid.com" />
	<link rel="openid2.provider" href="http://www.myopenid.com/server" />
	<meta http-equiv="X-XRDS-Location" content="http://www.myopenid.com/xrds?username=daniel15.myopenid.com" />
	*/
if ($this->pagename == 'index')
	echo '
	<!-- OpenID -->
	<link rel="openid.server" href="https://www.startssl.com/id.ssl" />
	<link rel="openid.delegate" href="https://daniel15.startssl.com/" />
	<link rel="openid2.provider" href="https://www.startssl.com/id.ssl" />
	<link rel="openid2.local_id" href="https://daniel15.startssl.com/" />
	<meta http-equiv="X-XRDS-Location" content="https://daniel15.startssl.com/xrds/" />
';
?>
</head>

<body id="site-<?php echo $this->pageID; ?>" class="site col-<?php echo $this->sidebarType; ?> no-js">
	<div id="main-container">
		<div id="header">
			<ul id="nav">
<?php
foreach ($menu as $uri => $title)
echo '
				<li', $this->pagename . '.htm' == $uri ? ' id="selected"' : '', '><a href="', $uri, '"><span>', $title, '</span></a></li>';
?>

			</ul>
			
			<h1><a href="/" title="Daniel15 is awesome"><?php echo empty($this->title) ? 'Daniel15' : $this->title; ?></a></h1>
		</div>
		
		<div id="colmask"><div id="colright">
			<div id="content_wrap"><div id="content">
<?php echo $this->body; ?>

			</div></div>
<?php
// Do we have a sidebar?
if (!empty($this->sidebar))
{
	echo '
			<div id="sidebar">
', $this->sidebar, '
			</div>
';
}
?>
		</div></div>
		<div id="footer">
			&copy;2008&ndash;2011 <a href="http://<?php echo $_SERVER['HTTP_HOST']; ?><?php echo $dirname, $dirname == '/' ? '' : '/'; ?>">Daniel15 (Daniel Lo Nigro)</a>. <?php
// Output a last modified date if we have one
if ($this->lastModified != 0)
	echo '<br />This page last modified ' . date('jS F Y', $this->lastModified);
?>
		</div>
	</div>
	<!-- Now for the JS -->
	<script src="http://ajax.googleapis.com/ajax/libs/mootools/1.3.0/mootools-yui-compressed.js" type="text/javascript"></script>
<?php if ($this->enableCompression) : ?>
	<script src="res/<?php echo $this->jsFile ?>" type="text/javascript"></script>
<?php else: ?>
	<script src="res/mootools-more-1.3.0.1.js" type="text/javascript"></script>
	<script src="res/scripts_r1.js" type="text/javascript"></script>
<?php endif; ?>
<?php
if (!empty($this->js))
	echo '
	<script type="text/javascript">', $this->js, '</script>
';
?>
</body>
</html>
