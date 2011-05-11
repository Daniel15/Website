<?php defined('SYSPATH') or die('No direct script access.'); ?>
<ul>
	<li>
		<p>
			<a href="http://feeds.d15.biz/daniel15" style="display: inline-block"><img src="http://feeds.d15.biz/~fc/daniel15?bg=99CCFF&amp;fg=444444&amp;anim=0" height="26" width="88" style="border:0" alt="" style="display: inline-block" /></a>
			<a href="http://twitter.com/Daniel15" style="display: inline-block"><img src="http://twittercounter.com/counter/?username=Daniel15" alt="TwitterCounter for @Daniel15" width="88" height="26" border="0" /></a></p>
		</p>
	</li>
	<li>
		<?php echo View::factory('includes/search'); ?>
	</li>
	<li>
		<h2>Archives</h2>
		<ul id="sidebar-archives">
<?php
foreach ($years as $year => $months)
{
	echo '
			<li>
				<a href="">', $year, ' ▼</a>
				<ul>';
				
	foreach ($months as $month => $count)
	{
		echo '
					<li><a href="', Route::url('blog_archive', array('month' => str_pad($month, 2, '0', STR_PAD_LEFT), 'year' => $year)), '">', $month_names[$month], ' ', $year, ' (', $count, ')</a></li>';
	}
				
	echo '
				</ul>
			</li>';
}
?>
		</ul>
	</li>
	<li>
		<h2>Categories</h2>
		<ul>
<?php
foreach ($categories as $category)
{
	echo '
			<li><a href="', $category->url(), '">', $category->title, '</a></li>';
}
?>
		</ul>
	</li>
</ul>