(function () {
    angular.module("app-measureUnit")
        .controller("measureUnitController", measureUnitController);
    measureUnitController.$inject = ['$http', '$filter', '$scope',  'measureUnitService'];
    function measureUnitController($http, $filter, $scope, measureUnitService) {
        var vm = this;
        vm.measureUnitActionChange = measureUnitActionChange;
        vm.saveMeasureUnit = saveMeasureUnit;
       // vm.getAllMeasureUnit = getAllMeasureUnit;
      //  vm.inacludeInactiveMeasureUnit = inacludeInactiveMeasureUnit;
        vm.activateMeasureUnit = activateMeasureUnit;
        vm.check = false;
        vm.editMeasureUnit = editMeasureUnit;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.checkIfExistCode = checkIfExistCode;
        vm.checkIfExistName = checkIfExistName;
        vm.searchParamChanged = searchParamChanged;
        vm.measureUnitBtnText = "Save";
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
                angular.forEach(vm.measureUnit, function (check) {
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
           
            measureUnitService.deletedSelected(selectedItemid).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        searchParamChanged();
                        $scope.selected = [];
                        $scope.selectAll = [];

                    } else {
                        alert("errors");
                    }

                });
            
        }


        function checkIfExistCode(code) {
            if (code != undefined) {
                measureUnitService.checkIfMeasureCodeExists(code).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Measure Unit already exists");
                                editMeasureUnit(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your measure unit?",
                                    "info",
                                    "This measure unit is already exists but has been disabled. Do you want to activate this measure unit?",
                                    "Active",
                                    "Your measure unit has been activated.",
                                    "active",
                                    result.data);
                            }
                        } else {
                            vm.activeMeasureUnit.measure = "";
                            vm.activeMeasureUnit.entryMethod = -1;
                        }
                    }
                    else
                    {
                        alert(result.errors);
                    }
                });
            }
        }

        function checkIfExistName(name) {
            if (name != undefined) {
                measureUnitService.checkIfMeasureNameExists(name).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Measure Unit already exists");
                                editMeasureUnit(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your measure unit?",
                                    "info",
                                    "This measure unit is already exists but has been disabled. Do you want to activate this measure unit?",
                                    "Active",
                                    "Your measure unit has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }


        function init() {
            loadEntryMethodTypes();
            //getAllMeasureUnit();
            searchParamChanged();
          
        }

        $scope.hide = function () {
            vm.activeMeasureUnit = null;
            $scope.showModal = false;
            vm.measureUnitForm.$setUntouched();
        }

        $scope.open = function () {
            vm.activeMeasureUnit = null;
            $scope.showModal = true;
            vm.measureUnitBtnText = "Save";
            vm.measureUnitForm.$setUntouched();


        }

        //function inacludeInactiveMeasureUnit(event, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getInactiveMeasureunit();
        //    } else {
        //        vm.check = false;
        //        getAllMeasureUnit();
        //    }
        //}

        function activateMeasureUnit(unitId) {
            measureUnitService.activeMeasureUnit(unitId)
                .then(function (result) {
                    if (result.success) {
                        swal("Measure Unit Activated.");
                        searchParamChanged();
                    }
                });
        }


        //function getInactiveMeasureunit() {
        //    measureUnitService.getInactiveMeasureUnit()
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.measureUnit = result.data;
        //            }
        //        });
        //}

        //function getAllMeasureUnit() {
        //    measureUnitService.getMeasureUnit()
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.measureUnit = result.data;
        //            } else {
        //                alert(result.message);
        //            }
        //        });
        //}

        function measureUnitActionChange(measure, action) {
            if (Number(action) === 1) {
                vm.activeMeasureUnit = null;
                vm.measureUnitBtnText = "Update";
                editMeasureUnit(measure.id, measure.active);
            } else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Measure Unit will be deactivated but still you can activate your measure unit in future.", "Yes, delete it!", "Your measure unit has been deactivated.", "delete", measure);
            }
            vm.action = null;
        }

        function editMeasureUnit(measureId, checked) {
            if (checked) {
                measureUnitService.measureUnitById(measureId).then(function (result) {
                    if (result.success) {
                        vm.activeMeasureUnit = result.data;
                        $scope.showModal = true;
                    }
                });
            }
            else{
                swal("You cann't edit this item.please activate it first.");
            }
        }

        function deleteMeasureUnit(successMessage,measure) {
            measureUnitService.deleteMeasureUnit(measure.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage,"success");
                        searchParamChanged();
                    } else {
                       console.log(result.message);
                    }
                });
        }

        function saveMeasureUnit(measure) {
            if (measure.id) {
                measureUnitService.updateMeaureUnit(measure)
                    .then(function (result) {
                        if (result.success) {
                            init();
                            vm.activeMeasureUnit = null;
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
                measureUnitService.createMeasureUnit(measure)
                    .then(function (result) {
                        if (result.success) {
                            init();
                            vm.activeMeasureUnit = null;
                            $scope.showModal = false;
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

        function loadEntryMethodTypes() {
            measureUnitService.getEntryMethodTypes()
                .then(function (result) {
                    if (result.success) {
                        vm.entryMethodTypes = result.data;
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
                        deleteMeasureUnit(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateMeasureUnit(value.id);
                        editMeasureUnit(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
            measureUnitService.searchTextForMeasureUnit(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.measureUnit = result.data;
                    }
                });
        }
    }
})();
