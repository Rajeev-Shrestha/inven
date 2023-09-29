(function() {
    'use strict';

    angular
        .module('app-home')
        .factory('homeService', homeService);

    homeService.$inject = ['$http', '$sessionStorage'];

    function homeService($http, $sessionStorage) {
        var service = {};

        service.getProducts = getProducts;
      

        return service;

        function getProducts() {
          return  $http.get('/api/product').then(handleSuccess, handleError);
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