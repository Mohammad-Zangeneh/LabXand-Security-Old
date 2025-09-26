var host = window.location.host;
module Base {
    export class Config {
        static AppRoot = "http://" + host;
        static ServiceRoot = "http://localhost:60615";
        static LoginPage = Config.AppRoot + "/login/index";
    }
}