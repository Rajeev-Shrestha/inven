(function () {
    'use strict';

    angular
        .module('app-salesOrder')
        .service('salesOrderService', salesOrderService);

    salesOrderService.$inject = ['$http'];

    function salesOrderService($http) {
        var service = {};
        service.getAllUser = getAllUser;
        service.getAllProduct = getAllProduct;
        service.getPaymentTerm = getPaymentTerm;
        service.createSalesOrder = createSalesOrder;
        service.getDeliveryMethods = getDeliveryMethods;
        //service.getOrderList = getOrderList;
        service.getUserAddressById = getUserAddressById;
        service.addCustomerAddress = addCustomerAddress;
        service.getDateByTerm = getDateByTerm;
        service.getCustomer = getCustomer;
        service.getActiveTax = getActiveTax;
        service.getCompanyUser = getCompanyUser;
        service.getdefaults = getdefaults;
        service.deleteSalesOrder = deleteSalesOrder;
        service.updateSalesOrder = updateSalesOrder;
        //service.getAllOrderList = getAllOrderList;
        service.getById = getById;
       // service.activeSalesOrder = activeSalesOrder;
        //service.searchText = searchText;
        service.deleteSalesOrderLines = deleteSalesOrderLines;
        service.getDueDateByDateAndTermId = getDueDateByDateAndTermId;
        service.getPaymentMethod = getPaymentMethod;
        service.getAllCustomerWithoutPaging = getAllCustomerWithoutPaging;
        service.getActiveTaxWithoutPaging = getActiveTaxWithoutPaging;
        service.getCompanyUserWithoutPaging = getCompanyUserWithoutPaging;
        service.getAllProductWithoutPaging = getAllProductWithoutPaging;
        service.getAllDiscountTypes = getAllDiscountTypes;
        service.getDueDateByTermId = getDueDateByTermId;
        service.getSalesOrderTypes = getSalesOrderTypes;
        service.getSalesOrderStatuses = getSalesOrderStatuses;
        service.getDefaultCompanyWebSetting = getDefaultCompanyWebSetting;
        service.searchTextForSalesOrder = searchTextForSalesOrder;
        service.getPageSize = getPageSize;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/salesOrder/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        function getDefaultCompanyWebSetting() {
            return $http.get('/api/CompanyWebSetting/getcompanywebsetting').then(handleSuccess, handleError);
        }

        function getSalesOrderTypes() {
            return $http.get('/api/salesOrder/getsalesordertypes').then(handleSuccess, handleError);
        }

        function getSalesOrderStatuses() {
            return $http.get('/api/salesOrder/getsalesorderstatuses').then(handleSuccess, handleError);
        }

        function getDueDateByTermId(dateData) {
            return $http.post('/api/PurchaseOrder/GetDueDate/', dateData).then(handleSuccess, handleError);
        }

        function getPaymentMethod() {
            return $http.get('/api/PaymentMethod/searchPaymentMethods/' + true + '/' + "null").then(handleSuccess, handleError);
        }
        //function activeSalesOrder(id) {
        //    return $http.get('/api/salesOrder/activatesalesorder/' + id).then(handleSuccess, handleError);
        //}

        function getById(id) {
            return $http.get('/api/salesOrder/getsalesorder/' + id).then(handleSuccess, handleError);
        }

        function getCustomer(pageIndex, pageSize) {
            return $http.get('/api/customer/getactivecustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        function getAllCustomerWithoutPaging(){
            return $http.get('/api/customer/activecustomerwithoutpagaing/').then(handleSuccess, handleError);
        }

        function getAllUser(pageIndex, pageSize) {
            return $http.get('/api/customer/getactivecustomers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        function deleteSalesOrder(id) {
            return $http.delete('/api/salesOrder/' + id).then(handleSuccess, handleError);
        }

        //function getOrderList(pageIndex, pageSize) {
        //    return $http.get('/api/salesOrder/getactivesalesorders/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        //function getAllOrderList(pageIndex, pageSize) {
        //    return $http.get('/api/salesOrder/getallsalesorders/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function getDateByTerm(id) {
            return $http.get('/api/paymentTerm/getduedate/' + id).then(handleSuccess, handleError);
        }

        function getdefaults(id) {
            return $http.get('/api/salesOrder/getdefaults/' + id).then(handleSuccess, handleError);
        }

        function getAllProduct(pageIndex, pageSize) {
            return $http.get('/api/product/getactiveproducts/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }
        
        function getAllProductWithoutPaging() {
            return $http.get('/api/product/getallactiveproducts/').then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function getDeliveryMethods() {
            return $http.get('/api/deliveryMethod/searchDeliveryMethods/' + true + '/' + "null").then(handleSuccess, handleError);
        }

        //function searchText(pageIndex, pageSize,text , active) {
        //    if (active === 'false' || active === false) {
        //        return $http.get('/api/salesOrder/searchactivesalesorders/' +
        //                buildUriForSearchPaging(pageIndex, pageSize, text, active))
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/salesOrder/searchallsalesorders/' +
        //                buildUriForSearchPaging(pageIndex, pageSize, text, active))
        //            .then(handleSuccess, handleError);
        //    }
        //}

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }

        function getCompanyUser(pageIndex, pageSize) {
            return $http.get('/api/user/getactiveusers/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }
        function getCompanyUserWithoutPaging() {
            return $http.get('/api/user/activeuserswithoutpaging/').then(handleSuccess, handleError);
        }

        

        function getPaymentTerm() {
            return $http.get('/api/paymentTerm/searchPaymentTerms/' + true + '/' + "null").then(handleSuccess, handleError);
        }

        

        function getActiveTax(pageIndex, pageSize) {
            return $http.get('/api/tax/searchTaxes/' + true+'/'+"null").then(handleSuccess, handleError);
        }

        function getActiveTaxWithoutPaging() {
            return $http.get('/api/tax/activetaxeswithoutpaging').then(handleSuccess, handleError);
        }

        function createSalesOrder(salesOrder) {
            return $http.post('/api/salesOrder/createneworder', salesOrder).then(handleSuccess, handleError);
        }

        function updateSalesOrder(salesOrder) {
            return $http.put('/api/salesOrder/updateorder', salesOrder).then(handleSuccess, handleError);
            
        }

        function addCustomerAddress(address) {
            return $http.post('/api/customer/savecustomeraddress/', address).then(handleSuccess, handleError);
        }

        function getUserAddressById(id) {
            return $http.get('/api/customer/getcustomerbyid/' + id).then(handleSuccess, handleError);
        }


        function deleteSalesOrderLines(id) {
            return $http.delete('/api/salesOrderLine/' + id).then(handleSuccess, handleError);
        }
       
        function getDueDateByDateAndTermId(termId, date) {
            //console.log("termId"+termId+"date:"+date);
           return $http.post('/api/salesOrder/getduedate/'+termId, date).then(handleSuccess, handleError);
        }


        function getAllDiscountTypes() {
            return $http.get('/api/salesOrder/getdiscounttypes').then(handleSuccess, handleError);
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/salesOrder/getPageSize').then(handleSuccess, handleError);
        }
        function searchTextForSalesOrder(text, checked, pageIndex, pageSize) {

            return $http.get('/api/salesOrder/searchSalesOrders/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
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