(function () {
    'use strict';

    angular
        .module('app-journalMaster')
        .service('journalMasterService', journalMasterService);

    journalMasterService.$inject = ['$http'];

    function journalMasterService($http) {
        var service = {};

        service.getAllJournalMaster = getAllJournalMaster;
        service.createJournalMaster = createJournalMaster;
        service.updateJournalMaster = updateJournalMaster;
        service.deleteJournalMaster = deleteJournalMaster;
        service.getJournalMasterById = getJournalMasterById;
        service.getAllActiveJournalMaster = getAllActiveJournalMaster;
        service.getActiveJournalMasterById = getActiveJournalMasterById;
        service.searchTextForJournalMaster = searchTextForJournalMaster;
        service.getAllJournalTypes = getAllJournalTypes;
        service.getPageSize = getPageSize;
        service.deletedSelected = deletedSelected;

        return service
        function deletedSelected(selectedList) {
            return $http.post('/api/journalmaster/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function getAllJournalMaster(pageIndex, pageSize) {
            return $http.get('/api/journalmaster/getalljournalmasters/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        function createJournalMaster(journalMaster) {
            return $http.post('/api/journalmaster/createjournalmaster', journalMaster).then(handleSuccess, handleError);
        }

        function updateJournalMaster(journalMaster) {
            return $http.put('/api/journalmaster/updatejournalmaster', journalMaster).then(handleSuccess, handleError);
        }

        function deleteJournalMaster(journalMaster) {
            return $http.delete('/api/journalmaster/' + journalMaster).then(handleSuccess, handleError);
        }

        function getJournalMasterById(id) {
            return $http.get('/api/journalmaster/getjournalmasterbyid/' + id).then(handleSuccess, handleError);
        }

        function getActiveJournalMasterById(id) {
            return $http.get('/api/journalmaster/activatejournalmaster/' + id).then(handleSuccess, handleError);

        }

        function getAllActiveJournalMaster(pageIndex, pageSize) {
            return $http.get('/api/journalmaster/getactivejournalmasters/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        //function searchTextForJournalMaster(pageIndex, pageSize, text, checked) {

        //    if (checked === 'false' || checked === false) {
        //        return $http.get('/api/journalmaster/searchactivejournalmasters/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/journalmaster/searchalljournalmasters/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
        //           .then(handleSuccess, handleError);
        //    }
        //}

        function getAllJournalTypes() {

            return $http.get('/api/journalmaster/getjournaltypes').then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/journalmaster/getPageSize').then(handleSuccess, handleError);
        }
        function searchTextForJournalMaster(text, checked, pageIndex, pageSize) {

            return $http.get('/api/journalmaster/searchJournalMasters/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

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