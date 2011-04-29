			<ul id="nav"><?php
foreach ($items as $uri => $item)
{
	echo '
				<li', $item['active'] ? ' id="selected"' : '', '><a href="', $uri, '"><span>', $item['title'], '</span></a></li>';
}
?>

			</ul>