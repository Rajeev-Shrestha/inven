(function () {
	'use strict';

	angular
		.module('app-checkout')
		.service('checkoutService', checkoutService);

	checkoutService.$inject = ['$http'];

	function checkoutService($http) {
		var service = {};
		service.getCartItems = getCartItems;
		service.getPaymentMethod = getPaymentMethod;
		service.crmLocation = crmLocation;
		service.getCustomerAddress = getCustomerAddress;
		service.confirmOrder = confirmOrder;
		service.getDeliveryMethod = getDeliveryMethod;
		service.getLoginUser = getLoginUser;
		service.getAddressById = getAddressById;
		service.getPaymentTerm = getPaymentTerm;
		service.getMethodById = getMethodById;
		service.updateCartItemsAfterLogin = updateCartItemsAfterLogin;
		service.getCompanyDetails = getCompanyDetails;
		service.getloginUser = getloginUser;
		service.sendEmailToCustomer = sendEmailToCustomer;
	    service.companyWebSetting = companyWebSetting;
	    service.getCompanyLogo = getCompanyLogo;
		
		return service;
		function getCompanyLogo() {
		    var apiUrl = "/api/EcommerceSetting/getactiveEcommerceSettings/";
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function companyWebSetting() {
		    var apiUrl = '/api/CompanyWebSetting/getcompanywebsetting';
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
	    }
		function sendEmailToCustomer(email) {
		    var apiUrl = "/api/ShoppingCart/SendEmailToCustomer/";
		    var dataToPost = { url: apiUrl, data: email }
		    return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
		}
		function getCompanyDetails(id) {
		    var apiUrl = '/api/company/getcompanybyid/' + id;
		    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function updateCartItemsAfterLogin(cartId, customerId) {
			var apiUrl = "/api/ShoppingCart/updatecartwithcustomerdetails/" + cartId + "/" + customerId;
			return $http.post('/api/postdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getMethodById(id) {
			var apiUrl = "/api/paymentTerm/getPayMethodsInPayTerm/" + id;
				return $http.get('/api/getdata?apiUrl='+ encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getPaymentTerm() {
			var apiUrl = '/api/PaymentTerm/getTermsWithoutAccountType/';
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getAddressById(id) {
			var apiUrl = '/api/Address/getaddress/' + id;
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}
		function getLoginUser() {
			return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
		}

		function getDeliveryMethod() {
		    var apiUrl = "/api/DeliveryMethod/searchDeliveryMethods/"+true+"/"+null;
			return $http.get('/api/getdata?apiUrl='+ encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function confirmOrder(checkout) {
			var apiUrl = "/api/ShoppingCart/ConvertCartToOrder/";
			var dataToPost = { url: apiUrl, data: checkout }
			return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
		}
		function getCustomerAddress(id) {
			var apiUrl = "/api/Customer/getcustomerbyid/" + id;
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function crmLocation() {
			return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
		}

		function getloginUser() {
		    return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
		}
		function getPaymentMethod() {
			var apiUrl = "/api/PaymentMethod/getactivepaymentmethods";
			return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
		}

		function getCartItems(id, loginUser) {
			var apiUrl = "/api/ShoppingCart/getshoppingcart/" + id + '/' + loginUser;
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