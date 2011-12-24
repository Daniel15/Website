			<nav>
				<ul><?php
foreach ($items as $uri => $item)
{
	echo '
					<li', $item['active'] ? ' class="selected"' : '', '><a href="', $uri, '">', $item['title'], '</a></li>';
}
?>

				</ul>
			</nav>
