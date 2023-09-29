(function () {
    "use strict";
    
    angular.module("app-customer", ['ui.router', 'hrevertCrm', 'bw.paging', 'ngSanitize', 'ngCsv', 'ngCsvImport', 'common'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/customer');

        $stateProvider
            .state('customer',
            {
                url: '/customer',
                templateUrl: '/templates/customer/customers.html',
                controller: 'customersController',
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