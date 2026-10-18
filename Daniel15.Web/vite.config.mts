import {FontaineTransform} from 'fontaine';
import {defineConfig} from 'vite';
import path from 'node:path';
import {compression} from 'vite-plugin-compression2';
import {createRequire} from 'node:module';

const require = createRequire(import.meta.url);

export default defineConfig(({mode}) => ({
	base: mode === 'development' ? '' : 'cache',
	build: {
		manifest: true,
		outDir: 'cache',
		sourcemap: true,
	},
	input: 'Content/js/main.ts',
	plugins: [
		compression({algorithms: ['zstd', 'brotli', 'gzip']}),
		FontaineTransform.vite({
			fallbacks: {},
			resolvePath: source => require.resolve(source),
		}),
	],
	root: path.resolve(import.meta.dirname, 'wwwroot'),
}));
