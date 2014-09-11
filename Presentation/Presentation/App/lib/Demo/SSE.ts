
import Library = require("lib/Demo/Library");

export class SSE {
    private _sse: sse.IEventSourceStatic;

    static splitOnFirst(s: string, c: string) {
        if (!s) return [s];
        var pos = s.indexOf(c);
        return pos >= 0 ? [s.substr(0, pos), s.substr(pos + 1)] : [s];
    }

    constructor(url: string, channel: string) {
        this._sse = new EventSource(url + '?channel=' + channel + '&t=' + new Date().getTime());

        this._sse.onmessage = this.handle;
        this._sse.onerror = this.onError;
        this._sse.onopen = this.onOpen;
    }

    getState(): sse.ReadyState {
        return this._sse.readyState;
    }

    handle(message: sse.IMessageEvent): void {
        console.log(message);
        var parts = SSE.splitOnFirst(message.data, ' ');
        console.log(parts);
        if (parts[0] !== 'events') return;

        var json = parts[1];
        console.log('json: ' + json);
        var data : Library.Responses.IEvent = json ? JSON.parse(json) : null;
        console.log('data: ' + data);
        if (data === null) return;

        var event = data.domain + '.' + data.eventName;
        console.log('Publishing to: ' + event);
        amplify.publish(event, data);
    }

    onOpen(event: Event): void {

    }

    onError(event: Event): void {
    }
}

import s = Library.Services;
export class Service {
    static resources: s.Definitions = {
        'subscribe': new s.Definition<Services.Subscribe>('/subscribe', 'POST'),
        'unsubscribe': new s.Definition<Services.Unsubscribe>('/unsubscribe', 'POST')
    };

    static Subscribe(model: Services.Subscribe): JQueryPromise<boolean> {
        return Service.resources['subscribe'].request(model);
    }
    static Unsubscribe(model: Services.Unsubscribe): JQueryPromise<boolean> {
        return Service.resources['unsubscribe'].request(model);
    }
}

export module Services {
    export interface Subscribe {
        domain: string;
    }

    export interface Unsubscribe {
        domain: string;
    }
}