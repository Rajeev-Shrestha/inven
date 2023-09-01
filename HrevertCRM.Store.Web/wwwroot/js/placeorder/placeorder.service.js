(function () {
	'use strict';

	angular
        .module('app-placeorder')
        .service('placeorderService', placeorderService);

	placeorderService.$inject = ['$http'];

	function placeorderService($http) {
	    var service = {};
	    service.getCartItems = getCartItems;
	    service.crmLocation = crmLocation;
	    service.getCustomerAddress = getCustomerAddress;
	    //service.getShippingAddress = getShippingAddress;
	    service.getAddressById = getAddressById;
	    service.getPaymentInformation = getPaymentInformation;
	    service.getloginUser = getloginUser;

        
	    return service;

	    function getloginUser() {
	        return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
	    }
	    function getPaymentInformation(id) {
	        var apiUrl = '/api/PaymentMethod/getpaymentmethodbyid/' + id;
	        return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
	    }
	    function getAddressById(id) {
	        var apiUrl = '/api/Address/getaddress/' + id;
	        return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
	    }

	    function getCustomerAddress(id) {
	        var apiUrl = "/api/Customer/getcustomerbyid/" + id;
	        return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
	    }
	    function crmLocation() {
	        return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
	    }

	    function getCartItems(id, loginUser) {
	        var apiUrl = "/api/ShoppingCart/getcart/" + id + "/" + loginUser;
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