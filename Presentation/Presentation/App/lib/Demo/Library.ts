
export class Guid {
    private id: string;
    private static emptyGuid: Guid = new Guid("00000000-0000-0000-0000-000000000000");
    constructor(id: string) {
        this.id = id.toLowerCase();
    }
    static empty() {
        return Guid.emptyGuid;
    }
    static newGuid() {
        return new Guid(
            Guid.s4() + Guid.s4() + '-' + Guid.s4() + '-' + Guid.s4() + '-' +
            Guid.s4() + '-' + Guid.s4() + Guid.s4() + Guid.s4()
            );
    }
    static regex(format?: string) {
        switch (format) {
            case 'x':
            case 'X':
                return (/\{[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12}\}/i);

            default:
                return (/[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12}/i);
        }
    }
    private static s4() {
        return Math
            .floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    toString(format?: string) {
        switch (format) {
            case "x":
            case "X":
                return "{" + this.id + "}";

            default:
                return this.id;
        }
    }
    valueOf() {
        return this.id;
    }
}

export interface IHasId<T> {
    Id: T;
}

export interface IHasIntId extends IHasId<number> {
    Id: number;
}

export interface IHasGuidId extends IHasId<Guid> {
    Id: Guid;
}

export interface IHasStringId extends IHasId<string> {
    Id: string;
}

export module Queries {
    export interface BasicQuery {
    }

    export interface PagedQuery extends BasicQuery {
        Page: number;
        PageSize: number;
    }
}

export module Responses {
    export interface Envelope {
        Status: string;
        Message: string;
        Payload: any;
    }

    export interface IEvent {
        domain: string;
        eventName: string;

        urn: string;
        updated: string;

        Payload: any;
    }
}

import amplify = require('amplify');
export module Services {

    export interface Definitions {
        [index: string]: IDefinition;
    }

    export interface IDefinition {
        id: Guid;
        url: string;
        type: string;
        toString(format?: string);
        valueOf();
        request(model: any): JQueryPromise<any>;
    }

    export class Definition<T> implements IDefinition {
        constructor(url: string, type: string, decoder?: string) {
            this.id = Guid.newGuid();
            this.url = url;
            this.type = type;

            var ajax: amplifyAjaxSettings = {
                url: this.url,
                type: this.type,
                decoder: decoder,
                data: { format: 'json' }
            };

            amplify.request.define(this.id.toString(), 'ajax', ajax);
        }
        id: Guid;
        url: string;
        type: string;

        toString(format?: string) {
            return this.id.toString(format);
        }
        valueOf() {
            return this.id;
        }

        request(model: T): JQueryPromise<any> {
            return amplify.request(this.toString(), model);
        }
    }

    amplify.request.decoders['full'] = (data?: Responses.Envelope, status?: string, xhr?: JQueryXHR, success?: (...args: any[]) => void, error?: (...args: any[]) => void) => {
        if (data.Status === "success") {
            success(data.Payload);
        } else if (data.Status === "fail" || data.Status === "error") {
            error(data.Message, data.Status);
        } else {
            error(data.Message, "fatal");
        }
    }

    }

export module Events {
    export class Handler {
        private _domain: string;

        constructor(domain: string) {
            this._domain = domain;

            var m: string;
            for (m in this) {
                if (typeof this[m] !== 'function' || m.substr(0,2) !== 'on' ) continue;

                // register new event handler
                var event = this._domain + '.' + m.substr(2);
                amplify.subscribe(event, this.apply);
            }
        }

        apply(event: Responses.IEvent): void {
            var func = this['on' + event.eventName];
            if (func === undefined) return;
            func(event.Payload);
        }
    }

}