// Shhh! You didn't see this.

const KEY_SEQUENCE = [
	'arrowup',
	'arrowup',
	'arrowdown',
	'arrowdown',
	'arrowleft',
	'arrowright',
	'arrowleft',
	'arrowright',
	'b',
	'a',
	'enter',
];
let step = 0;

window.addEventListener('keydown', evt => {
	// If incorrect key is pressed, start again!
	if (evt.key.toLowerCase() !== KEY_SEQUENCE[step]) {
		step = 0;
		return;
	}

	++step;
	// All entered correctly?!
	if (step === KEY_SEQUENCE.length) {
		const head = document.head;
		head.getElementsByTagName('link')[0].disabled = true;

		const newCSS = document.createElement('link');
		newCSS.setAttribute('rel', 'stylesheet');
		newCSS.setAttribute('href', 'Content/not_an_easter_egg/zero.css');
		newCSS.setAttribute('type', 'text/css');
		head.appendChild(newCSS);
	}
});
