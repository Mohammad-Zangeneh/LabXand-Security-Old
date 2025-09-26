module nodak.Tools {
    export enum UserStatus {

        RegisterRequest = 1,
        Active=2,
        Block=3,
        Disable = 4,
        DeclineRequest = 5
    }

    //interface IInstantiateErrorNotifier {

    //}
    //export class ObjectFactory {
    //    static createEntity<TEntity>(type: { new (): TEntity; }): TEntity {
    //        return new type();
    //    }
    //}
    export enum Subsystems {
        Common,
        EntranceExit,
        Communication,
        Environment
    }

    export class Helper {
        /** 
       *Attention!!! You can not instantiate from this class
       * @Error  Any entity will be accepted.
       */
        //constructor(error: IInstantiateErrorNotifier) {
        //    console.log("You can not instantiate from this class");
        //}

        /** 
        *Check If the property is True or False 
        * @field  if field is undefined or false then return false else return true
        */
        public static IsTrue(field: boolean) {
            if (field != undefined && field == true)
                return true;
            else {
                return false;
            }
        }

        public static MyElement(_HTMLInputElementId: string) {
            return angular.element(document.getElementById(_HTMLInputElementId));
        }
        public static MyHtmlInputElement(_HTMLInputElementId: string) {
            return <HTMLInputElement>(document.getElementById(_HTMLInputElementId));
        }

        /** 
    *Check If the property is Empty or not 
    * @inputData  input string 
    */
        public static IsEmpty(inputData: string): boolean
        /** 
    *Check If the property is Empty or not 
    * @inputData  input number
    */
        public static IsEmpty(inputData: number): boolean
        /** 
    *Check If the property is Empty or not 
    * @inputData  if object == undefined then return false else return true
    */
        public static IsEmpty(inputData: Object): boolean


        public static IsEmpty(inputData: any) {

            if (inputData == undefined) {

                return true;
            }
            else if ((typeof inputData).toLocaleLowerCase() == 'object') {
                return false;
            }
            else if (typeof inputData == 'string') {
                if (inputData.trim().length > 0)
                    return false;
                return true;
            }
            else if (typeof inputData == 'number') {
                if (inputData > 0)
                    return false;
                return true;

            }
            else {
                throw new Error("input type is invalid");
            }

        }
        /** 
   *Check If Compare Date is between the From and To Date 
   * @compareDate  Date which will be compared.
   * @fromDate  start comparision Date.
   * @toDate  end comparision Date.
   */
        public static IsBetweenDate(compareDate: Date, fromDate: Date, toDate: Date) {
            if (compareDate >= fromDate && compareDate <= toDate)
                return true;
            return false;
        }

        public static GoToPath(page: string, subSystem: Subsystems = Subsystems.EntranceExit, openInNewPage: boolean = true) {

            let SubsystemName = '/' + Subsystems[subSystem] + '/';
            if (openInNewPage)
                window.open(Base.Config.AppRoot + SubsystemName + page);
            else
                window.location.href = Base.Config.AppRoot + SubsystemName + page;

        }


        //public static GoToPath(page: string, subSystemName: string = '/EntranceExit/', openInNewPage: boolean = true) {
        //    if (openInNewPage)
        //        window.open(Base.Config.AppRoot + subSystemName + page);
        //    else
        //        window.location.href = Base.Config.AppRoot + subSystemName + page;

        //}

        public static GetQueryStringValue(parameterName: string) {
            return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(parameterName).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
        }

    }


}

