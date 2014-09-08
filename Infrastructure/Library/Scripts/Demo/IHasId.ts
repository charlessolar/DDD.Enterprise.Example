module Demo.Library {
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
}