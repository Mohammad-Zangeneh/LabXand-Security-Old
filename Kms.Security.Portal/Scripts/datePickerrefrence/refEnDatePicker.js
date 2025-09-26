//http://jsfiddle.net/TAeNF/1092/
var datePicker = angular.module('DateApp', []);

datePicker.directive("datepicker1", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, ngModelCtrl) {
            var updateModel = function (dateText) {
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(dateText);
                });
            };
            var options = {
                dateFormat: "yy/mm/dd",
                onSelect: function (dateText) {
                    updateModel(dateText);
                }
            };

            console.log('elem', elem);
            if (elem.datepicker)
                elem.datepicker(options);


        }
    }
});