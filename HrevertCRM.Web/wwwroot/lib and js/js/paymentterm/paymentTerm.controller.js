(function () {
    angular.module("app-paymentTerm")
        .controller("paymentTermController", paymentTermController);
    paymentTermController.$inject = ['$http', '$filter', '$scope', 'paymentTermService'];
    function paymentTermController($http, $filter, $scope, paymentTermService) {

        var vm = this;
        vm.onAccount = onAccount;
        vm.onSpecifiedDays = onSpecifiedDays;
        vm.onDiscountType = onDiscountType;
        vm.termActionChanged = termActionChanged; 
        vm.activePaymentTerms = activePaymentTerms;
      //  vm.termCheckInactive = termCheckInactive;
        vm.deletePaymentTerm = deletePaymentTerm;
        vm.savePaymentTerm = savePaymentTerm;  
        vm.editPaymentTerm = editPaymentTerm;
        //vm.paymentTermText = paymentTermText;
        vm.activePaymentTerm = null;
        vm.check = false;
        vm.onAccountSelected = null;
        vm.onSpecifiedDaysSelected = null;
        vm.onDiscountTypeSelected = null;
        vm.termNameSelected = termNameSelected;
        vm.savePaymentRole = savePaymentRole;
        vm.checkDirty = checkDirty;
        vm.dirtyList = false;
        vm.termAssign = false;
        vm.activeTermId = null;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.checkIfExistCode = checkIfExistCode;
        vm.checkIfExistName = checkIfExistName;
        vm.searchParamChanged = searchParamChanged;
        vm.btnPaymentTermText = "Save";

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
                angular.forEach(vm.allPaymentTerm, function (check) {
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

            var allSelectedid = [];
            for (var i = 0; i < selectedItem.length; i++) {
                allSelectedid.push(selectedItem[i].id);
            }
          
            paymentTermService.deletedSelected(allSelectedid).then(function (result) {
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
        init();

        function checkIfExistCode(code) {
            if (code != undefined) {
                paymentTermService.checkIfPaymentTermCodeExists(code).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Payment Term already exists");
                                editPaymentTerm(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your payment term?",
                                    "info",
                                    "This payment term is already exists but has been disabled. Do you want to activate this payment term?",
                                    "Active",
                                    "Your payment term has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    } 
                });
            }
        }

        function checkIfExistName(name) {
            if (name != undefined) {
                paymentTermService.checkIfPaymentTermNameExists(name).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Payment Term already exists");
                                editPaymentTerm(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your payment term?",
                                    "info",
                                    "This payment term is already exists but has been disabled. Do you want to activate this payment term?",
                                    "Active",
                                    "Your payment term has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }

        function init() {
            getAllPaymentMethods();
            searchParamChanged();
           // getPaymentTerm();
            loadAllPaymentTermTypes();
            loadAllPaymentDiscountTypes();
            loadAllDueTypes();
            loadAllDueDateTypes();
        }

        //function getPaymentTerm() {
        //    paymentTermService.getPaymentTerm().then(function (result) {
        //        if (result.success) {
        //            vm.allPaymentTerm = result.data;
        //        } else {
        //            alert("Sorry");
        //        }
        //    });
        //}
     
        function checkDirty() {
            vm.dirtyList = true;
        }

        function termNameSelected(term, payment) {
            if (vm.dirtyList) {
                if (confirm('Would you like leave changes? Please Save before change.')) {
                    vm.dirtyList = false;

                    getPaymentTermItems(term);

                } 
            } else {
                getPaymentTermItems(term);
            }
        }

        function getPaymentTermItems(term) {
            vm.activeTermId = term.id;
            vm.termName = term.termName;
            vm.termAssign = true;
            getAllPaymentMethods();
            paymentTermService.getPaymentByTermId(term.id)
                .then(function (result) {
                    if (result.success) {
                        vm.paymentGroup = result.data;
                        for (var i = 0; i < vm.paymentGroup.length; i++) {
                            for (var j = 0; j < vm.allPaymentMethod.length; j++) {
                                if (vm.paymentGroup[i].id === vm.allPaymentMethod[j].id) {
                                    vm.allPaymentMethod.splice(vm.allPaymentMethod.indexOf(vm.allPaymentMethod[j]), 1);
                                }
                            }
                        }

                    }
                });
            vm.notAssignedPaymentMethod = vm.allPaymentMethod;
        }

        function savePaymentRole(items) {
            vm.newItems = {};
            vm.newItems.payTermId = vm.activeTermId;
            vm.newItems.payMethodIds = [];
            for (var i = 0; i < items.length; i++) {
                vm.newItems.payMethodIds.push(items[i].id);
            }
            paymentTermService.savePaymentRoles(vm.newItems)
                .then(function (result) {
                    if (result.success) {
                        vm.dirtyList = false;
                        var message = "Payment method assigned succesfully.";
                        dialogMessageForSuccess(message);
                        setTimeout(function () {
                            $('#successMessage').html('');
                        }, 3000);
                      
                        //alert(message);
                    }
                });
        }
    
        function loadAllPaymentTermTypes() {
            paymentTermService.GetAllPaymentTermTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.termType = result.data;
                            // console.log("load all termTypes:"+JSON.stringify(vm.termType));
                        } else {
                            alert("Sorry");
                    }
        });
    }

        function loadAllPaymentDiscountTypes() {
            paymentTermService.GetAllPaymentDiscountTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.discountType = result.data;
                        } else {
                            alert("Sorry");
                    }
        });
    }

        function loadAllDueTypes() {
            paymentTermService.GetAllDueTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.dueType = result.data;
                        } else {
                            alert("Sorry");
                    }
        });
    }

        function loadAllDueDateTypes() {
            paymentTermService.GetAllDueDateTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.dueDateType = result.data;
                        } else {
                            alert("Sorry");
                    }
        });
    }
        function termActionChanged(term, action) {
            if (Number(action) === 1) {
                vm.btnPaymentTermText = "Update";
                editPaymentTerm(term.id, term.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Payment term will be deactivated but still you can activate your payment term in future.", "Yes, delete it!", "Your payment term has been deactivated.", "delete", term);
              
        }

            vm.action = null;
    }

        function editPaymentTerm(termId, checked) {
            if (checked) {
                paymentTermService.getPaymentTermById(termId).then(function (result) {
                    if (result.success) {
                        vm.onAccountSelected = null;
                        vm.onSpecifiedDaysSelected = null;
                        vm.onDiscountTypeeSelected = null;
                        vm.activePaymentTerm = result.data;
                        if (vm.activePaymentTerm.dueType === 3) {
                            vm.onAccountSelected = 3;
                            if (vm.activePaymentTerm.dueDateType === 2) {
                                vm.onSpecifiedDaysSelected = 2;
                                if (vm.activePaymentTerm.discountType === 2) {
                                    vm.onDiscountTypeSelected = 2;
                            }
                        }
                    }
                        $scope.showModal = true;
                }
            });
               
            }

            else {
                swal("you cann't edit this item.please activate it first.");
        }
    }

    //    function searchTextChangedForPaymentTerm(text, checked) {
    //        paymentTermService.searchTextForPaymentTerm(text, checked)
    //          .then(function (result) {
    //              if (result.success) {
    //                  vm.allPaymentTerm = result.data;
    //          }
    //    });
    //}

    //    function paymentTermText(text, checked) {
    //        if (checked === true) {
    //            if (text === "") {
    //                getInactivePaymentTerm();
    //            } else {
    //                searchTextChangedForPaymentTerm(text, checked);
    //        }
    //        } else {
    //            if (text === "") {
    //                getPaymentTerm();
    //            } else {
    //                searchTextChangedForPaymentTerm(text, checked);
    //        }
    //    }
    //}

    //    function termCheckInactive(active, checked) {
    //        if (checked) {
    //            vm.check = true;
    //            getInactivePaymentTerm();
              
    //        }
           
    //        else {
    //            vm.check = false;
    //            getPaymentTerm();
               
    //    }
    //}

        function getInactivePaymentTerm() {
            paymentTermService.getInactivePaymentTerm()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentTerm = result.data;
                        } else {
                            console.log("Sorry"+result.message);
                    }
        });
    }

        function activePaymentTerms(id) {
            paymentTermService.activeTerm(id)
                    .then(function (result) {
                        if (result.success) {
                            swal("Payment Term Activated.");
                            searchParamChanged();
                        } else {
                            console.log("Sorry" + result.message);
                    }
        });
    }

        function getPaymentTermWithoutAccountType() {
            paymentTermService.getPaymentTermWithoutAccountType()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentTermWithoutAccountType = result.data;
                        } else {
                            alert("Sorry");
                    }
        });
    }

        function deletePaymentTerm(successMessage, item) {
            paymentTermService.deletePaymentTerm(item.id).then(function (result) {
                if (result.success) {
                    swal(successMessage, "success");
                    searchParamChanged();
            }

        });
    }

        function savePaymentTerm(item) {

            if (item.discountType === null) {
                item.discountType = 0;
        }
            if (item.dueDateType === null) {
                item.dueDateType = 0;
        }
            if (item.id === undefined) {  
                paymentTermService.createPaymentTerm(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activePaymentTerm = null;
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
            } else {
                paymentTermService.updatePaymentTerm(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activePaymentTerm = null;
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

        function onAccount(dueType) {
            if (Number(dueType) === 3) {
                vm.onAccountSelected = Number(dueType);
            }
            else {
                vm.onAccountSelected = null;
                vm.onSpecifiedDaysSelected = null;
                vm.onDiscountTypeSelected = null;
                vm.activePaymentTerm.dueDateValue = null;
                vm.activePaymentTerm.discountValue = null;
                vm.activePaymentTerm.discountType = null;
                vm.activePaymentTerm.discountDays = null;
                vm.activePaymentTerm.dueDateType = null;
                vm.activePaymentTerm.dueType = Number(dueType);
        }

    }

        function onSpecifiedDays(specifiedDays) {
            if (Number(specifiedDays) === 2) {
                vm.onSpecifiedDaysSelected = Number(specifiedDays);
            }
            else {
                vm.onSpecifiedDaysSelected = null;
                vm.onDiscountTypeSelected = null;
                vm.activePaymentTerm.dueDateValue = null;
                vm.activePaymentTerm.discountValue = null;
                vm.activePaymentTerm.discountType = null;
                vm.activePaymentTerm.discountDays = null;
        }
    }

        function onDiscountType(specifiedDays) {
            if (Number(specifiedDays) === 2) {
                vm.onDiscountTypeSelected = Number(specifiedDays);
            }
            else {
                vm.onDiscountTypeSelected = null;
                vm.activePaymentTerm.discountDays = null;
        }
    }

        function getAllPaymentMethods() {
            paymentTermService.GetAll()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentMethod = result.data;
                        } else {
                            alert("Sorry");
                    }
        });
    }

        $scope.open = function () {
            vm.activePaymentTerm = null;
            vm.onAccountSelected = "1";
            vm.onSpecifiedDaysSelected = "1";
            vm.onDiscountTypeSelected = "1";
            $scope.showModal = true;
            vm.btnPaymentTermText = "Save";
            vm.paymentTermForm.$setUntouched();
    }

        $scope.assignPayment = function () {
            $('#successMessage').html('');
            getAllPaymentMethods();
            getPaymentTermWithoutAccountType();
            $scope.showAssignModal = true;
            $scope.showModal = false;
    }

        $scope.hide = function () {
            vm.activePaymentTerm = null;
            $scope.showModal = false;
            $scope.showAssignModal = false;
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
                        deletePaymentTerm(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activePaymentTerms(value.id, false);
                        editPaymentTerm(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
                paymentTermService.searchTextForPaymentTerm(vm.searchText, !vm.check)
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentTerm = result.data;
                        }
                    });
        }

    }
})();