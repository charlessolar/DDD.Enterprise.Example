/// <reference path="../Scripts/typings/requirejs/require.d.ts" />
requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

define('jquery', function () {
    return jQuery;
});
define('knockout', function () {
    return ko;
});

define(function (require) {
    var app = require('durandal/app'), viewLocator = require('durandal/viewLocator'), system = require('durandal/system');

    system.debug(true);

    //>>excludeEnd("build");
    app.title = 'Durandal Starter Kit';

    app.configurePlugins({
        router: true,
        dialog: true
    });

    app.start().then(function () {
        //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
        //Look for partial views in a 'views' folder in the root.
        //viewLocator.useConvention();
        //Show the app by setting the root view model for our application with a transition.
        app.setRoot('shell/shell', 'entrance');
    });
});
//# sourceMappingURL=main.js.map
