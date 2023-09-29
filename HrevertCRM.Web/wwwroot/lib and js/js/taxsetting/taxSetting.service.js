(function () {
    'use strict';

    angular
        .module('app-taxSetting')
        .service('taxSettingService', taxSettingService);

    taxSettingService.$inject = ['$http'];

    function taxSettingService($http) {
        var service = {};
        service.getAllTaxes = getAllTaxes;
        service.checkIfTaxCodeExists = checkIfTaxCodeExists;
        service.createTax = createTax;
        service.getById = getById;
        service.updateTax = updateTax;
        service.deleteTax = deleteTax;
        service.searchTextForTaxes = searchTextForTaxes;
        service.actiateTax = actiateTax;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/tax/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function getAllTaxes() {
            return $http.get('/api/tax/getallactivetaxes').then(handleSuccess, handleError);
        }
        function checkIfTaxCodeExists(code) {
            return $http.get('/api/tax/CheckIfDeletedTaxWithSameTaxCodeExists/' + code).then(handleSuccess, handleError);
        }
        function createTax(tax) {
            return $http.post('/api/tax/createTax/', tax).then(handleSuccess, handleError);
        }
        function updateTax(tax) {
            return $http.put('/api/tax/updateTax/', tax).then(handleSuccess, handleError);
        }
        function deleteTax(id) {
            return $http.delete('/api/tax/' + id).then(handleSuccess, handleError);
        }
        function getById(id) {
            return $http.get('/api/tax/getTax/' + id).then(handleSuccess, handleError);
        }

        function searchText(pageIndex, pageSize, text, checked) {
            if (checked === false || checked === 'false') {
                return $http.get('/api/customer/searchactivecustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
            } else {
                return $http.get('/api/customer/searchallcustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
            }
        }
        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        function searchTextForTaxes(searchText, active) {
            var api = '/api/tax/searchTaxes/' + active + '/' + searchText;
            return $http.get(api).then(handleSuccess, handleError);
        }
        function actiateTax(id) {
            return $http.get('/api/tax/activateTax/' + id).then(handleSuccess, handleError);
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