import Plausible from 'plausible-tracker';

const {trackPageview, enableAutoOutboundTracking} = new Plausible({
	apiHost: 'https://hits.d.sb',
	trackLocalhost: true,
});

trackPageview();
enableAutoOutboundTracking();
