(function () {
    angular.module("app-paymentMethod")
        .controller("paymentMethodController", paymentMethodController);
    paymentMethodController.$inject = ['$http', '$filter', '$scope','paymentMethodService'];
    function paymentMethodController($http, $filter, $scope, paymentMethodService) {
      
        var vm = this;
       // vm.searchTextForPaymentMethod = searchTextForPaymentMethod;
        //vm.paymentCheckInactive = paymentCheckInactive;
        vm.paymentActionChanged = paymentActionChanged;
        vm.savePaymentMethod = savePaymentMethod;
        vm.activePaymentMethod = activePaymentMethod;
        vm.deleteSelected = deleteSelected;
        vm.editPayment = editPayment;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.activeItem = null;
        vm.check = false;
        vm.checkIfExistCode = checkIfExistCode;
        vm.checkIfExistName = checkIfExistName;
        vm.searchParamChanged = searchParamChanged;
        // getAllPaymentMethods();

        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteAllSelected = deleteAllSelected;
        $scope.selected = [];
        vm.btnPaymentMethodText = "Save";

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
                angular.forEach(vm.allPaymentMethod, function (check) {
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
        function deleteAllSelected(selectedItem) {
            var selectedItemId = [];
            for (var i = 0; i < selectedItem.length; i++) {
                selectedItemId.push(selectedItem[i].id);
            }
  
                paymentMethodService.deletedSelected(selectedItemId).then(function (result) {
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
        searchParamChanged();

        function checkIfExistCode(code) {
            if (code !== undefined) {
                paymentMethodService.checkIfPaymentMethodCodeExists(code).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Payment Method already exists");
                                editPayment(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your payment method?",
                                    "info",
                                    "This payment method is already exists but has been disabled. Do you want to activate this payment method?",
                                    "Active",
                                    "Your payment method has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }

        function checkIfExistName(name) {
            if (name !== undefined) {
                paymentMethodService.checkIfPaymentMethodNameExists(name).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Payment Method already exists");
                                editPayment(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your payment method?",
                                    "info",
                                    "This payment method is already exists but has been disabled. Do you want to activate this payment method?",
                                    "Active",
                                    "Your payment method has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }

        //function searchTextChangedForPaymentMethod(text, checked) {
        //    paymentMethodService.searchTextForPaymentMethod(text, checked)
        //      .then(function (result) {
        //          if (result.success) {
        //              vm.allPaymentMethod = result.data;
        //          }
        //      });
        //}

        //function searchTextForPaymentMethod(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getInactivePaymentMethod();
        //        } else {
        //            searchTextChangedForPaymentMethod(text, checked);
        //        }
               
        //    } else {
        //        if (text === "") {
        //            getAllPaymentMethods();
        //        } else {
        //            searchTextChangedForPaymentMethod(text, checked);
        //        }
              
        //    }
        //}

        //function paymentCheckInactive(active, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getInactivePaymentMethod();
               
        //    }
        //    else {
        //        vm.check = false;
        //        getAllPaymentMethods();
               

        //    }
        //}

        //function getInactivePaymentMethod() {
        //    paymentMethodService.getInactivePaymentMethod()
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.allPaymentMethod = result.data;

        //                } else {
        //                    console.log(result.message);
        //                }
        //            });
        //}

        //function getAllPaymentMethods() {
        //    paymentMethodService.GetAll()
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.allPaymentMethod = result.data;
        //                } else {
        //                    console.log(result.message);
        //                }
        //            });
        //}

        function paymentActionChanged(payment, action) {
            if (Number(action) === 1) {
                vm.btnPaymentMethodText = "Update";
                editPayment(payment.id,payment.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Payment method will be deactivated but still you can activate your payment in future.", "Yes, delete it!", "Your payment has been deactivated.", "delete", payment);
            }

            vm.action = null;
        }

        function deleteSelected(successMessage, item) {
            paymentMethodService.Delete(item.id).then(function (result) {
                if (result.success) {
                    swal(successMessage, "success");
                    searchParamChanged();
                }
            });
        }

        function savePaymentMethod(item) {
            if (item.id === undefined) {
                paymentMethodService.CreatePaymentMethod(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeItem = null;
                            $scope.showModal = false;
                            searchParamChanged();

                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            } else {
                                swal(result.message);
                            }
                        }
                    });
            } else {
                paymentMethodService.UpdatePaymentMethod(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeItem = null;
                            $scope.showModal = false;
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

        }

        function editPayment(paymentId, active) {
            if (active) {
                paymentMethodService.getPaymentMethodById(paymentId).then(function (result) {
                    if (result.success) {
                        vm.activeItem = result.data;
                        $scope.showModal = true;
                    }
                });
            }
            else {
                swal("You cann't edit this item.Please active it first");
            }
        }

        function activePaymentMethod(id) {
            paymentMethodService.activeMethod(id).then(function (result) {
                if (result.success) {
                    swal("Payment Method Activated.");
                    searchParamChanged();
                }

            });
        }

        $scope.open = function () {
            $scope.showModal = true;
            vm.btnPaymentMethodText = "Save";
            vm.paymentMethodForm.$setUntouched();
        };

        $scope.hide = function () {
            vm.activeItem = null;
            $scope.showModal = false;
        };
    
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
                        deleteSelected(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activePaymentMethod(value.id);
                        editPayment(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText === undefined || vm.searchText === "")
                vm.searchText = null;
                paymentMethodService.searchTextForPaymentMethod(vm.searchText, !vm.check)
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentMethod = result.data;
                        }
                });
        }
    }
})();