(function () {
    "use strict";

    angular.module("app-purchase", ['ui.router', 'ui.bootstrap.modal', 'bw.paging', 'ngSanitize'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/purchase');

        $stateProvider
            .state('purchase',
            {
                url: '/purchase',
                templateUrl: '/templates/purchase/purchase.html',
                controller: 'purchaseController',
                controllerAs: 'vm'

            });


    };


})();