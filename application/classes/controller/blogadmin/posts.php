<?php defined('SYSPATH') or die('No direct script access.');

class Controller_BlogAdmin_Posts extends Controller_BlogAdmin
{
	public function action_edit($id = null)
	{
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
			->bind('selected_tags', $selected_tags);
			
		$this->template
			->set('title', 'Edit Post')
			->bind('content', $page);
	}
	
	protected function edit($post)
	{
		$old_id = $post->id;
		
		$post->title = Arr::get($_POST, 'title');
		$post->slug = Arr::get($_POST, 'slug');
		$post->date = strtotime(Arr::get($_POST, 'date', 'now'));
		$post->content = Arr::get($_POST, 'content');
		$post->maincategory_id = Arr::get($_POST, 'maincategory');
		$post->published = !empty($_POST['published']);
		$post->save();
		$this->save_categories($post, $_POST['categories'], 'categories');
		$this->save_categories($post, $_POST['tags'], 'tags');
		
		// If it was a new post, redirect to the "proper" edit URL
		if ($old_id != $post->id)
			$this->request->redirect('blogadmin/posts/edit/' . $post->id);
	}
	
	protected function save_categories(&$post, $values, $type = 'categories')
	{
		$plural = $type;//Inflector::plural($type);
		$current = array();
		$current_rows = $post->{$plural}->find_all();
			
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