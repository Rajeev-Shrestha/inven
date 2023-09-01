(function () {
    "use strict";
    angular.module("app-items", ['common', 'ui.router', 'ui.bootstrap', 'ngStorage', 'ngCookies', 'ngSanitize', 'wipImageZoom'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider','$qProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider,$qProvider) {
        $urlRouterProvider.otherwise('/items');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('itemDetail',
            {
                url: '/items/:id',
                templateUrl: '/templates/itemDetail.html',
                controller: 'itemDetailController',
                controllerAs: 'vm'

            })
           .state('items',
           {
               url: '/items',
               templateUrl: '/templates/allitem.html',
               controller: 'allitemController',
               controllerAs: 'vm'

           });
        $locationProvider.html5Mode(true);
    };


})();