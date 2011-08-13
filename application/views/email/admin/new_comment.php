<?php defined('SYSPATH') or die('No direct script access.'); ?>
A new comment on the post "<?php echo $comment->post->title ?>" is waiting for your approval.

Post: <?php echo $comment->post->title ?>

<?php echo $comment->post->url(true) ?>

Author: <?php echo $comment->author ?> (<?php echo $comment->email ?>)

IP: <?php echo $comment->ip, (!empty($comment->ip2) ? ' - ' . $comment->ip2 : '') ?>

URL: <?php echo $comment->url ?>

_____________________________________________________________________

<?php echo $comment->content ?>

_____________________________________________________________________

See all the pending comments at <?php echo Url::site('blogadmin', true) ?>