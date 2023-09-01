(function () {
    "use strict";
    angular.module("app-taskManager", ['ui.router', 'ngStorage', 'ui.bootstrap.modal', 'bw.paging', 'common', 'ui.bootstrap.datetimepicker'])
        .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);
    //'angularjs-datetime-picker',
    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/taskmanager');

        $stateProvider
            .state('taskmanager', 
                {
                    url: '/taskmanager',
                    templateUrl: '/templates/taskmanager/taskmanager.html',
                    controller: 'taskManagerController',
                    controllerAs: 'vm'

                });
       // $locationProvider.html5Mode(true);
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