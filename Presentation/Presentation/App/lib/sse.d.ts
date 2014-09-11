// Type definitions for Server-Sent Events
// Specification: http://dev.w3.org/html5/eventsource/
// Definitions by: Yannik Hampe <https://github.com/yankee42>

declare var EventSource: sse.IEventSourceStatic;

declare module sse {

    enum ReadyState { CONNECTING = 0, OPEN = 1, CLOSED = 2 }

    interface IEventSourceStatic extends EventTarget {
        new (url: string, eventSourceInitDict?: IEventSourceInit): IEventSourceStatic;
        url: string;
        withCredentials: boolean;
        readyState: ReadyState;
        onopen: (event: Event) => void;
        onmessage: (event: IMessageEvent) => void;
        onerror: (event: Event) => void;
        close: () => void;
    }

    interface IEventSourceInit {
        withCredentials?: boolean;
    }

    interface IMessageEvent extends Event {
        data: string;
        lastEventId: number;
        origin: string;
    }
}