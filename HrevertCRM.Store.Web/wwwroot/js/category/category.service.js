(function () {
	'use strict';

	angular
		.module('app-category')
		.service('categoryService', categoryService);

	categoryService.$inject = ['$http'];

	function categoryService($http) {
		var service = {};
		service.getItemsByCategoryId = getItemsByCategoryId;
		service.getCrmLocation = getCrmLocation;
		service.addItemInCart = addItemInCart;
		service.getcategoryTree = getcategoryTree;
		service.getItemsByCategoryIdList = getItemsByCategoryIdList;
		return service;

		function getItemsByCategoryIdList(list) {
			var urlString = '';
			for (var i = 0; i < list.length; i++) {
				urlString = urlString + 'listOfCategoryId=' + list[i] + '&'
			}
			var newStr = urlString.substring(0, urlString.length - 1);
			var apiUrl = '/api/Product/getProductsByCategoryIdList?' + newStr;
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getcategoryTree() {
			var apiUrl = "/api/ProductCategory/categorytree";
			return $http.get('/api/getdata?apiURl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function addItemInCart(item) {
			var apiUrl = "/api/ShoppingCart/addtocart";
			var dataToPost = { url: apiUrl, data: item }
			return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
		}
		function getCrmLocation() {
			return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
		}

		function getItemsByCategoryId(id) {
			var apiUrl = "/api/Product/getProductsByCategoryId/" + id;
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
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