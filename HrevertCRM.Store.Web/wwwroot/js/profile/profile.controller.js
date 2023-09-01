(function () {
    "use strict";
    angular.module("app-profile")
        .controller("profileController", profileController);
    profileController.$inject = ['$http', '$filter', '$location', '$cookies', '$mdDialog', 'profileService'];
    function profileController($http, $filter, $location, $cookies, $mdDialog, profileService) {
        var vm = this;
        //load spinner
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        
        //stop spinner
        vm.class = 'loader loader-default';
        
    }
})();