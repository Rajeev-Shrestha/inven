(function () {
    angular.module("app-emailSetting")
        .controller("emailSettingController", emailSettingController);
    emailSettingController.$inject = ['$http', '$filter', '$scope', 'emailSettingService'];
    function emailSettingController($http, $filter, $scope, emailSettingService) {

        var vm = this;
        vm.editEmailSetting = editEmailSetting;
        vm.emailSettingTermActionChanged = emailSettingTermActionChanged;
        vm.saveEmailSetting = saveEmailSetting;
        vm.deleteEmailSetting = deleteEmailSetting;
        vm.check = false;
       // vm.checkInactiveEmailSetting = checkInactiveEmailSetting;
        vm.activeEmailSetting = activeEmailSetting;
       // vm.searchTextForEmailSetting = searchTextForEmailSetting;
        vm.emailSettingEmail = false;
        vm.emailSettingPassword = true;
        vm.emailSettingConfirmPassword = true;
        vm.searchParamChanged = searchParamChanged;
        vm.emailSettingBtnText = "Save";
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        init();

        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;

        $scope.selected = [];

        vm.exist = function (item) {
            return $scope.selected.indexOf(item) > -1;
        }

        function toggleSelection(item, event) {
            if (event.currentTarget.checked) {
                $scope.selected.push(item);
            }
            else {
                for (var i = 0; i < $scope.selected.length; i++) {
                    if ($scope.selected[i].id === item.id) {
                        $scope.selected.splice($scope.selected.indexOf($scope.selected[i]), 1);
                        $scope.selectAll = [];
                        return;
                    }
                }
            }

        }

        function checkAll() {
            if ($scope.selectAll) {
                angular.forEach(vm.allEmailSettings, function (check) {
                    var index = $scope.selected.indexOf(check);
                    if (index >= 0) {
                        return true;
                    } else {
                        $scope.selected.push(check);
                    }
                })
            } else {
                $scope.selected = [];
            }
        }
        function deleteSelected(selectedItem) {
            var selectedItemid = [];
            for (var i = 0; i < selectedItem.length; i++) {
                selectedItemid.push(selectedItem[i].id);
            }
            emailSettingService.deletedSelected(selectedItemid).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        $scope.selectAll = [];
                        $scope.selected = [];
                        searchParamChanged();

                    } else {
                        alert("errors");
                    }

                });
            
        }
        function init() {
            searchParamChanged();
           // getActiveEmailSetting();
            loadAllEncryptionTypes();
        }

        function loadAllEncryptionTypes() {
            emailSettingService.getAllEncryptionTypes().then(function (result) {
                if (result.success) {
                    vm.encryptionType = result.data;
                }
            });
            }

        function getAllEmail() {
            emailSettingService.getAllEmail().then(function (result) {
                if (result.success) {
                    vm.allEmailSettings = result.data;
                } else {
                    console.log(result.message);
                }

            });
        }
      
        function editEmailSetting(settingId, checked) {
            if (checked) {
                emailSettingService.getEmailSettingById(settingId)
                .then(function (result) {
                    if (result.success) {
                        vm.activeEmailSetting = result.data;
                        vm.emailSettingEmail = true;
                        vm.emailSettingPassword = false;
                        vm.emailSettingConfirmPassword = false;
                        $scope.showModal = true;
                    } else {
                        console.log(result.message);
                    }
                });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
            
        }

        function emailSettingTermActionChanged(data, action) {
            vm.action = null;

            if (Number(action) === 1) {
                vm.emailSettingBtnText = "Update";
                editEmailSetting(data.id,data.active);

            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Email setting will be deactivated but still you can activate your email setting in future.", "Yes, delete it!", "Your email setting has been deactivated.", "delete", data);

               
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
                    if (alertFor === 'delete') {
                        deleteEmailSetting(successMessage, value);

                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeDiscountSetting(value);
                        editDiscountSetting(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }

        function saveEmailSetting(emailSetting) {
            if (emailSetting.confirmPassword === null) {
                emailSetting.confirmPassword = "";
            }
            if (emailSetting.password === emailSetting.confirmPassword) {
                if (emailSetting.id === undefined) {
                    emailSettingService.createEmailSetting(emailSetting)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeEmailSetting = null;
                            searchParamChanged();
                            $scope.showModal = false;
                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            }else
                            {
                                swal(result.message);
                            }
                        }
                    });

                } else {
                    emailSettingService.updateEmailSetting(emailSetting)
                        .then(function (result) {
                            if (result.success) {
                                vm.activeEmailSetting = null;
                                searchParamChanged();
                                $scope.showModal = false;
                            } else {
                                if (result.message.errors) {
                                    swal(result.message.errors[0]);
                                } else {
                                    swal(result.message);
                                }
                            }
                        });
                }
            }
            else {
                 swal("Passwords don't match");
            }
        }

        function deleteEmailSetting(successMessage, emailSetting) {
            emailSettingService.deleteEmailSetting(emailSetting.id).then(function (result) {
                if (result.success) {
                    swal(successMessage, "success");
                    searchParamChanged();
                }

            });
        }

        //function checkInactiveEmailSetting(active,checked) {
        //    if (checked) {
        //        vm.check = true;
        //        searchParamChanged();
        //    }
           
        //    else {
        //        vm.check = false;
        //        searchParamChanged();
        //    }
        //}

        function activeEmailSetting(settingId) {
            emailSettingService.getActiveEmailSetting(settingId).then(function(result) {
                if (result.success) {
                    swal("Email successfully activated.");
                        searchParamChanged();
                    } else {
                        console.log(result.message);
                    }
                });
        }

        //function getActiveEmailSetting() {
        //    emailSettingService.getAllActiveEmailSetting().then(function (result) {
        //        if (result.success) {
        //            vm.allEmailSettings = result.data;
        //        } else {
        //            console.log(result.message);
        //        }

        //    });
        //}

        //function searchTextChangedForEmailSetting(text, checked) {
        //    emailSettingService.searchTextForEmailSetting(text, checked)
        //      .then(function(result) {
        //          if (result.success) {
        //              vm.allEmailSettings = result.data;
        //          }
        //      });
        //}

        //function searchTextForEmailSetting(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getAllEmail();
        //        } else {
        //            searchTextChangedForEmailSetting(text, checked);
        //        }
        //    } else {
        //        if (text === "") {
        //            getActiveEmailSetting();
        //        } else {
        //            searchTextChangedForEmailSetting(text, checked);
        //        }
        //    }
        //}

        $scope.hide = function () {
            vm.emailSettingEmail = false;
            vm.emailSettingPassword = true;
            vm.emailSettingConfirmPassword = true;
            vm.activeEmailSetting = null;
            $scope.showModal = false;
        }

        $scope.open = function () {
            vm.emailSettingPassword = true;
            vm.emailSettingConfirmPassword = true;
            vm.emailSettingEmail = false;
            vm.activeEmailSetting = null;
            $scope.showModal = true;
            vm.emailSettingBtnText = "Save";
            vm.emailSettingForm.$setUntouched();
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
            emailSettingService.searchTextForEmailSetting(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.allEmailSettings = result.data;
                    }
                });
        }

    }
})();