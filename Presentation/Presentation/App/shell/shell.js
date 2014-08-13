/// <reference path="../../Scripts/typings/durandal/durandal.d.ts" />
define(["require", "exports", 'plugins/router', 'durandal/app'], function(require, exports, _router, app) {
    exports.router = _router;

    function search() {
        app.showMessage('Search not yet implemented...');
    }
    exports.search = search;

    function activate() {
        exports.router.map([
            { route: '', title: 'Welcome', moduleId: 'welcome/welcome', nav: true },
            { route: 'flickr', moduleId: 'flickr/flickr', nav: true }
        ]).buildNavigationModel();

        return exports.router.activate();
    }
    exports.activate = activate;
});
