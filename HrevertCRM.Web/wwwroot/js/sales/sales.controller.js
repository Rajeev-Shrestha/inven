(function () {
    angular.module("app-sales")
        .controller("salesController", salesController);
    salesController.$inject = ['$http', '$filter', '$scope', 'salesService'];
    function salesController($http, $filter, $scope, salesService) {

        var vm = this;
        vm.onAccount = onAccount;
        vm.onSpecifiedDays = onSpecifiedDays;
        vm.onDiscountType = onDiscountType;
        vm.termActionChanged = termActionChanged;
        vm.activePaymentTerms = activePaymentTerms;
        vm.termCheckInactive = termCheckInactive;
        vm.deletePaymentTerm = deletePaymentTerm;
        vm.savePaymentTerm = savePaymentTerm;
        vm.editPaymentTerm = editPaymentTerm;
        vm.searchTextForPaymentTerm = searchTextForPaymentTerm;
        vm.activePaymentTerm = null;
        vm.termIncludeInactive = false;
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

        init();

        function init() {
            getAllPaymentMethods();
            getPaymentTerm();
            loadAllPaymentTermTypes();
            loadAllPaymentDiscountTypes();
            loadAllDueTypes();
            loadAllDueDateTypes();
        }

        function getPaymentTerm() {
            salesService.getPaymentTerm().then(function (result) {
                if (result.success) {
                    vm.allPaymentTerm = result.data;
                } else {
                    alert("Sorry");
                }
            });
        }


        function checkDirty() {
            vm.dirtyList = true;
        }

        function termNameSelected(term, payment) {
            if (vm.dirtyList) {
                if (confirm('Would you like to discard changes? Please Save before change.')) {
                    vm.dirtyList = false;

                    getPaymentTermItems(term);

                } else {

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
            salesService.getPaymentByTermId(term.id)
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
            salesService.savePaymentRoles(vm.newItems)
                .then(function (result) {
                    if (result.success) {
                        vm.dirtyList = false;
                        var message = "Payment method assigned succesfully";
                        alert(message);
                    }
                });
        }

        function loadAllPaymentTermTypes() {
            salesService.GetAllPaymentTermTypes()
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
            // vm.discountTypeForDiscountSetting.length = 0;
            salesService.GetAllPaymentDiscountTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.discountType = result.data;
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function loadAllDueTypes() {
            salesService.GetAllDueTypes()
                    .then(function (result) {
                        if (result.success) {
                            vm.dueType = result.data;
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function loadAllDueDateTypes() {
            salesService.GetAllDueDateTypes()
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
                editPaymentTerm(term.id, term.active);
            }
            else if (Number(action) === 2) {
                deletePaymentTerm(term);
            }

            vm.action = null;
        }

        function editPaymentTerm(termId, checked) {
            if (checked) {
                salesService.getPaymentTermById(termId).then(function (result) {
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
                alert("You cann't edit this item. Please activate it first.");
            }
        }

        function searchTextChangedForPaymentTerm(text, checked) {
            salesService.searchTextForPaymentTerm(text, checked)
              .then(function (result) {
                  if (result.success) {
                      vm.allPaymentTerm = result.data;
                  }
              });
        }


        function searchTextForPaymentTerm(text, checked) {
            if (checked === 'true') {
                if (text === "") {
                    getInactivePaymentTerm();
                } else {
                    searchTextChangedForPaymentTerm(text, checked);
                }
            } else {
                if (text === "") {
                    getPaymentTerm();
                } else {
                    searchTextChangedForPaymentTerm(text, checked);
                }
            }
        }


        function termCheckInactive(active, checked) {
            if (checked) {
                getInactivePaymentTerm();
                vm.termIncludeInactive = true;
            }

            else {
                vm.termIncludeInactive = false;
                getPaymentTerm();

            }
        }

        function getInactivePaymentTerm() {
            salesService.getInactivePaymentTerm()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentTerm = result.data;
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function activePaymentTerms(id) {
            salesService.activeTerm(id)
                    .then(function (result) {
                        if (result.success) {
                            getInactivePaymentTerm();
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function getPaymentTermWithoutAccountType() {
            salesService.getPaymentTermWithoutAccountType()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentTermWithoutAccountType = result.data;
                        } else {
                            alert("Sorry");
                        }
                    });
        }

        function deletePaymentTerm(item) {
            salesService.deletePaymentTerm(item.id).then(function (result) {
                if (result.success) {
                    if (vm.termIncludeInactive === 'true') {
                        getInactivePaymentTerm();
                    } else {
                        getPaymentTerm();
                    }
                }

            });
        }

        function savePaymentTerm(item) {

            if (item.discountType == null) {
                item.discountType = 0;
            }
            if (item.dueDateType == null) {
                item.dueDateType = 0;
            }
            if (item.id === undefined) {
                salesService.createPaymentTerm(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activePaymentTerm = null;
                            getPaymentTerm();
                            $scope.showModal = false;
                        } else {
                            alert(result.message);
                        }
                    });
            } else {
                salesService.updatePaymentTerm(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activePaymentTerm = null;
                            getPaymentTerm();
                            $scope.showModal = false;
                        } else {

                            alert(result.message);
                        }
                    });
            }

        }

        function onAccount(dueType) {
            console.log("term type changed:" + dueType);
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
            salesService.GetAll()
                    .then(function (result) {
                        if (result.success) {
                            vm.allPaymentMethod = result.data;
                        } else {
                            alert("Sorry");
                        }
                    });
        }


        $scope.open = function () {
            $scope.showModal = true;
        }

        $scope.assignPayment = function () {
            getAllPaymentMethods();
            getPaymentTermWithoutAccountType();
            $scope.showAssignModal = true;
        }

        $scope.hide = function () {
            vm.activePaymentTerm = null;
            $scope.showModal = false;
            $scope.showAssignModal = false;
        }
    }
})();