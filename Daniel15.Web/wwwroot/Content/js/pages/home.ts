import {$} from '../dom';
import {loadAndInsertItems} from './socialfeed';

/**
 * Initialise the index page
 */
export async function initIndex(): Promise<void> {
	$('#email_address').innerHTML = '&#100;&#064;&#100;&#046;&#115;&#098;';

	await loadAndInsertItems('/socialfeed.htm?count=10&showDescription=false');
	const loadingPlaceholder = $('.minifeed-loading');
	loadingPlaceholder.parentElement?.removeChild(loadingPlaceholder);
}
