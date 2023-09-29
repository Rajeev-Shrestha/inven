(function () {
    "use strict";
    angular.module("app-account")
        .service("accountService", accountService);

    accountService.$inject=['$http'];

  

    function accountService($http) {
        var service = {};
        service.getAccountTree = getAccountTree;
        service.saveAccount = saveAccount;
        service.getAccountById = getAccountById;
        service.updateAccount = updateAccount;
        service.deleteAccount = deleteAccount;
        service.getAllAccountTree = getAllAccountTree;
        service.getAccountActivated = getAccountActivated;
        service.searchAction = searchAction;
        service.getAllAccountLevels = getAllAccountLevels;
        service.getAllAccountDetailsType = getAllAccountDetailsType;
        service.getAllAccountCashFlowTypes = getAllAccountCashFlowTypes;
        service.getAllAccountTypes = getAllAccountTypes;
        service.getAllActiveAccount = getAllActiveAccount;
        service.checkIfAccountCodeExists = checkIfAccountCodeExists;
        service.checkIfAccountDescriptionExists = checkIfAccountDescriptionExists;
        service.deleteOnlyAccount = deleteOnlyAccount;
        return service;


        function deleteOnlyAccount(id) {
            return $http.delete('/api/account/deleteOnlyAccount/' + id).then(handleSuccess, handleError);
        }

        function getAllActiveAccount() {
            return $http.get('/api/account/getallactiveaccounts').then(handleSuccess, handleError);
        }

        function getAllAccountCashFlowTypes() {
            return $http.get('/api/account/getaccountcashflowtypes').then(handleSuccess, handleError);
        }

        function getAllAccountDetailsType() {
            return $http.get('/api/account/getaccountdetailtypes').then(handleSuccess, handleError);
        }
        function getAllAccountLevels() {
            return $http.get('/api/account/getaccountleveltypes').then(handleSuccess, handleError);
        }

        function getAllAccountTypes() {
            return $http.get('/api/account/getaccounttypes').then(handleSuccess, handleError);
        }

        function getAccountTree() {
            return $http.get('/api/account/accounttree').then(handleSuccess, handleError);
        }


        function getAllAccountTree() {
            return $http.get('/api/account/getallaccounttree').then(handleSuccess, handleError);
        }

        function getAccountById(id) {
            return $http.get('/api/account/getaccountbyid/' + id).then(handleSuccess, handleError);
        }

        function updateAccount(account) {
            return $http.put('/api/account/updateaccount/' , account).then(handleSuccess, handleError);
        }

        function saveAccount(account) {
            return $http.post('/api/account/createaccount' , account).then(handleSuccess, handleError);
            
        }

        function deleteAccount(id) {
            return $http.delete('/api/account/'+ id).then(handleSuccess, handleError);
        }

        function getAccountActivated(id) {
            return $http.get('/api/account/activateaccount/' + id).then(handleSuccess, handleError);
        }


        function searchAction(text, status){
            if (status === 'false' || status === false) {
                
                return $http.get('/api/account/searchactiveaccounts/' + text).then(handleSuccess, handleError);
            }
            else {

                return $http.get('/api/account/searchallaccounts/' + text).then(handleSuccess, handleError);
            }
        }


        function checkIfAccountCodeExists(code) {
            return $http.get('/api/account/CheckIfDeletedAccountWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }

        function checkIfAccountDescriptionExists(des) {
            return $http.get('/api/account/CheckIfDeletedAccountWithSameDescriptionExists/' + des).then(handleSuccess, handleError);
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