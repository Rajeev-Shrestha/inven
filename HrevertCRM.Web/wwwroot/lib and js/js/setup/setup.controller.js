(function () {
    angular.module("app-setup")
        .controller("setupController", setupController);
    setupController.$inject = ['$http', '$scope', '$filter', '$window', 'setupService'];

    function setupController($http, $scope, $filter, $window, setupService) {
        var vm = this;
        vm.saveSetting = saveSetting;
        vm.addCompanyLogo = addCompanyLogo;
        vm.closeCompanyModal = closeCompanyModal;
        vm.updateCompanyLogo = updateCompanyLogo;
        vm.createCompany = createCompany;
        vm.companySetup = {};
        vm.fiscaltypeDisabled = false;
        
        vm.fiscalYearType = [{ id: 1, name: 'FY-20Y1/Y2' }, { id: 1, name: '20Y1/Y2' }, { id: 1, name: 'FY-20YY' }];
        loginUserDetails();

        var email = null;
        function loginUserDetails() {
            setupService.getLoginUserDetails().then(function (result) {
                if (result.success) {
                    if (result.data.id) {
                        vm.loginuser = result.data;
                        vm.companySetup = result.data.companyViewModel;
                        vm.companySetup.name = result.data.companyName;
                        email = result.data.email;
                        if (result.data.companyViewModel.fiscalYearFormat == 0 || result.data.companyViewModel.fiscalYearFormat === null || result.data.companyViewModel.fiscalYearFormat === undefined) {
                            vm.fiscaltypeDisabled = false;
                        } else {
                            vm.fiscaltypeDisabled = true;
                        }
                      
                    } else {
                       
                        vm.compampanySetup = "";
                        vm.fiscaltypeDisabled = false;
                    }
                    
                }
                else {
                    
                    alert(result.errors);
                }
            });
        }
        function createCompany(company) {

            if (vm.companySetup.panRegistrationNo === undefined || vm.companySetup.panRegistrationNo === null && vm.companySetup.vatRegistrationNo === undefined || vm.companySetup.vatRegistrationNo===null)
            {
                swal("Please enter any one of VAT number or PAN number");
                return;
            }
            company.id = vm.loginuser.companyId;
            company.version = vm.loginuser.companyVersion;
            company.email = email;
            
            setupService.createCompany(company).then(function (result) {
                if (result.success) {
                    //vm.companySetup = result.data;
                    $window.location.href = '/';
                }
                else {
                    if(result.message.errors){
                        errorMessageForSweetAlert(result.message.errors);
                    }
                    else {
                        errorMessageForSweetAlert(result.message);
                    }
                    console.log(result.message);
                }
            });
        }


        //Image Crop
        $scope.logoImage = '';
        $scope.logoCroppedImage = '';
        $scope.$watch('logoCroppedImage', function () {
            console.log($scope.logoArray);
        });

        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.logoImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

        function addCompanyLogo() {
           // $('#errorMessage').html('');
            vm.companyLogo = null;
            $scope.showCompanyModal = true;
        }

        function closeCompanyModal() {
            vm.companyLogo = null;
            $scope.showCompanyModal = false;
        }

        function saveSetting(setting) {
            setupService.saveSetting(setting).then(function(result) {
                if (result.success) {
                    $window.location.href = 'app/home#!/home';
                } else {
                    // error here
                }
            });
        }

        loadCompanyLogo();
        function loadCompanyLogo() {
            setupService.getActiveCompanyLogo().then(function(result) {
                if (result.success) {
                    vm.logo = result.data;
                } else {
                    // console.log(result.message);
                }
            });
        }

        function updateCompanyLogo(companyLogo, image) {

            if (image === "") {
                var message = "upload company logo.";
                //dialogMessage(message);
                errorMessageForSweetAlert(message);
                return;
            }

            if (image != undefined) {
                var spiltString = image.substring(0, 22);
                vm.imageForBanner = {
                    FileName: companyLogo.companyName + ".jpg",
                    ImageBase64: image.replace(spiltString, '')
                };

            }
            companyLogo.logoImage = vm.imageForBanner;
            if (companyLogo.id == undefined) {
                setupService.createCompanyLogo(companyLogo).then(function (result) {
                    if (result.success) {
                        //loadCompanyLogo();
                        $scope.showCompanyModal = false;
                    } else {
                        if (result.message.errors) {
                            //dialogMessage(result.message.errors);
                            errorMessageForSweetAlert(result.message.errors);
                        }
                        else {
                            //dialogMessage(result.message);
                            errorMessageForSweetAlert(result.message);
                        }
                    }

                });
            }
            else {
                setupService.updateLogo(companyLogo).then(function (result) {
                    if (result.success) {
                        //loadCompanyLogo();
                        $scope.showCompanyModal = false;
                    }
                    else {
                        if (result.message.errors) {
                            //dialogMessage(result.message.errors);
                        }
                        else {
                            //dialogMessage(result.message);
                        }
                    }
                });
            }
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

