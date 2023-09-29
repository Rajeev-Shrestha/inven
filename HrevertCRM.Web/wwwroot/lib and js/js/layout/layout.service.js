(function () {
    'use strict';

    angular
        .module('app-layout')
        .service('layoutService', layoutService);

    layoutService.$inject = ['$http'];

    function layoutService($http) {

        var service = {};
        service.checkLogin = checkLogin;
        service.getCompanyLogo = getCompanyLogo;
        service.getLoggedinUserDetails = getLoggedinUserDetails;
        return service;
        function getCompanyLogo() {
            return $http.get('/api/CompanyLogo/getactivecompanylogos/').then(handleSuccess, handleError);
        }
        function checkLogin() {
            return $http.get('/api/user/CheckFirstLogin/').then(handleSuccess, handleError);
        }
        function getLoggedinUserDetails() {
            return $http.get('/api/user/GetLoggedInUserDetail/').then(handleSuccess, handleError);
        }
        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();