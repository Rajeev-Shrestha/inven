(function () {
    'use strict';

    angular
        .module('app-discountSetting')
        .service('discountSettingService', discountSettingService);

    discountSettingService.$inject = ['$http'];

    function discountSettingService($http) {
        var service = {};
        service.activeDiscount = activeDiscount;
        service.getDiscountById = getDiscountById;
       // service.getActiveDiscount = getActiveDiscount;
        service.deleteDiscount = deleteDiscount;
        service.updateDiscount = updateDiscount;
        service.createDiscount = createDiscount;
        service.getCustomer = getCustomer;
       // service.inactiveDiscount = inactiveDiscount;
        //service.searchDiscount = searchDiscount;
        service.getCustomerLevel = getCustomerLevel;
        service.getDiscountTypes = getDiscountTypes;
        service.getAllProduct = getAllProduct;
        service.getAllProductCategory = getAllProductCategory;
        service.searchTextForDiscountSetting = searchTextForDiscountSetting;
        service.getPageSize = getPageSize;
        service.deletedSelected = deletedSelected;
       // service.checkIfDiscoutExistForThisProduct = checkIfDiscoutExistForThisProduct;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/discount/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        //function searchDiscount(pageIndex, pageSize, text, active) {
        //    if (active) {
        //        return $http.get('/api/Discount/searchactiveProductPriceRules/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/Discount/searchallProductPriceRules/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
        //    }
        //}

        function getCustomerLevel() {
            return $http.get('/api/CustomerLevel/getallcustomerlevels').then(handleSuccess, handleError);
        }

        function getCustomer() {
            return $http.get('/api/Customer/activecustomerwithoutpagaing/').then(handleSuccess, handleError);
        }

        function createDiscount(discount) {
            return $http.post('/api/discount/creatediscount', discount).then(handleSuccess, handleError);
        }

        function updateDiscount(discount) {
            return $http.put('/api/Discount/updateDiscount/', discount).then(handleSuccess, handleError);
        }

        function deleteDiscount(id) {
            return $http.delete('/api/Discount/' + id).then(handleSuccess, handleError);
        }

        //function getActiveDiscount(pageIndex, pageSize) {
        //    return $http.get('/api/Discount/getactiveDiscounts/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        //function inactiveDiscount(pageIndex, pageSize) {
        //    return $http.get('/api/Discount/getallDiscounts' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function getDiscountById(id) {
            return $http.get('/api/Discount/getDiscountbyid/' + id).then(handleSuccess, handleError);
        }

        function activeDiscount(id) {
            return $http.get('/api/Discount/activateDiscount/' + id).then(handleSuccess, handleError);
        }

        function getDiscountTypes() {
            return $http.get('/api/Discount/getdiscounttypes/').then(handleSuccess, handleError);
        }

        function getAllProductCategory() {
            return $http.get('/api/ProductCategory/getallcategories').then(handleSuccess, handleError);
        }

        function getAllProduct() {
            return $http.get('/api/Product/getallactiveproducts').then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/discount/getPageSize').then(handleSuccess, handleError);
        }
        function searchTextForDiscountSetting(text, active, pageIndex, pageSize) {

            return $http.get('/api/discount/getDiscounts/' + buildUriForSearchPaging(pageIndex, pageSize, text, active))
                    .then(handleSuccess, handleError);
        }
        //function checkIfDiscoutExistForThisProduct(itemId) {
        //    return $http.get('/api/Discount/getDiscountByProductId/' + itemId).then(handleSuccess, handleError);
        //}
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