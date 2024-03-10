---
id: 45
title: Restore MySQL dump (backup) using PHP
published: true
publishedDate: 2006-12-08 17:03:28Z
lastModifiedDate: 2006-12-08 17:03:28Z
categories:
- Linux
- PHP

---

<p>The other day, I was looking for an easy way to restore a MySQL dump (or backup, whatever you like to call it) in PHP. I've previously used a segment of the code from <a href="http://www.absoft-my.com/pondok/backup.php">PHP MySQL Backup V 2.2</a> for this, but it didn't seem to support FULLTEXT indicies that well. So, I searched around, but couldn't find anything. I even asked on the PHP IRC channel, and they suggested to use shell_exec to call mysql (unfortunately, I've disabled shell_exec for  security reasons). Looking closer, I noticed that this was actually quite easy to do. <!--more-->Here's the code I wrote to restore a phpMyAdmin MySQL dump (not sure if it works with mysqldump dumps):</p>
<p><pre class="brush: php">
< ?php
/*
 * Restore MySQL dump using PHP
 * (c) 2006 Daniel15
 * Last Update: 9th December 2006
 * Version: 0.2
 * Edited: Cleaned up the code a bit. 
 *
 * Please feel free to use any part of this, but please give me some credit :-)
 */
 
// Name of the file
$filename = 'test.sql';
// MySQL host
$mysql_host = 'localhost';
// MySQL username
$mysql_username = 'root';
// MySQL password
$mysql_password = '';
// Database name
$mysql_database = 'test';

//////////////////////////////////////////////////////////////////////////////////////////////

// Connect to MySQL server
mysql_connect($mysql_host, $mysql_username, $mysql_password) or die('Error connecting to MySQL server: ' . mysql_error());
// Select database
mysql_select_db($mysql_database) or die('Error selecting MySQL database: ' . mysql_error());

// Temporary variable, used to store current query
$templine = '';
// Read in entire file
$lines = file($filename);
// Loop through each line
foreach ($lines as $line)
{
	// Skip it if it's a comment
	if (substr($line, 0, 2) == '--' || $line == '')
		continue;

	// Add this line to the current segment
	$templine .= $line;
	// If it has a semicolon at the end, it's the end of the query
	if (substr(trim($line), -1, 1) == ';')
	{
		// Perform the query
		mysql_query($templine) or print('Error performing query \'<strong>' . $templine . '\': ' . mysql_error() . '<br /><br />');
		// Reset temp variable to empty
		$templine = '';
	}
}

?>
</pre></p>
<p>See? How easy is that? :D</p>
<p><strong>Update:</strong> This also works for mysqldump dumps :)</p>

