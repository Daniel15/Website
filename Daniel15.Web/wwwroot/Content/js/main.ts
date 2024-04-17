import '../css/main.less';

import {initIndex} from './pages/home';
import {initProjects} from './pages/projects';
import {initSocialFeed} from './pages/socialfeed';

import './analytics';
import '../not_an_easter_egg/secret';

switch (document.body.id) {
	case 'site-index':
		initIndex();
		break;

	case 'site-socialfeed':
		initSocialFeed();
		break;

	case 'project-index':
		initProjects();
		break;
}
