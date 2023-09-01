(function () {
    "use strict";
    angular.module("hrevertCrm", ['ui.router', 'ngStorage'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', configRoutes])
    .directive('pageSize', getPageSize);

    function getPageSize() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                sizes: "=",
                bindTo: '=',
                pageChange: '&'
            },
            templateUrl: '/templates/directives/pageSizeDirective.html',
            link: function (scope, element, attrs) {
                scope.sizes = scope.sizes;
            }
        }
    };

   // angular.module("hrevertCrm").run(configBlock);

    //configBlock.$inject = ['$rootScope', '$templateCache', '$state', 'authService'];
    //angular.module("hrevertCrm")
    //    .factory('configFactory',
    //        function($rootScope, $templateCache, $state, authService) {
    //            return {
    //                config: function() {

    //                    configBlock($rootScope, $templateCache, $state, authService);
    //                }
    //            };

    //});
    //function configBlock($rootScope, $templateCache, $state, authService) {
    //    $rootScope.$on('$viewContentLoaded', function () {
    //        $templateCache.removeAll();
    //    });

    //    $rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
    //        var isLogin = toState.name === "login";
    //        var athenticated = authService.isAuthenticated();
    //        if (isLogin) {
    //            if (athenticated) {
    //                e.preventDefault();
    //                $state.go(fromState.name);
    //            } else {
    //                return;
    //            }
    //        }

    //        if (!athenticated) {
    //            // stop current execution
    //            e.preventDefault();

    //            // go to login
    //            var redirectUrl = $state.href(toState.name, toParams);

    //            $state.go('login', { returnState: redirectUrl });
    //        }
    //    });
    //}

    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider) {
        $urlRouterProvider.otherwise('/login');

        $stateProvider
         .state('login',
         {
             url: '/login?returnUrl=:returnState',
             templateUrl: '/templates/login.html',
             controller: 'loginController',
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