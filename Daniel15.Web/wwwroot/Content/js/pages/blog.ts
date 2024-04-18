import {$} from '../dom';

const originalHeight: Map<HTMLElement, number> = new Map();

export function initBlog(): void {
	initSidebar();
}

/**
 * Initialise the blog sidebar
 */
function initSidebar(): void {
	var archivesEl = $('#sidebar-archives');
	if (!archivesEl) {
		return;
	}

	//var years = archives.children();
	//for (var i = 0, count = years.length; i < count; i++) {
	for (let yearEl of archivesEl.children) {
		const innerEl = $('ul', yearEl);
		// Store the height for later (for animating)
		originalHeight.set(innerEl, innerEl.offsetHeight);
		innerEl.style.height = '0';
		$('a', yearEl).addEventListener('click', evt => toggleYear(innerEl, evt));
	}
}

/**
 * Toggle displaying of a year in the sidebar
 */
function toggleYear(innerEl: HTMLElement, evt: MouseEvent): void {
	// Animation is handled by the CSS
	const newHeight =
		innerEl.offsetHeight === 0 ? originalHeight.get(innerEl) ?? 100 : 0;
	innerEl.style.height = `${newHeight}px`;
	evt.preventDefault();
}
