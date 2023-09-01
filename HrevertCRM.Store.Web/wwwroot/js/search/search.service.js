(function () {
	'use strict';

	angular
		.module('app-search')
		.service('searchService', searchService);

	searchService.$inject = ['$http'];

	function searchService($http) {
		var service = {};
		service.searchText = searchText;
		service.getLoginUserId = getLoginUserId;
		service.searchInAllCategory = searchInAllCategory;
		service.getCartItems = getCartItems;
		return service;

		function searchInAllCategory(pageIndex, pageSize, text, active) {
			var apiUrl = '/api/Product/searchactiveproducts/' + buildUriForSearchPaging(pageIndex, pageSize, text, active);
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getLoginUserId() {
			return $http.get('/api/getloggedinuser/').then(handleSuccess, handleError);
		}
		function searchText(item) {
		    var apiUrl = "/api/ProductCategory/searchbycategoryIdAndText";
		    var dataToPost = { url: apiUrl, data: item }
		    return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);

		}

		function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
			var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
			return uri;
		}

		function getCartItems(id, customerId) {
			var apiUrl = "/api/ShoppingCart/getshoppingcart/" + id + "/" + customerId;
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