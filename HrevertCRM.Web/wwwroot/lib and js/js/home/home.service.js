(function () {
    'use strict';

    angular
        .module('app-home')
        .service('homeService', homeService);

    homeService.$inject = ['$http'];

    function homeService($http) {
        var service = {};
        service.getFiscalYear = getFiscalYear;
        service.getDashboardValue = getDashboardValue;
        service.report = report;
        service.getCompanyWebSetting = getCompanyWebSetting;
        service.checkLogin = checkLogin;
        service.getCurrentFiscalYear = getCurrentFiscalYear;
        return service;

   
        function checkLogin() {
            return $http.get('/api/user/CheckFirstLogin/').then(handleSuccess, handleError);
        }
        function getCompanyWebSetting(id) {
            return $http.get('/api/Company/getcompanybyid/' + id).then(handleSuccess, handleError);
        }
        function report(bug) {
            return $http.post('/api/BugLogger/reportBug', bug).then(handleSuccess, handleError);
        }


        function getDashboardValue(fiscalYear) {
            return $http.get("/api/dashboard/GetDashboardValues/" + fiscalYear).then(handleSuccess, handleError);
        }
        function getFiscalYear() {
            return $http.get("/api/fiscalyear/searchFiscalYears/"+ true + '/'+ null).then(handleSuccess, handleError);
        }
        function getCurrentFiscalYear() {
            return $http.get('/api/fiscalYear/getCurrentFiscalYear/').then(handleSuccess, handleError);
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