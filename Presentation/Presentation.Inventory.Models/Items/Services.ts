module Demo.Inventory.Models.Items.Services {
    import Guid = Demo.Library.Guid;

    export interface Create {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;

        CatalogPrice: number;
        CostPrice: number;
    }

    export interface ChangeDescription {
        Id: Guid;
        Description: string;
    }

    export interface Delete {
        Id: Guid;
    }

    export interface Find extends Demo.Library.Queries.PagedQuery {
        Number?: string;
        Description?: string;

        Page: number;
        PageSize: number;
    }

    export interface Get extends Demo.Library.Queries.BasicQuery {
        Id: Guid;
    }

} 