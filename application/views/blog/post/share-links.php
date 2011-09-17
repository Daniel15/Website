			<!-- Begin share links -->
			<div class="share">
			<span>Share on:</span>
			<ul>
<?php
foreach ($share_links as $name => $link)
{
	echo '
				<li class="', $name, '">
					<a href="', $link, '" title="Share on ', ucfirst($name), '">
						<!--', ucfirst($name), '-->123
						<img class="icon" src="res/icons/social/large/facebook.png" />
					</a>
				</li>';
}
?>
			</ul>
			</div>
			<!-- End share links -->