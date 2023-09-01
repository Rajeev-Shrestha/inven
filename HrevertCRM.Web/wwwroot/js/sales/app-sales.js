(function () {
    "use strict";

    angular.module("app-sales", ['ui.router', 'ui.bootstrap.modal', 'bw.paging', 'ngSanitize'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/sales');

        $stateProvider
            .state('sales',
            {
                url: '/sales',
                templateUrl: '/templates/sales/sales.html',
                controller: 'salesController',
                controllerAs: 'vm'

            });


    };


})();