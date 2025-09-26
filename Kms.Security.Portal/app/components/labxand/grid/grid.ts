module labxand.components.core {
    export interface IGridBaseScope<T extends nodak.models.ModelBase, U> extends ng.IScope {
        Base: IGridBase<T, U>;
        Id: string;
        Id1: string;
        dataList: Array<T>;
    }

    export interface IBindColumnsGrid extends ng.IScope {
        func: Function;
    }

    export interface IBindColumnsAttribute extends ng.IAttributes {
        bind: any;
    }

    export class BindColumnsGrid implements ng.IDirective {
        restrict = 'A';
        scope = {
            func: '&'
        };
        link = (scope: IBindColumnsGrid, element: ng.IAugmentedJQuery, attrs: IBindColumnsAttribute) => {
            element.append('<div>' + attrs.bind + '</div>');
        };
    }

    export class GridReadOnly implements ng.IDirective {
        static $inject = ['$timeout'];
        isCrudGrid: boolean;
        constructor(private $timeout: ng.ITimeoutService, isCrudGrid: boolean) {
            this.isCrudGrid = isCrudGrid;
        };

        templateUrl = () => {
            if (!this.isCrudGrid)
                return Base.Config.AppRoot + '/app/components/labxand/grid/grid.html';
            else
                return Base.Config.AppRoot + '/app/components/labxand/grid/grid.crud.html';
        };// 

        restrict = 'E';
        scope = {
            Base: "=base",
            dataList: "=list"
        };

        link = (scope: IGridBaseScope<any, any>, el, attrs) => {
            if (this.isCrudGrid)
                scope.Base.Data.Results = scope.dataList;
            scope.Id = new Date().getMilliseconds().toLocaleString();
            scope.Id1 = new Date().getMilliseconds().toLocaleString() + "1";//a.getMilliseconds().toLocaleString();

            scope.$watch(() => { return scope.Base.Data }, (o,n) => {
                scope.Base.build();
            });

            this.$timeout(() => {
                $("#" + scope.Id + " .MyTable th").each(function (key, data) {
                    $("#" + scope.Id1 + " .NewHeader tr").append(this);
                });
            });

        };
    }
}

angular.module('nodak.components').directive('grid', ($timeout) => { return new labxand.components.core.GridReadOnly($timeout, false); });
angular.module('nodak.components').directive('gridCrud', ($timeout) => { return new labxand.components.core.GridReadOnly($timeout, true); });
angular.module('nodak.components').directive('bind', () => { return new labxand.components.core.BindColumnsGrid(); });