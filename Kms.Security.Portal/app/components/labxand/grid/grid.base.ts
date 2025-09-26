module labxand.components.core {
    export class DataGrid<T> {
        TotalCount: number;
        Results: Array<T>;
    }


    //USE IN CLASS GRID
    export interface IGridBase<GridModel, SearchModel> extends IGrid<GridModel, SearchModel> {
        Data: DataGrid<GridModel>;
        Columns: Array<IGridColumns>;
        OnSelectedRow(model: GridModel): void;
        SetTypeOfService(serviceType: nodak.enums.ServiceTypeEnum): void;
        build();
        MultiSelect: boolean;
    }

    export class GridBase<GridModel extends nodak.models.ModelBase, SearchModel> implements IGridBase<GridModel, SearchModel> {
        SelectedRow: GridModel;
        selectedRows: Array<GridModel>;
        MultiSelect: boolean = false;
        SelectedId: string;
        Filters: FilterBase<GridModel>;
        protected service: nodak.service.IServiceBase<DataGrid<GridModel>>;
        Data: DataGrid<GridModel>;
        Columns: Array<IGridColumns>;
        protected Model: GridModel;
        protected SearchModel: SearchModel;
        protected SearchConfiguration: SearchModelConfiguration<SearchModel>;
        currentFilter: Array<FilterSpecification>;
        specificationOfDataList: SpecificationOfDataList;
        specificationOfDataListForReport: SpecificationOfDataListForReport;
        sortedField: string;
        searchCalled: boolean;
        cardMode: boolean;
        private serviceType: nodak.enums.ServiceTypeEnum;
        modelId: string;
        totalItems;
        countOfPages;
        _pageSize;
        currentPage;
        List;
        Hide;
        page;
        adjacent;
        reportModel: SearchModel;
        protected messageBox: IMessageBox;

        constructor(injector: ng.auto.IInjectorService,service) {
            this.Columns = [];
            this.SearchConfiguration = new SearchModelConfiguration(this.Model, this.SearchModel);
            this.Data = new DataGrid<GridModel>();
            this.modelId = "Id";
            this.totalItems = 0;
            this.serviceType = nodak.enums.ServiceTypeEnum.GetForGrid;
            this.messageBox = injector.get('messageBox');
            this.service = service;
            this.selectedRows = new Array<GridModel>();
           

        }


        private searchMethod() {
            return this.service.Post(this.specificationOfDataList, this.serviceType)
                .then((response) => {
                    
                    this.Data = response; this.ComputePageCount(response.TotalCount); this.totalItems = response.TotalCount; /*console.log(response.Results)*/;
                });
        }

        SetTypeOfService(serviceType: nodak.enums.ServiceTypeEnum) {
            this.serviceType = serviceType;
        }

        ComputePageCount(totalCount: number) {
            this.countOfPages = Math.floor(totalCount / this._pageSize) + 1;
        }

        GoToPage(pageNumber: number) {
            if (!this.specificationOfDataList)
                return;
            this.specificationOfDataList.pageIndex = pageNumber;
            this.searchMethod();
        }

        NextPage() {
            if (this.countOfPages.toString() == this.currentPage.toString())
                return;
            if (!this.specificationOfDataList) return;
            this.currentPage += 1;
            this.specificationOfDataList.pageIndex = this.currentPage;
            this.searchMethod();
        }

        PreviousPage() {
            if (!this.specificationOfDataList || this.currentPage <= 1) return;
            this.currentPage -= 1;
            this.specificationOfDataList.pageIndex = this.currentPage;
            this.searchMethod();
        }

        CreateSearchConfiguration<TResult>(filed: TResult): FilterSearchModel {
            return this.SearchConfiguration.Bound(filed);
        }

        public SetPageSize(size: number) {
            this._pageSize = size;
        }

        protected Bound<TResult>(propertyName: TResult) {
            var gridCol = new GridColumns(nodak.PropertyManipulating.GetNameModelGrid(propertyName));
            this.Columns.push(gridCol);
            return gridCol;
        }

        HasKey<TResult>(propertyName: TResult) {
            this.modelId = nodak.PropertyManipulating.GetName(propertyName);
        }

        AfterSelect(selectedModel: GridModel) { }
        AfterSelectRows(selectedModels: Array<GridModel>) { }
        AfterChange(changedModel: GridModel) { }
        Disable() { }
        Enable() { }
        ClearSelected() {
            this.SelectedId = null;
        }

        SearchById(id: string) {
            //IMPLEMENTS LATER 
        }

        SearchBySpecificField(fieldName: string, typeOfFilter: string, value: any) {
            //IMPLEMENTS LATER
        }
        Search(model: SearchModel) {
            this.searchCalled = true;
            this.sortedField = null;
            this.specificationOfDataList = new SpecificationOfDataList();
            this.specificationOfDataList.ascendingSortDirection = false;
            this.currentFilter = this.SearchConfiguration.createFilter(model);
            this.specificationOfDataList.filterSpecifications = this.currentFilter;
            this.specificationOfDataList.pageIndex = 0;
            this.currentPage = 0;
            this.specificationOfDataList.pageSize = this._pageSize;
            this.specificationOfDataList.sortSpecification = null;
            //alert('search');
            this.page = 1;

            this.reportModel = angular.copy(model);

            return this.searchMethod();
        };
        //morsa{
        //Report(type = "PDF", model: SearchModel = null) {
        //    if (model == null && this.searchCalled) {
        //        model = this.reportModel;
        //    }
        //    if (model == null && this.searchCalled == false)
        //        return;
        //    this.sortedField = null;
        //    this.specificationOfDataListForReport = new SpecificationOfDataListForReport();
        //    this.specificationOfDataListForReport.ascendingSortDirection = false;
        //    this.currentFilter = this.SearchConfiguration.createFilter(model);
        //    this.specificationOfDataListForReport.filterSpecifications = this.currentFilter;
        //    this.specificationOfDataListForReport.pageIndex = 0;
        //    this.currentPage = 0;
        //    this.specificationOfDataListForReport.pageSize =99999;
        //    this.specificationOfDataListForReport.sortSpecification = null;
        //    this.specificationOfDataListForReport.columns = this.Columns;
        //    this.specificationOfDataListForReport.Url = this.service.Url + "/GetForGrid";
        //    this.specificationOfDataListForReport.ExportFormat = type;
        //    //alert('search');
        //    this.page = 1;
        //    if (type == "View")
        //        return this.reportViewMethod();
        //    return this.reportMethod();
        //}

        //private reportMethod() {
        //    return this.service.Report(this.specificationOfDataListForReport)
        //        .then((response) => {
        //            window.open(Base.Config.ReportService + "/Report/DownloadPDF?type=" + this.specificationOfDataListForReport.ExportFormat
        //                + "&reportName=" + response
        //                , "_blanck");
        //        });
        //}

        //private reportViewMethod() {

        //    return this.service.Report(this.specificationOfDataListForReport)
        //        .then((response) => {
        //            window.open(Base.Config.ReportService + "/ReportCo/ReportViewer?reportName=" + response, "_blanck");
        //        });
        //}
        //}morsa
        Clear() {
            this.Data = null;
            this.countOfPages = 0;
            this.totalItems = 0;
        }

        OnSelectedRow(model: GridModel) {
               let changed: boolean = false;
            if (this.SelectedRow == model)
                changed = true;
            this.SelectedRow = model;
            this.SelectedId = model[this.modelId];

            this.AfterSelect(model);
            if (changed)
                this.AfterChange(model);
         
        }



        OnSelectedRows(model: GridModel) {

            let itemInSelectedRows = this.selectedRows.filter(item => item == model)[0];

            if (this.SelectedRow == model || itemInSelectedRows != undefined) {
                if (this.SelectedRow == model && itemInSelectedRows != undefined) {
                    this.DeleteFromArray(this.selectedRows, model);
                    this.SelectedRow = undefined;
                    this.SelectedRow = undefined;
                }

                else if (itemInSelectedRows != undefined)
                    this.DeleteFromArray(this.selectedRows, model);

                else {
                    this.SelectedRow = undefined;
                    this.SelectedId = undefined;
                }
            }
            else {
                this.selectedRows.push(model);
                this.SelectedRow = model;
                this.SelectedId = model[this.modelId];
                this.AfterSelect(model);
                this.AfterSelectRows(this.selectedRows);
            }

            this.AfterChange(model);
        }

        
        private DeleteFromArray(models: Array<any>, itemToremove) {
            let index = models.indexOf(itemToremove)
            if (index > -1)
                models.splice(index, 1);
        }
        
        Check(name: string) {
            if (name == this.sortedField)
                return true;
            return undefined;
        }

        CheckSelectedRows(selectedRow: GridModel) {
           
            let itemInSelectedRows = this.selectedRows.filter(item => item == selectedRow)[0];
            if (itemInSelectedRows != undefined)
                return true;
            //if (this.selectedRow == selectedRow)
            //    return true;

            return false;
        }

        CheckSelected(selectedId: string) {
           
            if (selectedId == this.SelectedId)
                return true;

            return false;
        }

        Orderby(field: string, asc: boolean) {
            if (!this.searchCalled)
                return;
            if (asc != undefined)
                asc = !asc;
            if (asc == undefined)
                asc = true;

            this.sortedField = field;
            this.specificationOfDataList = new SpecificationOfDataList();
            this.specificationOfDataList.ascendingSortDirection = asc;
            this.specificationOfDataList.filterSpecifications = this.currentFilter;
            this.specificationOfDataList.pageIndex = this.currentPage;
            this.specificationOfDataList.pageSize = this._pageSize;
            this.specificationOfDataList.sortSpecification = field;
            this.searchMethod();

            return asc;
        }

        ////////////////////////////////////////

        setValues() {
            this.List = [];
            this.Hide = false;
            this.page = parseInt(this.page) || 1;
            this.totalItems = parseInt(this.totalItems) || 0;
            this.adjacent = parseInt(this.adjacent) || 2;
            //this.pa
        }

        validateValues(pageCount) {
            if (this.page > pageCount) {
                this.page = pageCount;
            }
            if (this.page <= 0) {
                this.page = 1;
            }
            if (this.adjacent <= 0) {
                this.adjacent = 2;
            }
            if (pageCount < 1) {
                this.Hide = 'true';
            }
            //alert(pageCount);
        }

        addRange(start, finish) {
            var i = 0;
            for (i = start; i <= finish; i++) {
                var liClass = this.page == i ? 'active' : '';
                var x = i;
                this.List.push({
                    value: i,
                    liClass: liClass
                });
            }
        }

        addDots() {
            this.List.push({
                value: '...',
                liClass: 'disabled'
            });
        }

        addFirst(next) {
            this.addRange(1, 2);
            if (next != 3) {
                this.addDots();
            }
        }

        addLast(pageCount, prev) {
            if (prev != pageCount - 2) {
                this.addDots();
            }
            this.addRange(pageCount - 1, pageCount);
        }

        internalAction(item) {
            if (item.liClass == "disabled") {
                return
            }
            if (item.value == "...") {
                return
            }
            if (this.page == item.value) {
                return;
            }
            if (item.aClass == 'fa fa-angle-double-right') {
                this.page = 1;
            }
            else if (item.aClass == 'fa fa-angle-right') {
                this.page--;
            }
            else if (item.aClass == 'fa fa-angle-double-left') {
                this.page = this.countOfPages;
            }
            else if (item.aClass == 'fa fa-angle-left') {
                this.page++;
            }
            else {
                this.page = item.value;
            }
            this.specificationOfDataList.pageIndex = this.page - 1;
            this.searchMethod();
        }

        addPrevNext(pageCount, mode) {
            if (pageCount < 1) {
                return;
            }
            var disabled, alpha, beta;
            if (mode === 'prev') {
                disabled = this.page - 1 <= 0;
                var prevPage = this.page - 1 <= 0 ? 1 : this.page - 1;
                alpha = {
                    aClass: 'fa fa-angle-double-right',
                    page: 1,
                };
                beta = {
                    aClass: 'fa fa-angle-right',
                    page: prevPage,
                };
            }
            else {
                disabled = this.page + 1 > pageCount;
                var nextPage = this.page + 1 >= pageCount ? pageCount : this.page + 1;
                alpha = {
                    aClass: 'fa fa-angle-left',
                    page: nextPage,
                };
                beta = {
                    aClass: 'fa fa-angle-double-left',
                    page: pageCount,
                };
            }

            var buildItem = function (item, disabled) {
                return {
                    aClass: item.aClass,
                    value: item.aClass ? '' : item.value,
                    liClass: disabled ? 'disabled' : ''
                };
            };

            if (alpha) {
                var alphaItem = buildItem(alpha, disabled);
                this.List.push(alphaItem);
            }

            if (beta) {
                var betaItem = buildItem(beta, disabled);
                this.List.push(betaItem);
            }
        }

        build() {
            if (!this._pageSize || this._pageSize <= 0) {
                this._pageSize = 1;
            }
            var pageCount = Math.ceil(this.totalItems / this._pageSize);
            this.setValues();
            this.validateValues(pageCount);
            var start, finish;
            var fullAdjacentSize = (this.adjacent * 2) + 2;
            this.addPrevNext(pageCount, 'prev');
            if (pageCount <= (fullAdjacentSize + 2)) {
                start = 1;
                this.addRange(start, pageCount);
            }
            else {
                if (this.page - this.adjacent <= 2) {
                    start = 1;
                    finish = 1 + fullAdjacentSize;
                    this.addRange(start, finish);
                    this.addLast(pageCount, finish);
                }
                else if (this.page < pageCount - (this.adjacent + 2)) {
                    start = this.page - this.adjacent;
                    finish = this.page + this.adjacent;
                    this.addFirst(start);
                    this.addRange(start, finish);
                    this.addLast(pageCount, finish);
                }
                else {

                    start = pageCount - fullAdjacentSize;
                    finish = pageCount;
                    this.addFirst(start);
                    this.addRange(start, finish);
                }
            }
            this.addPrevNext(pageCount, 'next');
        }
    }

    export class CrudGrid<GridModel extends nodak.models.ModelBase, SearchModel> extends GridBase<GridModel, SearchModel> {
        buttons: Array<Button>;
        deleteFromDB: boolean;
        insertFromDB: boolean;
        editFromDB: boolean;
        dataList: Array<GridModel>;

        constructor(injector: ng.auto.IInjectorService,service) {
            super(injector,service);
            this.buttons = new Array<Button>();
            this.deleteFromDB = false;
            this.insertFromDB = false;
            this.editFromDB = false;
        }

        OnSelectedRow(model: GridModel) {
            super.OnSelectedRow(model);
        }

        SelectedRowNotAssigned(message: string) {
            if (this.SelectedRow == null) {
                this.messageBox.OkButton("پیام", message).then((response) => { });
                return true;
            }

            return false;
        }

        DeleteSelectedRow() {
            if (this.SelectedRowNotAssigned("موردی برای حذف انتخاب نشده است"))
                return;
            this.messageBox.YesNoButton("حذف", "آیا مایل به حذف سطر انتخابی هستید؟")
                .then((response) => {
                    if (this.deleteFromDB) {
                        this.service.Post(this.SelectedRow, nodak.enums.ServiceTypeEnum.Delete)
                            .then((response) => { })
                            .catch((error) => { });
                    }
                    else
                        this.Data.Results.removeItem(this.SelectedRow);

                    this.SelectedRow = null;
                })
                .catch(() => {
                    this.SelectedRow = null;
                });



        }

        EditSelectedRow(newItem: GridModel, index: number) {

            if (this.editFromDB) {
                this.service.Post(this.SelectedRow, nodak.enums.ServiceTypeEnum.Edit)
                    .then((response) => { })
                    .catch((error) => { });
            } else {
                this.Data.Results.splice(index, 1, newItem);
            }

        }

        InsertRow(model: GridModel) {
            if (this.insertFromDB) {
                this.service.Post(model, nodak.enums.ServiceTypeEnum.Save)
                    .then((response) => { })
                    .catch((error) => { });
            }
            else {
                this.Data.Results.push(model);
            }
        }

        AddDeleteButton() {
            let deleteButton = new Button("Delete");
            this.buttons.push(deleteButton);

            deleteButton.HasCaption("حذف")
                .HasTitle("")
                .HasClass("Delete")
                .SetOnClick(() => {
                    this.DeleteSelectedRow();
                });
        }

        AddInsertButton() {
            let insertButton = new Button("Insert");
            insertButton.HasCaption("جدید").HasClass("insert");
            this.buttons.push(insertButton);
        }

        AddEditButton() {
            let editButton = new Button("Edit");
            editButton.HasCaption("ویرایش").HasClass("edit");
            this.buttons.push(editButton);
        }

        EditButtonOnSelecte(func: Function) {
            this.buttons.filter(q => q.id == "Edit")[0].SetOnClick(() => {
                if (this.SelectedRowNotAssigned("موردی برای ویرایش انتخاب نشده است"))
                    return;
                else
                    func();
            });
        }

        DeleteButtonOnSelected(func: Function) {
            this.buttons.filter(q => q.id == "Delete")[0].SetOnClick(func);
        }

        InsertButtonOnSelected(func: Function) {
            this.buttons.filter(q => q.id == "Insert")[0].SetOnClick(func);
        }
    }

}