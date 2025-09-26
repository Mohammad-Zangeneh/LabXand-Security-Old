module nodak.service {
    class IdClass {
        Id: string;
        constructor(id) { this.Id = id; }
    }
    class Redirect{
       static GetParameterByName(name) {
            var url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

       static Loginpage() {
           let url = Redirect.GetParameterByName("redirectPath");
           let r = Base.Config.AppRoot + "/login";
           if (url != null)
               r +="?redirectPath="+ url;
           window.location.href = r;
       }
       static checkError(error) {
           if (error.status == 401) {
               Redirect.Loginpage();
               
           }
           else if (error.status == 403)  {
               let temp = angular.copy(toastr.options);
               toastr.options = {
                   "closeButton": true,
                   "debug": false,
                   "newestOnTop": false,
                   "progressBar": false,
                   "positionClass": "toast-top-left",
                   "preventDuplicates": true,
                   "onclick": null,
                   "showDuration": "0",
                   "hideDuration": "1000",
                   "timeOut": 0,
                   "extendedTimeOut": 0,
                   "showEasing": "swing",
                   "hideEasing": "linear",
                   "showMethod": "fadeIn",
                   "hideMethod": "fadeOut",
                   "tapToDismiss": false
               }
               toastr.warning("شما دسترسی انجام بعضی از عملیات را ندارید");
               toastr.options = temp;
           }
       }
       static HomePage() {
           
           let url = Redirect.GetParameterByName("redirectPath");
           let r = Base.Config.AppRoot ;
           if (url != null)
               r = url;
           window.location.href = r;
           
        }
    }

    export interface IServiceBase<T> {
        GetArray(serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Array<T>>;
        PostArray(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Array<T>>;
        Get(serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<T>;
        Post(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<T>;
        PostLogin(model: any);
        BooleanPost(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Boolean>;
      //  GeneralPost(model: any, url: string): any;
    }



    export class modelServiceTest {
        Id: string;
    }

    export class ServiceBase<DTO> implements IServiceBase<DTO> {

        protected url: string;
        protected httpService: ng.IHttpService;
        protected configGet: ng.IRequestConfig;
        protected configPost: ng.IRequestConfig;
        private token: string;
        private serviceType: nodak.enums.ServiceTypeEnum;
        static $inject = ['$http'];

        constructor($http: ng.IHttpService, url: string) {
            this.httpService = $http;
            this.url = url;
            this.token = localStorage.getItem("authenticationToken");
        }

      


        GetArray(serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Array<DTO>> {
            let getUrl = this.url + '/' + nodak.enums.ServiceTypeEnum[serviceType];
            this.configGet = {
                method: 'GET',
                url: getUrl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer ' + this.token
                }
            };

            return this.httpService(this.configGet).then((response) => {
                /*console.log('Model Get From Url: ' + this.url + ' ');*/ /*console.log(response.data);*/

                return response.data
            }).catch((error) => {
                 /*console.log('Error While Loading Service: ' + getUrl); console.log(error);*/
                Redirect.checkError(error);
                return [];
            });

        }

        Get(serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<DTO> {
            let getUrl = this.url + '/' + nodak.enums.ServiceTypeEnum[serviceType];
            this.configGet = {
                method: 'GET',
                url: getUrl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer '+ this.token
                }
            };
            return this.httpService(this.configGet).then((response) => { return <DTO>response.data }).catch((error) => {
                Redirect.checkError(error);
                return <DTO>error;
            });
        }
        PostLogin(model: any){
            nodak.DOMManipulating.DisableScreen();
            localStorage.clear();
            $.ajax({
                url: Base.Config.ServiceRoot + '/login',
                data: {
                    username: model.userName,
                    password: model.password,
                    grant_type: "password",
                    "my-very-special-key1": "value1" // how to send additional parameters
                },
                type: 'POST', // POST `form encoded` data
                contentType: 'application/x-www-form-urlencoded'
            }).then(function (response) {
                let jwtToken = response.access_token;
                let refreshToken = response.refresh_token;
                localStorage.setItem("authenticationToken", jwtToken);
                var user: nodak.common.models.Member = JSON.parse( response.UserInfo);
                localStorage.setItem("User", response.UserInfo);
                localStorage.setItem("Permissions", response.Permissions);


                nodak.DOMManipulating.EnableScreen();
                Redirect.HomePage();
                }, function ( error) {
                    //var response = xhr.responseText;
                    //console.log("Morsa", error);
                    alert(JSON.stringify(error.responseJSON.error, null, ' '));
               // alert("نام کاربری یا رمز ورود اشتباه است یا کاربر غیر فعال است");
                nodak.DOMManipulating.EnableScreen();
                });


        }


        Post(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<DTO> {
            let postUurl = this.url + '/' + nodak.enums.ServiceTypeEnum[serviceType];
            if (serviceType == nodak.enums.ServiceTypeEnum.GetWithId)
                model = new IdClass(model);
            this.configPost = {
                method: 'POST',
                url: postUurl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer ' + this.token

                },
                data: model
            };
            nodak.DOMManipulating.DisableScreen();
            return this.httpService(this.configPost).then((response) => {
                nodak.DOMManipulating.EnableScreen();
                return <DTO>response.data;
            }).catch((error) => {
                Redirect.checkError(error);
                nodak.DOMManipulating.EnableScreen();
                //console.log("error", error);
                throw  <DTO>error;
                });
        }

        PostArray(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Array<DTO>> {
            let postUurl = this.url + '/' + nodak.enums.ServiceTypeEnum[serviceType];
            if (serviceType == nodak.enums.ServiceTypeEnum.GetWithId)
                model = new IdClass(model);
            this.configPost = {
                method: 'POST',
                url: postUurl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer ' + this.token
                },
                data: model
            };

            return this.httpService(this.configPost).then((response) => { /*console.log(response.data);*/ return response.data; })
                .catch((error) => {
                    Redirect.checkError(error);
                    return error;
                });
        }

         GeneralPost(model: any, url?: string): ng.IPromise<Array<DTO>> {

            let postUrl = this.url;
            if (url != null)
                postUrl = url;
            this.configPost = {
                method: 'POST',
                url: postUrl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer ' + this.token
                },
                data: model
            };

            return this.httpService(this.configPost).then((response) => { /*console.log(response.data);*/ return response.data; })
                .catch((error) => {
                    Redirect.checkError(error);
                    
                    throw <DTO>error;
                });
        }

        BooleanPost(model: any, serviceType: nodak.enums.ServiceTypeEnum): ng.IPromise<Boolean> {
            let postUurl = this.url + '/' + nodak.enums.ServiceTypeEnum[serviceType];
            if (serviceType == nodak.enums.ServiceTypeEnum.GetWithId)
                model = new IdClass(model);
            this.configPost = {
                method: 'POST',
                url: postUurl,
                headers: {
                    "Content-Type": "Application/Json",
                    "dataType": "json",
                    //'Authorization': 'Bearer ' + this.token
                },
                data: model
            };
            return this.httpService(this.configPost).then((response) => { /*console.log(response.data);*/ return <Boolean>response.data; })
                .catch((error) => {
                    Redirect.checkError(error);
                    throw <Boolean>error;
                });
        }
    }

}