module nodak.components {
    export interface ITagInput extends ng.IScope{
        loadTags(query): void;
        tags: Array<any>;
        kmsTags: Array<nodak.common.models.TagDto>;
        count: number;

    }
    export class TagInputDirective implements ng.IDirective {
        public static http: ng.IHttpService;
        static $inject = ['$http'];
        constructor(public $http: ng.IHttpService) {
            TagInputDirective.http = $http;
        }
        //static $inject = ["$http"];
        //constructor(public http: ng.IHttpService) { }

        template = `
                 <tags-input  ng-model="tags" class="bootstrap" >
                         <auto-complete  source="loadTags($query)"></auto-complete>
                 </tags-input>
               
                   `
        restrict = 'E';
        scope = {
            
            kmsTags: '='
        };

        link = (scope: ITagInput) => {
            
            scope.tags = new Array<any>();
            scope.kmsTags = new Array<nodak.common.models.TagDto>();
           
            
            scope.loadTags = function (query) {
                
                //console.log("tagAddress", Base.Config.ApiRoot + '/api/tag/Gets');
                var tag = new nodak.common.models.TagDto();
                tag.Id = query;

                return TagInputDirective.http.post(Base.Config.ServiceRoot + '/api/tag/GetTags', tag).then(function (response) {
                    return response.data;
                });

            };

            scope.$watch('tags.length', function () {
                
               // console.log("scope tag", scope.tags);
                if (scope.tags.length != 0) {
                    scope.kmsTags = new Array<nodak.common.models.TagDto>();
                    for (var i = 0; i < scope.tags.length; i++) {
                        var tagTemp = new nodak.common.models.TagDto();
                        tagTemp.Id = scope.tags[i].Id;
                        scope.kmsTags.push(tagTemp);
                        //  console.log("count" + scope.tagDto);
                    }
                }
            });
            scope.$watch('kmsTags', function () {
                //   console.log("new",scope.tagDto);
                
                if (scope.kmsTags == null) {
                    scope.tags = [];
                    return;
                }
                if (scope.kmsTags != null) {
                    if (scope.kmsTags.length == 0)
                        scope.tags = [];

                    else {
                        if (scope.kmsTags.length == scope.tags.length)
                            return;
                        for (var i = 0; i < scope.kmsTags.length; i++) {
                            var x = {
                                Id: ""
                            }
                            x.Id = scope.kmsTags[i].Id;
                            scope.tags.push(x);

                        }
                    }
                }
            }, true);



        }
        
    }
    
}
appDirectivesModule.directive('labxandTag', ($http) => { return new nodak.components.TagInputDirective($http); });