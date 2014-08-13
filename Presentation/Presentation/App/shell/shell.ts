/// <reference path="../../Scripts/typings/durandal/durandal.d.ts" />

import _router = require('plugins/router');
import app = require('durandal/app');

export var router = _router;

export function search() {
    app.showMessage('Search not yet implemented...');
}

export function activate() {
    router.map([
        { route: '', title: 'Welcome', moduleId: 'welcome/welcome', nav: true },
        { route: 'flickr', moduleId: 'flickr/flickr', nav: true }
    ]).buildNavigationModel();

    return router.activate();
}
