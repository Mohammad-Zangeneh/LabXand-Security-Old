module nodak.core {
    export interface ISearchControllerBase<T extends models.ModelBase, SearchModel> {
        searchEntity: SearchModel;
        userContext: nodak.models.User;
        userCommunicator: nodak.models.UserCommunicator;
        pageType: nodak.enums.PageType;
        messageHandler: nodak.models.MessageHandler;
        routeHandler: nodak.routing.RouteHandler;
        routeKey: nodak.routing.RouteKey;
        subSystemName: string;
        entityName: string;
        primaryLoad: boolean;
        gridEntity: labxand.components.IGrid<T, SearchModel>;
        Search(): void;
        NewSearch(): void;
    }
    export abstract class SearchControllerBase<T extends models.ModelBase, SearchModel> implements ISearchControllerBase<T, SearchModel> {
        searchEntity: SearchModel;
        userContext: nodak.models.User;
        userCommunicator: nodak.models.UserCommunicator;
        pageType: nodak.enums.PageType;
        messageHandler: nodak.models.MessageHandler;
        routeHandler: nodak.routing.RouteHandler;
        routeKey: nodak.routing.RouteKey;
        subSystemName: string;
        entityName: string;
        primaryLoad: boolean;
        gridEntity: labxand.components.IGrid<T, SearchModel>;
        eventName: string;
        protected rootScope: ng.IRootScopeService;
        protected messageBox: IMessageBox;
        protected scope: ng.IScope;

        constructor(injector: ng.auto.IInjectorService,
            entityName: string,
            // gridEntity: labxand.components.IGrid<T, SearchModel>,
            subSystem: nodak.enums.SubSystems,
            primaryLoad = true) {
            this.rootScope = injector.get('$rootScope');
            this.messageBox = injector.get('messageBox');
            //this.scope = injector.get('$scope');
            this.eventName = subSystem + entityName + "SControllerEvent";
            this.subSystemName = nodak.enums.SubSystems[subSystem];
            this.entityName = entityName;
            this.userCommunicator = new nodak.models.UserCommunicator(this.messageBox);
            this.messageHandler = new nodak.models.MessageHandler();

            this.routeHandler = new nodak.routing.RouteHandler();
            this.routeKey = new nodak.routing.RouteKey();
            this.primaryLoad = primaryLoad;

            let splitUrlBySlash = window.location.href.split("/");
            let type = splitUrlBySlash[splitUrlBySlash.length - 1];

            let localStorageKey = this.subSystemName + this.entityName + type;

            if (localStorage.getItem(localStorageKey) != null) {
                this.routeKey = JSON.parse(localStorage.getItem(localStorageKey));
            }

            angular.element(document).ready(() => {
                this.rootScope.$broadcast(this.eventName);
            });
            //this.scope.$on('cfpLoadingBar:completed', () => {
            //    if (!this.loadCompleted) {
            //        nodak.DOMManipulating.EnableScreen();
            //        this.loadCompleted = true;
            //        this.AfterEndAllServices();
            //    }

            //});
        }

        Search() {
            this.gridEntity.Search(this.searchEntity);
        }

        abstract NewSearch();
    }
}