(function () {
    angular.module("app-deliveryRate")
        .controller("deliveryRateController", deliveryRateController);
    deliveryRateController.$inject = ['$http', '$filter', '$scope',  'deliveryRateService'];
    function deliveryRateController($http, $filter, $scope, deliveryRateService) {
        var vm = this;
        vm.loadAllDeliveryRate = loadAllDeliveryRate;
        vm.saveDeliveryRate = saveDeliveryRate;
        vm.deliveryRateActionChange = deliveryRateActionChange;
        vm.includeInactiveDeliveryRate = includeInactiveDeliveryRate;
        vm.activateDeliveryRate = activateDeliveryRate;
        vm.check = false;
        vm.editDeliveryRate = editDeliveryRate;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.compareWeight = compareWeight;
        vm.searchParamChanged = searchParamChanged;
        vm.deliveryRatebtnText = "Save";
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
                angular.forEach(vm.deliveryRate, function (check) {
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
            deliveryRateService.deletedSelected(selectedItemid).then(function (result) {
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
            vm.activeDeliveryRate = null;
            $scope.showModal = true;
            vm.deliveryRatebtnText = "Save";
            vm.deliveryRateForm.$setUntouched();    
        }

        $scope.hide = function () {
            vm.activeDeliveryRate = null;
            $scope.showModal = false;
        }

        function init() {
            searchParamChanged();
            loadDeliverySetting();
            getAllCategory();
            getAllProducts();
            getDeliveryMethod();
            getAllMeasureUnit();
        }

        function compareWeight(wightFrom, weightTo) {
            if(wightFrom<weightTo)
            {
                swal("Weight To value should be greater than Weight From.");
                vm.activeDeliveryRate.weightTo = "";                
            }

        }

        function activateDeliveryRate(rate) {
            deliveryRateService.activeDeliveryRate(rate.id)
                .then(function (result) {
                    if (result.success) {
                        if (vm.check===true) {
                           searchParamChanged();
                        } else {
                            init();
                        }
                    }
                });
        }

        function includeInactiveDeliveryRate(event, checked) {
            if (checked) {
                vm.check = true;
                getIncludeInactiveRate();
            }
           else {
                vm.check = false;
                loadAllDeliveryRate();
            }
        }

        function getIncludeInactiveRate() {
            deliveryRateService.includeInactiveRate()
                .then(function (result) {
                    if (result.success) {
                        vm.deliveryRate = result.data;
                    } else {
                        alert(result.message);
                    }
                });
        }

        function loadAllDeliveryRate() {
            deliveryRateService.getAllDeliveryRate()
                .then(function (result) {
                    if (result.success) {
                        vm.deliveryRate = result.data;
                    } else {
                        alert(result.message);
                    }
                });
        }

        function deliveryRateActionChange(delivery, action) {
            if (Number(action) === 1) {
                vm.activeDeliveryRate = null;
                vm.deliveryRatebtnText = "Update";
                editDeliveryRate(delivery.id, delivery.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Delivery Rate will be deactivated but still you can activate your delivery rate in future.", "Yes, delete it!", "Your delivery rate has been deactivated.", "delete", delivery);

            }
            vm.action = null;
        }

        function editDeliveryRate(deliveryId, checked) {
            if (checked) {
                deliveryRateService.getDeliveryRateById(deliveryId)
                    .then(function (result) {
                        if (result.success) {
                            loadDeliverySetting();
                            getAllCategory();
                            getAllProducts();
                            getDeliveryMethod();
                            getAllMeasureUnit();
                            vm.activeDeliveryRate = result.data;
                            $scope.showModal = true;
                        } else {
                            alert(result.message);
                        }
                    });
            }
            else {
                swal("You cannot edit this item.please activate it first.");
            }
        }

        function deleteDeliveryRate(successMessage , delivery) {
            deliveryRateService.deleteDeliveryRate(delivery.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage, "success");
                       searchParamChanged();
                    } else {
                        console.log(result.message);
                    }
                });
        }

        function saveDeliveryRate(delivery) {
            if (delivery.id) {
                deliveryRateService.updateDeliveryRate(delivery)
                    .then(function (result) {
                        if (result.success) {
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
                
                deliveryRateService.createDeliveryRate(delivery)
                    .then(function (result) {
                        if (result.success) {
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

        function getAllCategory() {
            deliveryRateService.getAllProductCategory()
                .then(function (result) {
                    if (result.success) {
                        vm.productCategory = result.data;

                    } else {
                        alert(result.message);
                    }
                });
        }

        function getAllMeasureUnit() {
            deliveryRateService.getMeasureUnit()
                .then(function (result) {
                    if (result.success) {
                        vm.measureUnit = result.data;
                    } else {
                        alert(result.message);
                    }
                });
        }

        function getDeliveryMethod() {
            deliveryRateService.getAllDelivery()
                    .then(function (result) {
                        if (result.success) {
                            vm.allDeliveryMethod = result.data;
                        } else {
                            console.log(result.message);
                        }
                    });
        }

        function loadDeliverySetting() {
            deliveryRateService.getActiveZone().then(function (result) {
                if (result.success) {
                    vm.zone = result.data;
                } else {
                    console.log(result.message);
                }

            });

        }

        function getAllProducts() {
            deliveryRateService.getAllProduct().then(function (result) {
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
                        deleteDeliveryRate(successMessage, value);

                        //deleteSelectedDelivery(successMessage, value);
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
            deliveryRateService.searchTextForDeliveryRate(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.deliveryRate = result.data;
                    }
                });
        }

   
    }
})();