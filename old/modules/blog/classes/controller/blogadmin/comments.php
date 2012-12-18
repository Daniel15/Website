<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller for administering blog comments. Handles approving comments, and marking them as spam
 * (sending to Akismet)
 *
 * @author Daniel15 <daniel at dan.cx>
 */
class Controller_BlogAdmin_Comments extends Controller_BlogAdmin
{
	const ITEMS_PER_PAGE = 30;
	
	/**
	 * Show a listing of comments in a particular status. Uses "page" GET variable to determine
	 * which page to show.
	 * 
	 * @param	string	Status to show
	 */
	public function action_index()
	{
		$comment_status = $this->request->param('id');
		// Default to pending status
		if (empty($comment_status))
			$comment_status = 'pending';
			
		$total_count = Model_Blog_Comment::count_comments($comment_status);
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$pagination = Pagination::factory(array(
			'total_items' => $total_count,
			'items_per_page' => self::ITEMS_PER_PAGE,
			//'view' => 'includes/pagination',
		));
		
		$comments = ORM::factory('Blog_Comment')
			->where('status', '=', $comment_status)
			->order_by('id', 'desc')
			->limit($pagination->items_per_page)
			->offset($pagination->offset)
			->find_all();
			
		$page = View::factory('blog/admin/comments')
			->set('comment_status', ucfirst($comment_status))
			->bind('comments', $comments)
			->set('pagination', $pagination->render());
			
		$this->template
			->set('title', 'Comment Administration')
			->bind('content', $page);
	}
	
	/**
	 * Perform an action on a comment (such as marking it as spam, or approving it
	 * @param	int		ID of the comment
	 */
	public function action_action()
	{
		$comment_id = $this->request->param('id');
		
		$comment = ORM::factory('Blog_Comment', $comment_id);
		$post = $comment->post;
		
		if (isset($_POST['spam']))
		{
			$comment->status = 'spam';
			$this->change_spam_status($comment, 'spam');
		}
		elseif (isset($_POST['ham']))
		{
			$comment->status = 'visible';
			$this->change_spam_status($comment, 'ham');
		}
		elseif (isset($_POST['approve']))
		{
			$this->approve_comment($comment);
		}
		elseif (isset($_POST['unapprove']))
		{
			$comment->status = 'hidden';
		}
		elseif (isset($_POST['delete']))
		{
			$comment->delete();
			// Go back to where they came from
			$this->request->redirect($this->request->referrer());
		}
		else
		{
			die('Unknown action!');
		}
		
		$comment->save();
		$post->recalculate_comments();
		
		// Go back to where they came from
		$this->request->redirect($this->request->referrer());
	}
	
	/**
	 * Handle everything that needs to be done when approving a comment (like sending subscription
	 * emails)
	 * @param	Model_Blog_Comment		Comment
	 */
	public static function approve_comment(Model_Blog_Comment $comment)
	{
		$comment->status = 'visible';
		
		// Check if the post has any subscribers
		$subs = $comment->post->subscriptions->find_all();
		if (count($subs) == 0)
			return;
		
		// Need to email each one
		foreach ($subs as $sub)
		{
			// Skip if the subscriber is the current commenter
			if ($sub->email == $comment->email)
				continue;
				
			// Generate and send the email
			$email = View::factory('email/new_comment')
				->set('comment', $comment)
				->set('sub', $sub);
				
			Email::notification($sub->email, 'New comment on "' . $comment->post->title . '"', $email);
		}
	}
	
	/**
	 * Submit a post to Akismet, marking it as either spam or ham
	 * @param	Model_Blog_Comment	Comment
	 * @param	string				"spam" or "ham"
	 */
	protected function change_spam_status($comment, $status)
	{
		Akismet::factory(array(
			'user_ip' => $comment->ip,
			'user_agent' => $comment->user_agent,
			'referrer' => $comment->post->url(),
			'permalink' => $comment->post->url(),
			'comment_type' => 'comment',
			'comment_author' => $comment->author,
			'comment_author_email' => $comment->email,
			'comment_author_url' => $comment->url,
			'comment_content' => $comment->content
		))->{'submit_' . $status}();
	}
}
?>
