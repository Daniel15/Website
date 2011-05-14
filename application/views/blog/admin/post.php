<?php defined('SYSPATH') or die('No direct script access.'); ?>

<form action="<?php echo Url::site('blogadmin/posts/edit/' . $post->id); ?>" method="post">
	<div id="main">
		<p>
			<label for="title">Title:</label>
			<input type="text" name="title" id="title" size="35" value="<?php echo HTML::chars($post->title); ?>" />
		</p>
		
		<p>
			<label for="slug">Slug:</label>
			<input type="text" name="slug" id="slug" size="35" value="<?php echo HTML::chars($post->slug); ?>" />
		</p>
		
		<p>
			<label for="date">Date:</label>
			<input type="text" name="date" id="date" size="35" value="<?php echo empty($post->date) ? 'now' : date(Kohana::config('blog.full_date_format'), $post->date); ?>" />
		</p>
		
		<p>
			<input type="checkbox" name="published" id="published" <?php echo $post->published ? 'checked="checked"' : '' ?> />
			<label for="published">Published</label>
		
		<p>
			<label for="content">Post:</label><br />
			<textarea name="content" id="content" rows="15" cols="100"><?php echo HTML::chars($post->content); ?></textarea>
		</p>
	</div>
	
	<div id="metadata">
		<p>
			<label for="maincategory">Main category:</label>
			<?php echo Form::select('maincategory', $categories, $post->maincategory_id, array('id' => 'maincategory')) ?>
		</p>
		
		<p>
			<label>Categories:</label><br />
			<div id="categories">
				<?php echo Form::checkbox_list('categories', $categories, $selected_categories); ?>
			</div>
		</p>
		
		<p>
			<label>Tags:</label><br />
			<div id="tags">
				<?php echo Form::checkbox_list('tags', $tags, $selected_tags); ?>
			</div>
		</p>
	</div>
	
	<p style="clear: both" >
		<input type="submit" value="Save"/>
	</p>
</form>