(function () {
    'use strict';

    angular
        .module('app-customer')
        .service('customerService', customerService);

    customerService.$inject = ['$http'];

    function customerService($http) {
        var service = {};

        service.GetAll = getAll;
        service.GetById = getById;
        service.Create = create;
        service.Update = update;
        service.deleteCustomer = deleteCustomer;
       // service.GetCustomers = getCustomers;
        service.activatecustomer = activatecustomer;
        service.GetAllLevels = getAllLevels;
        service.GetLevelById = getLevelById;
        service.SaveCustomerLevel = saveCustomerLevel;
        service.GetPaymentMethod = getPaymentMethod;
        //service.getInactiveCustomers = getInactiveCustomers;
        service.Search = search;
        service.getAllRoles = getAllRoles;
       // service.searchText = searchText;
        service.getCustomerAddressById = getCustomerAddressById;
        service.checkCustomerCode = checkCustomerCode;
        service.getAllSuffixes = getAllSuffixes;
        service.getAllTitles = getAllTitles;
        service.getAllAddressTypes = getAllAddressTypes;
        service.getAccountTerm = getAccountTerm;
        service.getActiveZones = getActiveZones;
        service.getAllCountries = getAllCountries;
        service.importUsers = importUsers;
        service.getAllAddresses = getAllAddresses;
        service.activateAddressById = activateAddressById;
        service.checkIfCustomerCodeExists = checkIfCustomerCodeExists;
        service.searchTextForCustomer = searchTextForCustomer;
        service.getPageSize = getPageSize;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/customer/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function checkIfCustomerCodeExists(code) {
            return $http.get('/api/customer/CheckIfDeletedCustomerWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function importUsers(csv, updateExisting) {
            return $http.post('/api/customer/InsertCustomersViaCSV/'+ csv + '/' + updateExisting).then(handleSuccess, handleError);
        }

        function getAllAddresses(customerId) {
            return $http.get('/api/Customer/getCustomerAllAddresses/'+customerId).then(handleSuccess, handleError);
        }

        function activateAddressById(addressId) {
            return $http.get('/api/Address/activateaddress/' + addressId).then(handleSuccess, handleError);
        }

        function getActiveZones() {
            return $http.get('/api/DeliveryZone/searchDeliveryZones/'+true+'/'+null).then(handleSuccess, handleError);
        }

        function getAccountTerm() {
            return $http.get('/api/PaymentTerm/getOnAccountTerms').then(handleSuccess, handleError);
            
        }

        function getAllSuffixes() {

            return $http.get('/api/customer/getsuffixes').then(handleSuccess, handleError);
        }

        function getAllCountries() {

            return $http.get('/api/customer/getcountries').then(handleSuccess, handleError);
        }

        function getAllTitles() {

            return $http.get('/api/customer/gettitles').then(handleSuccess, handleError);
        }

        function getAllAddressTypes() {

            return $http.get('/api/address/getaddresstypes').then(handleSuccess, handleError);
        }

        function getAll() {
            return $http.get('/api/product').then(handleSuccess, handleError);
        }

        //function searchText(pageIndex, pageSize, text, checked) {
        //    if (checked === false || checked === 'false') {
        //        return $http.get('/api/customer/searchactivecustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/customer/searchallcustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
        //    }
           
        //}

        function getCustomerAddressById(id) {
            return $http.get('/api/customer/searchcustomers/' + id).then(handleSuccess, handleError);
        }

        function getAllRoles() {
            return $http.get('/api/securitygroup').then(handleSuccess, handleError);
        }

        //function getInactiveCustomers(pageIndex, pageSize) {
        //    return $http.get('/api/customer/getallcustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        //function getCustomers(pageIndex, pageSize) {
        //    return $http.get('/api/customer/getactivecustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
            
        //}

        function getAllLevels() {
            return $http.get('/api/customerlevel/getallcustomerlevels').then(handleSuccess, handleError);
        }

        function activatecustomer(id) {
            return $http.get('/api/customer/activatecustomer/' + id).then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' +pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }

        function getById(id) {
            return $http.get('/api/customer/getcustomerbyid/' + id).then(handleSuccess, handleError);
        }

        function getLevelById(id) {
            return $http.get('/api/customerlevel/getcustomerlevelbyid/' + id).then(handleSuccess, handleError);
        }

        function create(customer) {

            return $http.post('/api/customer/createcustomer', customer).then(handleSuccess, handleError);
        }

        function getPaymentMethod() {
            return $http.get('/api/paymentmethod/searchPaymentMethods/'+true+'/'+null).then(handleSuccess, handleError);
        }

        function saveCustomerLevel(level) {

            return $http.put('/api/customerlevel/createcustomerlevel', level).then(handleSuccess, handleError);
        }

        function search(customer) {

            return $http.get('/api/customer', customer).then(handleSuccess, handleError);
        }

        function update(customer) {
            return $http.put('/api/customer/updatecustomerdetails', customer).then(handleSuccess, handleError);

        }

        function deleteCustomer(id) {
            return $http.delete('/api/customer/' + id).then(handleSuccess, handleError);
        }

        function checkCustomerCode(code) {
            return $http.post('/api/customer/checkcode/' + code).then(handleSuccess, handleError);
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/customer/getPageSize').then(handleSuccess, handleError);
        }
        function searchTextForCustomer(text, checked, pageIndex, pageSize) {

            return $http.get('/api/customer/searchCustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

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