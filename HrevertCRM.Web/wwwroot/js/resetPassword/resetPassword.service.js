(function () {
    'use strict';

    angular
        .module('app-resetPassword')
        .service('resetPasswordService', resetPasswordService);

    resetPasswordService.$inject = ['$http'];

    function resetPasswordService($http) {

        var service = {};

        return service;


        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();