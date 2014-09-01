
import Guid = Demo.Library.Guid;

interface Repository<T extends Demo.Library.IHasGuidId> {
    get(id: Guid): JQueryPromise<Demo.Library.Responses.Base<T>>;
    update(model: T): JQueryPromise<boolean>;
    add(model: T): JQueryPromise<boolean>;
    remove(model: T): JQueryPromise<boolean>;
    removeById(id: Guid): JQueryPromise<boolean>;
}