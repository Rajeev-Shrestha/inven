(function () {
    'use strict';

    angular
        .module('app-home')
        .service('homeService', homeService);

    homeService.$inject = ['$http'];

    function homeService($http) {
        var service = {};
        // service here
        service.buyHrevertCrm = buyHrevertCrm;
        return service;
        //code here

        function buyHrevertCrm(email, name) {
            var apiUrl = "/api/Company/GetUserEmailAndCompanyNameAfterPurchase/" + email + "/" + name;
            return $http.post('/api/postdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
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