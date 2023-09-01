(function () {
    'use strict';

    angular
        .module('app-setup')
        .service('setupService', setupService);

    setupService.$inject = ['$http'];

    function setupService($http) {
        var service = {};
        service.saveSetting = saveSetting;
        service.updateLogo = updateLogo;
        service.createCompanyLogo = createCompanyLogo;
        service.getActiveCompanyLogo = getActiveCompanyLogo;
        service.createCompany = createCompany;
        service.getLoginUserDetails = getLoginUserDetails;
        return service;

        function getLoginUserDetails() {
            return $http.get('/api/User/CheckFirstLogin').then(handleSuccess, handleError);
        }
        function createCompany(company) {
            return $http.put('/api/Company/updatecompany/', company).then(handleSuccess, handleError);
        }
        function getActiveCompanyLogo() {
            return $http.get('/api/CompanyLogo/getactivecompanylogos/').then(handleSuccess, handleError);
        }
        function createCompanyLogo(companyLogo) {
            return $http.post('/api/CompanyLogo/createcompanylogo/', companyLogo).then(handleSuccess, handleError);
        }
        function updateLogo(companyLogo) {
            return $http.put('/api/CompanyLogo/updatecompanylogo/', companyLogo).then(handleSuccess, handleError);
        }
        function saveSetting(setting) {
            return $http.put('/api/CompanySetup/savesetting/', setting).then(handleSuccess, handleError);
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