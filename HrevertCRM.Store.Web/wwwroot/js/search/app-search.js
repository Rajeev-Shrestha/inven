(function () {
    "use strict";
    angular.module("app-search", ['common','ui.router', 'ngStorage', 'ngCookies'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider, $qProvider) {
        $urlRouterProvider.otherwise('/search');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('search',
            {
                url: '/search',
                templateUrl: '/templates/search.html',
                controller: 'searchController',
                controllerAs: 'vm'
            });
        $locationProvider.html5Mode(true);
    };

})();