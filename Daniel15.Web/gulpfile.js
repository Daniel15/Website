/// <binding AfterBuild='build' Clean='clean' />

var concat = require('gulp-concat'),
	del = require('del'),
	fs = require('fs');
	git = require('gulp-git'),
    gulp = require('gulp'),
    lazypipe = require('lazypipe'),
    less = require('gulp-less'),
    minifyCSS = require('gulp-minify-css'),
	react = require('gulp-react'),
    rename = require('gulp-rename'),
	shell = require('gulp-shell'),
    uglify = require('gulp-uglify'),
	urlAdjuster = require('gulp-css-url-adjuster');

var webroot = './wwwroot/';
var paths = {
	webroot: webroot,
	css: webroot + 'Content/css/',
	js: webroot + 'Content/js/',
	concatRoot: webroot + 'cache/',
	// Located in the drive root, otherwise the paths get too long :/
	tempPublish: 'c:/TempPublish/'
};

gulp.task('clean', function(cb) {
	del([
		paths.concatRoot + '/*',
		paths.tempPublish + '/*'
	], { force: true }, cb);
});

gulp.task('build:css', function() {
	return gulp
		.src([paths.css + 'main.less', paths.css + 'blogadmin.less'])
		.pipe(less())
		.pipe(urlAdjuster({
			replace: ['../', '/Content/']
		}))
		.pipe(gulp.dest(paths.concatRoot))
		.pipe(rename({ extname: '.min.css' }))
		.pipe(minifyCSS())
		.pipe(gulp.dest(paths.concatRoot));
});

var buildJS = lazypipe()
	.pipe(react, { harmony: true })
	.pipe(gulp.dest, paths.concatRoot)
	.pipe(uglify, { preserveComments: 'some' })
	.pipe(rename, { extname: '.min.js' })
	.pipe(gulp.dest, paths.concatRoot);

gulp.task('build:js:main', function() {
	return gulp
		.src([
			// Framework
			paths.js + 'framework/core.js',
			paths.js + 'framework/ajax.js',
			paths.js + 'framework/dom.js',
			paths.js + 'framework/events.js',

			// Site scripts
			paths.js + 'core.js',
			paths.js + 'site.jsx',
			paths.js + 'blog.js',
			paths.js + 'socialfeed.jsx'
			])
		.pipe(concat('main.js'))
		.pipe(buildJS());
});

gulp.task('build:js:syntaxHighlighter', function() {
	return gulp
		.src([
			paths.js + 'lib/syntaxhighlighter/shCore.js',
			paths.js + 'lib/syntaxhighlighter/shBrushJScript.js',
			paths.js + 'lib/syntaxhighlighter/shBrushPhp.js',
			paths.js + 'lib/syntaxhighlighter/shBrushCSharp.js',
			paths.js + 'lib/syntaxhighlighter/shBrushXml.js',
			paths.js + 'lib/syntaxhighlighter/shBrushDelphi.js',
			paths.js + 'lib/syntaxhighlighter/shBrushPlain.js',
			paths.js + 'lib/syntaxhighlighter/shBrushCss.js',
			paths.js + 'syntaxhighlighter.js'
		])
		.pipe(concat('syntaxHighlighter.js'))
		.pipe(buildJS());
});

gulp.task('build:js:blogadmin', function () {
	return gulp
		.src(paths.js + 'blogadmin.js')
		.pipe(buildJS());
});

gulp.task('build', [
	'build:css',
	'build:js:main',
	'build:js:syntaxHighlighter',
	'build:js:blogadmin',
	'build:config'
]);

gulp.task('build:config', function(cb) {
	git.revParse({ args: 'HEAD' }, function(err, revision) {
		git.exec({ args: 'log --pretty=format:%ad --date=iso -n 1' }, function(err, stdout) {
			var date = Date.parse(stdout);
			var config = {
				Site: {
					Git: {
						Revision: revision,
						Date: date / 1000
					}
				}
			};
			fs.writeFile('config.generated.json', JSON.stringify(config, null, 4), cb);
		});
	});
});

// TODO: Move this into an MSBuild script, there's no need for it to be in Gulp
gulp.task('package', function () {
	return gulp.src('.').pipe(shell('publishToTemp.bat', {cwd: '..'}));
});

function deploy(remoteDir) {
	return gulp.src(paths.tempPublish)
		.pipe(shell(
			'rsync -arvuz --progress --chmod=ug=rwX,o=rX --exclude "config.Production.json" <%= cygwinPath(file.path) %> daniel-deploy@dan.cx:/var/www/dan.cx/' + remoteDir + '/',
			{
				templateData: {
					cygwinPath: function (path) {
						return path.replace(/\\/g, '/').replace('c:/', '/cygdrive/c/') + '/';
					}
				}
			}
		));
}

gulp.task('deploy:staging', ['package'], function () {
	return deploy('staging');
});

gulp.task('deploy:live', ['package'], function () {
	return deploy('live');
});
