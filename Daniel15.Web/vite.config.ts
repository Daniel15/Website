import {defineConfig} from 'vite';
import path from 'node:path';
import {compression} from 'vite-plugin-compression2';

export default defineConfig(({mode}) => ({
	base: mode === 'development' ? '' : 'cache',
	build: {
		manifest: true,
		outDir: 'cache',
		sourcemap: true,
	},
	input: 'Content/js/main.ts',
	plugins: [compression({algorithms: ['zstd', 'brotli', 'gzip']})],
	root: path.resolve(__dirname, 'wwwroot'),
}));
