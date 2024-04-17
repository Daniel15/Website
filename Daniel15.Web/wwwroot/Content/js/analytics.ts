import Plausible from 'plausible-tracker';

const {trackPageview, enableAutoOutboundTracking} = Plausible({
	apiHost: 'https://d.sb',
	trackLocalhost: false,
});

trackPageview();
// TODO: This doesn't provide a way to ignore links, which breaks the
// JS-powered links on the projects page.
// https://github.com/plausible/plausible-tracker/blob/c0b87d997d839938c23023d35bac0d6683635bbc/src/lib/tracker.ts#L292
//enableAutoOutboundTracking();
