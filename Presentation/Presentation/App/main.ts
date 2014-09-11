

requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions',
        'knockout.mapping': '../Scripts/knockout.mapping-latest',
    }
});

define('jquery', () => {
    return jQuery;
});
define('knockout', () => {
    return ko;
});
define('amplify', () => {
    return amplify;
});

function splitOnFirst (s: string, c: string) { if (!s) return [s]; var pos = s.indexOf(c); return pos >= 0 ? [s.substring(0, pos), s.substring(pos + 1)] : [s]; };

define(function (require) {
    var app = require('durandal/app'),
        viewLocator = require('durandal/viewLocator'),
        system = require('durandal/system'),
        SSE = require('lib/Demo/SSE');

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

        var source = new SSE.SSE('/event-stream', 'events');

    });
});
 