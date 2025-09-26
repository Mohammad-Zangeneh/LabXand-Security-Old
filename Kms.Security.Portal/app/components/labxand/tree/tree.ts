interface JSTree {
    settings: any;
    plugins: any;
}
interface JQuery {
    bind(a: any, b: any);
}

module labxand.components.core {
    export interface ITreeBaseScope<T extends nodak.models.ModelBase> extends ng.IScope {
        Base: ITreeBase<T>;
        Id: string;
    }
    export class LabxandTree implements ng.IDirective {
        private treeOldSelection;//for change selection
        private treeModelDto: any;
        private mapTo(scope: ITreeBaseScope<any>) {
            if (scope.Base.Data == undefined) {
                this.treeModelDto = [];
                return;
            }
            this.treeModelDto = [];
            scope.Base.Data.forEach((item) => {
                let temp = { parent: '#', text: item[scope.Base.ItemSelectedText], id: item[scope.Base.ItemSelectedId] };
                if (item[scope.Base.ItemSelectedParentId] != null)
                    temp.parent = item[scope.Base.ItemSelectedParentId];
                this.treeModelDto.push(temp);
            });
        }
        constructor() {
        }
        template = `
<div class="panel panel-default ">
    <div class="panel-heading text-left" style="height:45px;">
        <div>
          <div class='middle-div'>
                 <a href="#" ng-click="Base.ClearSelected()"><span class="fa fa-square-o fa-lg mt-4"></span></a>
                <a href="#" ng-click="Base.DataBind()"><span class="fa fa-refresh fa-lg mt-4"></span></a>
          </div>
        </div>
       
    </div>
    <div class="panel-body treePanel">
        <div id="{{Id}}Tree">Tree</div>
    </div>
</div>
`;

        restrict = 'E';
        scope: any = {
            Base: "=base"
        };
        private init(scope: ITreeBaseScope<any>) {
            this.mapTo(scope);
            let plugins = ["changed", "types", "search"/*,"sort"*/]
            if (scope.Base.IsSortable)
                plugins.push("sort");
            //if (scope.operation.dnd == true)
            //    plugins.push("dnd");
            if (scope.Base.MultiSelect == true)
                plugins.push("checkbox");
            $(`#${scope.Id}Tree`).jstree({
                'plugins': plugins,
                "checkbox": {
                    "keep_selected_style": false
                },
                'core': {
                    "multiple": scope.Base.MultiSelect,
                    "check_callback": true,
                    "themes": {
                        "responsive": false
                    }
                }
                ,
                'search': {
                    'case_insensitive': true,
                    'show_only_matches': true
                },
                'sort': function (a, b) {
                    let a1 = scope.Base.FindModelById(a); 
                    let b1 = scope.Base.FindModelById(b);
                    if (a1[scope.Base.ItemSort] == b1[scope.Base.ItemSort]) {
                        return (a1.text > b1.text) ? 1 : -1;
                    } else {
                        return (a1[scope.Base.ItemSort] > b1[scope.Base.ItemSort]) ? 1 : -1;
                    }
                }
                //,
                //"types": {
                //    "default": {
                //        "icon": "fa fa-folder icon-state-warning icon-lg"
                //    },
                //    "file": {
                //        "icon": "fa fa-file icon-state-warning icon-lg"
                //    }
                //}
            });
            $(`#${scope.Id}Tree`).jstree(true).settings.core.data = this.treeModelDto;

            $(`#${scope.Id}Tree`).jstree('refresh');
            $(`#${scope.Id}Tree`).on("changed.jstree", (e, data) => {
                if (!(data.changed.selected.length === 0 && data.changed.deselected.length === 0))
                    if (this.treeOldSelection != data) {
                        this.treeOldSelection = data;
                        this.selectedChange(data, scope);
                    }
            });
        }
        private selecteItem(item, scope: ITreeBaseScope<any>) {
            if (scope.Base.MultiSelect == true)
                return;
            scope.Base.ClearSelected();
            if (item != undefined && item != null) {
                scope.Base.SelectedRows = [];
                scope.Base.SelectedRows.push(item);
                $(`#${scope.Id}Tree`).jstree('select_node', item.Id);
            }
        }
        private selecteItems(items, scope: ITreeBaseScope<any>) {
            if (items == undefined || items == null)
                return;
            if (scope.Base.MultiSelect != true)
                return;
            scope.Base.ClearSelected();
            let ids = [];
            for (let i = 0; i < items.length; i++) {
                if (items[i] != undefined && items[i] != null) {
                    ids.push(items[i].Id);
                }
            }
            $(`#${scope.Id}Tree`).jstree('select_node', ids);
            scope.Base.SelectedRow = scope.Base.SelectedRows.length != 0 ? scope.Base.SelectedRows[0] : null;
        }
        private findModelById(Id: string, scope: ITreeBaseScope<any>) {
            let result = null;
            scope.Base.Data.forEach((item) => {
                if (item[scope.Base.ItemSelectedId] == Id) {
                    result = item;
                }
            });
            return result;
        }
        private selectedChange(data, scope: ITreeBaseScope<any>) {
            if (data.action == "deselect_all") {
               
                scope.Base.SelectedRow = null;
                scope.Base.SelectedRows = [];
                 scope.Base.AfterSelect(scope.Base.SelectedRow);
                return;
            }
            if (data == undefined)
                return;
            if (data.action == "deselect_node" && data.selected.length == 0) {
                scope.Base.SelectedRow = null;
                scope.Base.SelectedRows = [];
                scope.Base.AfterSelect(scope.Base.SelectedRow);
                return;
            } let selectedRowsMapping = [];
            if (data.action != "deselect_node") {
                scope.Base.SelectedRow = this.findModelById(data.node.id, scope);
            }
            else
                scope.Base.SelectedRow = this.findModelById(data.selected[0], scope);


            for (let i = 0; i < data.selected.length; i++) {
                let temp = this.findModelById(data.selected[i], scope);
                if (temp != null)
                    selectedRowsMapping.push(temp);
            }
            scope.Base.SelectedRows = selectedRowsMapping;
            scope.Base.AfterSelect(scope.Base.SelectedRow);
        }
        link = (scope: ITreeBaseScope<any>, el, attrs) => {
            scope.Id = new Date().getMilliseconds().toLocaleString();
            scope.Id += Math.floor( Math.random() *10000);
            scope.Base.DataBind();
            scope.Base.build = () => {
                this.init(scope);
            };
            scope.Base.SetSelectedItem = (item) => {
                this.selecteItem(item, scope);
            };
            scope.Base.Refresh = () => { this.init(scope); }
            scope.Base.ClearSelected = () => { $(`#${scope.Id}Tree`).jstree("deselect_all"); }
            scope.Base.Search = (str) => {
                $(`#${scope.Id}Tree`).jstree(true).search(str);
            };
            scope.$watch(() => { return scope.Base.Data }, () => {
                scope.Base.build();
            });
            scope.$watch(() => { return scope.Base.SelectedRow }, (n, o) => {
                this.selecteItem(scope.Base.SelectedRow, scope);
            }, true);

            scope.$watch(() => { return scope.Base.SelectedRows }, (n, o) => {
                if (scope.Base.MultiSelect != true )
                    return;
                if (n == o)
                    return;
                this.selecteItems(scope.Base.SelectedRows, scope);
            }, true);
        };//end link
    }
}
angular.module('nodak.components').directive('labxandTree', () => { return new labxand.components.core.LabxandTree(); });
