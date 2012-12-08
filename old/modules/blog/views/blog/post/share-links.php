			<!-- Begin share links -->
			<div class="share">
				<ul>
<?php
foreach ($share_links as $name => $link)
{
	echo '
					<li class="', $name, '">
						<a href="', $link, '" title="Share on ', ucfirst($name), '">
							<span class="name">', ucfirst($name), '</span>
							<span class="count">&hellip;</span>
						</a>
					</li>';
}
?>
				</ul>
			</div>
			<!-- End share links -->
