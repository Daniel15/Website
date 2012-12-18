<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller for miscellaneous feed
 */
class Controller_Feed extends Controller
{
	public function action_sitemap()
	{
		$feed = simplexml_load_string('<?xml version="1.0" encoding="UTF-8"?><urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" />');
		
		$this->add_url($feed, '/', filemtime(Kohana::find_file('views', 'index')), 'weekly', '1.0');
		$this->add_url($feed, 'projects.htm', max(filemtime(Kohana::find_file('views', 'projects')), filemtime(Kohana::find_file('classes/model', 'projects'))), 'weekly', '0.9');
		$this->add_url($feed, 'socialfeed.htm', null, 'daily', '0.7');
		$this->add_url($feed, 'blog', null, 'daily', '0.9');
		
		// Get all the blog posts
		$posts = ORM::factory('Blog_Post')
			->order_by('date', 'desc')
			->find_all();
			
		foreach ($posts as $post)
			$this->add_url($feed, $post->url(), $post->date, 'monthly', '0.8');
		
		// Get all the categories
		$categories = ORM::factory('Blog_Category')
			->order_by('title')
			->find_all();
			
		foreach ($categories as $category)
			$this->add_url($feed, $category->url(), null, 'weekly', '0.7');
			
		// Get all the tags
		$tags = ORM::factory('Blog_Tag')
			->order_by('title')
			->find_all();
			
		foreach ($tags as $tag)
			$this->add_url($feed, $tag->url(), null, 'weekly', '0.7');
		
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
	
	protected function add_url(SimpleXMLElement& $sitemap, $url, $lastmod = null, $changefreq = null, $priority = null)
	{
		$url_element = $sitemap->addChild('url');
		$url_element->addChild('loc', Url::site($url, true));
		if (!empty($lastmod))
			$url_element->addChild('lastmod', date(DATE_W3C, $lastmod));
		if (!empty($changefreq))
			$url_element->addChild('changefreq', $changefreq);
		if (!empty($priority))
			$url_element->addChild('priority', $priority);
	}
}
?>