export function $(
	selector: string,
	parent: ParentNode = document,
): HTMLElement {
	const el = parent.querySelector(selector);
	if (el == null) {
		throw new Error(`Count not find element "${selector}"`);
	}
	if (!(el instanceof HTMLElement)) {
		throw new Error(`Not a HTMLElement: ${selector}`);
	}
	return el;
}

export function* $$(
	selector: string,
	parent: ParentNode = document,
): Generator<HTMLElement, void, void> {
	const els = parent.querySelectorAll(selector);
	for (let el of els) {
		if (el instanceof HTMLElement) {
			yield el;
		}
	}
}
