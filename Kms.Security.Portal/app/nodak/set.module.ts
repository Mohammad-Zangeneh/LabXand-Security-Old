var comboDirectivesModule = angular.module('combo.directives', []);
var tabDirectivesModule = angular.module('tab.directives', []);
var componentsControllersModule = angular.module('components.controllers', []);
var appControllersModule = angular.module('nodak.controllers', ["common.controllers"]);
var appDirectivesModule = angular.module('nodak.directives', []);
var appServicesModule = angular.module('nodak.services', ['common.services']);
var appComponents = angular.module('nodak.components', []);
var aapp = angular.module('app', ['nodak.controllers', 'nodak.services', 'nodak.directives', 'angular-loading-bar', 'nodak.components']);
aapp.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode({
        enabled: false,
        requireBase: false
    });
}]);