(function () {
    'use strict';

    angular
        .module('app-zoneSetting')
        .service('zoneSettingService', zoneSettingService);

    zoneSettingService.$inject = ['$http'];

    function zoneSettingService($http) {
        var service = {};
        service.createZone = createZone;
        service.getActiveZone = getActiveZone;
        service.deleteZone = deleteZone;
        service.getZoneById = getZoneById;
        service.updateZone = updateZone;
        service.getAllZone = getAllZone;
        service.activeZone = activeZone;
        service.checkIfDeliveryZoneCodeExists = checkIfDeliveryZoneCodeExists;
        service.searchTextForZoneSetting = searchTextForZoneSetting;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/DeliveryZone/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        function searchTextForZoneSetting(searchText, active) {
            return $http.get('/api/DeliveryZone/searchDeliveryZones/' + active + '/' + searchText).then(handleSuccess, handleError);
        }
        function checkIfDeliveryZoneCodeExists(code) {
            return $http.get('/api/DeliveryZone/CheckIfDeletedDeliveryZoneWithSameCodeExists/'+code).then(handleSuccess, handleError);
        }

        function updateZone(zone) {
            return $http.put('/api/DeliveryZone/updatezone', zone).then(handleSuccess, handleError);
        }

        function getZoneById(id) {
            return $http.get('/api/DeliveryZone/getDeliveryZonebyid/' + id).then(handleSuccess, handleError);
        }

        function deleteZone(id) {
            return $http.delete('/api/DeliveryZone/' + id).then(handleSuccess, handleError);
        }

        function getActiveZone() {
            return $http.get('/api/deliveryZone/searchDeliveryZones/' + true + '/' + "null").then(handleSuccess, handleError);
        }

        function createZone(zone) {
            return $http.post('/api/DeliveryZone/createzone', zone).then(handleSuccess, handleError);
        }

        function activeZone(id) {
            return $http.get('/api/DeliveryZone/activateDeliveryZone/' + id).then(handleSuccess, handleError);
        }

        function getAllZone() {
            return $http.get('/api/DeliveryZone/getallDeliveryZones').then(handleSuccess, handleError);
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