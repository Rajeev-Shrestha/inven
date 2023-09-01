(function () {
    'use strict';

    angular.module('app-layout')
        .factory('layoutService', layoutService);
    
    layoutService.$inject = ['$http', '$rootScope', '$sessionStorage'];

    function layoutService($http, $rootScope, $sessionStorage) {
        var service = {};
        
        service.getCategories = getCategories;
        service.getCartItems = getCartItems;
        service.getLoggedInUser = getLoggedInUser;
        service.logout = logout;
        service.getAllCategory = getAllCategory;
        service.getCartItem = getCartItem;
        service.searchProduct = searchProduct;
        service.getCrmLocation = getCrmLocation;
        service.removeCartItem = removeCartItem;
        service.bugReport = bugReport;
        service.getCompanyLogo = getCompanyLogo;
        service.getAllFooterDetails = getAllFooterDetails;
        service.getThemeGeneralSettings = getThemeGeneralSettings;
        service.getThemeColor = getThemeColor;
        service.getHeader = getHeader;
        return service;

        function getHeader() {
            var apiUrl = "/api/HeaderSetting/getHeaderSetting";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getThemeGeneralSettings() {
            var apiUrl = "/api/GeneralSetting/getAll";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }

        function getThemeColor() {
            var apiUrl = "/api/GeneralSetting/getAll";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getAllFooterDetails() {
            var apiUrl = "/api/FooterSetting/getAll";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }

        function getCompanyLogo() {
            var apiUrl = "/api/EcommerceSetting/getactiveEcommerceSettings/";
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function bugReport(bug) {
            var apiUrl = "/api/BugLogger/reportBug/";
            var dataToPost = { url: apiUrl, data: bug }
            return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
        }
        function removeCartItem(id) {
            var apiUrl = "/api/ShoppingCart/removeproductfromcart/" + id;
            return $http.post('/api/postdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getCrmLocation() {
            return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
        }
        function searchProduct(text, id) {
            var apiUrl = "/api/Product/SearchProductByCategoryIdAndText/" + id + "/" + text;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getCartItem(id) {
            var apiUrl = "/api/ShoppingCart/getshoppingcart/" + id;
            var result = $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            $rootScope.$broadcast("func_call", { myParam: result });
        }
        function getAllCategory() {
            var apiUrl = '/api/ProductCategory/getactivecategories';
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }


        function logout() {
            return $http.get('/account/Logout').then(handleSuccess, handleError);
        }
        function getLoggedInUser() {
            return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
        }
        function getCartItems(id,customerId) {
            var apiUrl = "/api/ShoppingCart/getshoppingcart/" + id+"/"+customerId;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            viewModelHelper.apiGet(apiUrl,
               null,
               function (result) {
                   return result.data;
               });
        }

        function getCategories() {
            var apiUrl = "/api/ProductCategory/categorytree";
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