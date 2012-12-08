<?php
// "tk" parameter from badge URL
$tk = 'z01q6amlqaf80ct0iuvnq226055735i723g9omh9525cu7ce7onoqd5vm7quktkdlts0i5d6c8nr113mhh7e06mlu92gmbv1506gcp26fdn3c45cpqlu652rb6ksdsodpjb95s019nqarbqo';
// URL for the badge
$badge_url = 'http://www.google.com/talk/service/badge/Show?tk=%s';
// URL to initiate chat
$chat_url = 'http://www.google.com/talk/service/badge/Start?tk=%s';
// Base URL for icons
$base_url = 'http://www.google.com';

$badge_data = file_get_contents(sprintf($badge_url, $tk));
//$badge_data = file_get_contents('c:/temp/online.htm');
$data = array();

// Grab the icon URL
preg_match('~<img id="b" src="([^"]+)"~', $badge_data, $matches);
$data['icon'] = $base_url . $matches[1];

// Grab the status message
preg_match('~(.+)</div></div></body>~', $badge_data, $matches);
$data['statusText'] = $matches[1];

// Set status based on icon
if (strpos($data['icon'], 'online') !== false)
	$data['status'] = 'Online';
elseif (strpos($data['icon'], 'busy') !== false)
	$data['status'] = 'Busy';
else
	$data['status'] = 'Offline';
	
// Only show chat link if status is "Online"
if ($data['status'] == 'Online')
{
	$data['chatURL'] = sprintf($chat_url, $tk);
}

header('Content-Type: text/javascript');
header('Cache-Control: max-age=0, no-cache, no-store, must-revalidate');
header('Pragma: no-cache');
header('Expires: Fri, 01 Jun 1990 00:00:00 GMT');
echo json_encode($data);
?>