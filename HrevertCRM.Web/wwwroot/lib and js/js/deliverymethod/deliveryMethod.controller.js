(function () {
    angular.module("app-deliveryMethod")
        .controller("deliveryMethodController", deliveryMethodController);
    deliveryMethodController.$inject = ['$http', '$filter', '$scope', 'deliveryMethodService'];
    function deliveryMethodController($http, $filter, $scope, deliveryMethodService) {
   
        var vm = this;
      //  vm.deliveryCheckInactive = deliveryCheckInactive;
        vm.editDelivery = editDelivery;
        vm.methodActionChanged = methodActionChanged;
        vm.activeDeliveryMethod = activeDeliveryMethod;
        vm.deleteSelectedDelivery = deleteSelectedDelivery;
        vm.saveDeliveryMethod = saveDeliveryMethod;
        vm.activeDeliveryItem = null;
      //  vm.deliveryMethodText = deliveryMethodText;
        vm.check = false;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.checkIfExistCode = checkIfExistCode;
        vm.searchParamChanged = searchParamChanged;


        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;
        vm.btnDeliveryMethodText = "Save";
        $scope.selected = [];

        vm.exist = function (item) {
            return $scope.selected.indexOf(item) > -1;
        }

        function toggleSelection(item, event) {
            if (event.currentTarget.checked) {
                $scope.selected.push(item);
            } else {
                for (var i = 0; i < $scope.selected.length; i++) {
                    if ($scope.selected[i].id == item.id) {
                        $scope.selected.splice($scope.selected.indexOf($scope.selected[i]), 1);
                        $scope.selectAll = [];
                        return;
                    }
                }
            }
        }
        function checkAll() {
            if ($scope.selectAll) {
                angular.forEach(vm.allDeliveryMethod, function (check) {
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
            deliveryMethodService.deletedSelected(selectedItemid).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        searchParamChanged();
                        $scope.selectAll = [];
                        $scope.selected = []; S
                    } else {
                        alert("errors");
                    }

                });
            
        }

        function checkIfExistCode(code) {
            if (code != undefined) {
                deliveryMethodService.checkIfDeliveryMethodCodeExists(code).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Delivery Method already exists");
                                editDelivery(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your delivery method?",
                                    "info",
                                    "This delivery method is already exists but has been disabled. Do you want to activate this delivery method?",
                                    "Active",
                                    "Your delivery method has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        } 
        searchParamChanged();
       // getDeliveryMethod();

        //function getDeliveryMethod() {
        //    deliveryMethodService.GetAllDelivery()
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.allDeliveryMethod = result.data;
        //                } else {
        //                    alert("Sorry");
        //                }
        //            });
        //}

        //function searchTextChangedForDeliveryMethod(text, checked) {
        //    deliveryMethodService.searchTextForDeliveryMethod(text, checked)
        //      .then(function (result) {
        //          if (result.success) {
        //              vm.allDeliveryMethod = result.data;
        //          }
        //      });
        //}

        //function deliveryMethodText(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getInactiveDeliveryMethod();
        //        } else {
        //            searchTextChangedForDeliveryMethod(text, checked);
        //        }
               
        //    } else {
        //        if (text === "") {
        //            getDeliveryMethod();
        //        } else {
        //            searchTextChangedForDeliveryMethod(text, checked);
        //        }
        //    }
        //}

        //function deliveryCheckInactive(active, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getInactiveDeliveryMethod();
               
        //    }
            
        //    else {
        //        vm.check = false;
        //        getDeliveryMethod();
        //    }
        //}

        //function getInactiveDeliveryMethod() {
        //    deliveryMethodService.GetInactiveDelivery()
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.allDeliveryMethod = result.data;
        //                } else {
        //                    alert("Sorry");
        //                }
        //            });
        //}

        function editDelivery(deliveryMethodId, checked) {
            if (checked) {
                deliveryMethodService.getDeliveryMethodById(deliveryMethodId).then(function (result) {
                    if (result.success) {
                        vm.activeDeliveryItem = result.data;
                        $scope.showModal = true;
                    }
                });
            }
            else {
                swal("you cann't edit this item.please activate it first.");
            }
        }

        function methodActionChanged(method, action) {
            if (Number(action) === 1) {
                vm.btnDeliveryMethodText = "Update";
                editDelivery(method.id, method.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Delivery method will be deactivated but still you can activate your delivery method in future.", "Yes, delete it!", "Your delivery method has been deactivated.", "delete", method);
            }

            vm.action = null;
        }

        function deleteSelectedDelivery(successMessage,item) {
            deliveryMethodService.deleteDelivery(item.id).then(function (result) {
                if (result.success) {
                    swal(successMessage,"success");
                    searchParamChanged();
                }

            });
        }

        function activeDeliveryMethod(id) {
            deliveryMethodService.activeDeliveryMethod(id)
                    .then(function (result) {
                        if (result.success) {
                            swal("Delivery Method Activated.");
                            searchParamChanged();
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function saveDeliveryMethod(item) {
            if (item.id === undefined) {
                deliveryMethodService.createDeliveryMethod(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeDeliveryItem = null;
                            searchParamChanged();
                            $scope.showModal = false;
                        } else {
                            swal("Failed to save. ");
                        }
                    });
            } else {
                deliveryMethodService.updateDeliveryMethod(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeDeliveryItem = null;
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

        $scope.hide = function () {
            vm.activeDeliveryItem = null;
            $scope.showModal = false;
        }

        $scope.open = function () {
            vm.activeDeliveryItem = null;
            $scope.showModal = true;
            vm.btnDeliveryMethodText = "Save";
            vm.deliveryMethodForm.$setUntouched();
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
                        deleteSelectedDelivery(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeDeliveryMethod(value.id);
                        editDelivery(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText=="")
                vm.searchText = null;
            deliveryMethodService.searchTextForDeliveryMethods(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.allDeliveryMethod = result.data;                     
                    }
                });
        }

    }
})();