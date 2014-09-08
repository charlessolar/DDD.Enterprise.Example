module Demo.Inventory.Items {
    import Services = Demo.Library.Services;
    import s = Demo.Inventory.Models.Items.Services;
    import r = Demo.Inventory.Models.Items.Responses;

    export class Service {
        static resources: Services.Definitions = {
            'create': new Services.Definition('/items', 'POST'),
            'find': new Services.Definition('/items', 'GET'),
            'read': new Services.Definition('/items/{id}', 'GET', 'full'),
            'update': new Services.Definition('/items/{id}', 'PUT'),
            'delete': new Services.Definition('/items/{id}', 'DELETE')
        };


        Get(model: s.Get): JQueryPromise<r.Item> {
            return Service.resources['read'].request(model);
        }
        Find(model: s.Find): JQueryPromise<r.Find> {
            return Service.resources['find'].request(model);
        }
        Create(model: s.Create): JQueryPromise<Demo.Library.Guid> {
            return Service.resources['create'].request(model);
        }
        ChangeDescription(model: s.ChangeDescription): JQueryPromise<Demo.Library.Guid> {
            return Service.resources['update'].request(model);
        }

    }
}