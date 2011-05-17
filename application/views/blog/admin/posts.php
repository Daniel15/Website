<?php defined('SYSPATH') or die('No direct script access.'); 

echo '
<h2>Posts</h2>';

if (count($posts) == 0)
	echo 'There are no posts to show.';
else
{
	echo '
<ul id="posts">';

	foreach ($posts as $post)
	{
		echo '
	<li id="post-', $post->id, '">
		<form action="', Url::site('blogadmin/posts/action/' . $post->id), '" method="post">
			', date('Y-m-d', $post->date), ' - <a href="', Url::site('blogadmin/posts/edit/' . $post->id), '">', HTML::chars($post->title), '</a>
		</form>
	</li>';
	}
	
	echo '
</ul>';
}

echo $pagination;
?>