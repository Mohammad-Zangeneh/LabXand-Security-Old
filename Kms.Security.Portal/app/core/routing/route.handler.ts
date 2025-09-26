module nodak.routing {

    export class RouteKey {
        public id: string;
        //public fullId: string;
        public v1: number;
        public v2: number;
        public dictionary: Array<nodak.models.KeyValuePair<string, any>>;

        constructor(id?, fullId?, v1?, v2?) {
            this.id = id;
            //this.fullId = fullId;
            this.v1 = v1;
            this.v2 = v2;

            this.dictionary = new Array<nodak.models.KeyValuePair<any, any>>();
        }

        AddKeyValue(key, value) {
            let keyValuePair = new nodak.models.KeyValuePair<any, any>(key, value);
            this.dictionary.push(keyValuePair);//setId()
        }

        AddDictionary(dictionary: Array<nodak.models.KeyValuePair<any, any>>) {
            dictionary.forEach((item) => {
                this.dictionary.push(item);
            });
        }

        GetValueFromDicByKey(key: string) {
            let result = this.dictionary.filter(t => t.Key == key);
            return result[0];
        }
    }

    export class RouteHandler {

        constructor() {

        }

        static RedirectToPage(subSystem: nodak.enums.SubSystems, entityName: string, typeOfEntityPage: nodak.enums.TypeOfEntityPage,
            routeKey?: RouteKey) {
            let localStorageKey = nodak.enums.SubSystems[subSystem] + entityName + nodak.enums.TypeOfEntityPage[typeOfEntityPage];

            if (localStorage.getItem(localStorageKey) != null)
                localStorage.removeItem(localStorageKey);

            localStorage.setItem(localStorageKey, JSON.stringify(routeKey));

            let controllerName = entityName;
            let actionName = nodak.enums.TypeOfEntityPage[typeOfEntityPage];


            if (typeOfEntityPage == nodak.enums.TypeOfEntityPage.Insert)
                actionName = nodak.enums.TypeOfEntityPage[nodak.enums.TypeOfEntityPage.Edit];

            let url = Base.Config.AppRoot + '/' + controllerName + '/Index/' + actionName;
            window.location.href = url;
        }

        static RedirectToDisplayPage(subSystem: nodak.enums.SubSystems, entityName: string, routeKey?: RouteKey) {
            this.RedirectToPage(subSystem, entityName, nodak.enums.TypeOfEntityPage.Display, routeKey);
        }

        static RedirectToListPage(subSystem: nodak.enums.SubSystems, entityName: string, routeKey?: RouteKey) {
            this.RedirectToPage(subSystem, entityName, nodak.enums.TypeOfEntityPage.List, routeKey);
        }

        static RedirectToEditPage(subSystem: nodak.enums.SubSystems, entityName: string, routeKey?: RouteKey) {
            this.RedirectToPage(subSystem, entityName, nodak.enums.TypeOfEntityPage.Edit, routeKey);
        }

        static RedirectToController() {
        }
    }
}   