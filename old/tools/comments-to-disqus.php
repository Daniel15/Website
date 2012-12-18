<?php
$dbname = 'daniel15_new';
$username = 'root';
$password = 'password';
$db = new PDO('mysql:host=localhost;dbname=' . $dbname, $username, $password);
$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);	
$db->query('SET NAMES "UTF8"');

// Start with a fresh WXR export XML
$output = simplexml_load_string('<?xml version="1.0" encoding="UTF-8"?><rss version="2.0" xmlns:content="http://purl.org/rss/1.0/modules/content/" xmlns:dsq="http://www.disqus.com/" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:wp="http://wordpress.org/export/1.0/" />');

// Start the initial XML output
$channel = $output->addChild('channel');

// Get all the posts
$posts_query = $db->query('SELECT * FROM blog_posts');
$comments_query = $db->prepare('SELECT * FROM blog_comments WHERE post_id = :post_id AND status <> "spam"');

while ($post = $posts_query->fetchObject())
{
	$item = $channel->addChild('item');
	$item->addChild('title', $post->title);
	$item->addChild('link', 'http://dan.cx/blog/' . date('Y/m', $post->date) . '/' . $post->slug);
	$item->addChild('encoded', str_replace('&', '&amp;', $post->content), 'http://purl.org/rss/1.0/modules/content/');
	$item->addChild('thread_identifier', $post->id, 'http://www.disqus.com/');
	$item->addChild('post_date_gmt', gmdate('Y-m-d H:i:s', $post->date), 'http://wordpress.org/export/1.0/');
	$item->addChild('comment_status', 'open', 'http://wordpress.org/export/1.0/');
	
	$comments_query->execute([':post_id' => $post->id]);
	while ($comment_row = $comments_query->fetchObject())
	{
		$comment = $item->addChild('comment', null, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_id', $comment_row->id, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_author', $comment_row->author, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_author_email', $comment_row->email, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_author_url', str_replace('&', '&amp;', $comment_row->url), 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_author_IP', $comment_row->ip, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_date_gmt', gmdate('Y-m-d H:i:s', $comment_row->date), 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_content', str_replace('&', '&amp;', $comment_row->content), 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_approved', $comment_row->status === 'visible' ? 1 : 0, 'http://wordpress.org/export/1.0/');
		$comment->addChild('comment_parent', $comment_row->parent_comment_id, 'http://wordpress.org/export/1.0/');
	}
}

header('Content-Type: text/xml; charset=UTF-8');
echo $output->asXML();

?>
