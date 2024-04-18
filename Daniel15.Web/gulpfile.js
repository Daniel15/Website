/// <binding AfterBuild='build' Clean='clean' />

var del = require('del'),
	fs = require('fs'),
	git = require('gulp-git'),
    gulp = require('gulp'),
	shell = require('gulp-shell');

var paths = {
	// Located in the drive root, otherwise the paths get too long :/
	tempPublish: 'c:/TempPublish/'
};

gulp.task('clean', function(cb) {
	del([
		paths.tempPublish + '/*'
	], { force: true }, cb);
});

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

gulp.task('build', gulp.series([
	'build:config'
]));

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

gulp.task('deploy:staging', gulp.series(['package']), function () {
	return deploy('staging');
});

gulp.task('deploy:live', gulp.series(['package']), function () {
	return deploy('live');
});
