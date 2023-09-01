(function () {
    "use strict";
    angular.module("app-placeorder", ['common', 'ui.router', 'ngCookies', 'app-layout'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider, $qProvider) {
        $urlRouterProvider.otherwise('/placeorder');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('placeorder',
            {
                url: '/placeorder',
                templateUrl: '/templates/placeorder.html',
                controller: 'placeorderController',
                controllerAs: 'vm'

            });
        $locationProvider.html5Mode(true);
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