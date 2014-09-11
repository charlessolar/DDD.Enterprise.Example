export class SSE {
    private _sse: sse.IEventSourceStatic;
    private _observers: Observers.IObserver[];

    constructor(url: string) {
        this._sse = new EventSource(url + '&t=' + new Date().getTime());

        this._sse.onmessage = this.handle;
        this._sse.onerror = this.onError;
        this._sse.onopen = this.onOpen;
    }

    getState(): sse.ReadyState {
        return this._sse.readyState;
    }

    addObserver(observer: Observers.IObserver): void {
        this._observers.push(observer);
    }
    removeObserver(observer: Observers.IObserver): void {
        var idx = this._observers.indexOf(observer);
        if (idx == -1) return;
        this._observers.splice(idx, 1);
    }

    handle(message: sse.IMessageEvent): void {

    }

    onOpen(event: Event): void {

    }

    onError(event: Event): void {
    }
}

export module Observers {
    export interface IObserver {
        Connected(event: Event): void;
        Error(event: Event): void

        All? (type: string, data: string): void;
    }

    export class AmplifyObserver implements IObserver {
        Connected(event: Event): void {
            amplify.publish("application", "event.connected");
        }
        Error(event: Event): void {
            amplify.publish("application", "event.error");
        }

        All(type: string, data: any): void {

        }
    }
}

export module Events {
    export interface Event {
        type: string;
        rawData: string;
    }
}