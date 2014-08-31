/// <reference path="../../../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Entity.ts"/>

interface Repository<T extends Entity> {
    get(id: Guid): JQueryPromise<T>;
    update(model: T): JQueryPromise<boolean>;
    add(model: T): JQueryPromise<boolean>;
    remove(model: T): JQueryPromise<boolean>;
    removeById(id: Guid): JQueryPromise<boolean>;
}