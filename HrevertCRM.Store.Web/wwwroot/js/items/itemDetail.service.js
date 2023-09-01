(function () {
	'use strict';

	angular
        .module('app-items')
        .service('itemDetailService', itemDetailService);

	itemDetailService.$inject = ['$http'];

	function itemDetailService($http) {
		var service = {};

		service.getItemsById = getItemsById;
		service.getAllCategory = getAllCategory;
		service.addtocart = addtocart;
		service.crmLocation = crmLocation;
		service.getSettings = getSettings;
		service.getLoginUserId = getLoginUserId;
		service.getCategoryTree = getCategoryTree;
	    service.getLatestProduct = getLatestProduct;
	    
		return service;

		function getLatestProduct(pageIndex, pageSize) {
		    var apiUrl = '/api/Product/lastestproducts/' + buildUriForPaging(pageIndex, pageSize);
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getCategoryTree() {
		    var apiUrl = '/api/ProductCategory/categorytree';
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		//function getItems(pageIndex, pageSize) {
		//    var apiUrl = "/api/product/getactiveproducts" + buildUriForPaging(pageIndex, pageSize);
		//    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
	    //}

		function getLoginUserId() {
		    return $http.get('/api/getloggedinuser/').then(handleSuccess, handleError);
		}

		function getSettings() {
		    var apiUrl = "/api/EcommerceSetting/getactiveEcommerceSettings";
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);

		}

		function crmLocation() {
		    return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
		}
		function addtocart(item) {
		    var apiUrl = "/api/ShoppingCart/addtocart/";
            var dataToPost= {url:apiUrl,data:item}
		    return $http.post('/api/PostWithData',dataToPost).then(handleSuccess, handleError);
	    }

		function getItemsById(customerId, id) {
		    var apiUrl = "/api/product/GetProductForStoreFront/" + customerId + "/" + id;
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getAllCategory() {
		    var apiUrl = "/api/ProductCategory/getactivecategories/";
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function buildUriForPaging(pageIndex, pageSize) {
		    var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
		    return uri;
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