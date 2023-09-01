(function () {
    'use strict';

    angular
        .module('app-compare')
        .service('compareService', compareService);

    compareService.$inject = ['$http'];

    function compareService($http) {
        var service = {};
        service.productById = productById;
        return service;

        function productById(id) {
            var apiUrl = '/api/Product/GetProduct/' + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);

        }
        // private functions

        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();