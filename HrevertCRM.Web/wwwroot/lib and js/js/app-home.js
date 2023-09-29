(function () {
    "use strict";
    angular.module("app-home", ['ui.router', 'hrevertCrm', 'ui.bootstrap', 'ngMaterial', 'ngImgCrop'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);
    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/home');

        $stateProvider
            .state('home',
            {
                url: '/',
                templateUrl: '/templates/home.html',
                controller: 'homeController',
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