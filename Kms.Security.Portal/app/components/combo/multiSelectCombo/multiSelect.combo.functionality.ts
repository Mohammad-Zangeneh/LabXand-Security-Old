module nodak.components {

    export class DropdownMultiSelectFunctions<T> {
        textSelected: string;
        selectedText: Array<string>;
        contentId: string;
        textId: string;
        SelectedRow_Index: number = -1;
        DownArrow_Counter: number = 7;
        UpArrow_Counter: number = 0;
        textboxId: string;
        valueMember: string;
        displayMember: string;
        model: any;
        headers: any;
        defaultSelectId: string;
        withoutHeader: boolean;

        constructor(public element: ng.IAugmentedJQuery, public scope: nodak.directives.IMultiSelectCombo) {
            this.scope.selectedModels = new Array<Object>();
            this.textSelected = "";
            this.textboxId = scope.id;
            this.textId = scope.id + 'Text';
            this.contentId = scope.id + 'Content';
            this.valueMember = scope.selectList.valueMember;
            this.displayMember = scope.selectList.displayMember;
            this.model = scope.model;
            this.headers = scope.selectList.headers;
            this.withoutHeader = scope.withoutHeader;
            if (scope.defaultId)
                this.defaultSelectId = scope.defaultId;

            scope.$watch(() => { return this.scope.selectedIds; }, (newValue, oldValue) => {
                this.textSelected = "";
                this.scope.selectedModels = new Array();
                if (scope.selectedIds && scope.model) {
                    for (var j = 0; j < scope.selectedIds.length; j++) {
                        var _model = scope.model.filter((data) => { return data[this.valueMember] == scope.selectedIds[j] });
                        if (_model.length != 0) {
                            this.textSelected += _model[0][this.displayMember] + '، ';
                            this.scope.selectedModels.push(_model[0]);
                        }
                    }
                }
            });

            element.parent().find('input').eq(1).on('blur', () => {
                this.SetBlur();
            });

            var width = this.element.find('input').eq(0).parent().width();

            this.Initiate();

            scope.$watch('IsDisabled', (newValue, oldValue) => { if (scope.isDisabled == true) this.element.find('input,button,a,div,textarea').off('click'); });

            if (!this.withoutHeader) {
                this.element.children('div').eq(1).children('div').eq(1).css('max-height', 245);
            }

        }

        public Initiate = () => {
            this.selectedText = [];
            if (!this.scope.selectedIds)
                this.scope.selectedIds = [];

            var head = this.element.find('table').find('thead').find('tr');

        };

        public SelectedAll = () => {
            var selected = false;
            if (this.model && this.scope.selectedIds)
                if (this.model.length == this.scope.selectedIds.length)
                    selected = true;

            return selected;
        };

        public IsSelected = (id) => {
            var selected = false;
            if (this.scope.selectedIds) {
                if (this.scope.selectedIds.indexOf(id) != -1)
                    selected = true;
            }
            return selected;
        };

        public GetState = () => {
            return angular.element(document.getElementById(this.contentId)).css('visibility') == 'hidden';
        };

        public SetBlur = () => {
            angular.element(document.getElementById(this.contentId)).css('visibility', 'hidden');
        };

        public Navigate = ($event) => {
            var el = angular.element($event.target);
            var height = this.element.find('table tbody tr').eq(0).height();
            var length = this.element.find('table tbody').children().length;


            if ($event.which === 38) {
                $event.preventDefault();
                if (this.SelectedRow_Index > 0) {

                    this.SelectedRow_Index = this.SelectedRow_Index - 1;
                    this.element.find('table tbody tr').not(':eq(' + this.SelectedRow_Index + ')').css('background-color', '#dedede');
                    this.element.find('table tbody tr').eq(this.SelectedRow_Index).css('background-color', '#6fb3e0');
                    this.element.find('table').parent().scrollTop(this.SelectedRow_Index * height);

                    if (this.DownArrow_Counter < 7) {
                        this.DownArrow_Counter = this.DownArrow_Counter + 1;
                    }

                    this.element.find('table').parent().scrollTop((this.SelectedRow_Index - 7 + this.DownArrow_Counter) * height);

                }

            }
            else if ($event.which === 40) {
                $event.preventDefault();

                if (this.SelectedRow_Index < length - 1) {
                    this.SelectedRow_Index = this.SelectedRow_Index + 1;
                    this.element.find('table tbody tr').not(':eq(' + this.SelectedRow_Index + ')').css('background-color', '#dedede');
                    this.element.find('table tbody tr').eq(this.SelectedRow_Index).css('background-color', '#6fb3e0');

                    if (this.DownArrow_Counter > 1) {
                        this.DownArrow_Counter = this.DownArrow_Counter - 1;
                    }

                    this.element.find('table').parent().scrollTop((this.SelectedRow_Index - 7 + this.DownArrow_Counter) * height);

                }

            }
            else if ($event.which === 9) {
                this.element.children().eq(0).css('visibility', 'visible');
                this.element.children().eq(1).css('visibility', 'hidden');
            }
            else if ($event.which === 13) {
                this.element.find('table tbody tr').eq(this.SelectedRow_Index).find('span').mousedown().trigger('mousedown');
                $event.preventDefault();
            }
            else if ($event.which === 27) {
                this.element.children().eq(1).css('visibility', 'hidden');
                this.element.children().eq(0).css('visibility', 'visible');
                $event.preventDefault();
            }
        };

        public Reset = () => {
            this.SelectedRow_Index = -1;
            this.element.find('table tbody tr').css('background-color', '#dedede');
        }

        public SetText = () => {
            var text = "";
            for (var i = 0; i < this.selectedText.length; i++) {
                text += this.selectedText[i] + ", ";
            }
            angular.element(document.getElementById(this.textId)).text(text);

        }

        public ChangeVisibility = () => {

            this.element.children().eq(1).css('visibility', this.GetState() ? 'visible' : 'hidden');
            this.element.children().eq(1).children().find('input').focus();
            this.element.find('table').parent().scrollTop(0);
        };

        public SetCheckAll = (e) => {
            e.preventDefault();
            var spanTag = angular.element(e.target);
            var checkState = spanTag.attr("class") == "fa fa-circle-thin";
            this.selectedText = [];
            this.scope.selectedIds = [];
            this.scope.selectedModels = [];
            if (checkState) {
                for (var i = 0; i < this.model.length; i++) {
                    this.selectedText.push(this.model[i][this.displayMember]);
                    this.scope.selectedIds.push(this.model[i][this.valueMember]);
                }
                this.scope.selectedModels = this.model;
            }

            this.element.children().eq(1).children().find('input').focus();
            var x = this.element.children().eq(1).children().find('input');
        }

        public SetCheck = (e, id) => {
            e.preventDefault();
            var spanTag = angular.element(e.target).parent().find('span');
            var checkState = spanTag.attr("class") == "fa fa-circle-thin";
            var itemIndex = -1;
            for (var i = 0; i < this.model.length; i++) {
                if (this.model[i][this.valueMember] == id) {
                    itemIndex = i;
                    break;
                }
            }
            if (itemIndex != -1) {
                var modelId = this.model[itemIndex];
                var dataId = modelId[this.valueMember];
                if ((this.scope.selectedIds.indexOf(dataId) == -1) && (checkState)) {

                    this.selectedText.push(modelId[this.displayMember]);
                    this.scope.selectedIds.push(modelId[this.valueMember]);
                }

                if ((this.scope.selectedIds.indexOf(dataId) != -1) && (!(checkState))) {
                    var index = this.scope.selectedIds.indexOf(dataId);
                    var textIndex = this.selectedText.indexOf(modelId[this.displayMember]);
                    if (index != -1) {
                        this.scope.selectedIds.splice(index, 1);
                        this.selectedText.splice(textIndex, 1);
                    }
                }
            }

            this.textSelected = "";
            this.scope.selectedModels = [];
            for (var j = 0; j < this.scope.selectedIds.length; j++) {
                var _model = this.model.filter((data) => { return data[this.valueMember] == this.scope.selectedIds[j] });
                this.scope.selectedModels.push(_model[0]);
                if (_model.length != 0) {
                    this.textSelected += _model[0][this.displayMember] + '، ';
                }
            }

            this.textSelected = this.textSelected.substring(0, this.textSelected.length - 2);
        }
    }
}