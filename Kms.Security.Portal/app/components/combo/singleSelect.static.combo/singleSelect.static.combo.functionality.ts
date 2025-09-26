module nodak.components {

    export class StaticDropdownMultiColumnsFunctions<T> {
        textId: string;
        SelectedRow_Index: number = -1;
        DownArrow_Counter: number = 7;
        UpArrow_Counter: number = 0;
        textboxId: string;
        defaultSelectId: string;
        contentId: string;
        valueMember: string;
        displayMember: string;
        model: any;
        headers: any;
        withoutHeader: boolean;
        withoutFilter: boolean;
        constructor(public element: ng.IAugmentedJQuery, public scope: nodak.directives.IStaticSingleSelctCombo) {
            this.element = element;
            this.textboxId = scope.id;
            this.textId = scope.id + 'Text';
            this.valueMember = scope.selectList.valueMember;
            this.displayMember = scope.selectList.displayMember;
            this.model = scope.model;
            this.headers = scope.selectList.headers;
            if (scope.defaultId)
                this.defaultSelectId = scope.defaultId;
            this.contentId = scope.id + 'Content';
            scope.$watch(() => { return this.scope.selectedId; }, (newValue, oldValue) => {
                if (scope.model) this.ChangeSelected();
                if (newValue == null || newValue == undefined || newValue == "") {
                    if (newValue == false)
                        return;
                    this.scope.selectedText = "";
                    this.scope.selectedId = null;
                }
                else {
                    this.scope.selectedId = newValue;
                }
            });

            element.parent().find('section').find('input').on('blur', () => {
                this.SetBlur();
            });

            this.Initiate();
            scope.$watch('IsDisabled', (newValue, oldValue) => { if (scope.isDisabled == true) this.element.find('input,button,a,div,textarea').off('click'); });
        }

        public Initiate = () => {

            if (this.model && this.scope.selectedId) {
                if (!this.scope.selectedId) {
                    let selected = this.scope.selectedId;
                    var model = this.model.filter(function (model) { return model[this.valueMember] == selected });
                    if (model.length != 0) {
                        this.scope.selectedText = model[0][this.displayMember];
                    }
                }
            }
            if (this.withoutFilter) {
                this.element.find('.contentdropdown section input').css('display', 'none');
            }

            if (this.withoutHeader) {
                this.element.find('.contentdropdown section').css('padding-top', 0);
                this.element.find('.contentDropdownContainter thead').css('display', 'none');
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
                this.element.find('table tbody tr').eq(this.SelectedRow_Index).mousedown();
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
            this.element.children().eq(0).css('visibility', this.GetState() ? 'hidden' : 'visible');

            this.element.children().eq(1).css('visibility', this.GetState() ? 'visible' : 'hidden');

            this.element.children().eq(1).children().find('input').focus();

            this.SelectedRow_Index = -1;

            this.element.find('table').parent().scrollTop(0);

            this.element.find('table tbody tr').css('background-color', '#dedede');

        }

        public ChangeSelected = () => {
            let id = this.scope.selectedId;
            let index = -1;
            let valueName = this.valueMember;
            let selected = this.scope.selectedId;
            let model = this.model.filter(function (model) { return model[valueName] == id });

            if (model.length != 0) {
                this.scope.selectedText = model[0][this.displayMember];
                this.scope.selectedId = model[0][this.valueMember];
            }
        }

        public SetCheck = (id) => {
            this.scope.selectedId = id;
            this.element.children().eq(1).css('visibility', 'hidden');
            this.element.children().eq(0).css('visibility', 'visible');
        }

    }

}  