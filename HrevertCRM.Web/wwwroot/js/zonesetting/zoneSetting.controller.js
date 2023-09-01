(function () {
    angular.module("app-zoneSetting")
        .controller("zoneSettingController", zoneSettingController);
    zoneSettingController.$inject = ['$http', '$filter', '$scope', 'zoneSettingService'];
    function zoneSettingController($http, $filter, $scope, zoneSettingService) {

        var vm = this;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.zoneActionChange = zoneActionChange;
        vm.saveZone = saveZone;
        vm.editZone = editZone;
        vm.includeInactive = includeInactive;
        vm.activeDeliveryZone = activeDeliveryZone;
        vm.checkIfExistCode = checkIfExistCode;
        vm.check = false;
        vm.zoneCode = false;
        vm.searchParamChanged = searchParamChanged;
        vm.zoneSettingBtnText = "Save";
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
                angular.forEach(vm.zone, function (check) {
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
            zoneSettingService.deletedSelected(selectedItemid).then(function (result) {
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

        function checkIfExistCode(code)
        {
            zoneSettingService.checkIfDeliveryZoneCodeExists(code).then(function (result) {
                if (result.success) {
                    if (result.data) {
                        if (result.data.active) {
                            swal("Delivery Zone already exists");
                            editZone(result.data.id, true);
                        } else {
                            yesNoDialog("Do you want to active your zone?", "info", "This zone is already exists but has been disabled. Do you want to activate this zone?", "Active", "Your product has been activated.", "active", result.data);
                        }
                    }
                }
            });
        }

        $scope.open = function () {
            vm.activeZone = null;
            vm.zoneCode = false;
            $scope.showModal = true;
            vm.zoneSettingBtnText = "Save";
            vm.zoneForm.$setUntouched();
        }

        $scope.hide = function () {
            vm.activeZone = null;
            vm.zoneCode = false;
            $scope.showModal = false;
        }

        function init() {
            //zoneSettingService.getActiveZone().then(function (result) {
            //    if (result.success) {
            //        vm.zone = result.data;
            //    } else {
            //        console.log(result.message);
            //    }

            //});
            searchParamChanged();

        }

        function editZone(zoneId, checked) {
            if (checked) {
                zoneSettingService.getZoneById(zoneId)
                    .then(function (result) {
                        if (result.success) {
                            vm.zoneCode = true;
                            vm.activeZone = result.data;
                            $scope.showModal = true;
                        }
                    });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
        }

        //function getIncludeInactiveZone() {
        //    zoneSettingService.getAllZone()
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.zone = result.data;
        //            } else {
        //                alert(result.message);
        //            }
        //        });
        //}

        function includeInactive(event, checked) {
            if (checked) {
                vm.check = true;
                //getIncludeInactiveZone();
                searchParamChanged();
            }
            else {
                vm.check = false;
                init();
            }
        }

        function activeDeliveryZone(zoneId) {
            zoneSettingService.activeZone(zoneId)
                .then(function (result) {
                    if (result.success) {
                        swal("Zone Activated.");
                        //getIncludeInactiveZone();
                        searchParamChanged();
                    }
                });
        }

        function deleteZone(successMessage,zone) {
            zoneSettingService.deleteZone(zone.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage, "success");
                        if (vm.check===true) {
                            //getIncludeInactiveZone();
                            searchParamChanged();
                        } else {
                            init();
                        }

                    } else {
                        console.log(result.message);
                    }
                });
        }

        function zoneActionChange(zone, action) {
            if (Number(action) === 1) {
                vm.zoneSettingBtnText = "Update";
                editZone(zone.id, zone.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Zone will be deactivated but still you can activate your zone in future.", "Yes, delete it!", "Your zone has been deactivated.", "delete", zone);
            }
            vm.action = null;
        }

        function saveZone(zone) {
            if (zone.id) {
                zoneSettingService.updateZone(zone)
                    .then(function (result) {
                        if (result.success) {
                            init();
                            $scope.showModal = false;
                            vm.zone = result.data;
                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            } else {
                                swal(result.message);
                            }
                        }
                    });
            } else {
                zoneSettingService.createZone(zone)
                .then(function (result) {
                    if (result.success) {
                        init();
                        $scope.showModal = false;
                        vm.zone = result.data;
                    }
                    if (result.message.errors) {
                        swal(result.message.errors[0]);
                    } else {
                        swal(result.message);
                    }

                });
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
                        deleteZone(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeDeliveryZone(value.id);
                        editZone(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
            zoneSettingService.searchTextForZoneSetting(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.zone = result.data;
                    }
                });
        }
    }
})();