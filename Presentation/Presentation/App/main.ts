

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
        system = require('durandal/system');

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

        // Connect to sse
        var source = new EventSource('/event-stream?channel=events&t=' + new Date().getTime());

        source.onopen = (e) => {
            console.log('con');
            console.log(e);
        };
        source.onmessage = (e) => {
            console.log('here');
            console.log(e);
            //var parts = splitOnFirst(e.data, ' ');
            //var json = parts[1];
            //var msg = json ? JSON.parse(json) : null;

            //if( msg != null && msg.Urn !== undefined )
            //    amplify.publish(msg.Urn, msg.Payload);

        };
    });
});
 