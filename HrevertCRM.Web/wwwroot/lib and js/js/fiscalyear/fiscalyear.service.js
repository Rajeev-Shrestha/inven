(function () {
    'use strict';

    angular
        .module('app-fiscalyear')
        .service('fiscalyearService', fiscalyearService);

    fiscalyearService.$inject = ['$http'];

    function fiscalyearService($http) {
        var service = {};
    //    service.GetAll = getAll;
        service.GetById = getById;
        service.Create = create;
        service.Update = update;
        service.Delete = Delete;
        service.SaveFiscalPeriod = saveFiscalPeriod;
        service.GetAllPeriod = getAllPeriod;
        service.GetPeriodByYearId = getPeriodByYearId;
        service.SavePeriodByFiscalYear = savePeriodByFiscalYear;
        service.UpdateFiscalPeriod = updateFiscalPeriod;
        service.DeletePeriod = deletePeriod;
      //  service.getInactiveYear = getInactiveYear;
        service.activeYear = activeYear;
        //service.searchFiscalYear = searchFiscalYear;
        service.activateFiscalPeriodById = activateFiscalPeriodById;
        service.getCompanySetting = getCompanySetting;
        service.searchTextForFiscalYear = searchTextForFiscalYear;
        service.deletedSelected = deletedSelected;

        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/fiscalyear/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function getCompanySetting() {
            return $http.get('/api/Company/getcompany').then(handleSuccess, handleError);
        }

        //function searchFiscalYear(text, status) {
        //    if (status === 'false' ||status === false) {
        //        return $http.get('/api/fiscalyear/searchactivefiscalyears/' + text).then(handleSuccess, handleError);
        //    }
        //    else {
        //        return $http.get('/api/fiscalyear/searchallfiscalyears/' + text).then(handleSuccess, handleError);
        //    }
        //}

        //function getAll() {
        //    return $http.get('/api/fiscalyear/getactivefiscalyears').then(handleSuccess, handleError);
        //}

        function getInactiveYear() {
            return $http.get('/api/fiscalyear/getallfiscalyears').then(handleSuccess, handleError);
        }

        function activeYear(id) {
            return $http.get('/api/fiscalyear/activatefiscalyear/' + id).then(handleSuccess, handleError);
        }

        function getAllPeriod() {
            return $http.get('/api/fiscalperiod').then(handleSuccess, handleError);
        }

        function updateFiscalPeriod(fiscalPeriod) {
            return $http.put('/api/fiscalperiod/updatefiscalperiod/', fiscalPeriod).then(handleSuccess, handleError);
        }

        function getPeriodByYearId(id) {
            return $http.get('/api/fiscalperiod/getfiscalperiodbyyearid/' + id).then(handleSuccess, handleError);
        }

        function savePeriodByFiscalYear(fiscalPeriod) {
            return $http.post('/api/fiscalperiod/createfiscalperiod' + fiscalPeriod).then(handleSuccess, handleError);
        }

        function getById(id) {
            return $http.get('/api/fiscalyear/getfiscalyearbyid/' + id).then(handleSuccess, handleError);
        }

        function create(fiscalYear) {

            return $http.post('/api/fiscalyear/createfiscalyear', fiscalYear).then(handleSuccess, handleError);
        }

        function saveFiscalPeriod(fiscalPeriod) {

            return $http.post('/api/fiscalperiod/createfiscalperiod', fiscalPeriod).then(handleSuccess, handleError);
        }

        function deletePeriod(id) {
            return $http.delete('/api/fiscalperiod/' + id).then(handleSuccess, handleError);
        }

        function update(fiscalYear) {
            return $http.put('/api/fiscalyear/updatefiscalyear', fiscalYear).then(handleSuccess, handleError);
        }

        function Delete(id) {
            return $http.delete('/api/fiscalyear/' + id).then(handleSuccess, handleError);
        }

       function activateFiscalPeriodById(fiscalPeriodId){
           return $http.get('/api/fiscalperiod/activatefiscalperiod/' + fiscalPeriodId).then(handleSuccess, handleError);
        }
       function searchTextForFiscalYear(text, checked) {
           return $http.get('/api/fiscalYear/searchFiscalYears/' + checked + '/' + text).then(handleSuccess, handleError);
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