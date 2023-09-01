(function () {
    angular.module("app-securityright")
        .controller("securityRightController", securityRightController);
    securityRightController.$inject = ['$http', '$filter','$scope', 'SecurityRightService'];

    function securityRightController($http, $filter, $scope, securityRightService) {
        var vm = this;
        vm.allRoles = [];
        vm.allSecurityList = [];
       // vm.getPageSize = getPageSize;
      //  vm.getAllSecurity = getAllSecurity;
        vm.loadingPage = true;
        vm.filteredCount = 0;
        vm.selectedGroupId = 0;
        vm.selectedUserId = "";
        vm.orderby = 'securityDescription';
        vm.reverse = false; 
        vm.reverse = false;
        vm.searchText = null;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.saveSecurity = saveSecurity;
        vm.assignedSecuirities = [];
        vm.hideGrid = true;
        vm.loadSelectedGroupWithAllSecurityAssignedOrNot = loadSelectedGroupWithAllSecurityAssignedOrNot;
        vm.loadSelectedUserWithAllSecurityAssignedOrNot = loadSelectedUserWithAllSecurityAssignedOrNot;
        vm.idSelectedRole = 0;
        vm.pageChanged = pageChanged;
      //  vm.getPageSize = getPageSize;
        vm.pageSize = "10";
       // vm.searchParamChanged = searchParamChanged;
        init();
        function init() {
            loadAllRoles();
            securityRightService.searchTextForUser("null", true)
                         .then(function (result) {
                             if (result.success) {
                                 vm.users = result.data;
                             }
                             else{
                                 alert(result.message);
                             }
                         });
        }
        vm.setSelected = function (idSelectedRole) {
            vm.idSelectedRole = idSelectedRole;
            vm.selectedGroupId = idSelectedRole;
            getSecurityAssignedGroup();
        }

        function loadSelectedGroupWithAllSecurityAssignedOrNot(role) {
            vm.loadingTable = true;
            vm.showSecurityList = false;
            vm.selectedGroupId = role.id;
            vm.selectedGroup = role;
            securityRightService.GetSecurityAssignedToGroup(vm.selectedGroup.id)
               .then(function (result) {
                   if (result.success) {
                       vm.assignedSecuirities = result.data;
                     //  getAllSecurity();
                       //  searchParamChanged();
                       //var selected = { 'selected': false }
                       //var allSecurities = angular.extend(result.data, selected);
                       //angular.forEach(result.data, function (security) {
                       //    if (security.allowed === true) {
                       //        selected = { 'selected': true }
                       //        angular.extend(security, selected);
                       //    }
                       //    else {
                       //        selected = { 'selected': false }
                       //        angular.extend(security, selected);
                       //    }

                       //    //if (vm.assignedSecuirities.indexOf(security.id) > -1) {
                       //    //    security.selected = true;
                       //    //}
                       //});

                       vm.allSecurityList = result.data;
                       vm.showSecurityList = true;
                       vm.hideGrid = false;
                       vm.loadingTable = false;
                   } else {
                       //Show error
                   }
               });
            vm.hideGrid = false;
        }

        function loadSelectedUserWithAllSecurityAssignedOrNot(role) {
            vm.loadingTable = true;
            vm.showSecurityList = false;
            vm.selectedUserId = role.id;
            vm.selectedGroup = role;
            securityRightService.GetSecurityAssignedToUser(vm.selectedGroup.id)
               .then(function (result) {
                   if (result.success) {
                       vm.assignedSecuirities = result.data;
                       vm.allSecurityList = result.data;
                       vm.showSecurityList = true;
                       vm.hideGrid = false;
                       vm.loadingTable = false;
                   } else {
                       //Show error
                   }
               });
            vm.hideGrid = false;
        }
        function loadAllRoles() {
            securityRightService.GetAll()
            .then(function (result) {
                if (result.success) {
                    vm.loadingTable = true;
                    vm.allRoles = result.data;
                    if (vm.allRoles.length > 0) {
                        loadSelectedGroupWithAllSecurityAssignedOrNot(vm.allRoles[0])
                    }
                    vm.loadingTable = false;
                    vm.loadingPage = false;
                } else {
                    //Show error
                }
            });
        }
        //function getSecurityRightsPageSize() {
        //    securityRightService.getPageSize().then(function (res) {
        //        if (res.success) {
        //            if (res.data != 0) {
        //                vm.pageSize = res.data;
        //                searchParamChanged();
        //            }
        //            else {
        //                vm.pageSize = 10;
        //                searchParamChanged();
        //            }
        //        }
        //    });
        //}
        //function searchParamChanged() {
        //    if (vm.searchText == undefined)
        //                vm.searchText = "";
        //                vm.check = true;
        //                securityRightService.searchTextForSecurityRights(vm.searchText, vm.check, vm.currentPage, vm.pageSize)
        //    //securityRightService.GetAllSecurity(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.totalPage = result.data.pageCount;
        //                vm.pageNumber = result.data.pageNumber;
        //               // extend result to make sure they are checked
        //                var selected = { 'selected': false }
        //                var allSecurities = angular.extend(result.data.items, selected);
        //                angular.forEach(result.data.items, function (security) {
        //                    angular.extend(security, selected);

        //                    if (vm.assignedSecuirities.indexOf(security.id) > -1) {
        //                        security.selected = true;
        //                    }
        //                });


        //                vm.allSecurityList = allSecurities;
        //            }
        //            else {
        //               // Show error
        //            }
        //        });
        //}

        function saveSecurity(security) {
            vm.saveItemInfo = {};
            vm.saveItemInfo = security;
            vm.saveItemInfo.groupId = vm.selectedGroupId;
            vm.saveItemInfo.userId = vm.selectedUserId;
            vm.saveItemInfo.isAssigned = security.allowed;
            //if (security.selected === false) {
            //    vm.saveItemInfo.isAssigned = true;
            //}
            //else {
            //    vm.saveItemInfo.isAssigned = false;
            //}

            //  vm.saveItemInfo.securityId = security.id;
            if (vm.saveItemInfo.groupId != 0 ) {
            securityRightService.saveSecurity(vm.saveItemInfo)
                .then(function (result) {
                    if (result.success) {
                        loadSelectedGroupWithAllSecurityAssignedOrNot(vm.selectedGroup);
                       // getAllSecurity();
                       // searchParamChanged();
                        vm.showSecurityList = true;
                    } else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        } else {
                            swal(result.message);
                        }
                    }
                });
            }
            else if ( vm.saveItemInfo.userId !=0 || vm.saveItemInfo.userId != null || vm.saveItemInfo.userId != "" ) {
                securityRightService.saveUserSecurity(vm.saveItemInfo)
            .then(function (result) {
                if (result.success) {
                    loadSelectedUserWithAllSecurityAssignedOrNot(vm.selectedGroup);
                    // getAllSecurity();
                    // searchParamChanged();
                    vm.showSecurityList = true;
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

        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    getAllSecurity();
        //}

        function pageChanged(page) {
            vm.currentPage = page;
            searchParamChanged();
        };

        //vm.searchTextChanged = function (text) {
        //    if (text != "") {
        //        securityRightService.searchText(vm.currentPage, vm.pageSize, text)
        //            .then(function (result) {
        //                if (result.success) {
        //                    var selected = { 'selected': false }
        //                    var allSecurities = angular.extend(result.data.items, selected);
        //                    angular.forEach(result.data.items, function (security) {
        //                        angular.extend(security, selected);

        //                        if (vm.assignedSecuirities.indexOf(security.id) > -1) {
        //                            security.selected = true;

        //                        }
        //                    });

        //                    vm.allSecurityList = allSecurities;
        //                    vm.totalPage = result.data.pageCount;
        //                    vm.pageNumber = result.data.pageNumber;
        //                } else {
        //                    //Show error
        //                }
        //            });
        //    }
        //else{
        //    getAllSecurity();
        //    }
        //}
        //function getSecurityRightsPageSize() {
        //    securityRightService.getPageSize().then(function (res) {
        //        if (res.success) {
        //            if (res.data != 0) {
        //                vm.pageSize = res.data;
        //                searchParamChanged();
        //            }
        //            else {
        //                vm.pageSize = 10;
        //                searchParamChanged();
        //            }
        //        }
        //    });
        //}
        //function searchParamChanged() {
        //    if (vm.searchText == undefined)
        //        vm.searchText = "";
        //        vm.check = true;
        //        securityRightService.searchTextForSecurityRights(vm.searchText, vm.check, vm.currentPage, vm.pageSize)
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.allSecurityList = result.data.items;
        //                    vm.totalPage = result.data.pageCount;
        //                    vm.pageNumber = result.data.pageNumber;
        //                }
        //            });
        //}
    }
})();

