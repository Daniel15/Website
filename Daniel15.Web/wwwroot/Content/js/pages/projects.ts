import {$, $$} from '../dom';

// Temporary until this code is refactored
// (...famous last words)
declare global {
	interface Window {
		tech_descs: Readonly<{[name: string]: string}>;
	}
}

let currentTech: HTMLElement | null = null;

/**
 * Projects page
 */
export function initProjects(): void {
	for (let techLink of $('#sidebar').querySelectorAll('a')) {
		techLink.addEventListener('click', showTech);
	}
}

function showTech(evt: MouseEvent): void {
	const link = evt.target as HTMLElement;
	if (link == null) {
		return;
	}
	const listItem = link.parentNode as HTMLElement;
	if (listItem == null) {
		return;
	}

	if (currentTech == listItem) {
		unselectTech();
		evt.preventDefault();
		evt.stopPropagation();
		return;
	}

	const id = listItem.id.replace('tech-', '');
	const name = link.textContent;

	// Unselect any currently selected item
	if (currentTech) {
		currentTech.classList.remove('active');
	}

	currentTech = listItem;
	listItem.classList.add('active');

	// Show the tech header and some info
	$('#intro').style.display = 'none';
	const info = $('#tech-info');
	info.style.display = 'block';
	$('h2', info).innerHTML = `About ${name}`;
	// tech_descs is defined in Views/Projects/Index.cshtml towards the end
	// TODO: Clean this up
	$('div', info).innerHTML = window.tech_descs[id];

	// Get all the projects that have used this technology
	const techProjects = Array.from($$(`.uses-${id}`, $('#content')));
	$('#tech-count').innerHTML = techProjects.length.toString();

	toggleAllProjects(false);
	// Now show the relevant projects
	for (let techProject of techProjects) {
		techProject.style.display = '';
		techProject.classList.add('visible');
	}

	// Show or hide the "active projects" section depending on if it actually contains anything.
	const activeProjects = $('#active_projects');
	activeProjects.style.display = activeProjects.querySelector('.visible')
		? ''
		: 'none';

	evt.preventDefault();
	evt.stopPropagation();
}

/**
 * Unselect the currently selected technology
 */
function unselectTech(): void {
	if (currentTech == null) {
		return;
	}

	currentTech.classList.remove('active');
	currentTech = null;
	// Hide the info
	$('#intro').style.display = '';
	$('#tech-info').style.display = '';

	// Show all projects
	toggleAllProjects(true);
	$('#active_projects').style.display = '';
}

/**
 * Hide or show all the projects
 */
function toggleAllProjects(toggle: boolean): void {
	// Toggle all the projects. The "visible" class is just used by the code that
	// hides/shows "Active projects" depending on if any are actually active

	const projects = $$('.projects > li', $('#content'));
	for (let project of projects) {
		project.style.display = toggle ? '' : 'none';
		project.classList.toggle('visible', toggle);
	}
}
