import Plausible from 'plausible-tracker';

const {trackPageview, enableAutoOutboundTracking} = new Plausible({
	apiHost: 'https://d.sb',
	trackLocalhost: false,
});

trackPageview();
enableAutoOutboundTracking();
