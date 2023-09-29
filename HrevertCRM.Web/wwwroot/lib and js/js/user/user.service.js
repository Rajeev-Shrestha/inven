(function () {
    'use strict';

    angular
        .module('app-user')
        .service('UserService', userService);

    userService.$inject = ['$http'];

    function userService($http) {
        var service = {};

       // service.GetAll = getAll;
        service.GetById = getById;
        service.Create = create;
        service.Update = update;
        service.Delete = Delete;
        service.getAllRoles = getAllRoles;
        service.GetMembers = getMembers;
        service.updateMembers = updateMembers;
        service.deleteUser = deleteUser;
      //  service.getInactives = getInactives;
        service.activeUser = activeUser;
      //  service.searchText = searchText;
        service.getsecurityRoles = getsecurityRoles;
        service.checkIfUserEmailExists = checkIfUserEmailExists;
        service.searchTextForUser = searchTextForUser;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/user/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function checkIfUserEmailExists(email) {
            return $http.get('/api/user/CheckIfDeletedUserWithSameEmailExists/' + email).then(handleSuccess, handleError);
        }


        function getsecurityRoles() {
            return $http.get('/api/SecurityGroup/GetSecurityGroups').then(handleSuccess, handleError);
        }
        function activeUser(id) {
            return $http.get('/api/user/activateuser/' + id).then(handleSuccess, handleError);
        }
        //function getAll() {
        //    return $http.get('/api/user/getactiveusers').then(handleSuccess, handleError);
        //}

        //function getInactives() {
        //    return $http.get('/api/user/getallusers').then(handleSuccess, handleError);
        //}

        function getAllRoles() {
            return $http.get('/api/User/getusertypes').then(handleSuccess, handleError);
        }

        function getMembers(id) {
            return $http.get('/api/securitygroupmember/getsecuritymember/' + id).then(handleSuccess, handleError);
        }

        function updateMembers(securityGroupId, listItem) {
            return $http.post('/api/securitygroupmember/savemembersingroup/' + securityGroupId, listItem).then(handleSuccess, handleError);
        }

        function getById(user) { 
            return $http.get('/api/user/getuserbyidwithoutidentity/' + user).then(handleSuccess, handleError);
        }

        function create(user) {

            return $http.post('/api/user/createuser/', user).then(handleSuccess, handleError);
        }

        function update(user) {
            return $http.put('/api/user/updateuser/', user).then(handleSuccess, handleError);
        }

        function Delete(email) {
            return $http.delete('/api/user/' + email).then(handleSuccess, handleError);
        }

        function deleteUser(id) {
            
            return $http.delete('/api/user/deleteuser/' + id).then(handleSuccess, handleError);

        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        //function searchText(pageIndex, pageSize, text, active) {
        //    if (active === 'false'|| active === false) {
        //        return $http.get('/api/user/searchactiveusers/' +
        //                buildUriForSearchPaging(pageIndex, pageSize, text, active))
        //            .then(handleSuccess, handleError);
        //    } else {
        //        return $http.get('/api/user/searchallusers/' +
        //                buildUriForSearchPaging(pageIndex, pageSize, text, active))
        //            .then(handleSuccess, handleError);
        //    }
        //}

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
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
            if (error.statusCode === 401) error.data = error.data;
            if (error.statusCode === 500) error.data = error.data;
            if (error.data === "" || error.data == null) error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();