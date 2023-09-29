(function () {
    angular.module("app-user")
        .controller("userController", userController);
    userController.$inject = ['$http', '$scope', '$filter', '$sessionStorage', 'UserService'];
    function userController($http, $scope, $filter, $sessionStorage, userService) {
        var vm = this;
        vm.users = [];
        vm.activeItem = null;
        vm.activeUserGroup = null;
        vm.newRoles = "";
        vm.saveUser = saveUser;
        vm.editUser = editUser;
        vm.groupUserName = groupUserName;
        vm.roles = [];
        vm.saveRole = saveRole;
        vm.pageChanged = pageChanged;
        vm.hide = hide;
        vm.openAssignRoleDialog = openAssignRoleDialog;
        vm.actionChanged = actionChanged;
      //  vm.checkInactive = checkInactive;
        vm.activeUsers = activeUsers;
        vm.checkDirty = checkDirty;
        vm.totalRecords = 0;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.filteredCount = 0;
      //  vm.searchText = searchText;
      //  vm.getPageSize = getPageSize;
        vm.gender = [{ id: 1, name: 'Male' }, { id: 2, name: 'Female' }, { id: 3, name: 'Other' }];
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.userEmail = "false";
        vm.userName = "false";
        vm.passwordRequired = "true";
        vm.confirmPasswordRequired = "true";
        vm.addEditUser = addEditUser;
        vm.hideAssignDialog = hideAssignDialog;
        vm.showModal = false;
        vm.showAssignRoleModal = false;
        vm.check = false;
        vm.roleChange = roleChange;
        vm.checkIfExistEmail = checkIfExistEmail;
        vm.activeUser = {};
        vm.userRoles = [];
        vm.editPanel = false;
       // vm.editItem = editItem;
        vm.deleteUser = deleteUserData;
        vm.userBtnText = "Save";
        vm.searchParamChanged = searchParamChanged;
      //  getAllActiveUser();
        searchParamChanged();

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
                angular.forEach(vm.users, function (check) {
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
            userService.deletedSelected(selectedItemid).then(function (result) {
                    if (result.success) {
                        swal("Successfully Deleted!");
                        searchParamChanged();
                        $scope.selectAll = [];
                        $scope.selected=[];
                    } else {
                        alert("errors");
                    }

                });
            
        }
        function checkIfExistEmail(email) {
            if (email !== undefined) {
                userService.checkIfUserEmailExists(email).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("User already exists");
                                vm.activeUser.email = "";
                                //editUser(result.data, true);
                            } else {
                                yesNoDialog("Do you want to active your user?", "info", "This user is already exists but has been disabled. Do you want to activate this user?", "Active", "Your user has been activated.", "active", result.data);
                            }
                        }
                    }
                });
            }
        }

        function roleChange(status, role) {
            var idx = vm.userRoles.indexOf(role.id);
            if (status === true) {
                vm.userRoles.push(role.id);
            }
           else{
                vm.userRoles.splice(idx,1);
            }
        }

        function hideAssignDialog() {
            vm.showAssignRoleModal = false;
        }

        vm.sortableCloneOptions1 = {
            containment: '#sortable-container',
            allowDuplicates: false,
            ctrlClone: false
        };

        vm.sortableCloneOptions2 = {
            containment: '#sortable-container',
            allowDuplicates: true,
            ctrlClone: true
        };

        userService.getAllRoles().then(function (res) {
            if (res.success) {
                var selected = { 'selected': true }
                vm.roles = angular.extend(res.data, selected);
            }
        });

        userService.getsecurityRoles().then(function (res) {
            if (res.success) {
                var selected = { 'selected': true }
                vm.securityRoles = angular.extend(res.data, selected);
            }
        });

        function addEditUser(){
            vm.userEmail = false;
            vm.userName = false;
            vm.passwordRequired = true;
            vm.confirmPasswordRequired = true;
            vm.userRoles.pop();
            vm.showModal = true;
            vm.userBtnText = "Save";
            vm.userForm.$setUntouched();    
        }
        
        function checkDirty() {
            vm.dirtyList = true;
        }

        function actionChanged(user, id) {
            if (Number(id) === 1) {
                vm.userEmail = true;
                vm.userBtnText="Update"
                editUser(user, user.active);    
            }
            else if (Number(id) === 2) {
                yesNoDialog("Are you sure?", "warning", "User will be deactivated but still you can activate your user in future.", "Yes, delete it!", "Your user has been deactivated.", "delete", user);
            }
            else if (Number(id) === 3) {
                alert("Create report not working");
            }
            vm.action = null;
        }
        function deleteUserData(user) {
            yesNoDialog("Are you sure?", "warning", "User will be deactivated but still you can activate your user in future.", "Yes, delete it!", "Your user has been deactivated.", "delete", user);
        }
        function deleteUser(successMessage,userId) {
            userService.deleteUser(userId).then(function (result) {
                if (result.success) {
                    swal(successMessage, "success");
                    searchParamChanged();
                }
                else {
                    swal(result.message);
                }
            });
        }

        function openAssignRoleDialog(ev) {
            $('#successMessage').html('');
            vm.showAssignRoleModal = true;
            vm.hideRoles = true;
        }

        function pageChanged(page) {
            $scope.selectAll = [];

            vm.currentPage = page;
            searchParamChanged();

        }

        function hide() {
            vm.userEmail = false;
            vm.userName = false; 
            vm.passwordRequired = true;
            vm.confirmPasswordRequired = true;
            vm.activeUser = null;
            vm.userRoles.pop();
            vm.showModal = false;
        }

        function editUser(user, active) {
            if (active) {
                angular.forEach(user.securityGroups, function (role) {
                    var foundRoles = $filter('filter')(vm.securityGroups, { id: role });
                    if (foundRoles.length > 0) {
                        foundRoles[0].selected = true;
                        user.securityGroups.push(foundRoles[0]);
                    }
                });

                userService.GetById(user.id).then(function (result) {
                    if (result.success) {
                        vm.activeUser = result.data;
                        if (vm.activeUser.securityGroupIdList!==null) {
                            vm.userRoles=vm.activeUser.securityGroupIdList;
                        }
                     
                        vm.passwordRequired = false;
                        vm.confirmPasswordRequired = false;
                        vm.userEmail = true;
                        vm.userName = true;
                        vm.showModal = true;
                    }

                });


            }
            else {
                swal("You cannot edit this item. Please active first.");
            }
            
        }

        //function getAllActiveUser() {
        //    userService.GetAll().then(function (res) {
        //        if (res.success) {
        //            vm.users = res.data;
        //            vm.totalRecords = res.data.totalRecords;
        //            vm.filteredCount = res.data.length;
        //        }
        //    });
        //}

        //function getInactiveUser() {
        //    userService.getInactives().then(function (res) {
        //        if (res.success) {
        //            vm.users = res.data;
        //            vm.filteredCount = res.data.length;
        //            vm.totalRecords = res.totalRecords;
        //        }
        //    });
        //}

        function activeUsers(user) {
            userService.activeUser(user.id).then(function (res) {
                if (res.success) {
                    swal("User Activated.");
                    searchParamChanged();
                    
                }
            });
        }

        //function checkInactive(active, checked) {
        //    if (checked) {
        //        getInactiveUser();
        //        vm.check = true;
        //    }
        //    else {
        //        vm.check = false;
        //        getAllActiveUser();
        //    }
        //}
       
        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    if (vm.check === true) {
        //        getInactiveUser();
        //    } else {
        //        getAllActiveUser();
        //    }
           
        //}

        function saveUser(user) {
            user.securityGroupIdList = vm.userRoles;
            if (user.id) {
                if (user.password === user.confirmPassword || (user.password === "" && user.confirmPassword === null)) {
                    if (user.password === "") {
                        user.password = null;
                    }
                    userService.Update(user).then(function (result) {
                        if (result.success) {
                            searchParamChanged();
                            vm.userRoles.pop();
                            hide();
                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            }
                            else {
                                swal(result.message);
                            }
                        }
                        });
                }
                else {
                    swal("Password doesn't match.");
                }
            } else {
                if (user.password === user.confirmPassword) {
                    userService.Create(user)
                        .then(function(result) {
                            if (result.success) {
                              
                                swal("user successfully created.");
                                searchParamChanged();
                                 //   vm.users.push(result.data);
                                    vm.showModal = false;
                                    
                             
                             
                              
                            } 
                            else {
                                if (result.message.errors) {
                                    swal(result.message.errors[0]);
                                }
                                else {
                                    swal(result.message);
                                }
                            }
                        });
                } else {
                    swal("Password doesn't matched.");
                }
            }
          
        }
     
        function groupUserName(group) {
            vm.hideRoles = false;
            searchParamChanged();
            if (vm.dirtyList) {
                if (confirm('Would you like leave changes? Please Save before change.')) {
                    vm.dirtyList = false;
                    getMemberById(group);

                } else {

                }
            } else {
                getMemberById(group);
            }
            vm.activeUserGroup = group.id;
            vm.assignRoleText = group.groupName;
        }

        function getMemberById(groupId) {
            userService.GetMembers(groupId.id)
                .then(function (response) {
                    if (response.success) {
                        vm.groupName = [];
                        for (var i = 0; i < response.data.length; i++) {
                            vm.groupName.push(response.data[i]);
                          
                        }
                        loadAllUserNotAssignedToGroup(vm.groupName);
                    }
                });
        }

        vm.assignRoleText = "Select user to assign roles";
        vm.notAssignedUsers = [];

        function loadAllUserNotAssignedToGroup(data) {
            searchParamChanged();
            vm.notAssignedUsers.length = 0;
            for (var l = 0; l < data.length; l++) {
                for (var k = 0; k < vm.users.length; k++) {
                    if (vm.users[k].id === data[l].id) {
                        vm.users.splice(vm.users.indexOf(vm.users[k]), 1);
                    }
                }
            }
            vm.notAssignedUsers = vm.users;
        }

        function saveRole(securityGroupId, listItem) {
            vm.groupItemList = [];
            for (var i = 0; i < listItem.length; i++) {
                //alert(listItem[i].id + " And " + securityGroupId);
                vm.groupItemList.push(listItem[i].id);
            }

            userService.updateMembers(securityGroupId, vm.groupItemList)
                .then(function (response) {
                    if (response.success) {
                        // alert("success");
                        vm.dirtyList = false;
                        hide();
                        searchParamChanged();
                        var message ="Role is  assigned succesfully.";
                        dialogMessageForSuccess(message);
                        setTimeout(function () {
                            $('#successMessage').html('');
                        }, 3000);

                    }
                    else {
                        alert("sorry");
                    }
                });
        }

        //function searchText(text,status) {
        //    if (status===true) {
        //        if (text === "") {
        //            getInactiveUser();

        //        } else {
        //            searchTextChanged(text, status);
        //        }
        //    } else {
        //        if (text === "") {
        //            getAllActiveUser();
        //        } else {
        //            searchTextChanged(text, status);
        //        }
        //    }
           
        //}
       
        //function searchTextChanged(text, checked) {
        //    userService.searchText(vm.currentPage, vm.pageSize, text, checked).then(function(result) {
        //    if (result.success) {
        //        vm.users = result.data.items;
        //        vm.totalRecords = result.data.totalRecords;
        //        vm.filteredCount = result.data.items.length;
        //    }
        //});
        //}  

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
                        deleteUser(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeUsers(value);
                        setTimeout(function() {
                            editUser(value, true);
                        }, 3000);
                      
                        //swal(successMessage, "success");
                    }

                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined || vm.searchText == "")
                vm.searchText = null;
                userService.searchTextForUser(vm.searchText, !vm.check)
                    .then(function (result) {
                        if (result.success) {
                            vm.users = result.data;
                        }
                    });
        }
    }

})();
