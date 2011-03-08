<?php
define('IN_CMS', true);
define('WEB_ROOT', __DIR__);
require('cms/cms.php');
$cms = new CMS();
$cms->run();
?>