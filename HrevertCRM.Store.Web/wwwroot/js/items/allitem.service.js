(function () {
	'use strict';

	angular
        .module('app-items')
        .service('allitemService', allitemService);

	allitemService.$inject = ['$http'];

	function allitemService($http) {
		var service = {};
		service.getcategoryTree = getcategoryTree;
		service.getProductById = getProductById;
		service.getProductByCategory = getProductByCategory;
		service.getProductByCategories = getProductByCategories;
		service.getProductByCategoryId = getProductByCategoryId;
        
		return service;

		function getProductByCategoryId(id) {
		    var apiUrl = "/api/Product/getProductsByCategoryId/" + id;
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function buildUriForListValue(id) {
		    var uri = "";
		    for (var i = 0; i < id.length; i++) {
		        if (id[0] === id[i]) {
		            uri += '?values=' + id[i];
		        }
		        else {
		            uri += '&values=' + id[i];
		        }
		        
		    }
		    
		    return uri;
		}

		function getProductByCategory(id) {
		    var apiUrl = "/api/ProductCategory/getproductcategory/" + id;
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getProductByCategories(id) {
		    var apiUrl = "/api/ProductCategory/getproductcategories/" + buildUriForListValue(id);
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getcategoryTree() {
		    
		    var apiUrl = "/api/productcategory/categorytree";
            return $http.get('/api/getdata?apiUrl=' +encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

        function getProductById() {

		    var apiUrl = "/api/productcategory/categorytree";
            return $http.get('/api/getdata?apiUrl=' +encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
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