module labxand.components {
    export interface IComponentBase<Model> {
        AfterSelect(selectedModel: Model);
        AfterChange(changedModel: Model);
        Disable();
        Enable();
        Clear();
    }

    export interface IGrid<Model, SearchModel> extends IComponentBase<Model> {
        Search(searchModel: SearchModel);
        SearchById(id: any);
        SearchBySpecificField(fieldName: string, typeOfFilter: string, value: any);
        ClearSelected();
        SelectedRow: Model;
        SelectedId: string;
        modelId: string;
    }

    export interface ITree<Model> extends IComponentBase<Model> {
        Search(searchTerm: string);
        SearchById(id: any);
        ClearSelected();
        SelectedRows: Array<Model>;
        SelectedRow: Model;
        SelectedId: Array<string>;
        modelId: string;
    }


    export interface IGridCrud<Model, SearchModel> extends IGrid<Model, SearchModel> {
        AddDeleteButton();
        AddInsertButton();
        AddEditButton();
        EditButtonOnSelecte(func: Function);
        DeleteButtonOnSelected(func: Function);
        InsertButtonOnSelected(func: Function);
        InsertRow(model: Model);
        EditSelectedRow(newItem: Model, index: number);
    }

    //export interface ITab {
    //    sectionSelected: string;
    //    tabSections: Array<labxand.components.core.TabSection>;
    //    defaultSection: string;
    //    SetActive(id: string): void;
    //    Bound(id: string): labxand.components.core.TabSection;
    //    SetDefaultSection(id: string): void;
    //    SetId(id: string): void;
    //    SetDisableAllTab(): void;
    //    SetDisableAllBut(id: string): void;
    //    SetEnableAllBut(id: string): void;
    //    SetDisableById(id: string): void;
    //    SetHiddenById(id: string): void;
    //    SetVisibleById(id: string): void;
    //    SetAfterSelectForAllTab(func: Function);
    //    SetAfterSelect(id: string, func: Function);
    //}

    export interface IPartialView {
        AfterLoaded: Function;
        isLoaded: boolean;
        loadAutomatically: boolean;
        LoadContent();
    }

    export interface IPartialViewWithController<TController> extends IPartialView {
        SetController(): void;
        ControllerNotAssigned(): boolean;
        controller: TController;
        CallFunction<U>(functionName: U);
        GetProperty<U>(propertyName: U);
        SetProperty<U>(propertyName: U, value: any);
    }

    export interface IPopup {
        Disable();
        Enable();
        AfterShow();
        Show();
        Close();
        AfterClose();
        AfterLoaded();
    }

    export interface ILookup<T extends nodak.models.ModelBase, SearchModel> extends IComponentBase<T>, IPopup {
        SetDisableCaption();
        SetEnableCaption();
        searchController:  nodak.core.ISearchControllerBase<T, SearchModel>;
       
    }
    export interface ITreeLookup<T extends nodak.models.ModelBase> extends IComponentBase<T>, IPopup {
        SetDisableCaption();
        SetEnableCaption();
        OnSelected();
        SetController();
        UpdateTextAndId();
        treeController: nodak.core.ITreeSearchControllerBase<T>;
    }

    export interface IModal extends IPopup {
        actionPanel: labxand.components.core.IActionPanel;
    }

    export interface IModalWithController<TController> extends IModal {
        controller: TController;
    }

    //export interface IModalScan extends IPopup {
    //    AfterSetScan(scan: nodak.common.models.BlobDescription): void;
    //    SetScan(): void;
    //    actionPanel: labxand.components.core.IActionPanel;
    //    scanTemp: nodak.common.models.BlobDescription;
    //}
}