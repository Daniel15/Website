			<!-- Begin share links -->
			<div class="share">
			<span>Share on:</span>
			<ul>
<?php
foreach ($share_links as $name => $link)
{
	echo '
				<li class="', $name, '">
					<a class="count" href="', $link, '" title="Share on ', ucfirst($name), '">
						<!--', ucfirst($name), '-->123
					</a>
					<a class="icon" href="', $link, '" title="Share on ', ucfirst($name), '">
						<img class="icon" src="res/icons/social/large/', $name, '.png" />
					</a>
					
				</li>';
}
?>
			</ul>
			</div>
			<!-- End share links -->