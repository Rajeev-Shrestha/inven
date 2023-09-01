(function () {
    angular.module("app-companywebSetting")
        .controller("companywebSettingController", companywebSettingController);
    companywebSettingController.$inject = ['$http', '$filter', '$window', '$scope', 'companywebSettingService'];
    function companywebSettingController($http, $filter, $window, $scope, companywebSettingService) {
        var vm = this;
        vm.saveOrUpdateCompanyWebSetting = saveOrUpdateCompanyWebSetting;
        vm.updateCompanyLogo = updateCompanyLogo;
        vm.searchTextForCompanyWebSetting = '';
        vm.activecompanyWebSetting = {};
        vm.createCompanyWebSetting = null;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.fiscalFormat = [{ id: 1, value: 'FY-20Y1/Y2' }, { id: 2, value: '20Y1/Y2' }, { id: 3, value: 'FY-20YY' }];
        vm.discountCalculationType = [{ id: 1, value: 'Minimum' }, { id: 2, value: 'Maximum' }];
        vm.shippingCalculationType = [{ id: 1, value: 'Minimum' }, { id: 2, value: 'Maximum' }];
        vm.companyWebSettingBtnText = "Save";
        init();

        loadDeliveryMethods();
        loadPaymentMethod();

        $scope.open = function () {
            checkIfExistsCompanyWebSetting();
            
            $scope.showModal = true;
        }

        $scope.hide = function () {
            vm.createCompanyWebSetting = null;
            $scope.showModal = false;
        }

       
        function loadShippingCalculationType() {
            companywebSettingService.getAllUser().then(function (result) {
                if (result.success) {
                    vm.allUser = result.data;
                }
            });
        }

        function init() {
            loadShippingCalculationType();
            loadCompanyWebSetting();
        }

        function loadCompanyWebSetting() {
            $('#successMessage').html('');
            companywebSettingService.getCompanyWebSettingById().then(function (result) {
                if (result.success) {
                    if (result.data === "") {
                        vm.companyWebSettingBtnText = "Save";
                        vm.activecompanyWebSetting.length = 0;
                    }
                    else {
                        vm.companyWebSettingBtnText = "Update";
                        vm.activecompanyWebSetting = result.data;
                        vm.activecompanyWebSetting.dateModified = moment(vm.activecompanyWebSetting.dateModified).format('YYYY-MM-DD');
                        vm.activecompanyWebSetting.shippingCalculationType = result.data.shippingCalculationType;
                        vm.activecompanyWebSetting.discountCalculationType = result.data.discountCalculationType;
                    }
                }
              
            });
        }

        function saveOrUpdateCompanyWebSetting(companyWebSetting) {
            if (vm.logo === null || vm.logo === undefined) {
                swal("Please add company logo");
            }
            else {
                if (companyWebSetting.id == undefined) {
                    companywebSettingService.createCompanyWebSetting(companyWebSetting).then(function (result) {
                        if (result.success) {
                            $scope.showModal = false;
                            loadCompanyWebSetting();
                            $window.location.href = '/';
                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            } else {
                                swal(result.message);
                            }
                        }
                    });
                }
                else {
                    companywebSettingService.updateCompanyWebSetting(companyWebSetting).then(function (result) {
                        if (result.success) {
                            vm.activecompanyWebSetting = result.data;
                            vm.activecompanyWebSetting.dateModified = moment(vm.activecompanyWebSetting.dateModified).format('YYYY-MM-DD');
                            dialogMessageForSuccess("Company Web Setting has been updated successfully.");
                            setTimeout(function () {
                                $('#successMessage').html('');
                            }, 3000);
                            $window.location.href = '/';
                            //checkFirstLogin();
                        }
                    });
                }
            }
            }

        function checkFirstLogin() {
            companywebSettingService.checkLogin().then(function(result) {
                if (result.success) {
                    
                    //nothing here
                }
            });
        }

        // company Logo starts from here

        vm.addCompanyLogo = addCompanyLogo;
        vm.closeCompanyModal = closeCompanyModal;
        vm.companyLogo = {};
        vm.removeLogo = removeLogo;

        

        // for cropping image

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
       
        loadCompanyLogo();

        function addCompanyLogo() {
            vm.companyLogo = null;
            $scope.showCompanyModal = true;
        }

        function closeCompanyModal() {
            vm.companyLogo = null;
            $scope.showModal = false;
        }

        function loadCompanyLogo() {
            companywebSettingService.getActiveCompanyLogo().then(function(result) {
                if (result.success) {
                    vm.logo = result.data;
                } else {
                    vm.logo = null;
                    //console.log(result.message);
                }
            });
        }

        function updateCompanyLogo(image) {

            if (image === "") {
                swal("upload company logo.");
                return;
            }

            if (image != undefined) {
                var spiltString = image.substring(0, 22);
                vm.imageForBanner = {
                    FileName: "logo" + ".jpg",
                    ImageBase64: image.replace(spiltString, '')
                };

            }
            var companyLogo = {
                logoImage :vm.imageForBanner
        }
            if (companyLogo.id == undefined) {
                companywebSettingService.createCompanyLogo(companyLogo).then(function (result) {
                    if (result.success) {
                        loadCompanyLogo();
                        $scope.showModal = false;
                    }else {
                        if (result.message.errors) {
                            swal(result.messsage.errors[0]);
                            $scope.showModal = false;
                        }
                        else{
                            swal(result.messsage);
                        }
                    }
                   
                });
            }
            else {
                companywebSettingService.updateCompanyLogo(companyLogo).then(function (result) {
                    if (result.success) {
                        loadCompanyLogo();
                        $scope.showModal = false;
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.messsage.errors);
                        }
                        else {
                            swal(result.messsage);
                        }
                    }
                });
            }
        }

        function removeLogo(logoImage) {
            companywebSettingService.deleteCompanyLogo(logoImage.id).then(function(result) {
                if (result.success) {
                    loadCompanyLogo();
                }
            });
        }
    
        function dialogMessageForSuccess(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-success alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '</div>'
                           + '</div>';
            $('#successMessage').html(message);
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
                    if (alertFor === 'delete') {
                       // deleteCategory(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateCompanyWebSetting(value.id, false);
                       
                        saveOrUpdateCompanyWebSetting(value, true);
                        //swal(successMessage, "success");
                    }

                });
        }

        function checkIfExistsCompanyWebSetting() {
            companywebSettingService.checkIfCompanySettingExists().then(function (result) {
                if (result.success) {
                    if (!result.data.active) {
                        yesNoDialog("Do you want to active your Compnay Web Setting?",
                            "info",
                            "This company web setting is already exists but has been disabled. Do you want to active this company web setting?",
                            "Active",
                            "Your company web setting has been activated.",
                            "active",
                            result.data);
                    }
                }
                
            });

        }

        function activateCompanyWebSetting(id) {
            companywebSettingService.activateCompanySetting(id).then(function (result) {
                if (result.success) {
                    swal("Company Web Setting Activated.");
                    $scope.showModal = false;
                    loadCompanyWebSetting();
                }
            });
        }

        function loadDeliveryMethods() {
            companywebSettingService.getAllActiveDeliveryMethods().then(function (result) {
                if (result.success) {
                    vm.deliveryMethods = result.data;
                }
            });
        }

        function loadPaymentMethod() {
            companywebSettingService.getPaymentMethod().then(function (result) {
                if (result.success) {
                    vm.paymentMethods = result.data;
                }
            });
        }
    }
})();