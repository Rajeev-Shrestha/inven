(function () {
    angular.module("app-error")
        .controller("errorController", errorController);
    errorController.$inject = ['$http', '$filter', '$cookies', 'errorService'];
    function errorController($http, $filter, $cookies, errorService) {
        var vm = this;
        
    }
})();