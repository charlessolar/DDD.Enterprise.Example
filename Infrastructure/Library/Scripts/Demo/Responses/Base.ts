module Demo.Library.Responses {
    export interface Basic {
        Status: string;
        Message: string;
    }

    export interface Diff<T extends IHasGuidId> extends Basic {
        Urn: string;
        Version: number;
        Updated: string;

        Payload: any;
    }

    export interface Full<T extends IHasGuidId> extends Basic {
        Urn: string;
        Version: number;
        Sessions: string[];
        Created: string;
        Updated: string;
        Payload: T;
    }
} 