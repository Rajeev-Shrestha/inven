(function () {
	'use strict';

	angular
		.module('app-registerUser')
		.service('registerUserService', registerUserService);

	registerUserService.$inject = ['$http'];

	function registerUserService($http) {
		var service = {};
		service.registerUser = registerUser;
		service.getAllLevels = getAllLevels;
		service.getAllSuffixes = getAllSuffixes;
		service.getAllTitles = getAllTitles;
		service.getActiveZones = getActiveZones;
		service.getAllCountries = getAllCountries;
		service.checkEmailExists = checkEmailExists;
		return service;

		function checkEmailExists(email) {
		    var apiUrl = "/api/customer/checkEmail/" + email;
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);

		}
		function registerUser(item) {
			var apiUrl = "/api/Customer/createcustomerforstorefront/";
			var dataToPost = { url: apiUrl, data: item }
			return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
		}

		function getAllCountries() {
			var apiUrl = "/api/customer/getcountries";
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		   
		}

		function getAllLevels() {
			var apiUrl = "/api/customerlevel/getallcustomerlevels";
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		
		function getAllSuffixes() {
			var apiUrl = "/api/customer/getsuffixes/";
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getAllTitles() {
			var apiUrl = "/api/customer/gettitles/";
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getActiveZones() {
		    var apiUrl = "/api/DeliveryZone/searchDeliveryZones/"+true+'/'+null;
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