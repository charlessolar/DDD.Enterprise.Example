module Demo.Inventory.Items {
    import Guid = Demo.Library.Guid;
    import Services = Demo.Library.Services;
    import s = Demo.Inventory.Models.Items.Services;
    import r = Demo.Inventory.Models.Items.Responses;

    export class Service {
        static resources: Services.Definitions = {
            'create': new Services.Definition<Guid>('/items', 'POST'),
            'find': new Services.Definition<r.Find>('/items', 'GET'),
            'read': new Services.Definition<r.Item>('/items/{id}', 'GET', 'full'),
            'update': new Services.Definition<Guid>('/items/{id}', 'PUT'),
            'delete': new Services.Definition<Guid>('/items/{id}', 'DELETE')
        };


        Get(model: s.Get): JQueryPromise<r.Item> {
            return Service.resources['read'].request(model);
        }
        Find(model: s.Find): JQueryPromise<r.Find> {
            return Service.resources['find'].request(model);
        }
        Create(model: s.Create): JQueryPromise<Guid> {
            return Service.resources['create'].request(model);
        }
        ChangeDescription(model: s.ChangeDescription): JQueryPromise<Demo.Library.Guid> {
            return Service.resources['update'].request(model);
        }

    }
}