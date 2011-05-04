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
		$file = preg_replace('~url\(([\'"]?)([^/])~', 'url($1../../../$2', $file);
		return $file;
	}
}

echo 'Compressing JS and CSS... ';
$siteConfig = include '../application/config/site.php';

$directory = '../res/';
$basename = date('d_Hi');
$outputDirBase = 'combined/' . date('Y') . '/' . date('m') . '/';
$outputDir = $directory . $outputDirBase;

@mkdir($outputDir, null, true);

$jsFiles = array(
	// MooTools More (MooTools included via Google AJAX API)
	'mootools-more-1.3.0.1.js', 
	// Syntax highligher
	'../lib/syntaxhighlighter/shCore.js', '../lib/syntaxhighlighter/shBrushJScript.js', '../lib/syntaxhighlighter/shBrushPhp.js', '../lib/syntaxhighlighter/shBrushCSharp.js', '../lib/syntaxhighlighter/shBrushXml.js', '../lib/syntaxhighlighter/shBrushPlain.js',
	// Generic scripts
	'scripts_r1.js'
);

$cssFiles = array(
	// Main stylesheets
	'style_r2.css', 'pages.css', 'sprites-processed.css', 'blog.css',
	// Print stylesheets
	'print.css',
	// IE hacks
	'style-ie6.css', 'style-ie7.css', 'style-ie8.css',
	// Syntax highlighter
	'../lib/syntaxhighlighter/shCore.css', '../lib/syntaxhighlighter/shThemeDefault.css',
);

/*$blogJSFiles = array(
	'../blog/wp-includes/js/l10n.js',
	'../blog/wp-includes/js/comment-reply.js',
	'../blog/wp-content/plugins/wpaudio-mp3-player/sm2/soundmanager2-nodebug-jsmin.js',
	'../blog/wp-content/plugins/wpaudio-mp3-player/wpaudio-mootools.js',
);
$blogCSSFiles = array(
	'../blog/wp-content/themes/Daniel15v4r2/style.css',
	'../blog/wp-content/plugins/wp-pagenavi/pagenavi-css.css',
	'../blog/wp-content/plugins/comment-info-detector/comment-info-detector.css',
	'../blog/wp-content/plugins/wp-syntax/wp-syntax.css',
);*/

$jsCompress = new Compressor_JS($directory);
file_put_contents($outputDir . $basename . '.js', $jsCompress->compressFiles($jsFiles));
//file_put_contents($outputDir . $basename . '_blog.js', $jsCompress->compressFiles($blogJSFiles));

$cssCompress = new Compressor_CSS($directory);
file_put_contents($outputDir . $basename . '.css', $cssCompress->compressFiles($cssFiles));
//file_put_contents($outputDir . $basename . '_blog.css', $cssCompress->compressFiles($blogCSSFiles));

if (empty($siteConfig))
	$siteConfig = array();
	
$siteConfig['latestJS'] = $outputDirBase . $basename . '.js';
$siteConfig['latestCSS'] = $outputDirBase . $basename . '.css';
//$siteConfig['latestBlogJS'] = $outputDirBase . $basename . '_blog.js';
//$siteConfig['latestBlogCSS'] = $outputDirBase . $basename . '_blog.css';

file_put_contents('../application/config/site.php', '<?' . 'php return ' . var_export($siteConfig, true) . '; ?' . '>');

echo 'Done, ', $outputDir, $basename, "\n";
?>