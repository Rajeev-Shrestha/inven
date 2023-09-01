(function () {
    'use strict';
    angular
        .module('app-taskManager')
        .service('taskManagerService', taskManagerService);

    taskManagerService.$inject = ['$http']; 

    function taskManagerService($http) {
        var service = {};
        service.createNewTask = createNewTask;
        service.getAllApplicationUsers = getAllApplicationUsers;
        service.getAllActiveTasks = getAllActiveTasks;
        service.updateTask = updateTask;
        service.deleteTask = deleteTask;
        service.getdocIdByDocType = getdocIdByDocType;
        service.getActiveTaskById = getActiveTaskById;
        service.searchTextForTask = searchTextForTask;
        service.sortTask = sortTask;
        service.getUserRightsByLogin = getUserRightsByLogin;
        return service;

        function getdocIdByDocType(id) {
            if (id === 1) {
                return $http.get('/api/User/getUserNames').then(handleSuccess, handleError);
            }
            else if (id === 2) {
                return $http.get('/api/Vendor/getVendorNames').then(handleSuccess, handleError);
            }
            else if (id === 3) {
                return $http.get('/api/salesorder/getSalesOrderNumbers').then(handleSuccess, handleError);
            }
            else if (id === 4) {
                return $http.get('/api/purchaseorder/getPurchaseOrderNumbers').then(handleSuccess, handleError);
            }
        }
        function sortTask(id) {
            return $http.get('/api/taskManager/sortTask/'+ id).then(handleSuccess, handleError);
        }
        function createNewTask(reminder) {
            return $http.post('/api/taskManager/create', reminder).then(handleSuccess, handleError);
        }
        function getAllActiveTasks() {
            return $http.get('/api/taskManager/getAllActiveTasks').then(handleSuccess, handleError);
        }       
   
        function getActiveTaskById(id) {
            return $http.get('/api/taskManager/ActiveTasks/' + id).then(handleSuccess, handleError);
        }
        function searchTextForTask(text, checked, pageIndex, pageSize, sortColumn, sortOrder) {

            return $http.get('/api/taskManager/searchTasks/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked, sortColumn, sortOrder))
                    .then(handleSuccess, handleError);
          
        }
        function buildUriForSearchPaging(pageIndex, pageSize, text, active,sortColumn, sortOrder) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active + '&sortColumn=' + sortColumn + '&sortOrder=' + sortOrder;
            return uri;
        }
        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }
        function deleteTask(id) {
            return $http.delete('/api/taskManager/'+id);
        }
        // Edit reminder
        function updateTask(task) {
            return $http.put('/api/taskManager/update', task).then(handleSuccess, handleError);
        }
        // Get all application users
        function getAllApplicationUsers() {
            return $http.get('/api/user/getactiveusers').then(handleSuccess, handleError);
        }
        function getUserRightsByLogin() {
            return $http.get('/api/taskManager/taskAssignRights').then(handleSuccess, handleError);
        }
        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();