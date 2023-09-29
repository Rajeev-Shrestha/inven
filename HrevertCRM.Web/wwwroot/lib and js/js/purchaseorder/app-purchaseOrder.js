(function () {
    "use strict";
    angular.module("app-purchaseOrder", ['ui.router', 'ngStorage', 'angular-sortable-view', 'bw.paging', 'ui.bootstrap.datetimepicker', 'ui.dateTimeInput'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/purchaseOrder');

        $stateProvider
            .state('purchaseOrder',
            {
                url: '/purchaseOrder',
                templateUrl: '/templates/purchaseOrder/purchaseOrder.html',
                controller: 'purchaseOrderController',
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