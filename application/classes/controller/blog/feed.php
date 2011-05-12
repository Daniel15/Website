<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller for blog RSS feed
 */
class Controller_Blog_Feed extends Controller
{
	/**
	 * Return an RSS feed of all the latest blog posts
	 */
	public function action_index()
	{
		// If the user is accessing directly, redirect to FeedBurner instead
		if (stripos($_SERVER['HTTP_USER_AGENT'], 'feedburner') === false 
			&& stripos($_SERVER['HTTP_USER_AGENT'], 'feedvalidator') === false
			&& !isset($_GET['feedburner_override']))
		{
			$this->request->redirect(Kohana::config('blog.feedburner_url'));
		}
		// Get the posts
		$posts = ORM::factory('Blog_Post')
			->order_by('date', 'desc')
			->limit(Kohana::config('blog.posts_in_feed'))
			->find_all();
			
		// First create a basic feed that we can load into SimpleXML
		$feed = 
'<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0"
	xmlns:content="http://purl.org/rss/1.0/modules/content/"
	xmlns:wfw="http://wellformedweb.org/CommentAPI/"
	xmlns:dc="http://purl.org/dc/elements/1.1/"
	xmlns:atom="http://www.w3.org/2005/Atom"
	xmlns:sy="http://purl.org/rss/1.0/modules/syndication/"
	xmlns:slash="http://purl.org/rss/1.0/modules/slash/"
>
	<channel></channel>
</rss>';
		$feed = simplexml_load_string($feed);
		$channel = &$feed->channel;
		$channel->addChild('title', 'Daniel15\'s Blog');
		$channel->addChild('description', 'Blog of Daniel, a slightly awesome 21-year-old web developer from Melbourne, Australia');
		$channel->addChild('language', 'en');
		$channel->addChild('generator', 'Daniel15\'s Website (http://dan.cx/)');
		$channel->addChild('lastBuildDate', date('r', $posts[0]->date));
		
		$atom_link = $channel->addChild('link', null, 'http://www.w3.org/2005/Atom');
		$atom_link->addAttribute('href', 'http://feeds.d15.biz/daniel15');
		$atom_link->addAttribute('rel', 'self');
		$atom_link->addAttribute('type', 'application/rss+xml');
		$channel->addChild('link', Url::site('blog', true));
			
		foreach ($posts as $post)
		{
			$content = str_replace('&', '&amp;', $post->content());
			$description = Text::limit_chars(strip_tags($content), Kohana::config('blog.summary_length'), null, true);
			$url = $post->url(true);
		
			$post_feed = $channel->addChild('item');
			$post_feed->addChild('title', str_replace('&', '&amp;', $post->title));
			$post_feed->addChild('link', $url);
			$post_feed->addChild('comments', $url . '#comments');
			$post_feed->addChild('pubDate', date('r', $post->date));
			$post_feed->addChild('creator', 'Daniel15', 'http://purl.org/dc/elements/1.1/');
			$post_feed->addChild('description', $description);
			$post_feed->addChild('encoded', $content, 'http://purl.org/rss/1.0/modules/content/');
			$post_feed->addChild('comments', $post->comment_count, 'http://purl.org/rss/1.0/modules/slash/');
			
			$guid = $post_feed->addChild('guid', $url);
			$guid->addAttribute('isPermaLink', 'true');
			
			// Add all categories
			foreach ($post->categories->find_all() as $category)
				$post_feed->addChild('category', $category->title);
		}
		
		// If the DOM methods exist, they can print prettier XML.
		if (function_exists('dom_import_simplexml'))
		{
			$feed = dom_import_simplexml($feed)->ownerDocument;
			$feed->formatOutput = true;
			$feed = $feed->saveXML();
		}
		else
			$feed = $feed->asXML();
		
		// TODO: Proper content type
		$this->response->headers('Content-Type', 'text/xml; charset=UTF-8');
		echo $feed, '
<!-- Generated on ', date('jS F Y \a\t g:i A'), ' -->';
	}
}
?>