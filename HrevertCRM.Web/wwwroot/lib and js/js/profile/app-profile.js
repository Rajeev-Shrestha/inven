(function () {
    "use strict";

    angular.module("app-profile", ['ui.router', 'ui.bootstrap.modal', 'bw.paging', 'ngSanitize'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes]);

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/profile');

        $stateProvider
            .state('profile',
            {
                url: '/profile',
                templateUrl: '/templates/profile/profile.html',
                controller: 'profileController',
                controllerAs: 'vm'

            });


    };


})();