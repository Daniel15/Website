<?php defined('SYSPATH') or die('No direct script access.');

/**
 * Controller for blog administration (posts). Lists all the current posts, and allows editing
 * and deleting of posts.
 */
class Controller_BlogAdmin_Posts extends Controller_BlogAdmin
{
	/**
	 * Post index - List all the posts
	 * @param	bool	True to show published posts, False to show unpublished posts
	 */
	public function action_index()
	{
		$published = $this->request->param('id');
		
		$total_count = Model_Blog_Post::count_posts($published);
		$page_number = !empty($_GET['page']) ? $_GET['page'] : 1;
		$pagination = Pagination::factory(array(
			'total_items' => $total_count,
			'items_per_page' => 50,
		));
		
		$posts = ORM::factory('Blog_Post')
			->where('published', '=', $published)
			->order_by('id', 'desc')
			->limit($pagination->items_per_page)
			->offset($pagination->offset)
			->find_all();
			
		$page = View::factory('blog/admin/posts')
			->bind('posts', $posts)
			->set('pagination', $pagination->render());
			
		$this->template
			->set('title', 'Post Administration')
			->bind('content', $page);
	}
	
	/**
	 * Edit a post
	 * @param	int		ID of post (or null to create a new one)
	 */
	public function action_edit()
	{
		$id = $this->request->param('id');
		$post = ORM::factory('Blog_Post', $id);
		
		// Was the page posted?
		if ($_POST)
		{
			$this->edit($post);
		}
		
		$selected_categories = array();
		$selected_tags = array();
		foreach ($post->categories->find_all() as $category)
			$selected_categories[] = $category->id;
		foreach ($post->tags->find_all() as $tag)
			$selected_tags[] = $tag->id;
		
		$page = View::factory('blog/admin/post')
			->bind('post', $post)
			->set('categories', Model_Blog_Category::aslist())
			->set('tags', Model_Blog_Tag::aslist())
			->bind('selected_categories', $selected_categories)
			->bind('selected_tags', $selected_tags)
			->set('new', !$post->loaded());
			
		$this->template
			->set('title', 'Edit Post')
			->bind('content', $page);
	}
	
	/**
	 * Save the edit to a post, based on POST data
	 * @param	Model_Blog_Post		The post
	 */
	protected function edit(Model_Blog_Post $post)
	{
		$old_id = $post->id;
		
		if (!isset($_POST['tags']))
			$_POST['tags'] = array();
		if (!isset($_POST['categories']))
			$_POST['categories'] = array();
			
		// Make sure main category is always included in categories
		if (!in_array($_POST['maincategory'], $_POST['categories']))
			$_POST['categories'][] = $_POST['maincategory'];
		
		$post->title = Arr::get($_POST, 'title');
		$post->slug = Arr::get($_POST, 'slug');
		$post->date = strtotime(Arr::get($_POST, 'date', 'now'));
		$post->content = Arr::get($_POST, 'content');
		$post->maincategory_id = Arr::get($_POST, 'maincategory');
		$post->published = !empty($_POST['published']);
		$post->save();
		$this->save_categories($post, $_POST['categories'], 'categories');
		$this->save_categories($post, $_POST['tags'], 'tags');
		
		
		// Post to Twitter and Facebook if enabled
		if (!empty($_POST['twitter']))
		{
			$twitter = new Social_Twitter();
			$twitter->new_post($post);
		}
		
		if (!empty($_POST['facebook']))
		{
			$facebook = new Social_Facebook();
			$facebook->new_post($post);
		}
		
		// Was it a new post?
		if ($old_id != $post->id)
			// Redirect to the "correct" (with post ID) URL
			$this->request->redirect('blogadmin/posts/edit/' . $post->id);
	}
	
	/**
	 * Save the categories or tags for a post. Synchronised POSTed list with the list currently in
	 * the database, adding and removing records as needed
	 * @param	Model_Blog_Post		The post
	 * @param	array				The post values
	 * @param	string				Type ("categories" or "tags")
	 */
	protected function save_categories(Model_Blog_Post& $post, $values, $type = 'categories')
	{
		$current = array();
		$current_rows = $post->$type->find_all();
			
		foreach ($current_rows as $row)
			$current[] = $row->id;
			
		// First delete any records in the DB that are NOT ticked
		foreach ($current as $id)
		{
			if (!in_array($id, $values))
				$post->remove($type, $id);
		}
		
		// Now add items missing from the database
		foreach ($values as $id)
		{
			if (!in_array($id, $current))
				$post->add($type, $id);
		}
	}
}
?>