(function () {
    'use strict';

    angular
        .module('app-companywebSetting')
        .service('companywebSettingService', companywebSettingService);

    companywebSettingService.$inject = ['$http'];

    function companywebSettingService($http) {
        var service = {};
        service.getCompanyWebSettingById = getCompanyWebSettingById;
        service.deleteCompanyWebSetting = deleteCompanyWebSetting;
        service.updateCompanyWebSetting = updateCompanyWebSetting;
        service.createCompanyWebSetting = createCompanyWebSetting;
        service.getDiscountCalculationType = getDiscountCalculationType;
        service.getShippingDiscountCalculationType = getShippingDiscountCalculationType;
        service.getAllUser = getAllUser;
        service.createCompanyLogo = createCompanyLogo;
        service.updateCompanyLogo = updateCompanyLogo;
        service.deleteCompanyLogo = deleteCompanyLogo;
        service.getCompanyLogoById = getCompanyLogoById;
        service.getActiveCompanyLogo = getActiveCompanyLogo;
        service.activateCompanyLogo = activateCompanyLogo;
        service.checkIfCompanySettingExists = checkIfCompanySettingExists;
        service.activateCompanySetting = activateCompanySetting;
        service.checkLogin = checkLogin;
        service.getAllActiveDeliveryMethods = getAllActiveDeliveryMethods;
        service.getPaymentMethod = getPaymentMethod;

        return service;


        function getAllActiveDeliveryMethods() {

            return $http.get('/api/DeliveryMethod/searchDeliveryMethods/'+true+'/'+ null).then(handleSuccess, handleError);
        }

        function getPaymentMethod() {
            return $http.get('/api/paymentMethod/searchPaymentMethods/' + true + '/' + null).then(handleSuccess, handleError);

        }

        function checkIfCompanySettingExists() {
            return $http.get('/api/CompanyWebSetting/CheckIfDeletedCompanyWebSettingExists').then(handleSuccess, handleError);
        }

        function activateCompanySetting(id) {
            return $http.get('/api/CompanyWebSetting/activatecompanywebsetting/'+id).then(handleSuccess, handleError);
        }

        function checkLogin() {
            return $http.get('/api/user/checkfirstlogin').then(handleSuccess, handleError);
        }
        function getAllUser() {
            return $http.get('/api/user/getUsers/'+true+'/'+null).then(handleSuccess, handleError);
        }
        function getCompanyWebSettingById() {
            return $http.get('/api/CompanyWebSetting/getcompanywebsetting').then(handleSuccess, handleError);
        }

        function createCompanyWebSetting(companywebsetting) {
            return $http.post('/api/CompanyWebSetting/createcompanywebsetting', companywebsetting).then(handleSuccess, handleError);
        }

        function updateCompanyWebSetting(companywebsetting) {
            return $http.put('/api/CompanyWebSetting/updatecompanywebsetting', companywebsetting).then(handleSuccess, handleError);
        }

        function deleteCompanyWebSetting(companyId) {
            return $http.delete('/api/CompanyWebSetting/' + companyId).then(handleSuccess, handleError);
        }

        function getDiscountCalculationType() {
            return $http.get('/api/CompanyWebSetting/getdiscountcalculationtypes').then(handleSuccess, handleError);
        }

        function getShippingDiscountCalculationType() {
            return $http.get('/api/CompanyWebSetting/getshippingdiscountcalculationtypes').then(handleSuccess, handleError);
        }

        function activateCompanyLogo(logoId) {
            return $http.get('/api/CompanyLogo/activatecompanylogo/' + logoId).then(handleSuccess, handleError);
        }

        function createCompanyLogo(companyLogo) {
            return $http.post('/api/CompanyLogo/createcompanylogo/', companyLogo).then(handleSuccess, handleError);
        }

        function updateCompanyLogo(companyLogo) {
            return $http.put('/api/CompanyLogo/updatecompanylogo/', companyLogo).then(handleSuccess, handleError);
        }

        function deleteCompanyLogo(logoId) {
            return $http.delete('/api/CompanyLogo/'+logoId).then(handleSuccess, handleError);
        }

        function getActiveCompanyLogo() {
            return $http.get('/api/CompanyLogo/getactivecompanylogos/').then(handleSuccess, handleError);
        }

        function getCompanyLogoById(logoId) {
            return $http.get('/api/CompanyLogo/getcompanylogobyid/' + logoId).then(handleSuccess, handleError);
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