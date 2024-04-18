import '../css/main.less';

import {initBlog} from './pages/blog';
import {initIndex} from './pages/home';
import {initProjects} from './pages/projects';
import {initSocialFeed} from './pages/socialfeed';

import {trackPageview} from './analytics';
import {attachKonamiCodeListener} from '../not_an_easter_egg/secret';

function initPage(): void {
	const controller = document.body.dataset.controller;
	const action = document.body.dataset.action;
	switch (controller) {
		case 'blog':
			initBlog();
			break;

		case 'projects':
			initProjects();
			break;

		case 'site':
			switch (action) {
				case 'index':
					initIndex();
					break;

				case 'socialfeed':
					initSocialFeed();
					break;
			}
			break;
	}
}

trackPageview();
initPage();
attachKonamiCodeListener();
