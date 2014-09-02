module Demo.Library.Responses {
    export class Basic {
        Status: string;
        Message: string;
    }

    export class Diff<T extends IHasGuidId> extends Basic {
        Urn: string;
        Version: number;
        Updated: string;

        Payload: any;
    }

    export class Full<T extends IHasGuidId> extends Basic {
        Urn: string;
        Version: number;
        Sessions: string[];
        Created: string;
        Updated: string;
        Payload: T;
    }
} 