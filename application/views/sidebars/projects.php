<?php defined('SYSPATH') or die('No direct script access.'); ?>
			<h2>Technologies</h2>
			<p>I use a wide range of technologies in my various projects. Select one to find out more about it and where I've used it:</p>
			<ul>
<?php
foreach ($techs as $key => $tech)
{
	echo '
				<li id="tech-', $key, '"><a href="', $tech['url'], '"><img width="16" height="16" src="images/tech_icons/', $tech['icon'], '.png" alt="', $tech['name'], ' icon" /> ', $tech['name'], '</a></li>';
}
?>

			</ul>