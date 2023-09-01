(function () {
    'use strict';

    angular
        .module('app-myaccount')
        .factory('myaccountService', myaccountService);
    myaccountService.$inject = ['$http', '$sessionStorage'];

    function myaccountService($http, $sessionStorage) {
            var service = {};
            service.crmLocation = crmLocation;
            service.getLoginUserId = getLoginUserId;
            service.GetById = GetById;
            service.Create = Create;
            service.Update = Update;
            service.deleteCustomer = deleteCustomer;
            service.GetCustomers = GetCustomers;
            service.activatecustomer = activatecustomer;
            service.GetAllLevels = GetAllLevels;
            service.GetLevelById = GetLevelById;
            service.SaveCustomerLevel = SaveCustomerLevel;
            service.GetPaymentMethod = GetPaymentMethod;
            service.getInactiveCustomers = getInactiveCustomers;
            service.Search = Search;
            service.getAllRoles = getAllRoles;
            service.searchText = searchText;
            service.getCustomerAddressById = getCustomerAddressById;
            service.checkCustomerCode = checkCustomerCode;
            service.getOrderSummary = getOrderSummary;
            service.salesOrderById = salesOrderById;
            service.getAllUser = getAllUser;
            service.getDeliveryMethods = getDeliveryMethods;
            service.getPaymentTerm = getPaymentTerm;
            //service.getCompanyUser = getCompanyUser;
            service.getAllProduct = getAllProduct;
            service.getActiveTax = getActiveTax;
            service.getMyOrders = getMyOrders;
            service.getAllSuffixes = getAllSuffixes;
            service.getAllTitles = getAllTitles;
            service.getActiveZones = getActiveZones;
            service.getAllAddressTypes = getAllAddressTypes;
            service.getAllCountries = getAllCountries;
            return service;

        function getAllCountries() {
            var apiUrl = "/api/customer/getcountries";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);

        }

            function getMyOrders(id) {
                var apiUrl = "/api/customer/getorderssummary/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getActiveTax() {
                var apiUrl = "/api/tax/getallactivetaxes/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getAllProduct() {
                var apiUrl = "/api/product/getallactiveproducts/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            //function getCompanyUser() {
            //    var apiUrl = "/api/user/getactiveusers/";
            //    return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //}
            function getPaymentTerm() {
                var apiUrl = "/api/PaymentTerm/getactivepaymentterms/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getDeliveryMethods() {
                var apiUrl = "/api/DeliveryMethod/getactivedeliverymethods/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getAllUser() {
                var apiUrl = "/api/User/getactiveusers/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function salesOrderById(id) {
                var apiUrl = "/api/SalesOrder/getsalesorder/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getOrderSummary(id) {
                var apiUrl = "/api/Customer/getorderssummary/"+ id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getLoginUserId() {
                return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
            }
            function crmLocation() {
                return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
            }




        function searchText(pageIndex, pageSize, text, checked) {
            if (checked === 'false') {
                var apiUrl = "/api/customer/searchactivecustomers/" +buildUriForSearchPaging(pageIndex, pageSize, text, checked);
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
                //return $http.get('/api/customer/searchactivecustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
            } else {
                var apiUrl = "/api/customer/searchallcustomers/" + buildUriForSearchPaging(pageIndex, pageSize, text, checked);
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
                //return $http.get('/api/customer/searchallcustomers/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked)).then(handleSuccess, handleError);
            }
           
        }

        function getCustomerAddressById(id) {
            var apiUrl = "/api/customer/searchcustomers/" + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer/searchcustomers/' + id).then(handleSuccess, handleError);
        }

        function getAllRoles() {
            var apiUrl = "/api/securitygroup";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/securitygroup').then(handleSuccess, handleError);
        }

        function getInactiveCustomers(pageIndex, pageSize) {
            var apiUrl = "/api/customer/getallcustomers/" + buildUriForPaging(pageIndex, pageSize);
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer/getallcustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        function GetCustomers(pageIndex, pageSize) {
            var apiUrl = "/api/customer/getactivecustomers/";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer/getactivecustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
            
        }

        function GetAllLevels() {
            var apiUrl = "/api/customerlevel/getallcustomerlevels";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }

        function activatecustomer(id) {
            var apiUrl = "/api/customer/activatecustomer/" + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer/activatecustomer/' + id).then(handleSuccess, handleError);
        }


        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pagenumber=' + pageIndex + '&pageSize=' +pageSize;
            return uri;
        }
        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pagenumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        function GetById(id) {
            var apiUrl = "/api/customer/getcustomerbyid/" + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer/' + id).then(handleSuccess, handleError);
        }

        function GetLevelById(id) {
            var apiUrl = "/api/customerlevel/" + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customerlevel/' + id).then(handleSuccess, handleError);
        }

        function Create(Customer) {
            var apiUrl = "/api/customer/createcustomer";
            var dataToPost = { url: apiUrl, data: Customer }
            return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
            //return $http.post('/api/customer/createcustomer', Customer).then(handleSuccess, handleError);
        }

        function GetPaymentMethod() {
            var apiUrl = "/api/paymentmethod/getactivepaymentmethods";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/paymentmethod/getactivepaymentmethods').then(handleSuccess, handleError);
        }

        function SaveCustomerLevel(Customer) {
            var apiUrl = "/api/customerlevel";
            var dataToPost = { url: apiUrl, data: Customer }
            return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
            //return $http.put('/api/customerlevel', Customer).then(handleSuccess, handleError);
        }

        function Search(Customer) {

            var apiUrl = "/api/customer" + Customer;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            //return $http.get('/api/customer', Customer).then(handleSuccess, handleError);
        }

        function Update(Customer) {
            var apiUrl = "/api/customer/updatecustomerdetails";
            var dataToPost = { url: apiUrl, data: Customer }
            return $http.put('/api/putwithdata', dataToPost).then(handleSuccess, handleError);
            //return $http.put('/api/customer', Customer).then(handleSuccess, handleError);

        }

        function deleteCustomer(id) {
            return $http.delete('/api/customer/' + id).then(handleSuccess, handleError);
        }

        function checkCustomerCode(code) {
            return $http.post('/api/customer/checkcode/' + code).then(handleSuccess, handleError);
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
            var apiUrl = "/api/DeliveryZone/searchDeliveryZones/"+true+"/"+null;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }

        function getAllAddressTypes() {
            var apiUrl = "/api/address/getaddresstypes/";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }

         // private functions

        function handleSuccess(res) {
                return { success: true, data: res.data };
            }

        function handleError(error) {
                console.log(JSON.stringify(error.data));
                if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

                return { success: false, message: error.data };

            }


    }
})();