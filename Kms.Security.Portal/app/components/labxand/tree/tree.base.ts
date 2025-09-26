module labxand.components.core {
    export interface ITreeBase<Model> extends components.ITree<Model> {
        Data: /*Array<Model>*/any;
        OnSelectedRow(model: Model): void;
        SetTypeOfService(serviceType: nodak.enums.ServiceTypeEnum): void;
        build();
        ItemSelectedText: string;
        ItemSelectedId: string;
        ItemSelectedParentId: string;
        DataBind();
        MultiSelect: boolean;
        CallFromService: boolean;
        Refresh();
        SetSelectedItem(item);
        IsSortable;
        ItemSort;
        FindModelById(Id: string);
    }

    export class TreeBase<TModel extends nodak.models.ModelBase> implements ITreeBase<TModel> {
        Data: any/*Array<Model>*/;
        CallFromService: boolean = true;
        ItemSelectedText = "Name";
        ItemSelectedId = "Id";
        ItemSelectedParentId = "ParentId";
        MultiSelect: boolean = false;
        SelectedRow: TModel;
        testInputTime: number;
        protected ModelEntity: TModel;
        protected service: nodak.service.IServiceBase<TModel>;
        constructor(service) {
            this.service = service;
        }
        FindModelById(Id: string) {
            let result = null;
            this.Data.forEach((item) => {
                if (item[this.ItemSelectedId] == Id) {
                    result = item;
                }
            });
            return result;
        }
        public DataBind() {
            if (this.CallFromService)
                return this.service.Get(nodak.enums.ServiceTypeEnum.Get)
                    .then((response) => {
                        this.Data = response;
                    });
        }
        Refresh() { }
        HasItemSelectedId<TResult>(propertyName: TResult) {
            this.ItemSelectedId = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
        HasItemSelectedText<TResult>(propertyName: TResult) {
            this.ItemSelectedText = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
        HasItemSelectedParent<TResult>(propertyName: TResult) {
            this.ItemSelectedParentId = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
        OnSelectedRow(model: TModel) {
            let changed: boolean = false;
            //if (this.SelectedRows == model)
            //    changed = true;
            this.SelectedRows.push(model);
            this.SelectedId = model[this.modelId];
            //this.AfterSelect(model);
            //if (changed)
            //    this.AfterChange(model);
        }
        SetTypeOfService(serviceType: nodak.enums.ServiceTypeEnum): void { }
        build() {

        }

        Search(searchTerm: string) {

        }
        SearchById(id: any) {
            for (var i = 0; i < this.Data.length; i++) {
                if (this.Data[i][this.ItemSelectedId] == id)
                    return this.Data[i];
            }
            return null;
        }
        ClearSelected() { }
        SelectedRows: Array<TModel>;
        SelectedId: Array<string>;
        modelId: string;
        AfterSelect(model: TModel) {
            //console.log(model);

        }
        AfterChange(changedModel: TModel) { }
        Disable() { }
        Enable() { }
        Clear() { }
        SetSelectedItem(selecteditem) { }
        ItemSort: string;
        IsSortable = false;
        HasItemSort<TResult>(propertyName: TResult) {
            this.IsSortable = true;
            this.ItemSort = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
    }
}