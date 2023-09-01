(function () {
    'use strict';

    angular
        .module('app-emailSetting')
        .service('emailSettingService', emailSettingService);

    emailSettingService.$inject = ['$http'];

    function emailSettingService($http) {
        var service = {};

        service.getAllEmail = getAllEmail;
        service.createEmailSetting = createEmailSetting;
        service.updateEmailSetting = updateEmailSetting;
        service.deleteEmailSetting = deleteEmailSetting;
        service.getEmailSettingById = getEmailSettingById;
      //  service.getAllActiveEmailSetting = getAllActiveEmailSetting;
        service.getActiveEmailSetting = getActiveEmailSetting;
        service.searchTextForEmailSetting = searchTextForEmailSetting;
        service.getAllEncryptionTypes = getAllEncryptionTypes;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/EmailSetting/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function getAllEmail() {
            return $http.get('/api/EmailSetting/getallemailsettings').then(handleSuccess, handleError);
        }

        function createEmailSetting(emailSetting) {
            return $http.post('/api/EmailSetting/createemailsetting', emailSetting).then(handleSuccess, handleError);
        }

        function updateEmailSetting(emailSetting) {
            return $http.put('/api/EmailSetting/updateemailsetting', emailSetting).then(handleSuccess, handleError);
        }

        function deleteEmailSetting(emailSetting) {
            return $http.delete('/api/EmailSetting/' + emailSetting).then(handleSuccess, handleError);
        }

        function getEmailSettingById(id) {
            return $http.get('/api/EmailSetting/getemailbyid/' + id).then(handleSuccess, handleError);
        }

        //function getAllActiveEmailSetting() {
        //    return $http.get('/api/EmailSetting/getactiveemailsettings').then(handleSuccess, handleError);
        //}

        function getActiveEmailSetting(emailSettingId) {
            return $http.get('/api/EmailSetting/activateemailsetting/' + emailSettingId).then(handleSuccess, handleError);
        }

        //function searchTextForEmailSetting(text, checked) {
        //    if (checked === 'false' || checked === false) {
        //        return $http.get('/api/EmailSetting/searchactiveemailsettings/' + text)
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/EmailSetting/searchallemailsettings/' + text)
        //           .then(handleSuccess, handleError);
        //    }
        //}

        function getAllEncryptionTypes() {
            return $http.get('/api/EmailSetting/getencryptiontypes').then(handleSuccess, handleError);
        }
        function searchTextForEmailSetting(text, checked) {
            return $http.get('/api/emailSetting/searchEmailSettings/' + checked + '/' + text).then(handleSuccess, handleError);
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