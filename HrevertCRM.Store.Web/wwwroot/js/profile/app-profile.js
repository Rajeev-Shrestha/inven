(function () {
    "use strict";
    angular.module("app-profile", ['ui.router', 'ngStorage',  'ui.bootstrap', 'ngCookies'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes]);


    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider, $qProvider) {
        $urlRouterProvider.otherwise('/profile');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('profile',
        {
            url: '/profile',
                templateUrl: 'templates/profile.html',
                controller: 'profileController',
                controllerAs: 'vm'

            });
        $locationProvider.html5Mode(true);
        };
    
        }) ();