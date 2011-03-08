<?php
$this->title = 'What I\'ve Been Doing';
$this->pageID = 'socialfeed_nojs';
$this->lastModified = null;

header('Location: feed.htm');
die();
?>
<h2>What I've Been Doing</h2>
<?php
include WEB_ROOT . '/socialfeed/loadhtml.php';
?>