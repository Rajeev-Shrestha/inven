(function () {
    'use strict';

    angular
        .module('app-paymentMethod')
        .service('paymentMethodService', paymentMethodService);

    paymentMethodService.$inject = ['$http'];

    function paymentMethodService($http) {
        var service = {};
       // service.GetAll = getAll;
        service.UpdatePaymentMethod = updatePaymentMethod;
        service.CreatePaymentMethod = createPaymentMethod;
        service.Delete = Delete;
      //  service.searchTextForPaymentMethod = searchTextForPaymentMethod;
        //service.getInactivePaymentMethod = getInactivePaymentMethod;
        service.activeMethod = activeMethod;
        service.getPaymentMethodById = getPaymentMethodById;
        service.checkIfPaymentMethodCodeExists = checkIfPaymentMethodCodeExists;
        service.checkIfPaymentMethodNameExists = checkIfPaymentMethodNameExists;
        service.searchTextForPaymentMethod = searchTextForPaymentMethod;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/PaymentMethod/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        function checkIfPaymentMethodNameExists(name) {
            return $http.get('/api/PaymentMethod/CheckIfDeletedPaymentMethodWithSameNameExists/' + name).then(handleSuccess, handleError);
        }

        function checkIfPaymentMethodCodeExists(code) {
            return $http.get('/api/PaymentMethod/CheckIfDeletedPaymentMethodWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function activeMethod(id) {
            return $http.get('/api/PaymentMethod/activatepaymentmethod/' + id).then(handleSuccess, handleError);
        }

        //function getInactivePaymentMethod() {
        //    return $http.get('/api/PaymentMethod/getallpaymentmethods/').then(handleSuccess, handleError);
        //}

        //function getAll() {
        //    return $http.get('/api/PaymentMethod/getactivepaymentmethods').then(handleSuccess, handleError);
        //}

        function createPaymentMethod(paymentMethod) {
            return $http.post('/api/PaymentMethod/', paymentMethod).then(handleSuccess, handleError);
        }

        function updatePaymentMethod(paymentMethod) {
            return $http.put('/api/PaymentMethod', paymentMethod).then(handleSuccess, handleError);
        }

        //function searchTextForPaymentMethod(text, checked) {
        //    if (checked === 'false' || checked === false) {
        //        return $http.get('/api/PaymentMethod/searchactivepaymentmethods/' + text)
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/PaymentMethod/searchallpaymentmethods/' + text)
        //           .then(handleSuccess, handleError);
        //    }
        //}

        function Delete(item) {
            return $http.delete('/api/PaymentMethod/' + item).then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }

        function getPaymentMethodById(methodId) {
            return $http.get('/api/PaymentMethod/getpaymentmethodbyid/'+methodId).then(handleSuccess, handleError);
        }
        function searchTextForPaymentMethod(searchText, active) {
            var api = '/api/PaymentMethod/searchPaymentMethods/' + active + '/' + searchText;
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