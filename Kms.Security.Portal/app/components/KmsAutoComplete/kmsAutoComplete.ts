module nodak.components {
    export interface IAutocompelete {
        id: string;
        placeholder: string;
        selectedObject;
        url: string;
        dataField: string;
        titleField: string;
        descriptionField: string;
        insideDes: string;
        imageField: string;
        imageUri: string;
        inputClass: string;
        userPause: string;
        localData: Array<any>;
        searchFields: string;
        minLengthUser: string;
        matchClass: string;
        //it can be any entity
        model: any;
        service: nodak.service.IServiceBase<any>;

        lastSearchTerm: string;
        currentIndex: number;
        justChanged: boolean;
        searchTimer;
        hideTimer;
        searching: boolean;
        pause: number;
        minLength: number;
        searchStr: string;
        search: Gridsearch<any, any>;

    }


    export class Gridsearch<MyModel extends nodak.models.ModelBase, SearchModel> extends labxand.components.core.GridBase<MyModel, SearchModel>
    {
        
        searchFields = [];
        model;
        localData;
        localDataIsRecieved: boolean;
        filters: Array<labxand.components.core.FilterSearchModel>;
        constructor(
            injector: ng.auto.IInjectorService,
            public service: nodak.service.IServiceBase<labxand.components.core.DataGrid<MyModel>>,
            searchField: string) {
            super(injector, service);
            this.searchFields = searchField.split(",");
            this.filters = new Array<labxand.components.core.FilterSearchModel>();
        }
      



    }


    export function kmsAutocompelete($parse, $http, $sce, $timeout) {
        return {
            restrict: 'EA',
            scope: {
                "id": "@id",
                "placeholder": "@placeholder",
                "selectedObject": "=selectedobject",
                "url": "@url",
                "dataField": "@datafield",
                "titleField": "@titlefield",
                "descriptionField": "@descriptionfield",
                "insideDes": "@insidedes",
                "imageField": "@imagefield",
                "imageUri": "@imageuri",
                "inputClass": "@inputclass",
                "userPause": "@pause",
                "localData": "=localdata",
                "searchFields": "@searchfields",
                "minLengthUser": "@minlength",
                "matchClass": "@matchclass",
                model: "=",
                service: "="

            },
            template: '<div class="angucomplete-holder"> \
        <md-input-container class="col-sm-12 col-lg-4 md-default-theme md-input-invalid" flex-gt-sm="">\
        <label>جستجو کاربر</label>\
        <input style="width:100%;" id="{{id}}_value" \
        ng-model="searchStr" type="text"  \
         onmouseup="this.select();" ng-focus="resetHideResults()"         ng-blur="hideResults()" />   </md-input-container> \
         <div id="{{id}}_dropdown" class="angucomplete-dropdown" ng-if="showDropdown">\
        <div class="angucomplete-searching" ng-show="searching">در حال جستجو...</div>\
        <div class="angucomplete-searching" ng-show="!searching && (!results || results.length == 0)">کاربری با این اسم یافت نشد.</div>\
        <div class="angucomplete-row" ng-repeat="result in results" ng-mousedown="selectResult(result)"\
        ng-mouseover="hoverRow()" ng-class="{\'angucomplete-selected-row\': $index == currentIndex}">\
        <div ng-if="imageField" class="angucomplete-image-holder"><img ng-if="result.image && result.image != \'\'"\
        ng-src="{{result.image}}" class="angucomplete-image"/>\
        <div ng-if="!result.image && result.image != \'\'" class="angucomplete-image-default"></div></div>\
        <div class="angucomplete-title" ng-if="matchClass" ng-bind-html="result.title"></div>\
        <div class="angucomplete-title" ng-if="!matchClass">{{ result.title }}</div>\
        <div ng-if="result.description && result.description != \'\'" class="angucomplete-description">{{result.description}}</div></div></div></div>',

            link: function ($scope, elem, attrs) {
                $scope.lastSearchTerm = null;
                $scope.currentIndex = null;
                $scope.justChanged = false;
                $scope.searchTimer = null;
                $scope.hideTimer = null;
                $scope.searching = false;
                $scope.pause = 500;
                $scope.minLength = 3;
                $scope.searchStr = null;
                $scope.search = Gridsearch;
                
                if ($scope.minLengthUser && $scope.minLengthUser != "") {
                    $scope.minLength = $scope.minLengthUser;
                }

                if ($scope.localData == null) {
                    $scope.search = new Gridsearch ($scope.injector, $scope.service, $scope.searchFields);
                }

                $scope.getLocalData = function (str) {
                    
                    $scope.localData = null;
                    $scope.Create(str, $scope.model);
                    $scope.localData = $scope.search.localData;

                }


                $scope.Create = function (str, searchModel) {
                    $scope.search.filters = new Array<labxand.components.core.FilterSearchModel>();

                    for (var i = 0; i < $scope.search.searchFields.length; i++) {
                        var x = $scope.search.searchFields[i];
                        searchModel[x] = str;
                        var filterSearchModel = new labxand.components.core.FilterSearchModel(x);
                        filterSearchModel.filters.push($scope.filter(x));
                        $scope.search.filters.push(filterSearchModel);

                    }
                    $scope.Search(searchModel);
                }

                $scope.filter = function (propertyName: string) {
                    var filterdef = new labxand.components.core.FilterDefinition2;
                    filterdef.FilterOperation = labxand.components.core.FilterOperations.Like;
                    filterdef.filterValue = propertyName;
                    return filterdef;
                }



                $scope.Search = function (model) {
                    $scope.search.searchCalled = true;
                    $scope.search.sortedField = null;
                    $scope.search.specificationOfDataList = new labxand.components.core.SpecificationOfDataList();
                    $scope.search.specificationOfDataList.ascendingSortDirection = false;
                    $scope.search.currentFilter = $scope.createFilter(model);

                    $scope.search.specificationOfDataList.filterSpecifications = $scope.search.currentFilter;
                    $scope.search.specificationOfDataList.pageIndex = 0;
                    $scope.search.currentPage = 0;
                    $scope.search.specificationOfDataList.pageSize = 10;
                    $scope.search.specificationOfDataList.sortSpecification = null;
                    $scope.searchMethod();
                    //alert('search');
                    this.page = 1;
                }

                $scope.searchMethod = function () {
                    $scope.search.service.Post($scope.search.specificationOfDataList, nodak.enums.ServiceTypeEnum.GetForGrid)
                        .then(
                        (response) => {
                            
                            $scope.localData = response.Results;
                            $scope.searching = false;
                            $scope.processResults($scope.localData, $scope.searchStr);
                            //$scope.searchTimerComplete($scope.searchStr);



                        });
                }

                $scope.createFilter = function (model) {
                    var filterArray: Array<labxand.components.core.FilterSpecification> = [];
                    $scope.search.filters.forEach((item) => {
                        item.filters.forEach((q) => {
                            var filter = new labxand.components.core.FilterSpecification();
                            //console.log(item.propertyName);
                            filter.propertyName = item.propertyName;

                            if (model[q.filterValue]) {
                                filter.filterValue = model[q.filterValue];
                                filter.filterOperation = q.FilterOperation;
                                filterArray.push(filter);
                            }

                        })

                    })

                    return filterArray;
                }





                if ($scope.userPause) {
                    $scope.pause = $scope.userPause;
                }

                function isNewSearchNeeded(newTerm, oldTerm) {
                    return newTerm.length >= $scope.minLength && newTerm != oldTerm
                }

                $scope.processResults = function (responseData, str) {
                    
                    if (responseData && responseData.length > 0) {
                        $scope.results = [];

                        var titleFields = [];
                        if ($scope.titleField && $scope.titleField != "") {
                            titleFields = $scope.titleField.split(",");
                        }

                        for (var i = 0; i < responseData.length; i++) {
                            // Get title variables
                            var titleCode = [];

                            for (var t = 0; t < titleFields.length; t++) {
                                titleCode.push(responseData[i][titleFields[t]]);
                            }

                            var description = "";
                            if ($scope.descriptionField) {
                                description = responseData[i][$scope.descriptionField][$scope.insideDes];
                            }

                            var imageUri = "";
                            if ($scope.imageUri) {
                                imageUri = $scope.imageUri;
                            }

                            var image = "";
                            if ($scope.imageField) {
                                image = imageUri + responseData[i][$scope.imageField];
                            }

                            var text = titleCode.join(' ');
                            if ($scope.matchClass) {
                                var re = new RegExp(str, 'i');
                                var strPart = text.match(re)[0];
                                //     text = $sce.trustAsHtml(text.replace(re, '<span class="'+ $scope.matchClass +'">'+ strPart +'</span>'));
                            }

                            var resultRow = {
                                title: text,
                                description: description,
                                image: image,
                                originalObject: responseData[i]
                            }

                            $scope.results[$scope.results.length] = resultRow;
                        }


                    } else {
                        $scope.results = [];
                    }
                }

                $scope.searchTimerComplete = function (str) {
                    // Begin the search

                    if (str.length >= $scope.minLength) {
                        if ($scope.localData) {
                            var searchFields = $scope.searchFields.split(",");

                            var matches = [];



                            $scope.searching = false;
                            $scope.processResults($scope.localData, str);

                        } else {
                            $http.get($scope.url + str, {}).
                                success(function (responseData, status, headers, config) {
                                    $scope.searching = false;
                                    $scope.processResults((($scope.dataField) ? responseData[$scope.dataField] : responseData), str);
                                }).
                                error(function (data, status, headers, config) {
                                    console.log("error");
                                });
                        }
                    }
                }

                $scope.hideResults = function () {
                    $scope.hideTimer = $timeout(function () {
                        $scope.showDropdown = false;
                    }, $scope.pause);
                };

                $scope.resetHideResults = function () {
                    if ($scope.hideTimer) {
                        $timeout.cancel($scope.hideTimer);
                    };
                };

                $scope.hoverRow = function (index) {

                    $scope.currentIndex = index;
                }

                $scope.keyPressed = function (event) {

                    if (!(event.which == 38 || event.which == 40 || event.which == 13)) {
                        if (!$scope.searchStr || $scope.searchStr == "") {
                            $scope.showDropdown = false;
                            $scope.lastSearchTerm = null
                        } else if (isNewSearchNeeded($scope.searchStr, $scope.lastSearchTerm)) {
                            $scope.lastSearchTerm = $scope.searchStr
                            $scope.showDropdown = true;
                            $scope.currentIndex = -1;
                            $scope.results = [];

                            if ($scope.searchTimer) {
                                $timeout.cancel($scope.searchTimer);
                            }

                            $scope.searching = true;

                            $scope.searchTimer = $timeout(function () {
                                
                                $scope.getLocalData($scope.searchStr);


                                //$scope.searchTimerComplete($scope.searchStr);



                            }, $scope.pause);

                        }
                    } else {
                        event.preventDefault();
                    }
                }

                $scope.selectResult = function (result) {

                    
                    if ($scope.matchClass) {
                        result.title = result.title.toString().replace(/(<([^>]+)>)/ig, '');
                    }
                    $scope.searchStr = $scope.lastSearchTerm = result.title;
                    $scope.selectedObject = result.originalObject;
                    $scope.showDropdown = false;
                    $scope.results = [];
                    $scope.$apply();
                }

                var inputField = elem.find('input');

                inputField.on('keyup', $scope.keyPressed);

                elem.on("keyup", function (event) {

                    if (event.which === 40) {
                        if ($scope.results && ($scope.currentIndex + 1) < $scope.results.length) {
                            $scope.currentIndex++;
                            $scope.$apply();
                            event.preventDefault;
                            event.stopPropagation();
                        }

                        $scope.$apply();
                    } else if (event.which == 38) {
                        if ($scope.currentIndex >= 1) {
                            $scope.currentIndex--;
                            $scope.$apply();
                            event.preventDefault;
                            event.stopPropagation();
                        }

                    } else if (event.which == 13) {
                        if ($scope.results && $scope.currentIndex >= 0 && $scope.currentIndex < $scope.results.length) {
                            $scope.selectResult($scope.results[$scope.currentIndex]);
                            $scope.$apply();
                            event.preventDefault;
                            event.stopPropagation();
                        } else {
                            $scope.results = [];
                            $scope.$apply();
                            event.preventDefault;
                            event.stopPropagation();
                        }

                    } else if (event.which == 27) {
                        $scope.results = [];
                        $scope.showDropdown = false;
                        $scope.$apply();
                    } else if (event.which == 8) {
                        $scope.selectedObject = null;
                        $scope.$apply();
                    }
                });

            }
        };
    }
}
appDirectivesModule.directive('angucomplete', ($parse, $http, $sce, $timeout) => {
    return nodak.components.kmsAutocompelete($parse, $http, $sce, $timeout);
});


