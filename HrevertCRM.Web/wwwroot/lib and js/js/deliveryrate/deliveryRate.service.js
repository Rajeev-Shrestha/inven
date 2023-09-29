(function () {
    'use strict';

    angular
        .module('app-deliveryRate')
        .service('deliveryRateService', deliveryRateService);

    deliveryRateService.$inject = ['$http'];

    function deliveryRateService($http) {
        var service = {};
        service.getAllDeliveryRate = getAllDeliveryRate;
        service.getDeliveryRateById = getDeliveryRateById;
        service.deleteDeliveryRate = deleteDeliveryRate;
        service.createDeliveryRate = createDeliveryRate;
        service.updateDeliveryRate = updateDeliveryRate;
        service.getAllProductCategory = getAllProductCategory;
        service.includeInactiveRate = includeInactiveRate;
        service.activeDeliveryRate = activeDeliveryRate;
        service.getAllProduct = getAllProduct;
        service.getActiveZone = getActiveZone;
        service.getAllDelivery = getAllDelivery;
        service.getMeasureUnit = getMeasureUnit;
        service.searchTextForDeliveryRate = searchTextForDeliveryRate;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/DeliveryRate/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function searchTextForDeliveryRate(searchText, active) {
            return $http.get('/api/DeliveryRate/searchDeliveryRates/' + active + '/' + searchText).then(handleSuccess, handleError);
            
        }

        function getAllProductCategory() {
            return $http.get('/api/ProductCategory/getallactivecategories').then(handleSuccess, handleError);
        }

        function includeInactiveRate() {
            return $http.get('/api/DeliveryRate/searchDeliveryRates').then(handleSuccess, handleError);
        }

        function activeDeliveryRate(id) {
            return $http.get('/api/DeliveryRate/activateDeliveryRate/' + id).then(handleSuccess, handleError);
        }

        function updateDeliveryRate(delivery) {
            return $http.put('/api/DeliveryRate/updatedeliveryrate', delivery).then(handleSuccess, handleError);
        }

        function createDeliveryRate(delivery) {
            return $http.post('/api/DeliveryRate/createdeliveryrate', delivery).then(handleSuccess, handleError);
        }

        function deleteDeliveryRate(id) {
            return $http.delete('/api/DeliveryRate/' + id).then(handleSuccess, handleError);
        }

        function getAllDeliveryRate() {
            return $http.get('/api/DeliveryRate/searchDeliveryRates/' + true + ' / ' + "null").then(handleSuccess, handleError);
        }

        function getDeliveryRateById(id) {
            return $http.get('/api/DeliveryRate/getDeliveryRatebyid/' + id).then(handleSuccess, handleError);
        }

        function getAllProduct() {
            return $http.get('/api/Product/getallactiveproducts').then(handleSuccess, handleError);
        }

        function getActiveZone() {
            return $http.get('/api/DeliveryZone/searchDeliveryZones/' + true + ' / ' + "null").then(handleSuccess, handleError);
        }

        function getAllDelivery() {
            return $http.get('/api/DeliveryMethod/searchDeliveryMethods/' + true + ' / ' + "null").then(handleSuccess, handleError);
        }

        function getMeasureUnit() {
            return $http.get('/api/MeasureUnit/searchMeasureUnits/'+true+'/'+"null").then(handleSuccess, handleError);
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