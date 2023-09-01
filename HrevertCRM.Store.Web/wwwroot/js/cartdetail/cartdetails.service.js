(function () {
    'use strict';

    angular
        .module('app-cartdetails')
        .service('cartdetailsService', cartdetailsService);

    cartdetailsService.$inject = ['$http'];

    function cartdetailsService($http) {
        var service = {};
        service.getCartItems = getCartItems;
        service.removeCartItem = removeCartItem;
        service.updateCartItem = updateCartItem;
        service.getLoginUser = getLoginUser;
        service.getItemsById = getItemsById;
        service.getLatestProduct = getLatestProduct;
        return service;

        function getLatestProduct(pageIndex, pageSize) {
            var apiUrl = '/api/Product/lastestproducts/' + buildUriForPaging(pageIndex, pageSize);
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getLoginUser() {
            return $http.get('/api/getloggedinuser').then(handleSuccess, handleError);
        }
        function updateCartItem(item) {
            var apiUrl = "/api/ShoppingCart/UpdateShoppingCartDetail";
            var dataToPost = { url: apiUrl, data: item }
            return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
        }
        function getCartItems(id, customerId, guid) {
            var apiUrl = "/api/ShoppingCart/getshoppingcart/" + id + "/" + customerId + "/" + guid;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function removeCartItem(id) {
            var apiUrl = "/api/ShoppingCart/removeproductfromcart/" + id;
            return $http.post('/api/postdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function getItemsById(customerId, id) {
            var apiUrl = "/api/product/GetProductForStoreFront/" + customerId + "/" + id;
            return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
        }
        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
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