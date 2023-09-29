(function () {
    'use strict';

    angular
        .module('app-retailer')
        .service('retailerService', retailerService);

    retailerService.$inject = ['$http'];

    function retailerService($http) {
        var service = {};
        service.getAllCompany = getAllCompany;
        service.getRetailersById = getRetailersById;
        service.searchRetailer = searchRetailer;
        service.assignRetailer = assignRetailer;
        return service;

        function assignRetailer(id) {
            return $http.post('/api/Retailer/SaveRetailers', id).then(handleSuccess, handleError);
        }
        function searchRetailer(text) {
            return $http.get('/api/Company/searchactivecompanies/' + text).then(handleSuccess, handleError);
        }
        function getRetailersById(id) {
            return $http.post('/api/Retailer/GetRetailers/' + id).then(handleSuccess, handleError);
        }
        function getAllCompany() {
            return $http.get('/api/Company/getactivecompanies').then(handleSuccess, handleError);
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