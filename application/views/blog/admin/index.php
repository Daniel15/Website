<?php defined('SYSPATH') or die('No direct script access.'); ?>

<h2>Comments</h2>
<p>There are:</p>
<ul>
	<li><a href="<?php echo Url::site('blogadmin/comments/index/pending'); ?>"><?php echo $pending; ?> pending comments</a></li>
	<li><a href="<?php echo Url::site('blogadmin/comments/index/spam'); ?>"><?php echo $spam; ?> spam comments</a></li>
	<li><a href="<?php echo Url::site('blogadmin/comments/index/hidden'); ?>"><?php echo $hidden; ?> hidden comments</a></li>
	<li><a href="<?php echo Url::site('blogadmin/comments/index/visible'); ?>"><?php echo $visible; ?> visible comments</a></li>
</ul>

<h2>Posts</h2>
<p>There are:</p>
<ul>
	<li><a href="<?php echo Url::site('blogadmin/posts/index/1'); ?>"><?php echo $published_posts; ?> published posts</a></li>
	<li><a href="<?php echo Url::site('blogadmin/posts/index/0'); ?>"><?php echo $unpublished_posts; ?> unpublished posts</a></li>
</ul>
<a href="<?php echo Url::site('blogadmin/posts/edit') ?>">Write a new post</a>