(function () {
	'use strict';

	angular
        .module('app-error')
        .service('errorService', errorService);

	errorService.$inject = ['$http'];

	function errorService($http) {
	    var service = {};
	    
        
	    return service;



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