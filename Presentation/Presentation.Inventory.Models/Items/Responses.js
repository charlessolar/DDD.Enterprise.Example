var Demo;
(function (Demo) {
    (function (Inventory) {
        (function (Models) {
            (function (Items) {
                (function (Responses) {
                    var Find = (function () {
                        function Find() {
                        }
                        return Find;
                    })();
                    Responses.Find = Find;

                    var Item = (function () {
                        function Item() {
                        }
                        return Item;
                    })();
                    Responses.Item = Item;
                })(Items.Responses || (Items.Responses = {}));
                var Responses = Items.Responses;
            })(Models.Items || (Models.Items = {}));
            var Items = Models.Items;
        })(Inventory.Models || (Inventory.Models = {}));
        var Models = Inventory.Models;
    })(Demo.Inventory || (Demo.Inventory = {}));
    var Inventory = Demo.Inventory;
})(Demo || (Demo = {}));
//# sourceMappingURL=Responses.js.map
