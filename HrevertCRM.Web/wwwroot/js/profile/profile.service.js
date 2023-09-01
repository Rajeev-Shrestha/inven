(function () {
    'use strict';

    angular
        .module('app-profile')
        .service('profileService', profileService);

    profileService.$inject = ['$http'];

    function profileService($http) {
        var service = {};
        service.getAllUserTypes = getAllUserTypes;
        service.getUserById = getUserById;
        service.updateUser = updateUser;
       
        return service;

        function updateUser(user) {
            return $http.put('/api/user/updateuser', user).then(handleSuccess, handleError);
        }

        function getUserById(id) {
            return $http.get('/api/user/GetLoggedInUserDetail').then(handleSuccess, handleError);
        }

        function getAllUserTypes() {
            return $http.get('/api/User/getusertypes').then(handleSuccess, handleError);
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