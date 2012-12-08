<?php defined('SYSPATH') or die('No direct script access.'); ?>

				<p id="intro">This site lists most of the large projects I've worked on in the past. There are also many other, smaller projects that are not listed on this page (if I listed them all, this page would be <em>way</em> too long...)</p>
				
				<div id="tech-info">
					<h2>About the selected technology</h2>
					<div>Loading...</div>
					<p>I have worked on at least <strong id="tech-count">9000</strong> projects using this technology:</p>
				</div>
				
				<div id="active_projects">
					<h2>Active Projects</h2>
					<p>These are all the projects I'm working on at the moment.</p>
					<?php echo $projects; ?>
					
				</div>
				
				<div id="other_projects">
					<h2>Other Projects</h2>
					<p>This section lists all the projects I have completed, as well as projects that I was previously working on but don't update any more due to time constraints.</p>
					<?php echo $prevProjects; ?>

				</div>
				
				<!-- I guess this is kinda ugly, but it's better than doing a separate request per tech when you click it -->
				<script type="text/javascript">
				//<![CDATA[
				var tech_descs = <?php echo json_encode($techDescs); ?>
				//]]>
				</script>