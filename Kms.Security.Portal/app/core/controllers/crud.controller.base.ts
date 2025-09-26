module nodak.core {
    //interface for new T, Generic type
    export interface ParamConstructor<T> {
        new (object?): T;
    }

    export interface IParmeters {
        Id: string;
        V1: number;
        V2: number;
    }

    export interface IScopeBase extends ng.IScope {
        parameters: IParmeters;
    }

    export abstract class CrudControllerBase<T extends models.ModelBase, Dto>  {
        protected service: nodak.service.IServiceBase<Dto>;
        public currentEntity: T;
        protected userContext: nodak.models.User;
        protected mapper: nodak.models.IEntityMapper<T, Dto>;
        protected userCommunicator: nodak.models.UserCommunicator;
        protected pageType: nodak.enums.PageType;
        public messageHandler: nodak.models.MessageHandler;
        protected routeHandler: nodak.routing.RouteHandler;
        protected routeKey: nodak.routing.RouteKey;
        protected subSystemName: string;
        protected entityName: string;
        protected isInsertMode: boolean;
        protected primaryLoad: boolean;
        public hasPriorityErrorMessages: boolean;
        protected IsShowErrorMessageSummary: boolean;
        protected scope: IScopeBase;
        protected loadCompleted: boolean;
        eventName: string;
        protected $timeout: ng.ITimeoutService;
        protected messageBox: IMessageBox;
        protected rootScope: ng.IRootScopeService;
        protected $q: ng.IQService;
        public actionPanel: labxand.components.core.IActionPanel;

        constructor(injector: ng.auto.IInjectorService,
            scope: IScopeBase,
            service: nodak.service.IServiceBase<Dto>,
            mapper: nodak.models.IEntityMapper<T, Dto>,
            subSystem: nodak.enums.SubSystems,
            entityName: string,
            titleOfPage: string,
            hasService: boolean = true,
            primaryLoad = true) {


            this.scope = scope;

            this.$timeout = injector.get('$timeout');
            this.messageBox = injector.get('messageBox');
            this.rootScope = injector.get('$rootScope');
            this.$q = injector.get('$q');
            this.eventName = subSystem + entityName + "ControllerEvent";

            this.subSystemName = nodak.enums.SubSystems[subSystem];
            this.entityName = entityName;
            this.userCommunicator = new nodak.models.UserCommunicator(this.messageBox);
            this.messageHandler = new nodak.models.MessageHandler();
            // ؟خطاهای مربوط به دامین قبل از خطاهای کنترلر بررسی شوند
            //یعنی تا زمانی که خطاهای دامینی داریم خطاهای کنترلر را بررسی نمی کنیم -- برای افزایش کارایی
            this.hasPriorityErrorMessages = true;
            this.service = service;
            this.mapper = mapper;

            this.routeHandler = new nodak.routing.RouteHandler();
            this.routeKey = new nodak.routing.RouteKey();

            angular.element(document).ready(() => {
                this.rootScope.$broadcast(this.eventName);
            });

            angular.element(document).ready(() => {
                window.setTimeout(this.AfterDOMComplete(), 1000);
            });
            //disable if page have not any service

            //this.loadCompleted = true;
            //this.scope.$on('cfpLoadingBar:started', () => {
            //    this.loadCompleted = false;
            //    nodak.DOMManipulating.DisableScreen();
            //});
            // Loaded All Service
            if (hasService) {
                nodak.DOMManipulating.DisableScreen();

                this.scope.$on('cfpLoadingBar:completed', () => {
                    if (!this.loadCompleted) {
                        nodak.DOMManipulating.EnableScreen();
                        this.loadCompleted = true;
                        this.AfterEndAllServices();
                    }
                });
            }
            else
                this.loadCompleted = true;
            this.primaryLoad = primaryLoad;

            let splitUrlBySlash = window.location.href.split("/");
            let type = splitUrlBySlash[splitUrlBySlash.length - 1];
            let localStorageKey = this.subSystemName + this.entityName + type;

            this.isInsertMode = true;
            if (localStorage.getItem(localStorageKey) != null) {
                this.routeKey = JSON.parse(localStorage.getItem(localStorageKey));
                if (this.routeKey != null && this.routeKey.id != null)
                    this.isInsertMode = false;
            }

            if (this.primaryLoad) {
                if (this.scope.parameters)
                    if (this.scope.parameters.Id != null)
                        this.GetDetails(this.scope.parameters.Id);
            }

            this.actionPanel = new labxand.components.core.ActionPanelBase();
            this.actionPanel.panelTitle = titleOfPage;
            this.actionPanel.AddButton(entityName + "SaveButton").HasCaption("ذخیره").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("ذخیره")
                .SetOnClick(() => { this.Save(); });
        }


        GetParameterByName(name) {
            var url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        Save() {
            this.CanSave().then(() => {
                this.userCommunicator.RemoveAllErrorIcon();
                this.BeforSave().then(() => {
                    nodak.DOMManipulating.DisableScreen();
                    let temp = this.mapper.MapToDto(this.currentEntity)
                    return this.service.Post(temp, nodak.enums.ServiceTypeEnum.Save).then((response) => {
                        nodak.DOMManipulating.EnableScreen();
                        //console.log("response", response);
                        this.AfterSave();
                    }).catch((errorService) => {
                        //console.log("CrudControllerSave", errorService);
                        nodak.DOMManipulating.EnableScreen();
                        this.userCommunicator.messageBox.OkButton("خطا", "ذخیره با خطا مواجه گردید.").then(() => {
                            if (errorService.data.ExceptionMessage != null)
                                this.userCommunicator.messageBox.OkButton("خطا", errorService.data.ExceptionMessage)
                        });

                    });
                }).catch((errorBefor) => {

                    //console.log('error BeforSave', errorBefor);
                });

            }).catch((showErrorType) => {

                switch (showErrorType) {
                    case nodak.enums.ShowErrorType.All:
                        {
                            let allMessages: Array<nodak.models.Message> = angular.copy(this.messageHandler.GetErrorMessages());
                            allMessages.concat(this.currentEntity.messageHandler.GetErrorMessages());
                            this.userCommunicator.ShowErrorMessages(allMessages, this.IsShowErrorMessageSummary, this.pageType);
                        }
                        break;
                    case nodak.enums.ShowErrorType.Model:
                        {
                            this.userCommunicator.ShowErrorMessages(this.currentEntity.messageHandler.GetErrorMessages(),
                                this.IsShowErrorMessageSummary, this.pageType);
                        }
                        break;
                    case nodak.enums.ShowErrorType.UnKnown:
                        this.userCommunicator.messageBox.OkButton("خطا", ".عملیات ذخیره با خطا مواجه گردید");
                        break;
                }
            })
        }

        private CanSave(): ng.IPromise<any> {
            let deferred: ng.IDeferred<{}> = this.$q.defer();

            if (this.hasPriorityErrorMessages) {
                if (this.currentEntity.IsValid()) {
                    this.AddBussinessControllerValidation().then(() => {
                        if (this.messageHandler.GetErrorMessages().length == 0)
                            deferred.resolve();
                        else
                            deferred.reject(nodak.enums.ShowErrorType.All);
                    }).catch(() => { deferred.reject(nodak.enums.ShowErrorType.UnKnown); });
                }
                else {
                    deferred.reject(nodak.enums.ShowErrorType.Model);
                }
            }

            else {
                this.currentEntity.IsValid();
                this.AddBussinessControllerValidation().then(() => {
                    let mergedMessages: Array<nodak.models.Message> = angular.copy(this.currentEntity.messageHandler.GetErrorMessages());
                    mergedMessages.concat(this.messageHandler.GetErrorMessages());
                    if (mergedMessages.length == 0)
                        deferred.resolve();
                    else
                        deferred.reject(nodak.enums.ShowErrorType.All);

                }).catch(() => {
                    deferred.reject(nodak.enums.ShowErrorType.UnKnown);
                })
            }

            return deferred.promise;
        }

        //اگر نیازمند کارهایی قبل از متد گت دیتیلز(گرفتن دیتا) بودیم  باید پراپرتی ایزپرایمریلود را فالس کرده و متد  گت دیتیلز را دستی هندل کنیم
        //GetDetails override this
        //primaryLoad set to false
        GetDetails(id: string) {
            if (this.routeKey.id) {
                this.service.Post(id, nodak.enums.ServiceTypeEnum.GetWithId).then((response) => {
                    this.currentEntity = this.mapper.MapToEntity(response);
                    this.AfterGetDetails();
                }).catch((error) => {
                    this.userCommunicator.messageBox.OkButton("خطا", "متاسفانه دریافت اطلاعات با خطا مواجه گردید.لطفا دوباره تلاش نمایید")
                });
            }
        }

        // ******************************** Abstract Section *************************************
        abstract AfterGetDetails();
        abstract BeforSave(): ng.IPromise<any>;
        abstract AfterSave();

        //Contoller validation
        abstract AddBussinessControllerValidation(): ng.IPromise<any>;
        // ******************************** Abstract Section *************************************

        //چگونه بفهمیم دام لود شده است یاخیر؟ در دایرکتیو میتوانیم، اما در کنترلر چگونه بفهمیم؟
        AfterDOMComplete() {

        }

        AfterEndAllServices() {

        }
    }
}