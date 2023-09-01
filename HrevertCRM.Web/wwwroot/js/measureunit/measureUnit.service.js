(function () {
    'use strict';

    angular
        .module('app-measureUnit')
        .service('measureUnitService', measureUnitService);

    measureUnitService.$inject = ['$http'];

    function measureUnitService($http) {
        var service = {};
        service.measureUnitById = measureUnitById;
        service.deleteMeasureUnit = deleteMeasureUnit;
        service.updateMeaureUnit = updateMeaureUnit;
        service.createMeasureUnit = createMeasureUnit;
      //  service.getMeasureUnit = getMeasureUnit;
       // service.getInactiveMeasureUnit = getInactiveMeasureUnit;
        service.activeMeasureUnit = activeMeasureUnit;
        service.getEntryMethodTypes = getEntryMethodTypes;
        service.checkIfMeasureCodeExists = checkIfMeasureCodeExists;
        service.checkIfMeasureNameExists = checkIfMeasureNameExists;
        service.searchTextForMeasureUnit = searchTextForMeasureUnit;
        service.deletedSelected = deletedSelected;

        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/MeasureUnit/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function searchTextForMeasureUnit(searchText, active) {
            return $http.get('/api/MeasureUnit/searchMeasureUnits/' + active + '/' + searchText).then(handleSuccess, handleError);
        }

        function checkIfMeasureCodeExists(code) {
            return $http.get('/api/MeasureUnit/CheckIfDeletedMeasureUnitWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function checkIfMeasureNameExists(code) {
            return $http.get('/api/MeasureUnit/CheckIfDeletedMeasureUnitWithSameNameExists/' + code).then(handleSuccess, handleError);
        }

        //function getMeasureUnit() {
        //    return $http.get('/api/MeasureUnit/getactiveMeasureUnits').then(handleSuccess, handleError);
        //}

        function deleteMeasureUnit(id) {
            return $http.delete('/api/MeasureUnit/' + id).then(handleSuccess, handleError);
        }

        function measureUnitById(id) {
            return $http.get('/api/MeasureUnit/getMeasureUnitbyid/' + id).then(handleSuccess, handleError);
        }

        function createMeasureUnit(measure) {
            return $http.post('/api/MeasureUnit/createmeasureunit', measure).then(handleSuccess, handleError);
        }

        function updateMeaureUnit(measure) {
           return $http.put('/api/MeasureUnit/updatemeasureunit', measure).then(handleSuccess, handleError);
        }

        function activeMeasureUnit(id) {
            return $http.get('/api/MeasureUnit/activateMeasureUnit/' + id).then(handleSuccess, handleError);
        }

        //function getInactiveMeasureUnit() {
        //    return $http.get('/api/MeasureUnit/searchMeasureUnits').then(handleSuccess, handleError);
        //}

        function getEntryMethodTypes() {
            return $http.get('/api/MeasureUnit/getallentrymethods').then(handleSuccess, handleError);
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
