module Demo.Items {
    import Guid = Demo.Library.Guid;

    export class CreateItem {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;

        CatalogPrice: number;
        CostPrice: number;
    }

    export class ChangeDescription {
        ItemId: Guid;
        Description: string;
    }

    export class DeleteItem {
        Id: Guid;
    }

    export class FindItems implements Demo.Library.Queries.PagedQuery {
        Number: string;
        Description: string;

        Page: number;
        PageSize: number;
    }

    export class GetItem implements Demo.Library.Queries.BasicQuery {
        Id: Guid;
    }

    export class Find {
        Results: Item[];
    }

    export class Item implements Demo.Library.IHasGuidId {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;
        CatalogPrice: number;
        CostPrice: number;
    }
}