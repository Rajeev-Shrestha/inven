(function () {
    "use strict";

    angular.module("app-salesOpportunities", ['ui.router', 'ui.bootstrap.modal', 'bw.paging', 'ngSanitize', 'ui.bootstrap.datetimepicker', 'angular-sortable-view'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/salesOpportunities');

        $stateProvider
            .state('salesOpportunities',
            {
                url: '/salesOpportunities',
                templateUrl: '/templates/salesOpportunities/salesOpportunities.html',
                controller: 'salesOpportunitiesController',
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