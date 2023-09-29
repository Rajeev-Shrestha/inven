(function () {
    'use strict';

    angular
        .module('app-paymentTerm')
        .service('paymentTermService', paymentTermService);

    paymentTermService.$inject = ['$http'];

    function paymentTermService($http) {
        var service = {};
        service.createPaymentTerm = createPaymentTerm;
        service.updatePaymentTerm = updatePaymentTerm;
        service.deletePaymentTerm = deletePaymentTerm;
        service.getPaymentByTermId = getPaymentByTermId;
        service.savePaymentRoles = savePaymentRoles;
       // service.getInactivePaymentTerm = getInactivePaymentTerm;
      //  service.searchTextForPaymentTerm = searchTextForPaymentTerm;
        service.GetAllPaymentTermTypes = getAllPaymentTermTypes;
        service.GetAllPaymentDiscountTypes = getAllPaymentDiscountTypes;
        service.GetAllDueTypes = getAllDueTypes;
        service.GetAllDueDateTypes = getAllDueDateTypes;
        service.getPaymentTermWithoutAccountType = getPaymentTermWithoutAccountType;
        //service.getPaymentTerm = getPaymentTerm;
        service.GetAll = getAll;
        service.activeTerm = activeTerm;
        service.getPaymentTermById = getPaymentTermById;
        service.checkIfPaymentTermCodeExists = checkIfPaymentTermCodeExists;
        service.checkIfPaymentTermNameExists = checkIfPaymentTermNameExists; 
        service.searchTextForPaymentTerm = searchTextForPaymentTerm;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/paymentTerm/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        function checkIfPaymentTermNameExists(name) {
            return $http.get('/api/paymentTerm/CheckIfDeletedPaymentTermWithSameNameExists/' + name).then(handleSuccess, handleError);
        }

        function checkIfPaymentTermCodeExists(code) {
            return $http.get('/api/paymentTerm/CheckIfDeletedPaymentTermWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function getAll() {
            return $http.get('/api/PaymentMethod/searchPaymentMethods/' + true + '/' + "null").then(handleSuccess, handleError);
        }

        //function searchTextForPaymentTerm(text, checked) {
        //    if (checked === 'false' || checked === false) {
        //        return $http.get('/api/PaymentTerm/searchactivepaymentterms/' + text)
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/PaymentTerm/searchallpaymentterms/' + text)
        //           .then(handleSuccess, handleError);
        //    }
        //}

        function activeTerm(id) {
            return $http.get('/api/paymentTerm/activatepaymenterm/' + id).then(handleSuccess, handleError);
        }

        //function getInactivePaymentTerm() {
        //    return $http.get('/api/paymentTerm/getallpaymentterms').then(handleSuccess, handleError);
        //}

        //function getPaymentTerm() {
        //    return $http.get('/api/paymentTerm/getactivepaymentterms').then(handleSuccess, handleError);
        //}

        function getPaymentByTermId(id) {
            return $http.get('/api/paymentTerm/getPayMethodsInPayTerm/' + id).then(handleSuccess, handleError);
        }

        function createPaymentTerm(deliveryMethod) {
            return $http.post('/api/PaymentTerm/', deliveryMethod).then(handleSuccess, handleError);
        }

        function savePaymentRoles(roles) {
            return $http.post('/api/PaymentTerm/savePayMethodInPayTerms/', roles).then(handleSuccess, handleError);
        }

        function getPaymentTermWithoutAccountType() {
            return $http.get('/api/PaymentTerm/getTermsWithoutAccountType/').then(handleSuccess, handleError);
        }

        function updatePaymentTerm(paymentTerm) {
            return $http.put('/api/PaymentTerm', paymentTerm).then(handleSuccess, handleError);
        }

        function deletePaymentTerm(item) {
            return $http.delete('/api/PaymentTerm/' + item).then(handleSuccess, handleError);
        }

        function getAllPaymentTermTypes() {
            return $http.get('/api/PaymentTerm/gettermtypes').then(handleSuccess, handleError);
        }

        function getAllPaymentDiscountTypes() {
            return $http.get('/api/PaymentTerm/getpaymentdiscounttypes').then(handleSuccess, handleError);
        }

        function getAllDueTypes() {
            return $http.get('/api/PaymentTerm/getduetypes').then(handleSuccess, handleError);
        }

        function getAllDueDateTypes() {
            return $http.get('/api/PaymentTerm/getduedatetypes').then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }


        function getPaymentTermById(id) {
            return $http.get('/api/PaymentTerm/getpaymenttermbyid/'+id).then(handleSuccess, handleError);
            
        }
        function searchTextForPaymentTerm(searchText, active) {
            var api = '/api/PaymentTerm/searchPaymentTerms/' + active + '/' + searchText;
            return $http.get(api).then(handleSuccess, handleError);
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