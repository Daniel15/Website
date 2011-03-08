<?php
$output_filename = '/home/daniel15/cms/data/blog_posts';
$xml = simplexml_load_file('http://feeds.d15.biz/daniel15');
$output = array();

foreach ($xml->channel->item as $post)
{
	$output[] = array(
		'title' => (string) $post->title,
		'url' => (string) $post->children('http://rssnamespace.org/feedburner/ext/1.0')->origLink,
		'date' => strtotime($post->pubDate),
		'desc' => str_replace("\n", ' ', (string) $post->description),
	);
}

file_put_contents($output_filename, serialize($output));
?>