(function () {
    'use strict';

    angular
        .module('app-purchaseOrder')
        .service('purchaseOrderService', purchaseOrderService);

    purchaseOrderService.$inject = ['$http'];

    function purchaseOrderService($http) {
        var service = {};
       // service.getActivePurchaseOrder = getActivePurchaseOrder;
       // service.getAllPurchaseOrder = getAllPurchaseOrder;
        service.getPurchaseOrderById = getPurchaseOrderById;
        //service.activePurchaseOrderById = activePurchaseOrderById;
        service.createPurchaseOrder = createPurchaseOrder;
        service.updatePurchaseOrder = updatePurchaseOrder;
        service.deletePurchaseOrder = deletePurchaseOrder;
        service.getDateByTerm = getDateByTerm;
        service.getActiveTax = getActiveTax;
        service.getAllActiveVendors = getAllActiveVendors;
        service.getAllActiveDeliveryMethods = getAllActiveDeliveryMethods;
        service.getPaymentTerm = getPaymentTerm;
        service.getCompanyUser = getCompanyUser;
        service.getDefaultValues = getDefaultValues;
        service.getAllActiveProduct = getAllActiveProduct;
      //  service.searchText = searchText;
        service.deletePurchaseOrderLines = deletePurchaseOrderLines;
        service.getDueDateByDateAndTermId = getDueDateByDateAndTermId;
        service.getAllActiveVendorsWithoutPaging = getAllActiveVendorsWithoutPaging;
        service.getActiveTaxWithoutPaging = getActiveTaxWithoutPaging;
        service.getCompanyUserWithoutPaging = getCompanyUserWithoutPaging;
        service.getAllActiveProductWithoutPaging = getAllActiveProductWithoutPaging;
        service.getAllDiscountTypes = getAllDiscountTypes;
        service.getDueDateByTermId = getDueDateByTermId;
        service.getPurchaseOrderTypes = getPurchaseOrderTypes;
        service.getPurchaseOrderStatuses = getPurchaseOrderStatuses;
        service.getDefaultCompanyWebSetting = getDefaultCompanyWebSetting;
        service.getPageSize = getPageSize;
        service.deletedSelected = deletedSelected;
        service.searchTextForPurchaseOrder = searchTextForPurchaseOrder;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/PurchaseOrder/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function getDefaultCompanyWebSetting() {
            return $http.get('/api/CompanyWebSetting/getcompanywebsetting').then(handleSuccess, handleError);
        }

        function getPurchaseOrderTypes() {
            return $http.get('/api/PurchaseOrder/getpurchaseordertypes').then(handleSuccess, handleError);
        }

        function getPurchaseOrderStatuses() {
            return $http.get('/api/PurchaseOrder/getpurchaseorderstatuses').then(handleSuccess, handleError);
        }

        function getDueDateByTermId(dateData) {
            return $http.post('/api/PurchaseOrder/GetDueDate/', dateData).then(handleSuccess, handleError);
        }

        //function getAllPurchaseOrder(pageIndex,pageSize) {
        //    return $http.get('/api/PurchaseOrder/getallpurchaseorders/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function getActivePurchaseOrder(pageIndex, pageSize) {
            return $http.get('/api/PurchaseOrder/getactivepurchaseorders/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        }

        function getPurchaseOrderById(id) {
            return $http.get('/api/PurchaseOrder/getpurchaseorder/' + id).then(handleSuccess, handleError);
        }

        //function activePurchaseOrderById(id) {
        //    return $http.get('/api/PurchaseOrder/activatpurchaseorder/' + id).then(handleSuccess, handleError);
        //}

        function createPurchaseOrder(purchaseOrder) {
            return $http.post('/api/PurchaseOrder/createneworder/', purchaseOrder).then(handleSuccess, handleError);
           // console.log(JSON.stringify(purchaseOrder));
        }

        function updatePurchaseOrder(purchaseOrder) {
            return $http.put('/api/PurchaseOrder/updatepurchaseorder', purchaseOrder).then(handleSuccess, handleError);
        }

        function deletePurchaseOrder(id) {
            return $http.delete('/api/PurchaseOrder/' + id).then(handleSuccess, handleError);
        }

        function getDateByTerm(id) {
            return $http.get('/api/paymentTerm/getduedate/' + id).then(handleSuccess, handleError);
        }

        function getAllActiveVendors(pageIndex, pageSize) {
            return $http.get('/api/Vendor/getactiveVendors/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
            
        }

        //function getActiveTax(pageIndex, pageSize) {
        //    return $http.get('/api/tax/searchTaxes/' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function getActiveTax(pageIndex, pageSize) {
            return $http.get('/api/tax/searchTaxes/' +true+'/'+null).then(handleSuccess, handleError);
        }

        function getActiveTaxWithoutPaging() {
            return $http.get('/api/tax/activetaxeswithoutpaging').then(handleSuccess, handleError);
        }

       function getAllActiveDeliveryMethods()
        {
            
           return $http.get('/api/DeliveryMethod/searchDeliveryMethods/'+true+'/'+"null").then(handleSuccess, handleError);
       }

       function getPaymentTerm() {
           return $http.get('/api/paymentTerm/searchPaymentTerms/'+true+'/'+"null").then(handleSuccess, handleError);

       }


       function getCompanyUser() {
           return $http.get('/api/user/getactiveusers/').then(handleSuccess, handleError);
       }

       function getCompanyUserWithoutPaging() {
           return $http.get('/api/user/activeuserswithoutpaging/').then(handleSuccess, handleError);
       }

       function getDefaultValues(id) {
           return $http.get('/api/PurchaseOrder/getdefaults/' + id).then(handleSuccess, handleError);
       }
     

       function getAllActiveProduct() {
           return $http.get('/api/Product/getactiveproducts/').then(handleSuccess, handleError);
       }

       function getAllActiveProductWithoutPaging() {
           return $http.get('/api/Product/getallactiveproducts/').then(handleSuccess, handleError);
       }

 
       //function searchText(pageIndex, pageSize, text, active) {
       //    if (active === 'false' || active === false) {
       //        return $http.get('/api/PurchaseOrder/searchactivepurchaseorders/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
       //    }
       //    return $http.get('/api/PurchaseOrder/searchallpurchaseorders/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
       //}
       function buildUriForPaging(pageIndex, pageSize) {
           var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
           return uri;
       }

       function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
           var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
           return uri;
       }
     
       function deletePurchaseOrderLines(id) {
           return $http.delete('/api/PurchaseOrderLine/' + id).then(handleSuccess, handleError);
       }

       function getDueDateByDateAndTermId(termId, date) {
           //console.log("termId"+termId+"date:"+date);
           return $http.post('/api/salesOrder/getduedate/' + termId, date).then(handleSuccess, handleError);
       }

      function getAllActiveVendorsWithoutPaging(){
          return $http.get('/api/Vendor/activevendorwithoutpagaing').then(handleSuccess, handleError);
       }

      function getAllDiscountTypes() {
          return $http.get('/api/PurchaseOrder/getdiscounttypes').then(handleSuccess, handleError);
      }
    //Get Page size
      function getPageSize() {
          return $http.get('/api/PurchaseOrder/getPageSize').then(handleSuccess, handleError);
      }
      function searchTextForPurchaseOrder(text, checked, pageIndex, pageSize) {

          return $http.get('/api/PurchaseOrder/searchPurchaseOrders/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                  .then(handleSuccess, handleError);

      }
        //// private functions

        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();