<?php
// Load the projects data
include $this->dir . 'data/projects.php';
// Since the data file changes this page too, the last modified date is the maximum
// of either this page itself, or the data file.
$this->lastModified = max(filemtime(__FILE__), filemtime($this->dir . 'data/projects.php'));
// These are the tech descriptions, used in JavaScript later on.
$tech_descs = array();

$this->title = 'Projects';
$this->meta = array(
	'Description' => 'A listing of projects that I\'m currently working on (including this site, the VCE ENTER Calculator, rTorrentWeb, Sharpamp, ObviousSpoilers.com, DanSoft Australia, and more).'
);
//$this->js = 'window.addEvent(\'domready\', Projects.init);';
$this->pageID = 'projects';
$this->sidebarType = 'right';
$this->sidebar = '
			<h2>Technologies</h2>
			<p>I use a wide range of technologies in my various projects. Select one to find out more about it and where I\'ve used it:</p>
			<ul>';

foreach ($techs as $key => $tech)
{
	/*$this->sidebar .= '
				<li><a href="' . $tech['url'] . '"><img width="16" height="16" src="images/tech_icons/' . $tech['icon'] . '.png" alt="' . $tech['name'] . ' icon" /> ' . $tech['name'] . '</a></li>';*/
	$this->sidebar .= '
				<li id="tech-' . $key . '"><a href="' . $tech['url'] . '"><img width="16" height="16" src="images/tech_icons/' . $tech['icon'] . '.png" alt="' . $tech['name'] . ' icon" /> ' . $tech['name'] . '</a></li>';
				
	// Save the description, for JavaScript later
	$tech_descs[$key] = isset($tech['desc']) ? $tech['desc'] : '';
}

$this->sidebar .= '
			</ul>';

// Output a list of projects
function output_projects($projects, &$techs, &$techs2)
{

	$alt = false;
	
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
		
		echo '">
							', (!empty($project['url']) ? '<a href="' . $project['url'] . '">' : ''), '<img class="thumb" src="images/', $project['thumb'], '.png" alt="Thumbnail for ', $project['name'], '" height="', $project['thumb_height'], '" width="', $project['thumb_width'], '" />', (!empty($project['url']) ? '</a>' : ''), '
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
		// Toggle the alternate class
		$alt = !$alt;
	}
}
?>
				<p id="intro">This site lists most of the large projects I've worked on in the past. There are also many other, smaller projects that are not listed on this page (if I listed them all, this page would be <em>way</em> too long...)</p>
				
				<div id="tech-info">
					<h2>About the selected technology</h2>
					<div>Loading...</div>
					<p>I have worked on at least <strong id="tech-count">9000</strong> projects using this technology:</p>
				</div>
				
				<div id="active_projects">
					<h2>Active Projects</h2>
					<p>These are all the projects I'm working on at the moment.</p>
					<ul class="projects"><?php
output_projects($projects, $techs, $techs2);
?>

					</ul>
				</div>
				
				<div id="other_projects">
					<h2>Other Projects</h2>
					<p>This section lists all the projects I have completed, as well as projects that I was previously working on but don't update any more due to time constraints.</p>
					<ul class="projects"><?php
output_projects($prev_projects, $techs, $techs2);
?>

					</ul>
				</div>
				
				<!-- I guess this is kinda ugly, but it's better than doing a separate request per tech when you click it -->
				<script type="text/javascript">
				//<![CDATA[ 
				var tech_descs = <?php echo json_encode($tech_descs); ?>

				//]]>
				</script>