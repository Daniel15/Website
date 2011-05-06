<?php defined('SYSPATH') or die('No direct script access.'); ?>

<h2>Blog Administration</h2>
<p>There are <a href="<?php echo Url::site('blogadmin/comments/index/pending'); ?>"><?php echo $pending; ?> pending comments</a> and <a href="<?php echo Url::site('blogadmin/comments/index/spam'); ?>"><?php echo $spam; ?> spam comments</a> in the queue.</p>