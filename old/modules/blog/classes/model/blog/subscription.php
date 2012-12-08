<?php defined('SYSPATH') or die('No direct script access.');

/**
 * A subscription to blog comments.
 * @author Daniel Lo Nigro <daniel at dan.cx>
 */
class Model_Blog_Subscription extends ORM
{
	protected $_belongs_to = array(
		'post' => array(
			'model' => 'Blog_Post',
		),
	);
	/**
	 * Get the URL to unsubscribe from this subscription
	 * @param	bool	If true, include protocol (http://) and host name at start of URL
	 */
	public function unsubscribe_url($protocol = false)
	{
		return Route::url('blog_unsub', array(
			'year' => date('Y', $this->post->date),
			'month' => date('m', $this->post->date),
			'slug' => $this->post->slug,
			'email' => $this->email,
		), $protocol);
	}
}
?>