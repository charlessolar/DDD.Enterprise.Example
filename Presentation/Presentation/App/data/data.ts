

    import http = require('plugins/http');
    import app = require('durandal/app');
    import ko = require('knockout');
    import mapping = require('knockout.mapping');
    import Library = require('lib/Demo/Library');
    import SSE = require('lib/Demo/SSE');
    import Items = require('lib/Demo/Inventory/Items');
    import Guid = Library.Guid;

    export interface Model extends Library.IHasGuidId {
        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;
        CatalogPrice: number;
        CostPrice: number;
    }

    class Handler extends Library.Events.Handler {
        constructor() {
            super("Inventory.Item");
        }

        onCreated(event: Items.Events.Created): void {
            data.push({
                Id: event.ItemId,
                Number: event.Number,
                Description: event.Description,
                UnitOfMeasure: event.UnitOfMeasure,
                CatalogPrice: event.CatalogPrice,
                CostPrice: event.CostPrice
            });
        }
        onDescriptionChanged(event: Items.Events.DescriptionChanged): void {
            console.log("here!");
            var item = ko.utils.arrayFirst(data(), (i) => {
                return i.Id === event.ItemId;
            });

            if (item === null) return;

            item.Description = event.Description;
        }
    }

    
    export var displayName = 'Data';
    export var data = ko.observableArray<Model>();
    export var detailedMapping = ko.observable<any>();
    export var ChangeDescriptionTo = ko.observable<string>();

    export var hasDetail = ko.computed(function () {
        return detailedMapping() != null;
    });

    export function activate() {
        //the router's activator calls this function and waits for it to complete before proceeding
        if (data().length > 0) {
            return;
        }

        SSE.Service.Subscribe({ domain: 'Inventory.Item' });
        var handler = new Handler();


        var that = this;

        Items.Service.Find({ Page: 1, PageSize: 10 }).then((r) => {
            that.data(r.Results);
        });
    }
    export function deactivate() {

        SSE.Service.Unsubscribe({ domain: 'Inventory.Item' });
    }

    export function getdetail(item: Items.Responses.Item) {
        Items.Service.Get({ Id: item.Id }).then((r) => {
            //amplify.subscribe(r.Urn, (d) => {
            //    mapping.fromJS(d, detailedMapping());
            //});

            detailedMapping(mapping.fromJS(r));
            //detailed(r.Payload)
        });
    }

    export function clicky() {

        Items.Service.ChangeDescription({ Id: detailedMapping().Id(), Description: ChangeDescriptionTo() });
    }

