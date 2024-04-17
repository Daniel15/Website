// Builds JavaScript using esbuild
import * as esbuild from 'esbuild';
import compress from 'esbuild-plugin-compress';
import {lessLoader} from 'esbuild-plugin-less';
import inlineImage from 'esbuild-plugin-inline-image';

const SOURCE_DIR = './wwwroot/Content/';
const OUTPUT_DIR = './wwwroot/cache/';

const baseOptions = {
	bundle: true,
	entryPoints: [SOURCE_DIR + 'js/main.ts'],
	legalComments: 'inline',
	outfile: OUTPUT_DIR + 'main.js',
	plugins: [inlineImage({limit: 5000}), lessLoader()],
	sourcemap: true,
	target: ['chrome58', 'firefox57', 'safari11', 'edge18'],
};

const devOptions = baseOptions;

const prodOptions = {
	...baseOptions,
	minify: true,
	outfile: OUTPUT_DIR + 'main.min.js',
};

const prodCompressOptions = {
	...prodOptions,
	write: false,
	plugins: [
		...prodOptions.plugins,
		compress({
			outputDir: '',
			exclude: ['**/*.map'],
		}),
	],
};

// Run all in parallel
await Promise.allSettled([
	esbuild.build(devOptions),
	esbuild.build(prodOptions),
	esbuild.build(prodCompressOptions),
]);
