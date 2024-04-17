import Plausible from 'plausible-tracker';

const {trackPageview, enableAutoOutboundTracking} = Plausible({
	apiHost: 'https://d.sb',
	trackLocalhost: false,
});

trackPageview();
enableAutoOutboundTracking();
