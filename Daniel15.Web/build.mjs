// Builds JavaScript using esbuild
import * as esbuild from 'esbuild';
import compress from 'esbuild-plugin-compress';

const SOURCE_DIR = './wwwroot/Content/js/';
const OUTPUT_DIR = './wwwroot/cache/';

const baseOptions = {
	bundle: true,
	entryPoints: [SOURCE_DIR + 'analytics.ts'],
	outfile: OUTPUT_DIR + 'analytics.js',
	sourcemap: true,
	target: ['chrome58', 'firefox57', 'safari11', 'edge18'],
};

const devOptions = baseOptions;

const prodOptions = {
	...baseOptions,
	minify: true,
	outfile: OUTPUT_DIR + 'analytics.min.js',
};

const prodCompressOptions = {
	...prodOptions,
	write: false,
	plugins: [
		compress({
			outputDir: '',
			exclude: ['**/*.map'],
		}),
	],
};

await esbuild.build(devOptions);
await esbuild.build(prodOptions);
await esbuild.build(prodCompressOptions);
