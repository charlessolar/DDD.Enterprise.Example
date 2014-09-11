
import Library = require("lib/Demo/Library");
import Guid = Library.Guid;

export module Responses {
    export interface Find {
        Results: Item[];
    }

    export interface Item extends Library.IHasGuidId {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;
        CatalogPrice: number;
        CostPrice: number;
    }
}

export module Services {
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

    export interface Find extends Library.Queries.PagedQuery {
        Number?: string;
        Description?: string;

        Page: number;
        PageSize: number;
    }

    export interface Get extends Library.Queries.BasicQuery {
        Id: Guid;
    }
}

import s = Library.Services;
export class Service {
    static resources: s.Definitions = {
        'create': new s.Definition<Guid>('/items', 'POST'),
        'find': new s.Definition<Responses.Find>('/items', 'GET'),
        'read': new s.Definition<Responses.Item>('/items/{id}', 'GET', 'full'),
        'update': new s.Definition<Guid>('/items/{id}', 'PUT'),
        'delete': new s.Definition<Guid>('/items/{id}', 'DELETE')
    };


    static Get(model: Services.Get): JQueryPromise<Responses.Item> {
        return Service.resources['read'].request(model);
    }
    static Find(model: Services.Find): JQueryPromise<Responses.Find> {
        return Service.resources['find'].request(model);
    }
    static Create(model: Services.Create): JQueryPromise<Guid> {
        return Service.resources['create'].request(model);
    }
    static ChangeDescription(model: Services.ChangeDescription): JQueryPromise<Guid> {
        return Service.resources['update'].request(model);
    }
}

export module Events {
    export interface Created {
        ItemId: Guid;
        Number: string;
        Description: string;

        UnitOfMeasure: string;
        CatalogPrice?: number;
        CostPrice?: number;
    }
    export interface DescriptionChanged {
        ItemId: Guid;

        Description: string;
    }
}
