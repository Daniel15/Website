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
							<span class="techs"><strong>Technologies used:</strong> ';
						
		foreach ($project['tech'] as $techId)
		{
			$tech = $techs[$techId];
			echo '
							<a href="', $tech['url'], '" title="', $tech['name'], '" class="', $techId, '">', $tech['name'], '</a>';
		}
		
		echo '</span>';
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
