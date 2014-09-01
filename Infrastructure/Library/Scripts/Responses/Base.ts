module Demo.Library.Responses {
    export interface Base<T extends IHasGuidId> {
        Urn: string;
        Version: number;
        Payload: T;
    }
} 