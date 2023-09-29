(function () {
    'use strict';

    angular
        .module('app-securityright')
        .service('SecurityRightService', securityRightService);

    securityRightService.$inject = ['$http'];

    function securityRightService($http) {
        var service = {};

        service.GetAll = getAll;
        service.GetById = getById;
        service.saveSecurity = saveSecurity;
        service.Update = update;
        service.Delete = Delete;
        //service.GetAllSecurity = getAllSecurity;
        service.GetSecurityAssignedToGroup = getSecurityAssignedToGroup;
        //service.searchText = searchText;
        service.searchTextForSecurityRights = searchTextForSecurityRights;
        service.getPageSize = getPageSize;
        service.searchTextForUser = searchTextForUser;
        service.GetSecurityAssignedToUser = GetSecurityAssignedToUser;
        service.saveUserSecurity = saveUserSecurity;
        return service;


        //function searchText(pageIndex, pageSize, text) {
        //    return $http.get('/api/security/searchactivesecurities/' + searchForPaging(pageIndex, pageSize, text)).then(handleSuccess, handleError);
        //}
        function getAll() {
            return $http.get('/api/securitygroup/getallsecuritygroup').then(handleSuccess, handleError);
        }

        function getSecurityAssignedToGroup(groupId) {
            return $http.get('/api/securityGroup/getsecurityassignedtogroup/' + groupId).then(handleSuccess, handleError);
        }
        function GetSecurityAssignedToUser(userId) {
            return $http.get('/api/securityRight/GetAssignedUserSecurity/' + userId).then(handleSuccess, handleError);
        }
        //function getAllSecurity(pageIndex, pageSize) {
        //    return $http.get('/api/security/getallsecurity' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function saveSecurity(saveItemInfo) {
            return $http.post('/api/securityGroup/assignsecuritytogroup', saveItemInfo).then(handleSuccess, handleError);
        }
        function saveUserSecurity(saveItemInfo) {
            return $http.post('/api/securityGroup/assignsecuritytouser', saveItemInfo).then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function searchForPaging(pageIndex, pageSize, text) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text;
            return uri;
        }
        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        function getById(id) {
            return $http.get('/api/customer/' + id).then(handleSuccess, handleError);
        }

        function update(Customer) {
            return $http.put('/api/customer', Customer).then(handleSuccess, handleError);

        }

        function Delete(id) {
            return $http.delete('/api/customer/' + id).then(handleSuccess, handleError);
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/security/getPageSize').then(handleSuccess, handleError);
        }
        function searchTextForSecurityRights(text, checked, pageIndex, pageSize) {
            return $http.get('/api/security/searchSecurities/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

        }
        function searchTextForUser(searchText, active) {
            var api = '/api/user/getUsers/' + active + '/' + searchText;
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