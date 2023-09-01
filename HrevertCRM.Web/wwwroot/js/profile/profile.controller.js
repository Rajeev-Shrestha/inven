(function () {
    angular.module("app-profile")
        .controller("profileController", profileController);
    profileController.$inject = ['$http', '$filter', '$scope', 'profileService'];
    function profileController($http, $filter, $scope, profileService) {

        var vm = this;
        vm.userEmail = true;
        vm.passwordRequired = true;
        vm.userName = true;
        vm.confirmPasswordRequired = true;
        vm.updateUser = updateUser;
        vm.gender = [{ id: 1, name: 'Male' }, { id: 2, name: 'Female' }, { id: 3, name: 'Other' }];
        init();
       
        function init() {
            $('#successMessage').html('');
          //  $('#errorMessage').html('');
            profileService.getAllUserTypes().then(function(result) {
                if (result.success) {
                    vm.userType = result.data;
                }
            });

            profileService.getUserById().then(function (result) {
                if (result.success) {
                    vm.activeUser = result.data;
                }
            });
        }

        function validatePassword(password,confirmPassword) {
            if (password.length < 6) {
                var message = "password length cann't be less than 6 digit.";
                errorMessageForSweetAlert(message);
                return;
            }
            if(password !== confirmPassword){
                var data = "password donot match.";
                errorMessageForSweetAlert(data);
                return;
            }

            return 0;
        }

        function updateUser(user) {
            $('#successMessage').html('');
         //   $('#errorMessage').html('');
            var result = validatePassword(user.password, user.confirmPassword);
            if (result === 0) {
                profileService.updateUser(user).then(function (result) {
                    if (result.success) {
                        var message = "user data has been updates successfully.";
                        dialogMessageForSuccess(message);
                        setTimeout(function () {
                            $('#successMessage').html('');
                        }, 3000);
                    }
                    else {
                        if (result.message.errors) {
                            errorMessageForSweetAlert(result.message.errors[0]);
                        }
                        else {
                            errorMessageForSweetAlert(result.message);
                        }
                    }
                });
            }
        }

        function dialogMessageForSuccess(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-success alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '</div>'
                           + '</div>';
            $('#successMessage').html(message);
        }
     
        //function dialogMessage(result) {
        //    var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
        //                   + '<div class="alert alert-danger alert-dismissable">'
        //                   + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
        //                   + result + '</div>'
        //                   + '</div>';
        //    $('#errorMessage').html(message);
        //}

        function errorMessageForSweetAlert(message) {
            swal(message);
        }
    }
})();