(function () {
    'use strict';

    angular
        .module('app-itemMeasure')
        .service('itemMeasureService', itemMeasureService);

    itemMeasureService.$inject = ['$http'];

    function itemMeasureService($http) {
        var service = {};
        service.createItemMeasure = createItemMeasure;
       // service.getActiveMeasureItem = getActiveMeasureItem;
        service.getItemMeasureById = getItemMeasureById;
        service.deleteItemMeasure = deleteItemMeasure;
        service.updateItemMeasure = updateItemMeasure;
      //  service.getAllItemMeasure = getAllItemMeasure;
        service.activeItemMeasure = activeItemMeasure;
        service.getMeasureUnit = getMeasureUnit;
        service.getAllProduct = getAllProduct;
        service.checkIfProductExists = checkIfProductExists;
        service.searchTextForItemMeasure = searchTextForItemMeasure;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(itemMeasuresId) {
            return $http.post('/api/ItemMeasure/bulkDelete', itemMeasuresId).then(handleSuccess, handleError);
        }
        function checkIfProductExists(code) {
            return $http.get('/api/Product/CheckIfDeletedProducWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function createItemMeasure(item) {
            return $http.post('/api/ItemMeasure/createitemmeasure', item).then(handleSuccess, handleError);
        }

        function updateItemMeasure(item) {
            return $http.put('/api/ItemMeasure/updateitemmeasure', item).then(handleSuccess, handleError);
        }

        function deleteItemMeasure(id) {
            return $http.delete('/api/ItemMeasure/' + id).then(handleSuccess, handleError);
        }

        //function getActiveMeasureItem() {
        //    return $http.get('/api/ItemMeasure/getactiveItemMeasures').then(handleSuccess, handleError);
        //}

        function getItemMeasureById(id) {
            return $http.get('/api/ItemMeasure/getItemMeasurebyid/' + id).then(handleSuccess, handleError);
        }

        //function getAllItemMeasure() {
        //    return $http.get('/api/ItemMeasure/getallItemMeasures').then(handleSuccess, handleError);
        //}

        function activeItemMeasure(id) {
            return $http.get('/api/ItemMeasure/activateItemMeasure/' + id).then(handleSuccess, handleError);
        }

        function getMeasureUnit() {
            return $http.get('/api/MeasureUnit/searchMeasureUnits/'+ true + '/'+ null).then(handleSuccess, handleError);
        }

        function getAllProduct() {
            return $http.get('/api/Product/getallactiveproducts').then(handleSuccess, handleError);
        }
        function searchTextForItemMeasure(searchText, active) {
            return $http.get('/api/ItemMeasure/searchItemMeasures/' + active + '/' + searchText).then(handleSuccess, handleError);
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