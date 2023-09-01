(function () {
    angular.module("app-home")
        .controller("homeController", homeController);
    homeController.$inject = ['$http', '$scope', '$window', '$filter', 'homeService'];

    function homeController($http, $scope, $window, $filter, homeService) {
        var vm = this;
        vm.buyCrm = buyCrm;

        function buyCrm(email, name) {
            homeService.buyHrevertCrm(email, name).then(function (result) {
                if (result.success) {
                    if (result.data.statusCode === 400) {
                        dialogMessage(result.data.value);
                    }
                    if (result.data === "success") {
                        $('#myModal').modal('hide');
                        //code here
                    }
                }
                else {
                    //code here
                    if (result.message.errors) {
                        dialogMessage(result.message.errors);
                    }
                    if (result.message) {
                        dialogMessage(result.message);
                    }
                    // $('#myModal').modal('hide');
                }
            });
        }

        function dialogMessage(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-danger alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '</div>'
                           + '</div>';
            $('#errorMessage').html(message);
        }

    }

})();

