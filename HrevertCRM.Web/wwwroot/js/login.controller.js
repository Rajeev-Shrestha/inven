(function () {
    'use strict';

    angular
        .module('hrevertCrm')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope', '$http', '$state', 'authService', '$location', '$window'];

    function loginController($scope, $http, $state, authService, $location, $window) {
        var vm = this;

        vm.loginInfo = {
            UserName: "admin@hrevert.com",
            Password: "p@77w0rd"
        };

        vm.login = function () {
            authService.login(vm.loginInfo, function (response) {
                if (response === "OK") {
                    authService.authenticate();
                    $window.open( $location.search().returnState, '_self');
                   // $state.go('home');
                }
            });
        }
    }
})();