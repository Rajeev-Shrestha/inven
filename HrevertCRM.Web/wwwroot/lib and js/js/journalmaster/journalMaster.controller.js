(function () {
    angular.module("app-journalMaster")
        .controller("journalMasterController", journalMasterController);
    journalMasterController.$inject = ['$http', '$filter', '$scope', 'journalMasterService'];
    function journalMasterController($http, $filter, $scope,  journalMasterService) {

        var vm = this;
     //   vm.getPageSize = getPageSize;
        vm.totalRecords = 0;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.filteredCount = 0;
        vm.activeJournalMaster = [];
        vm.editJournalMaster = editJournalMaster;
        vm.journalMasterTermActionChanged = journalMasterTermActionChanged;
        vm.saveJournalMaster = saveJournalMaster;
        vm.deleteJournalMaster = deleteJournalMaster;
        vm.check = false;
       // vm.checkInactiveJournalMaster = checkInactiveJournalMaster;
        vm.activateJournalMaster = activateJournalMaster;
        vm.pageChanged = pageChanged;
       // vm.searchTextForJournalMaster = searchTextForJournalMaster;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.authentications = [{ id: 'true', name: 'Yes' }, { id: 'false', name: 'No' }];
        vm.searchParamChanged = searchParamChanged;
        vm.totalJournalMaster = 0;
        init();
        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;
        vm.journalMasterBtnText = "Save";

        $scope.selected = [];

        vm.exist = function (item) {
            return $scope.selected.indexOf(item) > -1;
        }

        function toggleSelection(item, event) {
            if (event.currentTarget.checked) {
                $scope.selected.push(item)
            } else {
                for (var i = 0; i < $scope.selected.length; i++) {
                    if($scope.selected[i].id === item.id)
                    {
                        $scope.selected.splice($scope.selected.indexOf($scope.selected[i]), 1);
                        $scope.selectAll = [];
                        return;
                    }
                }
            }
        }

        function checkAll() {
            if ($scope.selectAll) {
                angular.forEach(vm.allJournalMaster, function (check) {
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
            journalMasterService.deletedSelected(selectedItemid).then(function (result) {
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
        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    if (vm.check === true) {
        //        getAllJournalMaster();
        //    } else {
        //        getAllActiveJournalMaster();
        //    }
        //}

        function pageChanged(page) {
            $scope.selectAll = [];

            vm.currentPage = page;
            searchParamChanged();
        };

        function init() {
            getJournalMasterPageSize();
            loadAllJounalTypes();
        }

        //function getAllJournalMaster() {
        //    journalMasterService.getAllJournalMaster(vm.currentPage, vm.pageSize).then(function (result) {
        //        if (result.success) {
        //            vm.allJournalMaster = result.data.items;
        //            vm.pageNumber = result.data.pageNumber;
        //            vm.totalPage = result.data.pageCount;
        //        } else {
        //            alert(result.message);
        //        }

        //    });
        //}

        $scope.open = function () {
            vm.activeJournalMaster = null;
            $scope.showModal = true;
            vm.journalMasterBtnText = "Save";
            vm.journalMasterForm.$setUntouched();
        }

        $scope.hide = function () {
            vm.activeJournalMaster = null;
            $scope.showModal = false;
        }
   
        function editJournalMaster(journalId, checked) {
            if (checked) {
                journalMasterService.getJournalMasterById(journalId)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeJournalMaster = result.data;
                            vm.activeJournalMaster.postedDate = moment(vm.activeJournalMaster.postedDate).format("YYYY-MM-DD");;
                            $scope.showModal = true;
                        } else {
                            console.log(result.message);
                        }
                    });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
        }

        function deleteJournalMaster(successMessage,journalMaster) {
            journalMasterService.deleteJournalMaster(journalMaster).then(function (result) {
                if (result.success) {
                    swal(successMessage,"success");
                    searchParamChanged();
                }

            });
        }

        function journalMasterTermActionChanged(data, action) {
            vm.action = null;

            if (Number(action) === 1) {
                vm.journalMasterBtnText= "Update";
                editJournalMaster(data.id, data.active);
            }
            else if (Number(action) === 2) {
                //deleteJournalMaster(data);
                yesNoDialog("Are you sure?", "warning", "Journal master will be deactivated but still you can activate your journal master in future.", "Yes, delete it!", "Your journal master has been deactivated.", "delete", data);
            }
        }

        function saveJournalMaster(journalMaster) {
            if (journalMaster.id === undefined) {
                journalMaster.postedDate = moment(journalMaster.postedDate).format("MM-DD-YYYY");
                journalMasterService.createJournalMaster(journalMaster)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeJournalMaster = null;
                            searchParamChanged();
                            $scope.showModal = false;
                        } else {
                           if (result.message.errors) {
                               swal("You have no rights");
                               vm.activeJournalMaster = "";
                            } else {
                               swal(result.errors);
                            }
                          
                        }
                    });

            } else {
                journalMasterService.updateJournalMaster(journalMaster)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeJournalMaster = null;
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

        //function checkInactiveJournalMaster(active,checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getAllJournalMaster();
        //    }
           
        //    else {
        //        vm.check = false;
        //        getAllActiveJournalMaster();
              

        //    }
        //}

        function activateJournalMaster(journalMaster) {
            journalMasterService.getActiveJournalMasterById(journalMaster.id)
                .then(function (result) {
                    if (result.success) {
                        swal("Journal Master Activated.");
                        searchParamChanged();
                    } else {
                        console.log(result.message);
                    }
                });
        }

        //function getAllActiveJournalMaster() {
        //    journalMasterService.getAllActiveJournalMaster(vm.currentPage, vm.pageSize).then(function (result) {
        //        if (result.success) {
        //            vm.pageNumber = result.data.pageNumber;
        //            vm.totalPage = result.data.pageCount;
        //            vm.allJournalMaster = result.data.items;
        //        } else {
        //            console.log(result.message);
        //        }
        //    });
        //}

        //function searchTextForJournalMaster(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getAllJournalMaster();
        //        } else {
        //            searchTextChanged(text, checked);
        //        }
        //    } else {
        //        if (text === "") {
        //            getAllActiveJournalMaster();
        //        } else {
        //            searchTextChanged(text, checked);
        //        }
        //    }
        //}

        //function searchTextChanged(text, checked) {
        //    journalMasterService.searchTextForJournalMaster(vm.currentPage, vm.pageSize, text, checked)
        //     .then(function (result) {
        //         if (result.success) {
        //             vm.allJournalMaster = result.data.items;
        //             vm.pageNumber = result.data.pageNumber;
        //             vm.totalPage = result.data.pageCount;
        //         }
        //     });
        //}

        function loadAllJounalTypes() {
            journalMasterService.getAllJournalTypes()
            .then(function (result) {
                if (result.success) {
                    vm.journalTypes = result.data;
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
                        deleteJournalMaster(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateJournalMaster(value);
                        editJournalMaster(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function getJournalMasterPageSize() {
            journalMasterService.getPageSize().then(function (res) {
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
        function searchParamChanged() {
            if (vm.searchText == undefined)
                vm.searchText = "";
            if (vm.totalJournalMaster < vm.pageSize)
                vm.currentPage = 1;
            journalMasterService.searchTextForJournalMaster(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.allJournalMaster = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.totalJournalMaster = result.data.totalRecords;
                    }
                });
        }
    }

})();