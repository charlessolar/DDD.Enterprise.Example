

    import http = require('plugins/http');
    import app = require('durandal/app');
    import mapping = require('knockout.mapping');
    import Library = require('../lib/Demo/Library');
    import Items = require('../lib/Demo/Inventory/Items');

    

    
    export var displayName = 'Data';
    export var data = ko.observableArray<Items.Responses.Item>();
    export var detailedMapping = ko.observable<any>();
    export var ChangeDescriptionTo = ko.observable<string>();

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

