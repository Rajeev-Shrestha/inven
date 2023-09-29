(function () {
    'use strict';

    angular
        .module('app-vendor') 
        .service('vendorService', vendorService);

    vendorService.$inject = ['$http'];

    function vendorService($http) {
        var service = {};
        service.GetAll = getAll;
        service.GetById = getById;
        service.Create = create;
        service.Update = update;
        service.deletevendor = deletevendor;
        //service.Getvendors = getvendors;
        service.activatevendor = activatevendor;
        service.GetAllLevels = getAllLevels;
        service.GetLevelById = getLevelById;
        service.SavevendorLevel = savevendorLevel;
        service.GetPaymentMethod = getPaymentMethod;
       // service.getInactivevendors = getInactivevendors;
        //service.Search = search;
        service.getAllRoles = getAllRoles;
       // service.searchText = searchText;
        service.getvendorAddressById = getvendorAddressById;
        service.checkvendorCode = checkvendorCode;
        service.getAllSuffixes = getAllSuffixes;
        //service.getAllCountries = getAllCountries;
        service.getAllTitles = getAllTitles;
        service.getAllAddressTypes = getAllAddressTypes; 
        service.getAccountTerm = getAccountTerm;
        service.getAllAddresses = getAllAddresses;
        service.activateAddressById = activateAddressById;
        service.checkIfVendorCodeExists = checkIfVendorCodeExists;
        service.checkIfUserEmailExists = checkIfUserEmailExists;
        service.getPageSize = getPageSize;
        service.searchTextForVendor = searchTextForVendor;
        service.deletedSelected = deletedSelected;

        return service;
        

        // private functions
        function deletedSelected(selectedList) {
            return $http.post('/api/Vendor/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function checkIfUserEmailExists(email) {
            return $http.get('/api/user/checkIfUserEmailExists/' + email).then(handleSuccess, handleError);
        }
        function checkIfVendorCodeExists(code) {
            return $http.get('/api/Vendor/CheckIfDeletedVendorWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function getAllAddresses(id) {
            return $http.get('/api/Vendor/getVendorAllAddresses/' + id).then(handleSuccess, handleError);
        }

        function activateAddressById(addressId) {
            return $http.get('/api/Address/activateaddress/' + addressId).then(handleSuccess, handleError);
        }

        function getAccountTerm() {
            return $http.get('/api/PaymentTerm/getOnAccountTerms').then(handleSuccess, handleError);

        }
        function getAllSuffixes() {

            return $http.get('/api/customer/getsuffixes').then(handleSuccess, handleError);
        }

        //function getAllCountries() {

        //    return $http.get('/api/customer/getcountries').then(handleSuccess, handleError);
        //}

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
        //    if (checked === 'false'||checked===false) {
        //        return $http.get('/api/vendor/searchactivevendors/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/vendor/searchallvendors/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
        //    }

        //}

        function getvendorAddressById(id) {
            return $http.get('/api/vendor/searchvendors/' + id).then(handleSuccess, handleError);
        }

        function getAllRoles() {
            return $http.get('/api/securitygroup/getallsecuritygroup').then(handleSuccess, handleError);
        }

        //function getInactivevendors(pageIndex, pageSize) {
        //    return $http.get('/api/vendor/getallvendors/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        //function getvendors(pageIndex, pageSize) {
        //    return $http.get('/api/vendor/getactivevendors/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);

        //}

        function getAllLevels() {
            return $http.get('/api/vendorlevel/getallvendorlevels').then(handleSuccess, handleError);
        }

        function activatevendor(id) {
            return $http.get('/api/vendor/activatevendor/' + id).then(handleSuccess, handleError);
        }


        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }
        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        function getById(id) {
            return $http.get('/api/vendor/getvendorbyid/' + id).then(handleSuccess, handleError);
        }

        function getLevelById(id) {
            return $http.get('/api/vendorlevel/getvendorlevelbyid/' + id).then(handleSuccess, handleError);
        }

        function create(vendor) {

            return $http.post('/api/vendor/createvendor', vendor).then(handleSuccess, handleError);
        }

        function getPaymentMethod() {
            return $http.get('/api/paymentmethod/getactivepaymentmethods').then(handleSuccess, handleError);
        }

        function savevendorLevel(level) {

            return $http.put('/api/vendorlevel/createvendorlevel', level).then(handleSuccess, handleError);
        }

        //function search(vendor) {

        //    return $http.get('/api/vendor', vendor).then(handleSuccess, handleError);
        //}

        function update(vendor) {
           
            return $http.put('/api/vendor/updatevendor', vendor).then(handleSuccess, handleError);

        }

        function deletevendor(id) {
            return $http.delete('/api/vendor/' + id).then(handleSuccess, handleError);
        }

        function checkvendorCode(code) {
            return $http.post('/api/vendor/checkcode/' + code).then(handleSuccess, handleError);
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/vendor/getPageSize').then(handleSuccess, handleError);
        }
        //search products
        function searchTextForVendor(text, checked, pageIndex, pageSize) {

            return $http.get('/api/vendor/getVendors/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

        }
        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();