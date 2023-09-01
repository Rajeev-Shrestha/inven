﻿(function () {
    "use strict";
    angular.module("app-checkout", ['common', 'ui.router', 'ngStorage', 'ngCookies'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider,$httpProvider, $locationProvider, $qProvider) {
        $urlRouterProvider.otherwise('/checkout');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('checkout',
            {
                url: '/checkout',
                templateUrl: '/templates/checkout.html',
                controller: 'checkoutController',
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