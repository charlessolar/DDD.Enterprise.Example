module Demo.Inventory.Models.Items.Responses {
    import Guid = Demo.Library.Guid;

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