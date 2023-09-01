(function () {
    'use strict';

    angular
        .module('hrevertCrm')
        .factory('authService', authService);

    authService.$inject = ['$http', '$sessionStorage'];

    function authService($http, $sessionStorage) {
        var service = {};

        service.login = login;
        service.authenticate = authenticate;
        service.clear = clear;
        service.isAuthenticated = isAuthenticated;

        return service;

        function login(userInfo, callback) {
            $http.post('/api/user/login', JSON.stringify(userInfo))
                .success(function (response) {
                    callback(response);
                });
        }

        function authenticate() {
            $sessionStorage.isAuthenticated = true;
        }

        function clear() {
            $sessionStorage.isAuthenticated = false;
        }

        function isAuthenticated() {
            if ($sessionStorage.isAuthenticated) {
                return true;
            }
            return false;
        }
    }
})();