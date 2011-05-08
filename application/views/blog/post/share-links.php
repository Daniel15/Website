			<!-- Begin share links -->
			<div class="share">
			<span>Share on:</span>
			<ul>
				<li class="twitter"><a href="http://twitter.com/share?<?php echo http_build_query(
array(
	'url' => $post->short_url(),
	'via' => 'Daniel15',
	'text' => $post->title,
	'related' => 'Daniel15',
	'count' => 'horizontal',
	'counturl' => $post->url(true),
)); ?>">Twitter</a></li>
				<li class="facebook"><a href="http://www.facebook.com/plugins/like.php?<?php echo http_build_query(
array(
	'href' => $post->url(true),
	'layout' => 'standard',
	'show_faces' => 'false',
	'width' => 450,
	'action' => 'like',
	'colorscheme' => 'light',
)); ?>">Facebook</a></li>
			</ul>
			</div>
			<!-- End share links -->