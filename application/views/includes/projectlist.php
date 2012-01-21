<?php defined('SYSPATH') or die('No direct script access.'); ?>

					<ul class="projects"><?php
foreach ($projects as $project)
{
	// Defaults
	if (empty($project['thumb_height']))
		$project['thumb_height'] = 160;
	if (empty($project['thumb_width']))
		$project['thumb_width'] = 200;
		
	echo '
						<li class="';
	
	// Add a list of technologies as classes
	foreach ($project['tech'] as $tech)
		echo 'uses-', $tech, ' ';	
	
	echo '">';
	
	// Is there a thumbnail?
	if (!empty($project['thumb']))
	{
		echo '
							', (!empty($project['url']) ? '<a href="' . $project['url'] . '">' : ''), '<img class="thumb" src="images/', $project['thumb'], '.png" alt="Thumbnail for ', $project['name'], '" height="', $project['thumb_height'], '" width="', $project['thumb_width'], '" />', (!empty($project['url']) ? '</a>' : '');
	}
	
	echo '
							<h3>', (!empty($project['url']) ? '<a href="' . $project['url'] . '">' . $project['name'] . '</a>' : $project['name']) , '</h3>
							<strong>Type:</strong> ', $project['type'], '<br />
							<strong>Description:</strong> ', $project['description'];
	// Do we have a date?
	if (!empty($project['date']))
		echo '<br />
							<strong>Date:</strong> ', $project['date'];
					
	// Here comes the tech
	if (!empty($project['tech']))
	{
		echo '<br />
							<strong>Technologies used:</strong> ';
						
		foreach ($project['tech'] as $tech)
		{
			$tech = $techs[$tech];
			echo '
							<a class="tech" href="', $tech['url'], '" title="', $tech['name'], '"><img width="16" height="16" src="images/tech_icons/', $tech['icon'], '.png" alt="', $tech['name'], ' icon" /></a>';
		}
	}
	
	if (!empty($project['tech2']))
	{
		$techlist = array();
		foreach ($project['tech2'] as $tech)
			$techlist[] = $techs2[$tech];
		
		echo '
							+ ', implode(', ', $techlist);
	}
						
	echo '
						</li>';
}
?>

					</ul>
