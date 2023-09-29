(function () {
    "use strict";
    angular.module("app-taxSetting", ['ui.router'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', configRoutes]);
    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider) {
        $urlRouterProvider.otherwise('/taxSetting');

        $stateProvider
            .state('taxSetting',
            {
                url: '/taxSetting',
                templateUrl: '/templates/taxSetting/taxSetting.html',
                controller: 'taxSettingController',
                controllerAs: 'vm'

            });

       
        configureHttp($httpProvider);
    };

    function configureHttp($httpProvider) {
        //initialize get if not there
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }

        //disable IE ajax request caching
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
        // extra
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    };


})();