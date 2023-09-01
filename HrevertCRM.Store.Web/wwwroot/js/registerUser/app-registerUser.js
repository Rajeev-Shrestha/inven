(function () {
    "use strict";
    angular.module("app-registerUser", ['common', 'ui.router', 'ngStorage','ui.bootstrap'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider, $qProvider) {
        $urlRouterProvider.otherwise('/registerUser');
        $qProvider.errorOnUnhandledRejections(false);
        $stateProvider
            .state('registerUser',
            {
                url: '/registerUser',
                templateUrl: '/templates/registerUser.html',
                controller: 'registerUserController',
                controllerAs: 'vm'

            });
        $locationProvider.html5Mode(true);
    };

})();