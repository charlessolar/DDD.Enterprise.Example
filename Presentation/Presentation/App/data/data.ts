/// <reference path="../../Scripts/typings/durandal/durandal.d.ts" />
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../lib/guid.ts" />


import http = require('plugins/http');
import app = require('durandal/app');
import ko = require('knockout');

export class Model {
    Id: Guid;
    Number: string;
    Description: string;
    UnitOfMeasure: string;
    CatalogPrice: number;
    CostPrice: number;
     

    toString() : string {
        return this.Number + ' - ' + this.Description;
    }
}

export var displayName = 'Data';
export var data = ko.observableArray<Model>();


export function activate() {
    //the router's activator calls this function and waits for it to complete before proceeding
    if (this.data().length > 0) {
        return;
    }


    var that = this;
    return http.get('/items', { page: 1, pagesize: 5, format: 'json' }).then(function (response: Model[]) {
        that.data(response);
    });
}

  