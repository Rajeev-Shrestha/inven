(function () {
    angular.module("app-fiscalyear")
        .controller("fiscalyearController", fiscalyearController)

    fiscalyearController.$inject = ['$http', '$scope', '$filter', 'fiscalyearService'];
    function fiscalyearController($http, $scope, $filter, fiscalyearService) {
        var vm = this;
        vm.editFiscal = editFiscal;
        vm.fiscalYears = [];
        vm.deletePeriod = deletePeriod;
        vm.actionChanged = actionChanged;
        vm.saveFiscalDetails = saveFiscalDetails;
        vm.check = false;
        vm.activeYear = activeYear;
        vm.searchText = null;
      //  vm.searchYear = searchYear;
        vm.actionItems = [{ id: 1, name: "Edit" }, { id: 2, name: "Delete" }];
        vm.addFiscalPeriod = addFiscalPeriod;
       // vm.checkInactive = checkInactive;
        vm.newFiscal = newFiscal;
        vm.fiscalPeriod = null;
        vm.validateDate = validateDate;
        vm.activeFiscalPeriod = activeFiscalPeriod;
        vm.fiscalYearSaveUpdateBtnText = "Save";
        vm.searchParamChanged = searchParamChanged;
        vm.getFiscalYear = getFiscalYear;
      
     
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
                angular.forEach(vm.fiscalYear, function (check) {
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
            fiscalyearService.deletedSelected(selectedItemid).then(function (result) {
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
        /* Bindable functions
 -----------------------------------------------*/;
        $scope.endDateBeforeRender = endDateBeforeRender
        $scope.endDateOnSetTime = endDateOnSetTime;
        $scope.startDateBeforeRender = startDateBeforeRender;
        $scope.startDateOnSetTime = startDateOnSetTime;
        var startDate; var endDate;

        function startDateOnSetTime(start) {
            startDate = start;
            $scope.$broadcast('start-date-changed');
        }

        function endDateOnSetTime(end) {
            endDate = end;
            $scope.$broadcast('end-date-changed');
        }

        function startDateBeforeRender($dates) {
            if (endDate) {
                var activeDate = moment(endDate);

                $dates.filter(function (date) {
                    return date.localDateValue() >= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = true;
                })
            }
        }
        function endDateBeforeRender($view, $dates) {
            if (startDate) {
                var activeDate = moment(startDate).subtract(1, $view).add(1, 'minute');
                $dates.filter(function (date) {
                    return date.localDateValue() <= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = false;
                })
            }
        }
        $scope.open = function () {
            vm.fiscalYearSaveUpdateBtnText = "Save";
            vm.activeFiscal = null;
            vm.fiscalPeriod = null;
            vm.showModal = true;
        };

        $scope.hide = function () {
            vm.activeFiscal = null;
            vm.fiscalPeriod = null;
            vm.fiscalPeriod = [];
            vm.showModal = false;
        };
        vm.fiscalYearText = "";
        function getFiscalYear(format) {         
                var typeOneFirst = format.substr(0, 2);
                var typeTwoFirst = format.substr(2, 2);        
                $scope.startYear = '20' + typeOneFirst + '-' + '01' + '-' + '01';
                $scope.endYear = '20' + typeOneFirst + '-' + '12' + '-' + '31';
                vm.fiscalYearText = "Please choose the date between " + $scope.startYear + " and " + $scope.endYear;
        }

       

        function getFiscalName(format, name) {
            var value = '';
            if (format === 1) {
                var typeOneFirst = name.substr(0, 2);
                var typeOneSecond = name.substr(2, 4);
                return value = 'FY-20' + typeOneFirst + '/' + typeOneSecond;
            }
            else if (format === 2) {
                var typeTwoFirst = name.substr(0, 2);
                var typeTwoSecond = name.substr(2, 4);
                return value = '20' + typeTwoFirst + '/' + typeTwoSecond;
            }
            else if (format === 3) {
                var typeThreeSecond = name.substr(2, 4);
                return value = 'FY-20' + typeThreeSecond;
            } else {
                var typeElseFirst = name.substr(0, 2);
                var typeElseSecond = name.substr(2, 4);
                return value = 'FY-20' + typeElseFirst + '/' + typeElseSecond;
            }
        }

        function processFiscalName(format) {
            if (format === 1) {
                vm.formatText = 'FY-20';
                vm.mask = '99/99';
            }
            else if (format === 2) {
                vm.formatText = '20';
                vm.mask = '99/99';
            }
            else if (format === 3) {
                vm.formatText = 'FY-20';
                vm.mask = '99';
            } else {
                vm.formatText = 'FY-20';
                vm.mask = '99/99';
            }
        }

        function init() {
            fiscalyearService.getCompanySetting().then(function (result) {
                if (result.success) {
                    vm.companySetting = result.data;
                    processFiscalName(result.data.fiscalYearFormat);
                }
                searchParamChanged();
                //fiscalyearService.GetAll().then(function (res) {
                //    if (res.success) {
                //        vm.fiscalYear = res.data;
                //        //for (var i = 0; i < vm.fiscalYear.length; i++) {
                //        //    vm.fiscalYear[i].name = getFiscalName(result.data.fiscalYearFormat, vm.fiscalYear[i].name);
                //        //}
                //    }

                //});
            });

        }

        //function searchYear(text, checked) {
        //    if (vm.check === true) {
        //        if (text === "") {
        //            getInactiveFiscalYear();
        //        } else {
        //            searchTextChanged(text, checked);
        //        }

        //    } else {
        //        if (text === "") {
        //            init();
        //        } else {
        //            searchTextChanged(text, checked);
        //        }
        //    }

        //}

        //function searchTextChanged(text, status) {
        //    fiscalyearService.searchFiscalYear(text, status).then(function (result) {
        //        if (result.success) {
        //            vm.fiscalYear = result.data;
        //        }
        //    });
        //}

        function actionChanged(fiscalYear, action) {
            if (Number(action) === 1) {
                editFiscal(fiscalYear.id, fiscalYear.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Fiscal Year will be deactivated but still you can activate your fiscal year in future.", "Yes, delete it!", "Your fiscal year has been deactivated.", "delete", fiscalYear);
              
            }

            vm.action = null;
        }

        function deleteFiscalYear(sucessMessage,fiscalYear) {
            fiscalyearService.Delete(fiscalYear.id).then(function (result) {
                if (result.success) {
                    swal(sucessMessage, "success");
                    if (vm.check === true) {
                        // getInactiveFiscalYear();
                        searchParamChanged();
                    } else {
                        init();
                    }
                }

            });
        }

        function activeYear(year) {
            fiscalyearService.activeYear(year.id).then(function (result) {
                if (result.success) {
                    swal("Fiscal Year Activated.");
                    //getInactiveFiscalYear();
                    searchParamChanged();
                }

            });
        }

        //function checkInactive(active) {
        //    if (active) {
        //        vm.check = true;
        //        getInactiveFiscalYear();

        //    }
        //    else {
        //        init();
        //        vm.check = false;

        //    }
        //}

        //function getInactiveFiscalYear() {
        //    fiscalyearService.getInactiveYear().then(function (res) {
        //        if (res.success) {
        //            vm.fiscalYear = res.data;
        //        }
        //    });
        //}

        function saveFiscalDetails(fiscalYear, period) {
            fiscalYear.fiscalPeriodViewModels = [];
            fiscalYear.fiscalPeriodViewModels = period;
            if (vm.companySetting.fiscalYearFormat === 1) {
                var typeOneFirst = fiscalYear.name.substr(0, 2);
                var typeOneSecond = fiscalYear.name.substr(2, 2);
                fiscalYear.name = vm.formatText + typeOneFirst + '/' + typeOneSecond;
            }
            else if (vm.companySetting.fiscalYearFormat === 2) {
                var typeTwoFirst = fiscalYear.name.substr(0, 2);
                var typeTwoSecond = fiscalYear.name.substr(2, 4);
                fiscalYear.name = vm.formatText + typeTwoFirst + '/' + typeTwoSecond;
            }
            else if (vm.companySetting.fiscalYearFormat === 3) {
                fiscalYear.name = vm.formatText + fiscalYear.name;
            }
            if (vm.activeFiscal.name === null || vm.activeFiscal.name === undefined) {
                swal("Please select the fiscal name first.");
             //   vm.activeFiscal.startDate = null;
             //   vm.activeFiscal.endDate = null;
                return;
            }
            var diff = typeTwoFirst - typeOneFirst;
                if (typeOneFirst >= typeOneSecond || diff != 1) {
                swal("Invalid fiscal name");
                fiscalYear.startDate = moment(fiscalYear.startDate).format("MM-DD-YYYY");
                fiscalYear.endDate = moment(fiscalYear.endDate).format("MM-DD-YYYY");
                vm.activeFiscal = null;
                return;
            }
            if (vm.activeFiscal.startDate === null || vm.activeFiscal.startDate === undefined && vm.activeFiscal.endDate === null || vm.activeFiscal.endDate === undefined) {
                swal("Please select the dates");
                return;
            }

           fiscalYear.startDate = moment(fiscalYear.startDate).format("MM-DD-YYYY");
           fiscalYear.endDate = moment(fiscalYear.endDate).format("MM-DD-YYYY");
            var startDate = Date.parse(fiscalYear.startDate);
            var endDate = Date.parse(fiscalYear.endDate);
            var fiscalStartDate = Date.parse(moment($scope.startYear).format("MM-DD-YYYY"));
            var fiscalEndDate = Date.parse(moment($scope.endYear).format("MM-DD-YYYY"));
          
            if (startDate < fiscalStartDate || startDate > fiscalEndDate && endDate<fiscalStartDate ||  endDate > fiscalEndDate) {
                swal("The date should be choosed between the selected fiscal name.");
                vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
                vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
                return;
            }

            if (fiscalYear.id) {
                        fiscalyearService.Update(fiscalYear)
                            .then(function (result) {
                                if (result.success) {
                                    vm.showModal = false;
                                    init();
                                } else {
                                    if (result.message.errors) {
                                        swal(result.message.errors[0]);
                                        vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
                                        vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
                                    } else {
                                        swal(result.message);
                                        vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
                                        vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
                                    }
                                }
                            });
                    }
                    else {
                        fiscalyearService.Create(fiscalYear)
                        .then(function (response) {
                            if (response.success) {
                                vm.showModal = false;
                                init();
                            }
                            else {
                                if (response.message.errors) {
                                    swalExtend(response.message.errors[0]);
                                    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
                                    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
                                } else {
                                    swal(response.message);
                                    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
                                    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
                                }
                            }
                        });
                     }
                
                

            //if (vm.companySetting.fiscalYearFormat === 1) {
            //    var typeOneFirst = fiscalYear.name.substr(0, 2);
            //    var typeOneSecond = fiscalYear.name.substr(2, 4);
            //    fiscalYear.name = vm.formatText + typeOneFirst + '/' + typeOneSecond;
            //}
            //else if (vm.companySetting.fiscalYearFormat === 2) {
            //    var typeTwoFirst = fiscalYear.name.substr(0, 2);
            //    var typeTwoSecond = fiscalYear.name.substr(2, 4);
            //    fiscalYear.name = vm.formatText + typeTwoFirst + '/' + typeTwoSecond;
            //}
            //else if (vm.companySetting.fiscalYearFormat === 3) {
            //    fiscalYear.name = vm.formatText + fiscalYear.name;
            //}
            //fiscalYear.startDate = moment(fiscalYear.startDate).format("MM-DD-YYYY");
          //  fiscalYear.endDate = moment(fiscalYear.endDate).format("MM-DD-YYYY");
            //var startDate = Date.parse(fiscalYear.startDate);
            //var endDate = Date.parse(fiscalYear.endDate);
            //fiscalYear.fiscalPeriodVieModels = period;
            //if (startDate > endDate ) {
            //    swal('End date must be greater than start date.');
            //    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //}
            //else {
            //    if (fiscalYear.id) {
            //        fiscalyearService.Update(fiscalYear)
            //            .then(function (result) {
            //                if (result.success) {
            //                    vm.showModal = false;
            //                    init();
            //                } else {
            //                    if (result.message.errors) {
            //                        swal(result.message.errors[0]);
            //                        vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //                        vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //                    } else {
            //                        swal(result.message);
            //                        vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //                        vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //                    }
            //                }
            //            });
            //    }
            //    else {
            //        fiscalyearService.Create(fiscalYear)
            //        .then(function (response) {
            //            if (response.success) {
            //                vm.showModal = false;
            //                init();

            //            }
            //            else {
            //                if (response.message.errors) {
            //                    swalExtend(response.message.errors[0]);
            //                    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //                    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //                } else {
            //                    swal(response.message);
            //                    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //                    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //                }
            //            }
            //        });
            //     }
            //}
            //else {
            //    swal('End date must be greater than start date.');
            //    vm.activeFiscal.startDate = moment(fiscalYear.startDate).format('YYYY-MM-DD');
            //    vm.activeFiscal.endDate = moment(fiscalYear.endDate).format('YYYY-MM-DD');
            //}
        }
        function editFiscal(fiscalYearId, checked) {
            var secondDate, firstDate;
            if (checked) {
                vm.fiscalYearSaveUpdateBtnText = "Update";
                fiscalyearService.GetById(fiscalYearId)
                        .then(function (response) {
                            if (response.success) {
                                vm.activeFiscal = response.data;
                                //testing code
                                //vm.companySetting.fiscalYearFormat = 3;
                                //var name = '2031/32';
                                if (vm.companySetting.fiscalYearFormat === 1) {
                                    vm.mask = '99/99';
                                     firstDate = response.data.name.substring(response.data.name.lastIndexOf("FY-20") + 5, response.data.name.lastIndexOf("/"));
                                     secondDate = response.data.name.substr(response.data.name.indexOf("/") + 1);
                                     vm.activeFiscal.name = firstDate + secondDate;
                                     var fy = vm.activeFiscal.name;
                                     var typeOneFirst = fy.substr(0, 2);
                                     $scope.startYear = '20' + typeOneFirst + '/' + '01' + '/' + '01';
                                     $scope.endYear = '20' + typeOneFirst + '/' + '12' + '/' + '31';
                                }
                                else if (vm.companySetting.fiscalYearFormat === 2) {
                                    vm.mask = '99/99';
                                     firstDate = name.substring(name.lastIndexOf("20") + 2, name.lastIndexOf("/"));
                                     secondDate = name.substr(name.indexOf("/") + 1);
                                     vm.activeFiscal.name = firstDate + secondDate;
                                     var fy = vm.activeFiscal.name;
                                     var typeOneFirst = fy.substr(0, 2);
                                     $scope.startYear = '20' + typeOneFirst + '/' + '01' + '/' + '01';
                                     $scope.endYear = '20' + typeOneFirst + '/' + '12' + '/' + '31';
                                }
                                else if (vm.companySetting.fiscalYearFormat === 3) {
                                    vm.mask = '99';
                                    var first = response.data.name.substr(response.data.name.indexOf("FY-20") + 5);
                                    vm.activeFiscal.name = first;
                                    var fy = vm.activeFiscal.name;
                                    var typeOneFirst = fy.substr(0, 2);
                                    $scope.startYear = '20' + typeOneFirst + '/' + '01' + '/' + '01';
                                    $scope.endYear = '20' + typeOneFirst + '/' + '12' + '/' + '31';
                                }
                                vm.activeFiscal.startDate = moment(vm.activeFiscal.startDate).format('YYYY-MM-DD')
                                vm.activeFiscal.endDate = moment(vm.activeFiscal.endDate).format('YYYY-MM-DD')
                                newFiscal(vm.activeFiscal.id);
                                vm.showModal = true;
                            }

                        });
            }
            else {

                 swal("You cannot edit this item. Please active first");
            }

        }

        function newFiscal(fiscalyearId) {
            fiscalyearService.GetPeriodByYearId(fiscalyearId).then(function (res) {
                if (res.success) {
                    vm.fiscalPeriod = res.data;
                    for (var i = 0; i < res.data.length; i++) {
                        res.data[i].isEditingItem = "";
                        vm.fiscalPeriod[i].startDate = moment(vm.fiscalPeriod[i].startDate).format("YYYY-MM-DD");
                        vm.fiscalPeriod[i].endDate = moment(vm.fiscalPeriod[i].endDate).format("YYYY-MM-DD");
                    }


                }
            });
        }

        vm.fiscalPeriod = [];
        function addFiscalPeriod() {
            if (vm.fiscalPeriod) {
                vm.fiscalPeriod.push({ name: "", startDate: "", endDate: "", isEditingItem: "True", active: true });
            }
            else {
                vm.fiscalPeriod = [];
                vm.fiscalPeriod.push({ name: "", startDate: "", endDate: "", isEditingItem: "True", active: true });
            }
            
        }

        function deletePeriod(fiscalperiod) {
            fiscalyearService.DeletePeriod(fiscalperiod.id)
                    .then(function (response) {
                        if (response.success) {
                            var index = vm.fiscalPeriod.indexOf(fiscalperiod);
                            vm.fiscalPeriod.splice(index, 1);
                            if (fiscalperiod.fiscalyearId) {
                                newFiscal(fiscalperiod.fiscalYearId);
                            }
                        } else {
                            console.log("Sorry,something went wrong while deleting the file.");
                        }
                    });
        }

        function activeFiscalPeriod(period) {
            fiscalyearService.activateFiscalPeriodById(period.id).then(function (result) {
                if (result.success) {
                    newFiscal(result.data.fiscalYearId);
                }
            });
        }

        function validateDate(fiscalPeriod) {
            var start = moment(new Date(fiscalPeriod.startDate)).format('YYYY-MM-DD');
            var end = moment(new Date(fiscalPeriod.endDate)).format('YYYY-MM-DD');
            if (start !== "" || end !== "") {
                if (start > end) {
                    //  alert('period end must be greater than period start date.');
                    swal('period end must be greater than period start date.');
                    fiscalPeriod.startDate = "";

                }
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
                        deleteFiscalYear(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeYear(value);
                        editItem(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText === undefined || vm.searchText === "")
                vm.searchText = null;
            fiscalyearService.searchTextForFiscalYear(vm.searchText, !vm.check)
                .then(function (result) {
                    if (result.success) {
                        vm.fiscalYear = result.data;
                    }
                });
        }
    }
})();
