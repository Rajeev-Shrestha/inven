(function () {
    "use strict";
    angular.module("app-home")
        .controller("homeController", homeController);
    homeController.$inject = ['$scope', 'homeService'];
    function homeController($scope, homeService) {
        var vm = this;
        $scope.men = [
      'John',
      'Jack',
      'Mark',
      'Ernie'
        ];


        $scope.women = [
        'Jane',
        'Jill',
        'Betty',
        'Mary'
        ];


        $scope.addText = "";


        $scope.dropSuccessHandler = function ($event, index, array) {
            array.splice(index, 1);
        };

        $scope.onDrop = function ($event, $data, array) {
            array.push($data);
        };

        
    }
});