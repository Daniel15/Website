<?php
Daniel15Blog::init();

/**
 * Class containing all the stuff for Danie1l5's Blog
 */
class Daniel15Blog
{
	const MICROBLOG_CAT = 123;
	public static $enableCompression = true;
	public static $siteData;
	
	public static function init()
	{
		// We support sidebars!
		if (function_exists('register_sidebar'))
			register_sidebar();
			
		// Add the various filters
		add_filter('body_class', 			'Daniel15Blog::body_class');
		add_filter('get_comments_number', 	'Daniel15Blog::comment_count', 0);
		add_filter('pre_get_posts', 		'Daniel15Blog::exclude_microblog');
		// Actions
		add_action('widgets_init', 			'Daniel15Blog::load_widgets');
		
		if (self::$enableCompression)
		{
			self::init_compression();
			add_action('wp_print_styles',	'Daniel15Blog::post_init_compression');
		}
	}
	
	public static function init_compression()
	{
		require(__DIR__ . '/../cms/data/site-data.php');
		self::$siteData = (object)$siteData;
	}
	
	public static function post_init_compression()
	{
		// Remove combined JS files
		wp_dequeue_script('soundmanager');
		wp_dequeue_script('wpaudio');
		wp_dequeue_script('l10n');
		//wp_enqueue_script('daniel15_combined', '/res/' . self::$siteData->latestBlogJS, null, null); 
		
		// Remove combined CSS files
		wp_dequeue_style('wp-pagenavi');
		wp_dequeue_style('comment-info-detector');
		wp_dequeue_style('wp-syntax');
	}
	
	/**
	 * Register widgets
	 */
	public static function load_widgets()
	{
		register_widget('Daniel15Microblog');
	}
	
	public static function body_class($classes)
	{
		$classes[] = 'col-right';
		return $classes;
	}
	
	/**
	 * Exclude pings from comment count
	 */
	public static function comment_count($count)
	{
		global $id;
		$comments_by_type = &separate_comments(get_comments('post_id=' . $id));
		return count($comments_by_type['comment']);
	}
	
	/**
	 * Exclude microblog from homepage
	 */
	public static function exclude_microblog($query)
	{
		if ($query->is_home)
			//$query->set('cat', '-' . self::MICROBLOG_CAT);
			// "Fix" for WordPress 3.1. http://wordpress.org/support/topic/wp-31-breaks-rss-customization-via-exclude_category
			$query->set('category__not_in', array(self::MICROBLOG_CAT));
		return $query;
	}
}

// Microblog sidebar widget
class Daniel15Microblog extends WP_Widget
{
	const POSTS = 10;
	
	public function Daniel15Microblog()
	{
		parent::WP_Widget(false, $name = 'Daniel15 Microblog');
	}
	
	public function widget($args, $instance)
	{
		global $post;
		$post_old = $post;
		
		extract($args);
		$title = apply_filters('widget_title', $instance['title']);
		
		echo $before_widget;
		if ($title)
			echo $before_title, $title, $after_title; 
			
		$posts = new WP_Query('showposts=' . self::POSTS . '&cat=' . Daniel15Blog::MICROBLOG_CAT);
		
		echo '<ul>';
		
		while ($posts->have_posts())
		{
			$posts->the_post();
			
		?>
			<li>
				<?php if (has_image()) : ?><a href="<?php the_permalink(); ?>"><img class="has-image" src="/res/icons/picture.png" alt="This post has an image attached" title="This post has an image attached" /></a><?php endif; the_excerpt(); ?> 
				<span><?php the_time('j M Y'); ?> <a href="<?php the_permalink(); ?>" title="Permanent link to <?php the_title_attribute(); ?>" rel="bookmark">View</a></span>
			</li>
		<?php
		}
		?>
			<li><a href="<?php echo get_category_link(Daniel15Blog::MICROBLOG_CAT); ?>">View all</a></li>
		</ul>
		<?
		
		echo $after_widget;
		
		$post = $post_old;
	}
	
	public function form($instance)
	{
		$title = esc_attr($instance['title']);
		?>
		<p><label for="<?php echo $this->get_field_id('title'); ?>"><?php _e('Title:'); ?> <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo $title; ?>" /></label></p>
		<?php 
	}
	
	public function update($new_instance, $old_instance)
	{

		$instance = $old_instance;
		$instance['title'] = strip_tags($new_instance['title']);
		return $instance;
	}	
}

function is_microblog()
{
	return is_category('microblog') || in_category('microblog');
}

function has_image()
{
	return strpos(get_the_content(), '<img') !== false;
}

// http://sivel.net/2008/10/wp-27-comment-separation/	
function list_pings($comment, $args, $depth)
{
	$GLOBALS['comment'] = $comment;
?>
	<li id="comment-<?php comment_ID(); ?>"><?php comment_author_link(); ?>
<?php
}
?>