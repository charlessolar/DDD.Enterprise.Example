

    import http = require('plugins/http');
    import app = require('durandal/app');
    import mapping = require('knockout.mapping');
    import Library = require('../lib/Demo/Library');
    import Items = require('../lib/Demo/Inventory/Items');
    import Guid = Library.Guid;

    interface Model extends Library.IHasGuidId {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;
        CatalogPrice: number;
        CostPrice: number;
    }

    class Handler implements Items.IHandler {
        apply(name: string, event: any): void {
            this[name](event);
        }
        Created(event: Items.Events.Created): void {
            data.push({
                Id: event.ItemId,
                Number: event.Number,
                Description: event.Description,
                UnitOfMeasure: event.UnitOfMeasure,
                CatalogPrice: event.CatalogPrice,
                CostPrice: event.CostPrice
            });
        }
        DescriptionChanged(event: Items.Events.DescriptionChanged): void {
            var item = ko.utils.arrayFirst(data(), (i) => {
                return i.Id === event.ItemId;
            });

            if (item === null) return;

            item.Description = event.Description;
        }
    }

    
    var displayName = 'Data';
    var data = ko.observableArray<Model>();
    var detailedMapping = ko.observable<any>();
    var ChangeDescriptionTo = ko.observable<string>();

    export var hasDetail = ko.computed(function () {
        return detailedMapping() != null;
    });

    export function activate() {
        //the router's activator calls this function and waits for it to complete before proceeding
        if (this.data().length > 0) {
            return;
        }
        

        var that = this;

        var s = new Items.Service();
        s.Find({ Page: 1, PageSize: 10 }).then((r) => {
            that.data(r.Results);
        });
    }

    export function getdetail(item: Items.Responses.Item) {
        var s = new Items.Service();
        s.Get({ Id: item.Id }).then((r) => {
            //amplify.subscribe(r.Urn, (d) => {
            //    mapping.fromJS(d, detailedMapping());
            //});

            detailedMapping(mapping.fromJS(r));
            //detailed(r.Payload)
        });
    }

    export function clicky() {

        var s = new Items.Service();
        s.ChangeDescription({ Id: detailedMapping().Id(), Description: ChangeDescriptionTo() });
    }

