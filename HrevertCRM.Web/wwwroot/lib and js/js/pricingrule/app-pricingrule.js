(function () {
    "use strict";
    angular.module("app-pricingrule", ['ui.router'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/pricingrule');

        $stateProvider
            .state('pricingrule',
            {
                url: '/pricingrule',
                templateUrl: '/templates/Rules/pricingrule.html',
                controller: 'pricingruleController',
                controllerAs: 'vm'

            })
    };


})();