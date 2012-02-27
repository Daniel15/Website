<?php
abstract class Compressor
{
	protected $_inputDir;
	protected $_cacheDir;
	
	public function __construct($inputDir, $cacheDir)
	{
		$this->_inputDir = $inputDir;
		$this->_cacheDir = $cacheDir;
	}
	
	abstract public function compress($file);
	public function compressFiles($files)
	{
		global $siteData;
		
		$output = '/* Combined file generated ' . date('Y-m-d G:i') . ' */';
		
		foreach ($files as $file)
		{
			$fullPath = $this->_inputDir . $file;
			$md5 = md5_file($fullPath);
			
			$output .= "\n/* " . $file . " */\n";
			
			// Was this file already compressed?
			if (file_exists($this->_cacheDir . $md5))
			{
				$output .= file_get_contents($this->_cacheDir . $md5);
			}
			else
			{
				$compressed = $this->compress($fullPath);
				$output .= $compressed;
				// Cache this result so it doesn't need to be recompressed later
				file_put_contents($this->_cacheDir . $md5, $compressed);
			}
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
		$file = preg_replace('~url\(([\'"]?)([^/])~', 'url($1../../../$2', $file);
		return $file . "\n";
	}
}

class Compressor_LESS extends Compressor
{
	public function compress($file)
	{
		$file = trim(shell_exec('lessc -x ' . escapeshellarg($file)));
		// Replace relative URLs
		$file = preg_replace('~url\(([\'"]?)([^/])~', 'url($1../../../$2', $file);
		return $file . "\n";
	}
}

echo 'Compressing JS and CSS... ';
$siteConfig = include '../application/config/site.php';
if (empty($siteConfig))
	$siteConfig = array();

$directory = dirname(__DIR__) . '/';
$basename = date('d_Hi');
$outputDirBase = 'combined/' . date('Y') . '/' . date('m') . '/';
$outputDir = $directory . $outputDirBase;
$cacheDir = __DIR__ . '/cache/';

@mkdir($outputDir, 0777, true);
@mkdir($cacheDir, 0777, true);


$sets = array(
	'latestJS' => array(
		'type' => 'js',
		'output' => $basename . '.js',
		'files' => array(
			// Framework
			'js/framework/core.js', 'js/framework/ajax.js', 'js/framework/dom.js', 
			'js/framework/events.js', 'js/framework/storage.js', 
			// Site
			'js/core.js', 'js/site.js', 'js/blog.js', 'js/socialfeed.js',
		),
	),
	'latestCSS' => array(
		'type' => 'less',
		'output' => $basename . '.css',
		'files' => array('css/main.less'),
	),
	'syntaxHighlightJS' => array(
		'type' => 'js',
		'output' => $basename . '_syntax.js',
		'files' => array(
			'lib/syntaxhighlighter/shCore.js', 'lib/syntaxhighlighter/shBrushJScript.js', 
			'lib/syntaxhighlighter/shBrushPhp.js', 'lib/syntaxhighlighter/shBrushCSharp.js', 
			'lib/syntaxhighlighter/shBrushXml.js', 'lib/syntaxhighlighter/shBrushPlain.js', 
			'lib/syntaxhighlighter/shBrushDelphi.js',
			
			'js/syntaxhighlighter.js',
		),
	),
);

foreach ($sets as $name => $set)
{	
	$className = 'Compressor_' . strtoupper($set['type']);	
	$compressor = new $className($directory, $cacheDir);
	
	file_put_contents($outputDir . $set['output'], $compressor->compressFiles($set['files']));
	$siteConfig[$name] = $outputDirBase . $set['output'];
}

file_put_contents('../application/config/site.php', '<?' . 'php return ' . var_export($siteConfig, true) . '; ?' . '>');

echo 'Done, ', $outputDir, $basename, "\n";
?>