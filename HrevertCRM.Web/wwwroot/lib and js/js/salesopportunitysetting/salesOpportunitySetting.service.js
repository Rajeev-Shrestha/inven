(function () {
    'use strict';

    angular
        .module('app-salesOpportunitySetting')
        .service('salesOpportunitySettingService', salesOpportunitySettingService);
    salesOpportunitySettingService.$inject = ['$http'];

    function salesOpportunitySettingService($http) {
        var service = {};
        //service for stage
        service.createStage = createStage;
        service.getAllActiveStages = getAllActiveStages;
        service.deleteStage = deleteStage;
        service.getAllStages = getAllStages;
        service.updateStage = updateStage;
        //end of stage service

        //service for source
        service.getAllActiveSources = getAllActiveSources;
        service.getAllSources = getAllSources;
        service.createSource = createSource;
        service.updateSource = updateSource;
        service.deleteSource = deleteSource;
        //end of source service

        //service for grade
        service.getAllActiveGrades = getAllActiveGrades;
        service.getAllGrades = getAllGrades;
        service.createGrade = createGrade;
        service.updateGrade = updateGrade;
        service.deleteGrade = deleteGrade;
        //end of grade service  

        //service for reason
        service.getAllActiveReasons = getAllActiveReasons;
        service.getAllReasons = getAllReasons;
        service.createReason = createReason;
        service.updateReason = updateReason;
        service.deleteReason = deleteReason;
        //end of reason service  
        return service;

        //stage function
        function getAllStages() {
            return $http.get('/api/Stage/getAllStages').then(handleSuccess, handleError);

        }
        function updateStage(stage) {
            return $http.put('/api/Stage/Update', stage).then(handleSuccess, handleError);

        }
        function getAllActiveStages() {
            return $http.get('/api/Stage/getAllActiveStages').then(handleSuccess, handleError);

        }
        function deleteStage(id) {
            return $http.delete('/api/Stage/' + id).then(handleSuccess, handleError);

        }
        function createStage(stage) {
            return $http.post('/api/Stage/Create', stage).then(handleSuccess, handleError);
        }
        //end stage

        // source function         
        function getAllSources() {
            return $http.get('/api/Source/getAllSources').then(handleSuccess, handleError);

        }
        function getAllActiveSources() {
            return $http.get('/api/Source/getAllActiveSources').then(handleSuccess, handleError);

        }
        function createSource(source) {
            return $http.post('/api/Source/Create', source).then(handleSuccess, handleError);

        }
        function updateSource(source) {
            return $http.put('/api/Source/Update', source).then(handleSuccess, handleError);

        }
        function deleteSource(id) {
            return $http.delete('/api/Source/' + id).then(handleSuccess, handleError);
        }
        // end source

        // grade function         
        function getAllGrades() {
            return $http.get('/api/grade/getAllGrades').then(handleSuccess, handleError);

        }
        function getAllActiveGrades() {
            return $http.get('/api/grade/getAllActiveGrade').then(handleSuccess, handleError);

        }
        function createGrade(grade) {
            return $http.post('/api/grade/Create', grade).then(handleSuccess, handleError);

        }
        function updateGrade(grade) {
            return $http.put('/api/grade/Update', grade).then(handleSuccess, handleError);
        }
        function deleteGrade(id) {
            return $http.delete('/api/grade/' + id).then(handleSuccess, handleError);
        }
        // end grade

        // reason function         
        function getAllReasons() {
            return $http.get('/api/reasonClosed/getAllReasonClosed').then(handleSuccess, handleError);

        }
        function getAllActiveReasons() {
            return $http.get('/api/reasonClosed/getAllActiveReasonClosed').then(handleSuccess, handleError);

        }
        function createReason(reason) {
            return $http.post('/api/reasonClosed/Create', reason).then(handleSuccess, handleError);
        }
        function updateReason(reason) {
            return $http.put('/api/reasonClosed/Update', reason).then(handleSuccess, handleError);
        }
        function deleteReason(id) {
            return $http.delete('/api/reasonClosed/' + id).then(handleSuccess, handleError);
        }
        // end reason
       
        //common
        function handleSuccess(res) {
            return { success: true, data: res.data };
        }
        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";
            return { success: false, message: error.data };
        }
        //end
    }
})();