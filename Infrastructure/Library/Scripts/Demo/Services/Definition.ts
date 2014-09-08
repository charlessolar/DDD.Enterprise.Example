module Demo.Library.Services {
    
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
                decoder: decoder
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

        request(model: T): JQueryPromise<T> {
            return amplify.request(this.toString(), model);
        }
    }
}