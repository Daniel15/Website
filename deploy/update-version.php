<?php
include '../cms/data/site-data.php';
if (empty($siteData))
	$siteData = array();

if (PHP_OS == 'WINNT')
{
	$svnversion = 'c:\cygwin\bin\svnversion';
}
else
{
	$svnversion = 'svnversion';
}
	
$version = shell_exec($svnversion . ' -n ' . __DIR__ . '/../');

// If there's a colon, split on it
if (strpos($version, ':') !== false)
{
	$version_bits = explode(':', $version);
	$version = (int)$version_bits[1];
}
else
{
	$version = (int)$version;
}

$siteData['svnRevision'] = $version;

file_put_contents('../cms/data/site-data.php', '<?' . 'php $siteData = ' . var_export($siteData, true) . '; ?' . '>');

?>