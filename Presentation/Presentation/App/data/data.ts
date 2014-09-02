/// <reference path="../../Scripts/typings/durandal/durandal.d.ts" />
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../Scripts/typings/knockout.mapping/knockout.mapping.d.ts" />
/// <reference path="../../Scripts/typings/amplifyjs/amplifyjs.d.ts" />
/// <reference path="../../Scripts/typings/moment/moment.d.ts" />
/// <reference path="../lib/data/Repository.ts" />

    import http = require('plugins/http');
    import app = require('durandal/app');
    import mapping = require('knockout.mapping');
    import Guid = Demo.Library.Guid;

    export class Model implements Demo.Library.IHasGuidId {

        Id: Guid;
        Number: string;
        Description: string;
        UnitOfMeasure: string;
        CatalogPrice: number;
        CostPrice: number;


        toString(): string {
            return this.Number + ' - ' + this.Description;
        }
    }

    class SearchDTO {
        Results: Model[];
    }

    class ItemRepository implements Repository<Model>{

        get(id: Guid): JQueryPromise<Demo.Library.Responses.Full<Model>> {
            return http.get('/items/' + id.toString(), { format: 'json' });
        }
        update(model: Model): JQueryPromise<boolean> {
            return $.Deferred().resolve(false);
        }
        add(model: Model): JQueryPromise<boolean> {
            return http.post('/items', { request: Model, format: 'json' });
        }
        remove(model: Model): JQueryPromise<boolean> {
            return $.Deferred().resolve(false);
        }
        removeById(id: Guid): JQueryPromise<boolean> {
            return $.Deferred().resolve(false);
        }

        search(page: number, pageSize: number, number: string = '', description: string = ''): JQueryPromise<SearchDTO> {
            return http.get('/items', { number: number, description: description, page: page, pageSize: pageSize, format: 'json' });
        }

        changeDescription(id: Guid, description: string) {
            return http.post('/items/' + id.toString(), { Description: description });
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
        if (this.data().length > 0) {
            return;
        }
        

        var that = this;

        var repo = new ItemRepository();
        repo.search(1, 10).then(function (r: SearchDTO) {
            that.data(r.Results);
        });
    }

    export function getdetail(item: Model) {
        var repo = new ItemRepository();
        repo.get(item.Id).then((r) => {
            amplify.subscribe(r.Urn, (d) => {
                mapping.fromJS(d, detailedMapping());
            });

            detailedMapping(mapping.fromJS(r.Payload));
            //detailed(r.Payload)
        });
    }

    export function clicky() {

        var repo = new ItemRepository();
        repo.changeDescription(detailedMapping().Id(), ChangeDescriptionTo());
    }

