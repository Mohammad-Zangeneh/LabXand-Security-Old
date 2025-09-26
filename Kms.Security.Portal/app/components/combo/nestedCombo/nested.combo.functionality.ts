module nodak.components {

    export class NestedDropdownFunctions<T> {
        

        selectedText: string;
        selectedId: string;
        valueMember: string;
        displayMember: string;
        model: any;
        parentName: string;
        headers: any;
        id: string;

        SelectedRow_Index: number = -1;
        DownArrow_Counter: number = 7;
        UpArrow_Counter: number = 0;

        id1: string;
        textId1: string;
        contentId1: string;
        selectedId1: string;
        model1: any;

        id2: string;
        textId2: string;
        contentId2: string;
        selectedId2: string;
        model2: any;

        id3: string;
        textId3: string;
        contentId3: string;
        selectedId3: string;
        model3: any;

        id4: string;
        textId4: string;
        contentId4: string;
        selectedId4: string;
        model4: any;

        constructor(public element: ng.IAugmentedJQuery, public scope: nodak.directives.INestedCombo) {
            this.selectedId = null;
            this.element = element;
            this.selectedText = "";
            this.valueMember = scope.selectList.valueMember;
            this.displayMember = scope.selectList.displayMember;
            this.model = scope.model;
            this.headers = scope.selectList.headers;
            this.id = scope.id;

            this.parentName = scope.selectList.parentName;

            this.id1 = scope.id + '1';
            this.textId1 = scope.id + 'Text1';
            this.contentId1 = scope.id + 'Content1';

            this.id2 = scope.id + '2';
            this.textId2 = scope.id + 'Text2';
            this.contentId2 = scope.id + 'Content2';

            this.id3 = scope.id + '3';
            this.textId3 = scope.id + 'Text3';
            this.contentId3 = scope.id + 'Content3';

            this.id4 = scope.id + '4';
            this.textId4 = scope.id + 'Text4';
            this.contentId4 = scope.id + 'Content4';
        }

        public SetBlur = (id) => {
            var div = angular.element(document.getElementById(id));
            div.css('visibility', 'hidden');
        }

        public GetState = (element: ng.IAugmentedJQuery) => {
            return element.css('visibility') == 'hidden';
        }

        public Navigate = ($event, id) => {

            var div = angular.element(document.getElementById(id));
            //var el = angular.element($event.target);
            var height = div.find('table tbody tr').eq(0).height();
            var length = div.find('table tbody').children().length;

            if ($event.which === 38) {
                $event.preventDefault();
                if (this.SelectedRow_Index > 0) {

                    this.SelectedRow_Index = this.SelectedRow_Index - 1;
                    div.find('table tbody tr').not(':eq(' + this.SelectedRow_Index + ')').css('background-color', '#dedede');
                    div.find('table tbody tr').eq(this.SelectedRow_Index).css('background-color', '#6fb3e0');
                    div.find('table').parent().scrollTop(this.SelectedRow_Index * height);

                    if (this.DownArrow_Counter < 7) {
                        this.DownArrow_Counter = this.DownArrow_Counter + 1;
                    }

                    div.find('table').parent().scrollTop((this.SelectedRow_Index - 7 + this.DownArrow_Counter) * height);
                }
            }
            else if ($event.which === 40) {
                $event.preventDefault();

                if (this.SelectedRow_Index < length - 1) {
                    this.SelectedRow_Index = this.SelectedRow_Index + 1;
                    div.find('table tbody tr').not(':eq(' + this.SelectedRow_Index + ')').css('background-color', '#dedede');
                    div.find('table tbody tr').eq(this.SelectedRow_Index).css('background-color', '#6fb3e0');

                    if (this.DownArrow_Counter > 1) {
                        this.DownArrow_Counter = this.DownArrow_Counter - 1;
                    }

                    div.find('table').parent().scrollTop((this.SelectedRow_Index - 7 + this.DownArrow_Counter) * height);

                }

            }
            else if ($event.which === 9) {
                div.parent().children().eq(0).css('visibility', 'visible');
                div.parent().children().eq(1).css('visibility', 'hidden');
            }
            else if ($event.which === 13) {
                div.find('table tbody tr').eq(this.SelectedRow_Index).mousedown().trigger('mousedown');
                div.parent().children().eq(0).css('visibility', 'visible');
                div.parent().children().eq(1).css('visibility', 'hidden');
                $event.preventDefault();
            }
            else if ($event.which === 27) {
                div.parent().children().eq(1).css('visibility', 'hidden');
                div.parent().children().eq(0).css('visibility', 'visible');
                $event.preventDefault();
            }
        }

        public Reset = (id) => {
            this.SelectedRow_Index = -1;
            var div = angular.element(document.getElementById(id));
            div.find('table tbody tr').css('background-color', '#dedede');
        }

        public SetText = (id, text) => {
            angular.element(document.getElementById(id)).val(text);
            angular.element(document.getElementById(id)).attr('title', text);



        }

        public ChangeVisibility = (id) => {

            var div = angular.element(document.getElementById(id));

            div.css('visibility', this.GetState(div) ? 'visible' : 'hidden');

            div.find('input').eq(0).focus();

            this.SelectedRow_Index = -1;

            div.find('table').parent().scrollTop(0);

            div.find('table tbody tr').css('background-color', '#dedede');
        }

        public SetVisibility = (id, typeOfVisibility) => {
            angular.element(document.getElementById(id)).css('visibility', typeOfVisibility);

            //alert('content1:' + angular.element(document.getElementById(this.contentId1)).css('visibility'));
            //alert('content2:' + angular.element(document.getElementById(this.contentId2)).css('visibility'));
            //alert('content3:' + angular.element(document.getElementById(this.contentId3)).css('visibility'));
            //alert('content4:' + angular.element(document.getElementById(this.contentId4)).css('visibility'));

            if (angular.element(document.getElementById(this.id4)).css('visibility') == 'visible') {
                angular.element(document.getElementById(this.id + 'Combo1')).css('width', '25%');
                angular.element(document.getElementById(this.id + 'Combo2')).css('width', '25%');
                angular.element(document.getElementById(this.id + 'Combo3')).css('width', '25%');
                angular.element(document.getElementById(this.id + 'Combo4')).css('width', '25%');

                angular.element(document.getElementById(this.contentId1)).css('width', '24%');
                angular.element(document.getElementById(this.contentId2)).css('width', '24%');
                angular.element(document.getElementById(this.contentId3)).css('width', '24%');
                angular.element(document.getElementById(this.contentId4)).css('width', '24%');
            }

            if (angular.element(document.getElementById(this.id4)).css('visibility') == 'hidden') {
                angular.element(document.getElementById(this.id + 'Combo1')).css('width', '33%');
                angular.element(document.getElementById(this.id + 'Combo2')).css('width', '33%');
                angular.element(document.getElementById(this.id + 'Combo3')).css('width', '33%');
                angular.element(document.getElementById(this.id + 'Combo4')).css('width', '0');

                angular.element(document.getElementById(this.contentId1)).css('width', '32%');
                angular.element(document.getElementById(this.contentId2)).css('width', '32%');
                angular.element(document.getElementById(this.contentId3)).css('width', '32%');
                angular.element(document.getElementById(this.contentId4)).css('width', '0');
            }

            if (angular.element(document.getElementById(this.id3)).css('visibility') == 'hidden') {
                angular.element(document.getElementById(this.id + 'Combo1')).css('width', '50%');
                angular.element(document.getElementById(this.id + 'Combo2')).css('width', '50%');
                angular.element(document.getElementById(this.id + 'Combo3')).css('width', '0');
                angular.element(document.getElementById(this.id + 'Combo4')).css('width', '0');

                angular.element(document.getElementById(this.contentId1)).css('width', '49%');
                angular.element(document.getElementById(this.contentId2)).css('width', '49%');
                angular.element(document.getElementById(this.contentId3)).css('width', '0');
                angular.element(document.getElementById(this.contentId4)).css('width', '0');
            }

            if (angular.element(document.getElementById(this.id2)).css('visibility') == 'hidden') {
                angular.element(document.getElementById(this.id + 'Combo1')).css('width', '100%');
                angular.element(document.getElementById(this.id + 'Combo2')).css('width', '0');
                angular.element(document.getElementById(this.id + 'Combo3')).css('width', '0');
                angular.element(document.getElementById(this.id + 'Combo4')).css('width', '0');

                angular.element(document.getElementById(this.contentId1)).css('width', '97%');
                angular.element(document.getElementById(this.contentId2)).css('width', '0');
                angular.element(document.getElementById(this.contentId3)).css('width', '0');
                angular.element(document.getElementById(this.contentId4)).css('width', '0');
            }

        }

        public getMemberById = (id) => {
            var valueName = this.valueMember;
            var model = this.model.filter(function (model) { return model[valueName] == id });
            return model;
        }

        public HasChild = (id) => {
            var parentName = this.parentName;
            var model = this.model.filter(function (m) { return m[parentName] == id });
            if (model.length != 0)
                return true;

            return false;
        }

        public getStructureTree = () => {
            var selected = [];
            var currentSelect = this.selectedId;

            if (currentSelect == null) {
                this.SetText(this.textId1, '');
                this.SetText(this.textId2, '');
                this.SetText(this.textId3, '');
                this.SetText(this.textId4, '');
                this.SetVisibility(this.id2, 'hidden');
                this.SetVisibility(this.id3, 'hidden');
                this.SetVisibility(this.id4, 'hidden');
                return;
            }
            selected.push(currentSelect);
            var model1 = [];
            var model2 = [];
            var model3 = [];
            var model4 = [];

            if (currentSelect != null) {
                var aa = this.valueMember;
                model1 = this.model.filter(function (mn) { return mn[aa] == currentSelect });
            }
            if (model1.length != 0) {
                currentSelect = model1[0][this.parentName];
                if (currentSelect != null) {
                    selected.push(currentSelect);
                    model2 = this.model.filter(function (mn) { return mn[aa] == currentSelect });
                }
            }
            if (model2.length != 0) {
                currentSelect = model2[0][this.parentName];
                if (currentSelect != null) {
                    selected.push(currentSelect);
                    model3 = this.model.filter(function (mn) { return mn[aa] == currentSelect });
                }
            }
            if (model3.length != 0) {
                currentSelect = model3[0][this.parentName];
                if (currentSelect != null) {
                    selected.push(currentSelect);
                    model4 = this.model.filter(function (mn) { return mn[aa] == currentSelect });
                }
            }
            if (model4.length != 0) {
                currentSelect = model4[0][this.parentName];
                if (currentSelect != null)
                    selected.push(currentSelect);
            }
            var selectedSorted = [];
            var selectedModel = [];
            for (var i = selected.length; i > 0; i--) {
                selectedSorted.push(selected[i - 1]);
            }

            selected = selectedSorted;


            if (selectedSorted[0] != null)
                this.SetCheck1(selectedSorted[0], true);
            if (selectedSorted[1] != null)
                this.SetCheck2(selectedSorted[1], true);
            if (selectedSorted[2] != null)
                this.SetCheck3(selectedSorted[2], true);
        }

        public SetSelected = (id) => {
            this.selectedId = id;

            this.getStructureTree();
        }

        public SetCheck1 = (id, first) => {
            this.SetVisibility(this.contentId1, 'hidden');

            var model = this.getMemberById(id);
            if (model.length != 0) {
                var selected = model[0][this.valueMember];
                if (this.selectedId1 == selected && !first)
                    return;

                this.selectedId1 = selected;
                this.selectedId = selected;
                var aa = this.parentName;
                var mn = this.model.filter(function (mn) { return mn[aa] == selected });

                this.SetText(this.textId1, model[0][this.displayMember]);

                this.selectedText = model[0][this.displayMember];
                this.scope.selectList.selectedText = model[0][this.displayMember];

                var parentName = this.parentName;

                this.SetText(this.textId2, '');
                this.model3 = null;
                this.selectedId3 = null;
                this.SetVisibility(this.id3, 'hidden');
                this.SetText(this.textId3, '');
                this.model4 = null;
                this.selectedId4 = null;
                this.SetVisibility(this.id4, 'hidden');
                this.SetText(this.textId3, '');
                if (this.selectedId1 != null && this.HasChild(this.selectedId1)) {
                    var parentName = this.parentName;
                    this.model2 = this.model.filter(function (model) { return model[parentName] == selected });
                    this.selectedId2 = null;
                    this.SetVisibility(this.id2, 'visible');
                }
                else {
                    this.model2 = null;
                    this.selectedId2 = null;
                    this.SetVisibility(this.id2, 'hidden');
                }
            }

        }

        public SetCheck2 = (id, first) => {
            this.SetVisibility(this.contentId2, 'hidden');

            var model = this.getMemberById(id);
            if (model.length != 0) {
                var selected = model[0][this.valueMember];
                if (this.selectedId2 == selected && !first)
                    return;

                this.selectedId2 = selected;
                this.selectedId = selected;

                this.SetText(this.textId2, model[0][this.displayMember]);

                this.selectedText = model[0][this.displayMember];
                this.scope.selectList.selectedText = model[0][this.displayMember];

                this.SetText(this.textId3, '');
                this.model4 = null;
                this.selectedId4 = null;
                this.SetVisibility(this.id4, 'hidden');
                this.SetText(this.textId4, '');

                if (this.HasChild(this.selectedId2)) {
                    var parentName = this.parentName;
                    this.model3 = this.model.filter(function (model) { return model[parentName] == selected });
                    this.selectedId3 = null;
                    this.SetVisibility(this.id3, 'visible');
                }
                else {
                    this.model3 = null;
                    this.selectedId3 = null;
                    this.SetVisibility(this.id3, 'hidden');
                }
            };

        }

        public SetCheck3 = (id, first) => {
            this.SetVisibility(this.contentId3, 'hidden');

            var model = this.getMemberById(id);
            if (model.length != 0) {
                var selected = model[0][this.valueMember];
                if (this.selectedId3 == selected && !first)
                    return;

                this.selectedId3 = selected;
                this.selectedId = selected;

                this.SetText(this.textId3, model[0][this.displayMember]);

                this.selectedText = model[0][this.displayMember];
                this.scope.selectList.selectedText = model[0][this.displayMember];

                if (this.HasChild(this.selectedId3)) {
                    var parentName = this.parentName;
                    this.model4 = this.model.filter(function (model) { return model[parentName] == selected });
                    this.selectedId4 = null;
                    this.SetVisibility(this.id4, 'visible');
                }
                else {
                    this.model4 = null;
                    this.selectedId4 = null;
                    this.SetVisibility(this.id4, 'hidden');
                    this.SetText(this.textId4, '');
                }
            };

        };
    }
}