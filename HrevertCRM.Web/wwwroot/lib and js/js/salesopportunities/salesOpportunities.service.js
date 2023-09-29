(function () {
    'use strict';

    angular
        .module('app-salesOpportunities')
        .service('salesOpportunitiesService', salesOpportunitiesService);
    salesOpportunitiesService.$inject = ['$http'];

    function salesOpportunitiesService($http) {
        var service = {};
        service.createOpportunity = createOpportunity;
        service.getAllCustomers = getAllCustomers;
        service.getAllSalesRepresentatives = getAllSalesRepresentatives;
        service.getAllStages = getAllStages;
        service.getAllSources = getAllSources;
        service.getAllReasonsClosed = getAllReasonsClosed;
        service.getAllGrades = getAllGrades;
        service.getAllAcitveSalesOpportunities = getAllAcitveSalesOpportunities;
        service.updateOpportunity = updateOpportunity;
        service.deleteOpportunity = deleteOpportunity;
        service.getAllSalesOpportunities = getAllSalesOpportunities;
        return service;

        function getAllSalesOpportunities() {
            return $http.get('/api/salesOpportunity/getAll').then(handleSuccess, handleError);

        }
        function getAllCustomers() {
            return $http.get('/api/customer/activecustomerwithoutpagaing').then(handleSuccess, handleError);

        }
        function getAllSalesRepresentatives() {
            return $http.get('/api/user/getactiveusers').then(handleSuccess, handleError);

        }
        function getAllStages() {
            return $http.get('/api/stage/getAllActiveStages').then(handleSuccess, handleError);

        }
      
        function getAllSources() {
            return $http.get('/api/source/getAllActiveSources').then(handleSuccess, handleError);

        }
        function getAllReasonsClosed() {
            return $http.get('/api/reasonClosed/getAllActiveReasonClosed').then(handleSuccess, handleError);

        }
        function getAllGrades() {
            return $http.get('/api/grade/getAllActiveGrade').then(handleSuccess, handleError);

        }
        function createOpportunity(opportunity) {
            return $http.post('/api/salesOpportunity/Create', opportunity).then(handleSuccess, handleError);
        }

        function updateOpportunity(opportunity) {
            return $http.put('/api/salesOpportunity/Update', opportunity).then(handleSuccess, handleError);
        }
        function deleteOpportunity(id) {
            return $http.delete('/api/salesOpportunity/'+id).then(handleSuccess, handleError);
        }
        function getAllAcitveSalesOpportunities() {
            return $http.get('/api/salesOpportunity/getAllActive').then(handleSuccess, handleError);
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