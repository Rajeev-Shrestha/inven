(function () {
    angular.module("app-discountSetting")
        .controller("discountSettingController", discountSettingController);
    discountSettingController.$inject = ['$http', '$filter', '$scope', 'discountSettingService'];
    function discountSettingController($http, $filter, $scope, discountSettingService) {

        var vm = this;
       // vm.getPageSize = getPageSize;
        vm.totalRecords = 0;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = "10"; 
        vm.currentPage = 1;
        vm.filteredCount = 0;
        vm.saveDiscount = saveDiscount;
        vm.discountActionChange = discountActionChange;
        vm.activeDiscountSetting = activeDiscountSetting;
        vm.editDiscountSetting = editDiscountSetting;
       // vm.includecheck = includecheck;
      //  vm.searchTextForDiscountSetting = searchTextForDiscountSetting;
        //vm.pageChanged = pageChanged;
        vm.disableFields = disableFields;
        vm.hideDiscountDialog = hideDiscountDialog;
        vm.check = false;
        vm.discountTypeForCategory = false;
        vm.discountValueChange = discountValueChange;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.searchParamChanged = searchParamChanged;
        vm.discountSettingBtnText = "Save";
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
                angular.forEach(vm.discount, function (check) {
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
            discountSettingService.deletedSelected(selectedItemid).then(function (result) {
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

        $scope.open = function () {
            getActiveCustomer();
            getActiveCustomerLevel();
            getAllCategory();
            getAllProducts();
            vm.activeDiscount = null;
            vm.disableDiscountProduct = false;
            vm.disableDiscountCategory = false;
            vm.disableDiscountCustomer = false;
            vm.disableDiscountLevel = false;
            $scope.showModal = true;
            vm.discountSettingBtnText = "Save";
            vm.discountForm.$setUntouched();

        }

        $scope.hide = function () {
            vm.activeDiscount = null;
            vm.discountTypeForCategory = "false";
            vm.imageTypeOption = "false";
            vm.productCategoryOption = "false";
            $scope.showModal = false;
        }

        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //}

        function loadDiscountTypes() {
            discountSettingService.getDiscountTypes().then(function (result) {
                if (result.success) {
                    vm.discountTypeForDiscountSetting = result.data;
                }
                });
        }

       function hideDiscountDialog() {
            vm.disableDiscountProduct = false;
            vm.disableDiscountCategory = false;
            vm.disableDiscountCustomer = false;
            vm.disableDiscountLevel = false;
            $scope.showModal = false;
        }

        function disableFields(discount) {
            if (Number(discount.itemId)) {
                //discountSettingService.checkIfDiscoutExistForThisProduct(discount.itemId).then(function (result) {
                //    if (result.success) {
                //        if (result.data) {
                //            if (result.data.id) {
                //                if (result.data.active) {
                //                    swal("Discount for this product already exist");
                //                    editDiscountSetting(result.data.id, true);
                //                } else {
                //                    yesNoDialog("Do you want to active?",
                //                        "info",
                //                        "This product is already exists but has been disabled. Do you want to activate this product?",
                //                        "Active",
                //                        "product has been activated.",
                //                        "active",
                //                        result.data);
                //                }
                //            }
                //        }
                //    }
                //    else {
                //        alert(result.errors);
                //    }
                //});
                vm.disableDiscountProduct = false;
                vm.disableDiscountCategory = true;
                vm.disableDiscountCustomer = true;
                vm.disableDiscountLevel = true;
                vm.discountTypeForCategory = false;
            }
            else if (Number(discount.categoryId)) {
                vm.disableDiscountCategory = false;
                vm.disableDiscountProduct = true;
                vm.disableDiscountCustomer = true;
                vm.disableDiscountLevel = true;
                vm.discountTypeForCategory = false;
            }
            else if (Number(discount.customerId)) {
                vm.disableDiscountCustomer = false;
                vm.disableDiscountCategory = true;
                vm.disableDiscountProduct = true;
                vm.disableDiscountLevel = true;
                vm.discountTypeForCategory = false;
            }
            else if (Number(discount.customerLevelId)) {
                vm.disableDiscountLevel = false;
                vm.disableDiscountCustomer = true;
                vm.disableDiscountCategory = true;
                vm.disableDiscountProduct = true;
                vm.discountTypeForCategory = false;
            }
            else {
                vm.disableDiscountProduct = false;
                vm.disableDiscountCategory = false;
                vm.disableDiscountCustomer = false;
                vm.disableDiscountLevel = false;
                vm.discountTypeForCategory = false;
            }
        }

        function discountValueChange(discountTypeId, discountValue) {

            if (Number(discountTypeId) === 1) {
                vm.activeDiscount.discountValue = 0;
            }

            var result = false;
            if (discountValue == undefined) return;
            if (Number(discountTypeId) === 2) {
                if (discountValue > 100) {
                    swal("Discount percentage cannot be greater than 100.");
                    vm.activeDiscount.discountValue = "";
                    result = true;
                }
                else if (discountValue <= 0) {
                    swal("Discount percentage cannot be less than 0.1");
                    result = true;
                }
                else {
                    result = false;
                }
            }

            if (Number(discountTypeId) === 3) {
                vm.activeDiscount.discountValue = discountValue;
                result = false;

            }

           
            return result;
        }

        //function searchTextForDiscountSetting(text) {
        //    discountSettingService.searchDiscount(vm.currentPage, vm.pageSize, text, vm.check)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.discount = result.data.items;
        //                vm.filteredCount = result.data.items.length;
        //                vm.totalRecords = result.data.totalRecords;
        //            }
        //        });
        //}

        //function pageChanged(pageSize) {
        //    vm.pageSize = pageSize;
        //    checkIfDiscountActive();
        //}


        //function includecheck(event, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getAllDiscountIncludeinactive();
        //    }
        //    else {
        //        vm.check = false;
        //        loadDiscount();
        //    }
        //}

        function init() {
            //loadDiscount();
            getDiscountSettingPageSize();
            loadDiscountTypes();

        }
        function getDiscountSettingPageSize() {
            discountSettingService.getPageSize().then(function (res) {
                if (res.success) {
                    if (res.data != 0) {
                        vm.pageSize = res.data;
                        searchParamChanged();
                    }
                    else {
                        vm.pageSize = 10;
                        searchParamChanged();
                    }
                }
            });
        }
        //function getAllDiscountIncludeinactive() {
        //    discountSettingService.inactiveDiscount(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.discount = result.data.items;
        //                vm.pageNumber = result.data.pageNumber;
        //                vm.totalPage = result.data.pageCount;
        //                angular.forEach(vm.discount, function (value, key) {
        //                    vm.discount[key].discountStartDate = new Date(value.discountStartDate);
        //                    vm.discount[key].discountEndDate = new Date(value.discountEndDate);
        //                });
        //            } else {
        //                console(result.message);
        //            }
        //        });
        //}

        function discountActionChange(discount, action) {
            if (Number(action) === 1) {
                vm.discountSettingBtnText = "Update";
                editDiscountSetting(discount.id, discount.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Discount setting will be deactivated but still you can activate your discount setting in future.", "Yes, delete it!", "Your discount setting has been deactivated.", "delete", discount);
               
            }
            vm.action = null;
        }

        //function checkIfDiscountActive() {
        //    if (vm.check===true) {
        //        getAllDiscountIncludeinactive();
        //    } else {
        //        loadDiscount();
        //    }
        //}

        function activeDiscountSetting(discount) {
            discountSettingService.activeDiscount(discount.id)
                .then(function (result) {
                    swal("Discount Setting Activated.");
                    if (result.success) {
                        searchParamChanged();                      
                    } else {
                        console(result.message);
                    }
                });
        }

        function editDiscountSetting(discountId, checked) {
            if (checked) {
                discountSettingService.getDiscountById(discountId)
                    .then(function (result) {
                        if (result.success) {
                            getActiveCustomer();
                            getActiveCustomerLevel();
                            getAllCategory();
                            getAllProducts();
                            vm.activeDiscount = result.data;
                            vm.activeDiscount.discountStartDate = moment(vm.activeDiscount.discountStartDate).format("YYYY-MM-DD");
                            vm.activeDiscount.discountEndDate = moment(vm.activeDiscount.discountEndDate).format("YYYY-MM-DD");
                            $scope.showModal = true;
                            disableFields(vm.activeDiscount);
                        } else {
                            console.log(result.message);
                        }
                    });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
        }
        //function loadDiscount() {
        //    discountSettingService.getActiveDiscount(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.discount = result.data.items;
        //                vm.pageNumber=result.data.pageNumber;
        //                vm.totalPage=result.data.pageCount;
        //                angular.forEach(vm.discount, function (value, key) {
        //                    vm.discount[key].discountStartDate = new Date(value.discountStartDate);
        //                    vm.discount[key].discountEndDate = new Date(value.discountEndDate);
        //                });
                       
        //            } else {
        //                console.log(result.message);
        //            }
        //        });
        //}

        function deleteDiscount(successMessage,discount) {
            discountSettingService.deleteDiscount(discount.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage,"success");
                        searchParamChanged();
                    } else {
                        console.log(result.message);
                    }
                });
        }

        function saveDiscount(discount) {
            discount.discountStartDate = moment(discount.discountStartDate).format("MM-DD-YYYY");
            discount.discountEndDate = moment(discount.discountEndDate).format("MM-DD-YYYY");
            var discountStartDate = Date.parse(discount.discountStartDate);
            var discountEndDate = Date.parse(discount.discountEndDate);
            if (discountStartDate > discountEndDate) {
                swal("End date must be greater than start date.");
                vm.activeDiscount.discountStartDate = moment(discount.discountStartDate).format("YYYY-MM-DD");
                vm.activeDiscount.discountEndDate = moment(discount.discountEndDate).format("YYYY-MM-DD");
            }
            else {

                var result = discountValueChange(discount.discountType, discount.discountValue);
                if (result !== true) {
                    if (discount.id) {
                        discountSettingService.updateDiscount(discount)
                            .then(function (result) {
                                if (result.success) {
                                    searchParamChanged();
                                    vm.activeDiscount = null;
                                    hideDiscountDialog();
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
                        discountSettingService.createDiscount(discount)
                            .then(function (result) {
                                if (result.success) {
                                    searchParamChanged();
                                  //  checkIfDiscountActive();
                                    vm.activeDiscount = null;
                                    hideDiscountDialog();
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
                    }
                }

            }
        }

        function getActiveCustomer() {
            discountSettingService.getCustomer()
                .then(function (result) {
                    if (result.success) {
                        vm.customers = result.data;
                    } else {
                        console.log(result.message);
                    }
                });
        }

        function getActiveCustomerLevel() {
            discountSettingService.getCustomerLevel()
                .then(function (result) {
                    if (result.success) {
                        vm.customerLevel = result.data;
                    } else {
                        console.log(result.message);
                    }
                });
        }

        function getAllCategory() {
            discountSettingService.getAllProductCategory()
                .then(function (result) {
                    if (result.success) {
                        vm.productCategory = result.data;

                    } else {
                        console.log(result.message);
                    }
                });
        }

        function getAllProducts() {
            discountSettingService.getAllProduct().then(function (result) {
                if (result.success) {
                    vm.products = result.data;
                } else {
                    console.log(result.message);
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
                        deleteDiscount(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeDiscountSetting(value);
                        editDiscountSetting(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
           if (vm.searchText == undefined) {
                vm.searchText = "";
            }
            discountSettingService.searchTextForDiscountSetting(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.discount = result.data.items;
                        vm.pagingList = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.pageSize = result.data.pageSize;
                        //angular.forEach(vm.discount, function (value, key) {
                        //    vm.discount[key].discountStartDate = new Date(value.discountStartDate);
                        //    vm.discount[key].discountEndDate = new Date(value.discountEndDate);
                        //});
                    }
                });
        }

    }
})();