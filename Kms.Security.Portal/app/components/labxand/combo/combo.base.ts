module labxand.components.core {
    export interface IComboBase<TModel> {

        Data: any;
        ItemSelectedId: string;
        ItemSelectedText: string;
        IsMultiSelect: boolean;
        Id: string;
        IsStatic: boolean;
        Service: nodak.service.IServiceBase<TModel>;
        AllowAdd: boolean;
        OnChange();
        SelectedItems: Array<TModel>;
        PlaceHolder: string;
        RTL: boolean;
        AllowClear: boolean;
        Clear();
        SelectOnClose: boolean;
    }
    export class ComboBase<TModel> implements IComboBase<TModel>/*, IMultiSelectCombo<TModel>*/ {
        SelectedItem: any;
        SelectedItems: Array<TModel>;
        ItemSelectedId: string;
        ItemSelectedText: string;
        IsMultiSelect: boolean;
        SelectOnClose: boolean;
        AllowClear: boolean;
        Data: any;
        Id: string;
        MultiSelect;
        PlaceHolder: string;
        RTL: boolean;
        AllowAdd: boolean;
        protected ModelEntity: TModel;
        IsStatic: boolean;
        Service: nodak.service.IServiceBase<TModel>;
        constructor(service: nodak.service.IServiceBase<TModel>) {
            this.Service = service;
            this.Id = "Combo" + new Date().getTime() + Math.floor(Math.random() * 100000);
            this.AllowAdd = false;
            this.RTL = true;
            this.PlaceHolder = "انتخاب کنید";
            this.AllowClear = false;
            //this.SelectOnClose = true;
            //this.SelectedItems = []; 
        }
        HasItemSelectedId<TResult>(propertyName: TResult) {
            this.ItemSelectedId = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
        HasItemSelectedText<TResult>(propertyName: TResult) {
            this.ItemSelectedText = nodak.PropertyManipulating.GetName(propertyName);
            return this;
        }
        Clear() {
            this.SelectedItem = null;
            this.SelectedItems = [];
        }
        changeSelection() {
          
            this.SelectedItems = new Array<TModel>();
            if (this.IsMultiSelect) {

                this.Data.forEach((item) => {
                    if (this.SelectedItem != null)
                        this.SelectedItem.forEach((t) => {
                            if (item[this.ItemSelectedId] == t)
                                this.SelectedItems.push(angular.copy(item));
                        });
                });

            }
            else {
                this.Data.forEach((item) => {
                    if (item[this.ItemSelectedId] == this.SelectedItem)
                        this.SelectedItems.push(angular.copy(item));
                });
            }
           
            this.OnChange();
        }
        OnChange() {
            //console.log("a", this.SelectedItems);
            //console.log("b", this.SelectedItem);
        };
    }

}