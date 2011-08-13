<?php defined('SYSPATH') or die('No direct script access.'); ?>
A new comment has been added by <?php echo $comment->author ?> to the post "<?php echo $comment->post->title ?>":

_____________________________________________________________________

<?php echo $comment->content ?>

_____________________________________________________________________

View the post at <?php echo $comment->post->url(true) ?>

To unsubscribe from email notifications for this post, please go to <?php echo $sub->unsubscribe_url(true) ?>