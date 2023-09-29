(function () {
	'use strict';

	angular
		.module('app-contactmanager')
		.service('contactManagerService', contactManagerService);

	contactManagerService.$inject = ['$http'];

	function contactManagerService($http) {
		var service = {};
	//	service.getAllCustomerGroup = getAllCustomerGroup;
		//service.getAllActiveCustomerGroup = getAllActiveCustomerGroup;
		service.createCustomerGroup = createCustomerGroup;
		service.updateCustomerGroup = updateCustomerGroup;
		service.deleteCustomerGroup = deleteCustomerGroup;
		service.getCustomerGroupById = getCustomerGroupById;
		service.getAllActiveCustomersWithoutPaging = getAllActiveCustomersWithoutPaging;
		service.sendEmail = sendEmail;
		service.getAllActiveEmail = getAllActiveEmail;
		//service.searchTextForCustomerContactGroup = searchTextForCustomerContactGroup;
		service.activateCustomerContactGroupById = activateCustomerContactGroupById;
		service.checkIfExistsGroupName = checkIfExistsGroupName;
		service.getPageSize = getPageSize;
		service.searchTextForContactGroup = searchTextForContactGroup;
		
		return service;

		//* Services of CustomerContact  Start*//
		//function getAllCustomerGroup(pageIndex,pageSize) {
		//    return $http.get('/api/CustomerContactGroup/getallgroups'+ buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
		//}

		function sendEmail(data, params) {
			//$http.post("/api/EmailSetting/postfile/", data, { headers: { 'Content-Type': undefined }, transformRequest: angular.identity, params: files });
			//return $http.post('/api/EmailSetting/sendemail/', mailItems).then(handleSuccess, handleError);
			return $http.post("/api/EmailSetting/sendemail/",
				data,
				{
					headers: { 'Content-Type': undefined },
					transformRequest: angular.identity,
					params: params
				});
		}

		function createCustomerGroup(customerGroup) {
			return $http.post('/api/CustomerContactGroup/creategroup', customerGroup).then(handleSuccess, handleError);
		}

		function updateCustomerGroup(customerGroup) {
			return $http.put('/api/CustomerContactGroup/updategroup', customerGroup).then(handleSuccess, handleError);
		}

		function deleteCustomerGroup(customerGroup) {
			return $http.delete('/api/CustomerContactGroup/' + customerGroup).then(handleSuccess, handleError);
		}

		function getCustomerGroupById(id) {
			return $http.get('/api/CustomerContactGroup/getcustomercontactgroup/' + id).then(handleSuccess, handleError);
		}

		//function getAllActiveCustomerGroup(pageIndex,pageSize) {
		//    return $http.get('/api/CustomerContactGroup/getactivegroups' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
		//}

		function getAllActiveCustomersWithoutPaging() {
		    return $http.get('/api/Customer/activecustomerwithoutpagaing').then(handleSuccess, handleError);
		}

		function getAllActiveEmail() {
			return $http.get('/api/EmailSetting/getactiveemailsettings/').then(handleSuccess, handleError);
		}

		//function searchTextForCustomerContactGroup(text, status) {
		//    if (status === 'false'||status === false) {
		//		return $http.get('/api/CustomerContactGroup/searchactivecustomercontactgroup/' + text).then(handleSuccess, handleError);
		//	} else {
		//		return $http.get('/api/CustomerContactGroup/searchallcustomercontactgroup/' + text).then(handleSuccess, handleError);
		//	}
		//}
		
		function buildUriForPaging(pageIndex, pageSize) {
			var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
			return uri;
		}
		function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
		    var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
		    return uri;
		}
		function activateCustomerContactGroupById(id) {
			return $http.get('/api/CustomerContactGroup/activatecustomercontactgroup/' + id).then(handleSuccess, handleError);
		}
		

	    function checkIfExistsGroupName(name) {
	        return $http.get('/api/CustomerContactGroup/CheckIfDeletedCustomerContactWithSameNameExists/' + name).then(handleSuccess, handleError);
	    }

	    //Get Page size
	    function getPageSize() {
	        return $http.get('/api/CustomerContactGroup/getPageSize').then(handleSuccess, handleError);
	    }
	    function searchTextForContactGroup(text, checked, pageIndex, pageSize) {

	        return $http.get('/api/CustomerContactGroup/getContactGroups/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

	    }
		//* Services of CustomerContact  End*//


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