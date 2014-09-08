module Demo.Library.Queries {
    export interface PagedQuery extends BasicQuery {
        Page: number;
        PageSize: number;
    }
}  