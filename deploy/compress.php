<?php
abstract class Compressor
{
	protected $_inputDir;
	
	public function __construct($inputDir)
	{
		$this->_inputDir = $inputDir;
	}
	
	abstract public function compress($file);
	public function compressFiles($files)
	{
		global $siteData;
		
		$output = '/* Combined file generated ' . date('Y-m-d G:i') . ', based on SVN revision ' . $siteData['svnRevision'] . ' */';
		
		foreach ($files as $file)
		{
			$fullPath = $this->_inputDir . $file;
			$output .= "\n\n/* Begin " . $file . ', modified '
				. date('Y-m-d G:i', filemtime($fullPath)) . ' (md5=' . md5_file($fullPath) . ") */\n";
			$output .= $this->compress($fullPath);
		}
		
		return $output;
	}
	
	protected function webRequest($url, $postData)
	{
		$context = stream_context_create(array('http' => array
		(
			'method' => 'POST',
			'header' => 'Content-type: application/x-www-form-urlencoded',
			'content' => http_build_query($postData),
		)));

		return file_get_contents($url, false, $context);
	}
}

class Compressor_JS extends Compressor
{
	public function compress($file)
	{
		$postData = array(
			'compilation_level'=>'SIMPLE_OPTIMIZATIONS',
			'output_format' => 'text',
			'output_info' => 'compiled_code',
			'js_code' => file_get_contents($file),
		);
		
		return $this->webRequest('http://closure-compiler.appspot.com/compile', $postData);
	}
}

class Compressor_CSS extends Compressor
{
	public function compress($file)
	{
		$postData = array(
			'type' => 'css',
			'file' => file_get_contents($file),
		);
		
		$file = $this->webRequest('http://vps.dan.cx/compress.php', $postData);
		$file = preg_replace('~url\(([\'"]?)~', 'url($1../../../', $file);
		return $file;
	}
}

echo 'Compressing JS and CSS... ';
include '../cms/data/site-data.php';

$directory = '../res/';
$basename = date('d_Hi');
$outputDirBase = 'combined/' . date('Y') . '/' . date('m') . '/';
$outputDir = $directory . $outputDirBase;

@mkdir($outputDir, null, true);

$jsFiles = array('mootools-more-1.3.0.1.js', 'scripts_r1.js');
$cssFiles = array(
	// Main stylesheets
	'style_r2.css', 'pages.css', 'sprites-processed.css', 
	// Print stylesheets
	'print.css',
	// IE hacks
	'style-ie6.css', 'style-ie7.css', 'style-ie8.css'
);

$jsCompress = new Compressor_JS($directory);
file_put_contents($outputDir . $basename . '.js', $jsCompress->compressFiles($jsFiles));

$cssCompress = new Compressor_CSS($directory);
file_put_contents($outputDir . $basename . '.css', $cssCompress->compressFiles($cssFiles));

if (empty($siteData))
	$siteData = array();
	
$siteData['latestJS'] = $outputDirBase . $basename . '.js';
$siteData['latestCSS'] = $outputDirBase . $basename . '.css';

file_put_contents('../cms/data/site-data.php', '<?' . 'php $siteData = ' . var_export($siteData, true) . '; ?' . '>');

echo 'Done, ', $outputDir, $basename, "\n";
?>