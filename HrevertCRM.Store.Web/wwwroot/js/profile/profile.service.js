(function () {
    'use strict';

    angular
        .module('app-profile')
        .factory('profileService', profileService);
    profileService.$inject = ['$http', '$sessionStorage'];

    function profileService($http, $sessionStorage) {
        var service = {};
        service.getLoginUserDetails = getLoginUserDetails;
        service.getLoginUserEmail = getLoginUserEmail;

            return service;

            function getLoginUserDetails(id) {
                var apiUrl = "/api/Customer/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getLoginUserEmail() {
                return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
            }
            function handleSuccess(res) {
                return { success: true, data: res.data };
            }

            function handleError(error) {
                console.log(JSON.stringify(error.data));
                if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

                return { success: false, message: error.data };

            }


    }
})();