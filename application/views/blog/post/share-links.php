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
							<span class="count">', ucfirst($name), '</span>
							<img class="icon" src="res/icons/social/large/', $name, '.png" />
						</a>
					</li>';
}
?>
				</ul>
			</div>
			<!-- End share links -->