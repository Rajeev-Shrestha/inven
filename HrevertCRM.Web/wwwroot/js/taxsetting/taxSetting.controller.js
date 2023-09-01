(function () {
    angular.module("app-taxSetting")
        .controller("taxSettingController", taxSettingController);
    taxSettingController.$inject = ['$http', '$filter', '$scope', '$state', 'taxSettingService'];
    function taxSettingController($http, $filter, $scope, $state, taxSettingService) {
        var vm = this;
        vm.taxes = null;
        vm.getAllTaxes = getAllTaxes;
        vm.editPanel = false;
        vm.taxTypes = [{ id: 1, name: 'Percent' }, { id: 2, name: 'Fixed' }];
        vm.checkIfTaxCodeExists = checkIfTaxCodeExists;
        vm.saveTax = saveTax;
        vm.editItem = editItem;
        vm.btnSaveText = "Save";
        vm.hideTaxDialog = hideTaxDialog;
        vm.deleteItem = deleteItem;
        vm.searchParamChanged = searchParamChanged;
        vm.activateTax = activateTax;
        vm.deleteTaxItem = deleteTaxItem;

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
            } else {
                for (var i = 0; i <  $scope.selected.length; i++) {
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
                angular.forEach(vm.taxes, function (check) {
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
            taxSettingService.deletedSelected(selectedItemid).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        searchParamChanged();
                        $scope.selectAll = [];
                        $scope.selected = [];

                    } else {
                        alert("errors");
                    }

                });
            
        }
        
        function init()
        {
            searchParamChanged();
           // getAllTaxes();
        }
        function getAllTaxes() {
            taxSettingService.getAllTaxes().then(function (result) {
                if (result.success) {
                    vm.taxes = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        $scope.open = function () {
            vm.editPanel = true;
            vm.tax = {};
            vm.btnSaveText = "Save";
            vm.taxForm.$setUntouched();
        };
            

        function checkIfTaxCodeExists(taxCode) {
            if (taxCode !== undefined) {
                taxSettingService.checkIfTaxCodeExists(taxCode).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Tax already exists");
                            } else {
                                return true;
                            }
                        }
                    }
                });
            }
        }
        function saveTax(tax) {
            tax.recoverableCalculationType = 1;
            if (tax.id != null) {
                taxSettingService.updateTax(tax).then(function (result) {
                    if (result.success) {
                        swal("Tax Successfully Updated.");
                        searchParamChanged();
                        vm.tax = {};
                        vm.editPanel = false;
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        } else {
                            swal(result.message);
                        }
                    }
                });
            }
            else {
                taxSettingService.createTax(tax).then(function (result) {
                    if (result.success) {
                        swal("New Tax Successfully Added.");
                        vm.tax = {};
                        vm.editPanel = false;
                        vm.taxes.push(result.data);
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        } else {
                            swal(result.message);
                        }
                    }
                });
            }
        }

        function editItem(taxId, active) {
            vm.btnSaveText = "Update";        
            if (active) {
                taxSettingService.getById(taxId).then(function (result) {
                    if (result.success) {
                        vm.editPanel = true;
                        vm.tax = result.data;
                        vm.btnSaveText = "Update";
                    }

                });
            }
            else {
                swal("You cannot edit this item. please active first.");
            }


        }

        function hideTaxDialog()
        {
            vm.editPanel = false;
        }

        function deleteTaxItem(successMessage, tax)
        {
            taxSettingService.deleteTax(tax.id).then(function (result) {
                if (result.success) {
                    swal(successMessage, "success");
                    searchParamChanged();
                } else {
                    if (result.message.errors) {
                        swal(result.message.errors[0]);
                    } else {
                        swal(result.message);
                    }
                }

            });

        }
        function deleteItem(tax) {
            yesNoDialog("Are you sure?", "warning", "Tax Setting will be deactivated but still you can activate your tax setting in future.", "Yes, delete it!", "Your tax setting has been deactivated.", "delete", tax);

        }
        function activateTax(tax) {
            taxSettingService.actiateTax(tax.id)
                        .then(function (result) {
                            if (result.success) {
                                swal("Tax Activated.");
                                searchParamChanged();
                            } else {
                                console.log("Sorry" + result.message);
                            }
                        });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
            taxSettingService.searchTextForTaxes(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.taxes = result.data;
                    }
                });
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
                        deleteTaxItem(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    //if (alertFor === 'active') {
                    //    activeDeliveryMethod(value.id);
                    //    editDelivery(value.id, true);
                    //    //swal(successMessage, "success");
                    //}

                });
        }
        
    }
})();

