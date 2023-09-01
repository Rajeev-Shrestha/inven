(function () {
    angular.module("app-registerUser")
        .controller("registerUserController", registerUserController);
    registerUserController.$inject = ['$http', '$filter', '$window', 'registerUserService', 'viewModelHelper'];
    function registerUserController($http, $filter, $window, registerUserService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.userRegister = userRegister;
        vm.itemDetails = [{ name: 'Dell Laptop', id: 1, price: 56200, quantity: 1, total: 56200 }, { name: 'Transend HDD', id: 2, price: 8500, quantity: 1, total: 56200 }, { name: 'Samsung Monitor', id: 3, price: 12000, quantity: 1, total: 56200 }];
      vm.checkEmailExists = checkEmailExists;
        vm.countries = [{ name: 'Nepal', id: 1, code: "+977" }];

        init();


        function checkEmailExists(email) {  
                registerUserService.checkEmailExists(email).then(function (result) {
                    if (result.success) {

                        if (result.data === "true") {
                            swal("Email Already exists.");
                        }
                    }
                });
            }


        
        function userRegister(user) {
            user.billingAddress.addressType = 1;
            user.billingAddress.isDefault = true;
            user.displayName = user.billingAddress.firstName + " " + user.billingAddress.lastName;
            for (var i = 0; i < vm.countries.length; i++) {
                if (user.billingAddress.countryId === vm.countries[i].id) {
                    user.billingAddress.mobilePhone = vm.countries[i].code + "-" + user.billingAddress.mobilePhone;
                }
            }
            var message = validatePassword(user.password, user.confirmPassword);
            if (message !== "Okay") {
                dialogMessage(message);
            } else {
                user.addresses = [];
                user.addresses[0] = user.billingAddress;
                registerUserService.registerUser(user).then(function (result) {
                    if (result.success) {

                        if (result.data != null) {
                            
                           //setTimeout(function(){
                           //         swal({
                           //            // title: "Wow!",
                           //             text: "User has been successfully created!!",
                           //             type: "success"
                           //         }, function() {
                           //             $window.location.href('/login');
                           //             //window.location = "redirectURL";
                           //         });
                           //     },10000);
                                //""
                            //);

                            sweetAlert({
                                title: 'success',
                                text: 'successfully registered user!',
                                
                            }, function (isConfirm) {
                                $window.location.href = '/login';
                            });
                            //$('.swal2-confirm').click(function () {
                            //    $window.location.href = '/login';
                            //});
                           
                                
                           // }
                            //dialogMessage("User created successfully.");
                              //  yesNoDialog("User has been successfully created!!", "success", "User will be redirected to loginp page", "Yes, delete it!", "Your task has been deactivated.", "success", user);
                        }
                    } else {
                        if (result.message.errors)
                            dialogMessage(result.message.errors);
                        var message = {};
                        message.message = "register user , " + result.message + " in register user.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
            }
        }

        function init() {
            registerUserService.getAllLevels().then(function (result) {
                if (result.success) {
                    vm.customerLevel = result.data;
                    registerUserService.getAllCountries().then(function (result) {
                        if (result.success) {
                            vm.country = result.data;
                            registerUserService.getAllTitles().then(function (result) {
                                if (result.success) {
                                    vm.titles = result.data;
                                    registerUserService.getAllSuffixes().then(function (result) {
                                        if (result.success) {
                                            vm.suffixes = result.data;
                                            registerUserService.getActiveZones().then(function (result) {
                                                if (result.success) {
                                                    vm.zone = result.data;
                                                    vm.class = 'loader loader-default';
                                                } else {
                                                    var message = {};
                                                    message.message = "get all zones , " + result.message + " register user.,";
                                                    viewModelHelper.bugReport(message,
                                                      function (result) {
                                                      });
                                                }
                                            });
                                        }
                                        else {
                                            var message = {};
                                            message.message = "get all suffixes , " + result.message + " iregister user.,";
                                            viewModelHelper.bugReport(message,
                                              function (result) {
                                              });
                                        }
                                    });
                                }
                                else {
                                    var message = {};
                                    message.message = "get all titles , " + result.message + " register user.,";
                                    viewModelHelper.bugReport(message,
                                      function (result) {
                                      });
                                }
                            });
                        }
                        else {
                            var message = {};
                            message.message = "get all countries , " + result.message + " register user.,";
                            viewModelHelper.bugReport(message,
                                function (result) {
                                });
                        }
                    });
                }
                else {
                    var message = {};
                    message.message = "get all level , " + result.message + " in register user.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }

        function validatePassword(password, confirmPassword) {
            var message = "";
            if (password == undefined || confirmPassword == undefined) { return; }
            if (password.length < 6) {
                message = "password cannot be less 6 than digit.";
            }

            if (password === confirmPassword) {
                message = "Okay";
            }
            if (password != confirmPassword) {
                message = "password don't match.";
            }
            return message;
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
    function yesNoDialog(title, type, text, buttonText, successMessage, alertFor, value) {
        swal({
            title: title,
            text: text,
            type: type,
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: buttonText,
            closeOnConfirm: false
        },
            function () {
                if (alertFor === 'success') {
                    deleteTask(successMessage, value);
                    //swal(successMessage, "success");
                }
                if (alertFor === 'active') {
                    activateTask(value);
                    //editCustomer(value.id, true);
                    // vm.customerForm.$invalid = true;
                    //swal(successMessage, "success");
                }

            });
    }
})();