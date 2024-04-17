import {$} from '../dom';

export function initSocialFeed(): void {
	const showMoreLink = $('#show-more');
	showMoreLink.addEventListener('click', evt => {
		loadMore(showMoreLink);
		evt.preventDefault();
	});
}

async function loadMore(showMoreLink: HTMLElement) {
	showMoreLink.textContent = 'Loading...';

	const url = showMoreLink.getAttribute('href');
	if (url == null) {
		return;
	}
	const doc = await loadAndInsertItems(url);

	// Replace "show more" link with new one containing new URL.
	const newShowMoreLink = $('#show-more', doc);
	showMoreLink.replaceWith(newShowMoreLink);
	// Need to initialize the new "show more" link!
	initSocialFeed();
}

export async function loadAndInsertItems(url: string) {
	const response = await fetch(`${url}&partial=true`);
	const parser = new DOMParser();
	const doc = parser.parseFromString(await response.text(), 'text/html');
	const items = doc.querySelectorAll('li.feeditem');

	const feed =
		document.querySelector('ul.socialfeed') ||
		document.querySelector('ul.minifeed');
	if (feed == null) {
		return;
	}

	for (const item of items) {
		feed.appendChild(item);
	}
	return doc;
}
