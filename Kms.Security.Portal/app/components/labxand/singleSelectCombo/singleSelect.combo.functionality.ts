module nodak.components {

    export class DropdownMultiColumnsFunctions<T> {
        // element: ng.IAugmentedJQuery;
        textId: string;
        SelectedRow_Index: number = -1;
        DownArrow_Counter: number = 7;
        UpArrow_Counter: number = 0;
        textboxId: string;
        defaultSelectId: string;
        contentId: string;
        //selectedId: string;
        //selectedText: string;
        valueMember: string;
        displayMember: string;
        // model: any;
        headers: any;
        withoutHeader: boolean;
        disable: boolean;
        changeByClick: boolean;

        constructor(public element: ng.IAugmentedJQuery, public scope: nodak.directives.ISingleSelectCombo) {

            this.element = element;
            this.textboxId = scope.id;
            this.textId = scope.id + 'Text';
            this.contentId = scope.id + 'Content';
            this.valueMember = scope.selectList.valueMember;
            this.displayMember = scope.selectList.displayMember;
            this.scope.selectList.comboModel = scope.model;
            this.headers = scope.selectList.headers;
            this.withoutHeader = scope.withoutHeader;
            //this.scope.selectedId = null;

            if (scope.defaultId)
                this.defaultSelectId = scope.defaultId;

            scope.$watch(() => { return this.scope.selectedId; }, (newValue: string, oldValue) => {
                
                if ((newValue == null || newValue == "") && oldValue != null) {
                    this.scope.selectedText = "";
                    this.scope.selectList.selectedText = "";
                }
                if (newValue != null && newValue != "") {
                    let selectedId: string = newValue;

                    //when id equal 0 is crashed
                    //if (selectedId == "") {
                    //    console.log('selectedId == ""')
                    //    console.log(newValue == "")
                    //    console.log(selectedId == "")
                    //    console.log(selectedId == "0")
                    //    this.scope.selectedText = "";
                    //    this.scope.selectList.selectedText = "";
                    //    this.scope.selectList.selectedId = null;
                    //    return;
                    //}

                    if (selectedId == null) {
                        this.scope.selectedText = "";
                        this.scope.selectList.selectedText = "";
                        return;
                    }

                    if (!this.changeByClick)
                        this.FindAndSetText(newValue, false);
                }
            });

            element.parent().find('input').eq(1).on('blur', () => {
                this.SetBlur();
            });

            this.Initiate();


            if (!this.withoutHeader) {
                this.element.children('div').eq(1).children('div').eq(0).css('max-height', 245);
            }
        }

        public Initiate = () => {

            if (this.scope.selectList.comboModel && this.scope.selectedId) {
                if (this.scope.selectedId.length != 0) {
                    var selected = this.scope.selectedId;
                    var model = this.scope.selectList.comboModel.filter((model) => { return model[this.valueMember] == selected });
                    if (model.length != 0) {
                        this.scope.selectedText = model[0][this.displayMember];
                        this.scope.selectList.selectedText = this.scope.selectedText;
                    }
                }
            }
            if (this.withoutHeader) {
                this.element.find('table').parent().height(215);
            }
            else {
                this.element.find('table').parent().height(245);
            }
        };

        public GetState = () => {
            return this.element.children().eq(1).css('visibility') == 'hidden';
        };

        public SetBlur = () => {
            this.element.children().eq(0).css('visibility', 'visible');
            this.element.children().eq(1).css('visibility', 'hidden');
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
                this.element.find('table tbody tr').eq(this.SelectedRow_Index).mousedown().trigger('mousedown');
                this.element.children().eq(0).css('visibility', 'visible');
                this.element.children().eq(1).css('visibility', 'hidden');
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
            angular.element(document.getElementById(this.textId)).text(this.scope.selectedText);
        }

        public ChangeVisibility = () => {
            if (this.scope.isDisabled)
                return;
            this.changeByClick = true;
            this.element.children().eq(0).css('visibility', this.GetState() ? 'hidden' : 'visible');

            this.element.children().eq(1).css('visibility', this.GetState() ? 'visible' : 'hidden');

            this.element.find('input').eq(1).focus();

            this.SelectedRow_Index = -1;

            this.element.find('table').parent().scrollTop(0);

            this.element.find('table tbody tr').css('background-color', '#dedede');
        }

        public FindAndSetText = (id, fromSetCheck: boolean) => {

            let selectedId = this.scope.selectedId;
            let valueMember = this.valueMember;
            let model = this.scope.selectList.comboModel.filter(function (model) { return model[valueMember] == selectedId });
            if (model.length != 0) {
                this.scope.selectedText = model[0][this.displayMember];
                this.scope.selectList.selectedText = this.scope.selectedText;
                this.scope.selectList.selectedModel = model[0];
            }

            if (fromSetCheck)
                this.scope.selectList.AfterSelect(id, model[0]);

            this.scope.selectList.AfterChange(id, model[0]);
            this.changeByClick = false;
        }

        public SetCheck = (id) => {
            this.changeByClick = true;
            this.scope.selectedId = id;
            let selectedId = this.scope.selectedId;
            let valueMember = this.valueMember;
            let model = this.scope.selectList.comboModel.filter(function (model) { return model[valueMember] == selectedId });
            this.scope.selectList.BeforSelect(id, model[0]);
            this.element.children().eq(1).css('visibility', 'hidden');
            this.element.children().eq(0).css('visibility', 'visible');
            this.changeByClick = true;
            this.FindAndSetText(id, true);

        }
    }
}