(function () {
    angular.module("app-itemMeasure")
        .controller("itemMeasureController", itemMeasureController);
    itemMeasureController.$inject = ['$http', '$filter', '$scope', 'itemMeasureService'];
    function itemMeasureController($http, $filter, $scope, itemMeasureService) {

        var vm = this;
        //vm.loadItemMeasure = loadItemMeasure;
        vm.itemMeasureActionChange = itemMeasureActionChange;
        vm.editItemMeasure = editItemMeasure;
        vm.saveItemMeasure = saveItemMeasure;
       // vm.includeInactiveItemMeasure = includeInactiveItemMeasure;
        vm.activeItemmeasure = activeItemmeasure;
        vm.check = false;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.checkIfExistProduct = checkIfExistProduct;
        vm.btnSaveItemText = "Save";
        vm.searchParamChanged = searchParamChanged;
        //multiple select

        // vm.selectedAll = false;
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
                angular.forEach(vm.itemMeasure, function (check) {
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
            var itemMeasuresId = [];
            for (var i = 0; i < selectedItem.length; i++) {
                itemMeasuresId.push(selectedItem[i].id)
            }
            itemMeasureService.deletedSelected(itemMeasuresId).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        $scope.selected = [];
                        $scope.selectAll = [];
                        searchParamChanged();
                        $scope.selectAll = [];

                    } else {
                        alert("errors");
                    }
                });
            //}
        }


      //  loadItemMeasure();
        searchParamChanged();
        getAllMeasureUnit();

        getAllProducts();

        $scope.open = function () {
            vm.activeItemMeasure = null;
            vm.btnSaveItemText = "Save";
            $scope.showModal = true;
            vm.itemMeasureForm.$setUntouched();
        }

        $scope.hide = function () {
            vm.activeItemMeasure = null;
            $scope.showModal = false;
            vm.itemMeasureForm.$setUntouched();
        }

        function checkIfExistProduct(productId) {
            itemMeasureService.checkIfProductExists(productId).then(function(result) {
                if (result.success) {
                    if (result.data) {
                        if (result.data.id) {
                            if (result.data.active) {
                                swal("Item Measure already exists");
                                editItemMeasure(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your item measure?",
                                    "info",
                                    "This item measure is already exists but has been disabled. Do you want to activate this item measure?",
                                    "Active",
                                    "Your item measure has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }else {
                        vm.activeItemMeasure.measureUnitId = "";
                        vm.activeItemMeasure.price = "";
                    }
                }
                else
                {
                    alert(result.errors);
                }
            });
        }

        //function loadItemMeasure() {
            
        //    getAllMeasureUnit();

        //    itemMeasureService.getActiveMeasureItem().then(function (result) {
        //        if (result.success) {
        //            vm.itemMeasure = result.data;
        //            itemMeasureService.getAllProduct().then(function(result) {
        //                if (result.success) {
        //                    vm.products = result.data;
        //                    for (var i = 0; i < vm.itemMeasure.length; i++) {
        //                        for (var j = 0; j < vm.products.length; j++) {
        //                            if (vm.itemMeasure[i].productId === vm.products[j].id) {
        //                                vm.itemMeasure[i].productName = vm.products[j].name;
        //                            }
        //                        }
        //                    }

        //                } else {
        //                    console.log(result.message);
        //                }

        //            });
                  
        //        } else {
        //            console.log(result.message);
        //        }
               
        //    });
           
        //}

        function activeItemmeasure(itemId) {
            itemMeasureService.activeItemMeasure(itemId)
                .then(function (result) {
                    if (result.success) {
                        swal("Item Measure Activated.");
                        searchParamChanged();
                    }
                });
        }

        //function getItemMeasureIncludeInactive() {
        //    itemMeasureService.getAllItemMeasure().then(function (result) {
        //        if (result.success) {
        //            vm.itemMeasure = result.data;
        //            itemMeasureService.getAllProduct().then(function (result) {
        //                if (result.success) {
        //                    vm.products = result.data;
        //                    for (var i = 0; i < vm.itemMeasure.length; i++) {
        //                        for (var j = 0; j < vm.products.length; j++) {
        //                            if (vm.itemMeasure[i].productId === vm.products[j].id) {
        //                                vm.itemMeasure[i].productName = vm.products[j].name;
        //                            }
        //                        }
        //                    }
        //                } else {
        //                    alert(result.message);
        //                }

        //            });
               
                      
        //            } else {
        //                console.log(result.message);
        //            }
        //        });
        //}

        //function includeInactiveItemMeasure(event, checked) {
        //    if (checked) {
        //      vm.check = true;
        //        getItemMeasureIncludeInactive();
        //    }
        //    else {
        //      vm.check = false;
        //        loadItemMeasure();
        //    }
        //}

        function itemMeasureActionChange(item, action) {
            if (Number(action) === 1) {
                vm.btnSaveItemText = "Update";
                vm.activeItemMeasure = null;
                editItemMeasure(item.id, item.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Item Measure will be deleted and you can't activate your item measure in future.", "Yes, delete it!", "Your item measure has been deleted.", "delete", item);
               
            }
            vm.action = null;
        }

        function editItemMeasure(itemId, checked) {
            if (checked) {
                itemMeasureService.getItemMeasureById(itemId)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeItemMeasure = result.data;
                            $scope.showModal = true;
                        } else {
                            console.log(result.message);
                        }
                    });
            }
            else {
                swal("You cannot edit this item,.please activate it first.");
            }
        }

        function deleteItemMeasure(successMessage, item) {
            itemMeasureService.deleteItemMeasure(item.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage,"success");
                        searchParamChanged();

                    } else {
                        swal(result.message);
                    }
                });
        }

        function saveItemMeasure(item) {
            if (item.id) {
                itemMeasureService.updateItemMeasure(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeItemMeasure = null;
                            searchParamChanged();
                            $scope.showModal = false;
                        }
                        else if(result.message)
                        {
                            alert(result.message)
                        }
                        else {
                            alert(result.errors);
                        }

                    });
            } else {
                itemMeasureService.createItemMeasure(item)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeItemMeasure = null;
                            searchParamChanged();
                            $scope.showModal = false;
                        }
                        else if (result.message) {
                            alert(result.message);
                        }
                        else {
                            alert(result.errors);
                        }
                    });
            }
        }

        function getAllMeasureUnit() {
            itemMeasureService.getMeasureUnit()
                .then(function (result) {
                    if (result.success) {
                        vm.measureUnit = result.data;
                    } else {
                        alert(result.message);
                    }
                });
        }

        function getAllProducts() {
            itemMeasureService.getAllProduct().then(function (result) {
                if (result.success) {
                    vm.products = result.data;
                } else {
                    alert(result.message);
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
                        deleteItemMeasure(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeItemmeasure(value.id);
                        editItemMeasure(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText === undefined || vm.searchText === "")
                vm.searchText = null;
            itemMeasureService.searchTextForItemMeasure(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.itemMeasure = result.data;
                    }
                    else {
                        swal(result.message);
                    }
                });
        }
    }
})();