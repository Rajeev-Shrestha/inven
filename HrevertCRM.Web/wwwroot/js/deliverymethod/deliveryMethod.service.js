(function () {
    'use strict';

    angular
        .module('app-deliveryMethod')
        .service('deliveryMethodService', deliveryMethodService);

    deliveryMethodService.$inject = ['$http'];

    function deliveryMethodService($http) {
        var service = {};
       // service.GetAllDelivery = getAllDelivery;
        service.deleteDelivery = deleteDelivery;
        service.createDeliveryMethod = createDeliveryMethod;
        service.updateDeliveryMethod = updateDeliveryMethod;
        //service.GetInactiveDelivery = getInactiveDelivery;
        service.activeDeliveryMethod = activeDeliveryMethod;
        service.getDeliveryMethodById = getDeliveryMethodById;
        //service.searchTextForDeliveryMethod = searchTextForDeliveryMethod;
        service.checkIfDeliveryMethodCodeExists = checkIfDeliveryMethodCodeExists;
        service.searchTextForDeliveryMethods = searchTextForDeliveryMethods;
        service.deletedSelected = deletedSelected;
        return service;
        function deletedSelected(selectedList) {
            return $http.post('/api/DeliveryMethod/bulkDelete', selectedList).then(handleSuccess, handleError);
        }

        function checkIfDeliveryMethodCodeExists(code) {
            return $http.get('/api/DeliveryMethod/CheckIfDeletedDeliveryMethodWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        //function searchTextForDeliveryMethod(text, checked) {
        //    if (checked === 'false' || checked === false) {
        //        return $http.get('/api/DeliveryMethod/searchactivedeliverymethods/' + text)
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/DeliveryMethod/searchalldeliverymethods/' + text)
        //           .then(handleSuccess, handleError);
        //    }
        //}


        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }

        //function getInactiveDelivery() {
        //    return $http.get('/api/DeliveryMethod/getalldeliverymethods/').then(handleSuccess, handleError);
        //}
        //function getAllDelivery() {
        //    return $http.get('/api/DeliveryMethod/getactivedeliverymethods').then(handleSuccess, handleError);
        //}

        function createDeliveryMethod(deliveryMethod) {
            return $http.post('/api/DeliveryMethod/', deliveryMethod).then(handleSuccess, handleError);
        }

        function updateDeliveryMethod(deliveryMethod) {
            return $http.put('/api/DeliveryMethod', deliveryMethod).then(handleSuccess, handleError);
        }

        function deleteDelivery(item) {
            return $http.delete('/api/DeliveryMethod/' + item).then(handleSuccess, handleError);
        }

        function activeDeliveryMethod(id) {
            return $http.get('/api/DeliveryMethod/activatedeliverymethod/' + id).then(handleSuccess, handleError);
        }

       function getDeliveryMethodById(methodId){
           return $http.get('/api/DeliveryMethod/getdeliverymethod/' + methodId).then(handleSuccess, handleError);
       }
       function searchTextForDeliveryMethods(searchText, active) {
           var api = '/api/DeliveryMethod/searchDeliveryMethods/' + active + '/' + searchText;
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