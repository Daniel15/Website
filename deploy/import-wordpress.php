<?php

// Required include from WordPress - For automatic <p> insertion
include('../blog/wp-includes/formatting.php');

class WordpressImporter
{
	protected $db;
	protected $xml;
	protected $xml_wordpress;
	protected $categories;
	protected $tags;
	
	public function load($filename)
	{
		$this->xml = simplexml_load_file($filename);
		$this->xml_wordpress = $this->xml->channel->children('http://wordpress.org/export/1.1/');
	}
	
	public function connectToDB($dbname, $username, $password)
	{
		$this->db = new PDO('mysql:host=localhost;dbname=' . $dbname, $username, $password);
		$this->db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);	
		$this->db->query('SET NAMES "UTF8"');
	}
	
	public function process()
	{
		$this->process_categories();
		$this->process_tags();
		$this->process_posts();
		$this->finalise();
	}
	
	protected function process_categories()
	{
		// Prepare the query for inserting the categories
		$statement = $this->db->prepare('
			INSERT IGNORE INTO blog_categories (id, title, slug, parent_category_id)
			VALUES (:id, :title, :slug, :parent_category_id)');
	
		$new = array();
		
		// First load all categories (so we can get their parent IDs)
		foreach ($this->xml_wordpress->category as $category)
		{
			$new[(int)$category->term_id] = (object)array(
				'title' => (string)$category->cat_name,
				'slug' => (string)$category->category_nicename,
				'cat_parent' => (string)$category->category_parent,
			);
			
			$this->categories[(string)$category->category_nicename] = (int)$category->term_id;
		}
		
		// Now insert them all
		foreach ($new as $id => $category)
		{
			$parent_id = !empty($category->cat_parent) ? $this->categories[$category->cat_parent] : null;
			
			$statement->execute(array(
				':id' => $id,
				':title' => $category->title,
				':slug' => $category->slug,
				':parent_category_id' => $parent_id,
			));
		}
		
		echo count($new), " categories\n";
	}
	
	protected function process_tags()
	{
		// Prepare the query for inserting the categories
		$statement = $this->db->prepare('
			INSERT IGNORE INTO blog_tags (id, title, slug)
			VALUES (:id, :title, :slug)');
	
		$new = array();
		
		// First load all categories (so we can get their parent IDs)
		foreach ($this->xml_wordpress->tag as $tag)
		{
			$new[(int)$tag->term_id] = (object)array(
				'title' => (string)$tag->tag_name,
				'slug' => (string)$tag->tag_slug,
			);
			
			$this->tags[(string)$tag->tag_slug] = (int)$tag->term_id;
		}
		
		// Now insert them all
		foreach ($new as $id => $tag)
		{			
			$statement->execute(array(
				':id' => $id,
				':title' => $tag->title,
				':slug' => $tag->slug,
			));
		}
		
		echo count($new), " tags\n";
	}
	
	protected function process_posts()
	{
		// Prepare some statements!
		$category_statement = $this->db->prepare('
			INSERT IGNORE INTO blog_post_categories (post_id, category_id)
			VALUES (:post_id, :taxonomy_id)');
			
		$tag_statement = $this->db->prepare('
			INSERT IGNORE INTO blog_post_tags (post_id, tag_id)
			VALUES (:post_id, :taxonomy_id)');
			
		$post_statement = $this->db->prepare('
			INSERT IGNORE INTO blog_posts (id, title, date, content, modified, comment_count, maincategory_id, slug)
			VALUES (:id, :title, :date, :content, :modified, :comment_count, :category, :slug)');
			
		$comment_statement = $this->db->prepare('
			INSERT IGNORE INTO blog_comments (id, post_id, author, email, url, ip, date, content, parent_comment_id, status)
			VALUES (:id, :post_id, :author, :email, :url, :ip, :date, :content, :parent_comment_id, :status)');
			
		foreach ($this->xml->channel->item as $post)
		{
			// WordPress-specific data for the post
			$wp = $post->children('http://wordpress.org/export/1.1/');
			
			// Skip attachments
			if ($wp->post_type == 'attachment')
				continue;
			
			// Skip drafts for now
			if ($wp->status == 'draft')
				continue;
				
			$date = strtotime($post->pubDate);
			
			$first_category = $post->xpath('./category[@domain="category"]');
			$first_category = $first_category[0];
			
			// Skip microblog articles (for now)
			if ((string)$first_category['nicename'] == 'microblog')
				continue;
				
			$content = (string)$post->children('http://purl.org/rss/1.0/modules/content/')->encoded;
			// Code blocks need to be treated specially, wpautop() removes <br /> tags in them, 
			// but we could have legitimate <br /> tags (in code samples!!). So we first remove all
			// code blocks, run wpautop() (to put paragraph tags around text), and then re-insert
			// all the code blocks
			$i = 0;
			$codeblocks = array();
			
			$content = preg_replace_callback('~<pre([^>]+)>([\S\s]+?)</pre>~', function($matches) use (&$i, &$codeblocks)
			{
				$name = '_DANIEL15_CODE_BLOCK_' . ++$i . '_';
				$codeblocks[$name] = $matches[0];
				return $name;
			}, $content);
			
			// Now run wpautop, and then put all the code blocks back
			$content = wpautop($content);
			foreach ($codeblocks as $name => $code)
			{
				// Replace old syntax highlighter and replace with code for JS SyntaxHighlighter
				$code = str_replace(array(
					'lang="javascript"',
					'lang="html4strict"',
					'lang="php"',
					'lang="csharp"',
					'lang="text"',
					'lang="pascal"',
				), array(
					'class="brush: javascript"',
					'class="brush: html"',
					'class="brush: php"',
					'class="brush: csharp"',
					'class="brush: plain"',
					'class="brush: pascal"',
				), $code);
				
				$content = str_replace($name, $code, $content);
			}
			
				
			$post_statement->execute(array(
				':id' => $wp->post_id,
				':title' => $post->title,
				':date' => $date,
				':content' => $content,
				':modified' => null,
				':comment_count' => count($wp->comment),
				':category' => $this->categories[(string)$first_category['nicename']],
				':slug' => $wp->post_name,
			));
			
			$this->process_categories_for($post, $wp, $category_statement, $tag_statement);
			$this->process_comments_for($post, $wp, $comment_statement);
		}
		
		echo count($this->xml->channel->item), " posts\n";
	}
	
	protected function process_categories_for(SimpleXMLElement $post, SimpleXMLElement $wp, 
		PDOStatement $category_statement, PDOStatement $tag_statement)
	{			
		foreach ($post->category as $category)
		{
			// Which type is it?
			if ($category['domain'] == 'category')
			{
				$category_statement->execute(array(
					':post_id' => $wp->post_id,
					':taxonomy_id' => $this->categories[(string)$category['nicename']],
				));
			}
			elseif ($category['domain'] == 'post_tag')
			{
				$tag_statement->execute(array(
					':post_id' => $wp->post_id,
					':taxonomy_id' => $this->tags[(string)$category['nicename']],
				));
			}
			else
			{
				echo 'Unknown category type: ', $category['domain'], "\n";
			}
		}
	}
	
	protected function process_comments_for(SimpleXMLElement $post, SimpleXMLElement $wp,
		PDOStatement $comment_statement)
	{
		foreach ($wp->comment as $comment)
		{
			// Ignore pingbacks and trackbacks for now
			if ($comment->comment_type == 'pingback' || $comment->comment_type == 'trackback')
				continue;
			
			$comment_statement->execute(array(
				':id' => $comment->comment_id,
				':post_id' => $wp->post_id,
				':author' => $comment->comment_author,
				':email' => $comment->comment_author_email,
				':url' => (string)$comment->comment_author_url,
				':ip' => $comment->comment_author_IP,
				':date' => strtotime($comment->comment_date),
				':content' => (string)$comment->comment_content,
				':parent_comment_id' => $comment->comment_parent == 0 ? null : $comment->comment_parent,
				':status' => $comment->comment_approved == 1 ? 'visible' : 'pending',
			));
		}
	}
	
	protected function finalise()
	{
		$this->db->query('UPDATE blog_comments SET email = "daniel@dan.cx", url = "http://dan.cx/" WHERE author = "Daniel15"');
	}
}

//header('Content-Type: text/plain');
echo '<pre>';
$importer = new WordpressImporter();
$importer->connectToDB('daniel15_new', 'root', 'password');
$importer->load('c:/Users/Daniel/Downloads/daniel15039sblog.wordpress.2011-05-03.xml');
$importer->process();
?>
All done!