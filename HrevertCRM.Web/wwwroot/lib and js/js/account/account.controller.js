(function () {
    "use strict";
    angular.module("app-account").controller("accountController", accountController);
    accountController.$inject = ['$location', '$http', '$filter', '$scope', '$window', '$timeout', 'accountService'];
    function accountController($location, $http, $filter, $scope, $window, $timeout, accountService) {

        var vm = this;
        vm.accounts = [];
        vm.nodeClicked = nodeClicked;
        vm.accountTypeChange = accountTypeChange;
        vm.levelChange = levelChange;
        vm.editAccount = editAccount;
        vm.updateAccount = updateAccount;
        vm.deleteAccount = deleteAccount;
        vm.nodeClickedForEdit = nodeClickedForEdit;
        vm.nodeClickedForDelete = nodeClickedForDelete;
        vm.checkInactive = checkInactive;
        vm.editingItem = {};
        vm.check = false;
        vm.includeInactive = false;
        vm.searchAccount = searchAccount;
        vm.deleteDialog = deleteDialog;
        vm.accountCode = false;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.acitvateAccount = acitvateAccount;
        vm.hideGrid = "disabled";
        vm.deleteData = null;
        vm.buttonText = "";
        vm.accountCashFlowType = false;
        vm.accountDetailTypeLabel = false;
        vm.parentAccountId = true;
        vm.showAccountTable = false;
        vm.checkIfExistCode = checkIfExistCode;
        vm.checkIfExistDescription = checkIfExistDescription;
        vm.operation = "Save";
        $scope.accountModal = false;

        function checkIfExistCode(code) {
            accountService.checkIfAccountCodeExists(code).then(function(result) {
                if (result.success) {
                    if (result.data) {
                        if (result.data.active) {
                            swal("Account already exists");
                            nodeClickedForEdit(result.data);
                        } else {
                            yesNoDialog("Do you want to active your account?", "info", "This account is already exists but has been disabled. Do you want to activate this account?", "Active", "Your account has been activated.", "active", result.data);
                        }
                    }
                }
            });
        }

        function checkIfExistDescription(desc) {
            if (desc !== undefined) {
                accountService.checkIfAccountDescriptionExists(desc).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Account already exists");
                                nodeClickedForEdit(result.data);
                            } else {
                                yesNoDialog("Do you want to active your account?",
                                    "info",
                                    "This account is already exists but has been disabled. Do you want to activate this account?",
                                    "Active",
                                    "Your account has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }

        loadAllActiveAccounts();

        loadAllAccountLevelsDetailsTypeAndCashFlowTypes();

        function accountTypeChange(type) {
           // console.log(type);
        }

        getAllActiveAccounts();

        function getAllActiveAccounts() {
            accountService.getAccountTree()
                .then(function(res) {
                    if (res.success) {
                        vm.accounts = res.data;
                    }
                });
        };
       
        function getAllAccounts() {
            accountService.getAllAccountTree()
                .then(function (res) {
                    if (res.success) {
                        vm.accounts = res.data;
                    }
                });
        };

        function checkInactive(active, checked) {
            if (checked) {
                getAllAccounts();
                vm.check = true;
            }
            else {
                vm.check = false;
                getAllActiveAccounts();
            }
        }

        function nodeClicked(item) {
            vm.hideGrid = "";
            vm.parentAccountId = item.id;
            vm.dataForGrid = item.children;
            vm.editingItem.parentAccountId = item.parentAccountId;
            vm.showAccountTable = true;
        }

        $scope.open = function () {
            vm.editingItem = {};
            vm.parentAccountId = false;
            vm.operation = "Save";
            $scope.showModal = true;
            vm.accountForm.$setUntouched();
        }

        $scope.hide = function () {
            vm.deleteData = null;
            getAllActiveAccounts();
            vm.accountCode = false;
            $scope.showModal = false;
            $scope.showDeleteModal = false;
        }
       
        function editAccount(data) {
            vm.accountCode = true;
            if (data.active) {
                vm.operation = "Update";
                vm.editingItem = data;
                $scope.showModal = true;
            }
            else {
                swal("You cannot edit this item.please activate it first.");
            }
        }

        function levelChange(level) {
            if (Number(level) === 1) {
                vm.accountCashFlowType = false;
                vm.accountDetailTypeLabel = false;
            }
            else {
                vm.accountCashFlowType = false;  
                vm.accountDetailTypeLabel = false;
            }

        }

        function updateAccount(account, text) {
            if (account.level === 1) {
                account.accountDetailType = 12;
                account.accountCashFlowType = 4;
            }
            
            if (text === "Update") {
                accountService.updateAccount(account)
                .then(function (res) {
                    if (res.success) {
                        $scope.showModal = false;
                        getAllActiveAccounts();
                        
                    }
                    else {
                        if (res.message.errors) {
                            swal(res.errors.message);
                        } else {
                            swal(res.message);
                        }
                      
                    }
                });

            }
            else if (text === "Save") {
                account.id = null;
                accountService.saveAccount(account)
                            .then(function (res) {
                                if (res.success) {
                                    $scope.showModal = false;
                                    getAllActiveAccounts();
                                }
                                else {
                                    if (res.message.errors) {
                                        swal(res.errors.message);
                                    } else {
                                        swal(res.message);
                                    }
                                }
                            });
            } 
        }

       
        vm.ok = ok;
        vm.cancel = cancel;
        vm.accountId = null;
        vm.deleteOnlyAccount = deleteOnlyAccount;
        
        function deleteDialog(data) {
            //if (data.children.length > 0) {

            //    vm.message = "Do you want to deactivate this account? By deactivating this account, every children account of this account will be deactivated.";
            //}
            //else {
            //    vm.message = "Do you want to deactivate this account?";
            //}
            //vm.showDeleteModal = true;
            //nodeClickedForDelete(data);
            //  $scope.accountModal = true;
            $scope.showAccountModal = true;
            vm.accountId = data.id;

            //yesNoDialog("Are you sure?", "warning", "Account will be deactivated but still you can activate your account in future.", "Yes, delete it!", "Your account has been deactivated.", "delete", data);
        }

        function ok(deleteCheck) {
            if (deleteCheck === true) {
                deleteAccount(vm.accountId);
              //  $scope.accountModal = false;
            } else {
                deleteOnlyAccount(vm.accountId);
               // $scope.accountModal = false;
            }
        };

        function cancel() {
            $scope.showAccountModal = false;
        };

        function deleteAccount(id) {
            accountService.deleteAccount(id)
                 .then(function (res) {
                     if (res.success) {
                         swal("success");
                         if (vm.check !== true) {
                             getAllActiveAccounts();
                         }
                         else {
                             getAllAccounts();
                         }
                     }
                     else {
                         alert("sorry");
                     }
                     $scope.showAccountModal = false;
                 });
        }

        function deleteOnlyAccount(id) {
            accountService.deleteOnlyAccount(id)
                 .then(function (res) {
                     if (res.success) {
                         swal("success");
                         if (vm.check !== true) {
                             getAllActiveAccounts();
                         }
                         else {
                             getAllAccounts();
                         }
                     }
                     else {
                         alert("error");
                     }
                     $scope.showAccountModal = false;
                 });
        }
                
        function nodeClickedForEdit(account) {
            vm.accountCode = true;
            accountService.getAccountById(account.id).then(function (result) {
                vm.editingItem = result.data;
                $scope.showModal = true;
                vm.operation = "Update";
            });
           
        }
    
        function nodeClickedForDelete(account) {
            $scope.showDeleteModal = true;
            vm.deleteData = account;
       }

        function searchAccount(text, status) {
           if (status === true) {
               if (text === "") {
                   getAllAccounts();
               } else {
                   searchAccountByText(text, status);
               }
           } else {
               if (text === "") {
                   getAllActiveAccounts();
               } else {
                   searchAccountByText(text, status);
               }
           }
       }

        function searchAccountByText(text, status) {
           accountService.searchAction(text, status).then(function(result) {
               if (result.success) {
                   vm.accounts = result.data;
               }
           });
       }

        function acitvateAccount(account) {
            accountService.getAccountActivated(account.id).then(function(result) {
                if (result.success) {
                    swal("Account Activated.");
                    getAllAccounts();
                }
              
           }, function (error) {
               console.log("something is wrong." + error);
           });
        
       }

        function loadAllAccountLevelsDetailsTypeAndCashFlowTypes(){

           accountService.getAllAccountCashFlowTypes().then(function(result){
               vm.cashFlow = result.data;
              // console.log("Cash Flow Type:"+JSON.stringify(result.data));
           });

           accountService.getAllAccountDetailsType().then(function (result) {
               vm.accountDetailTypes = result.data;
               //console.log("Account Details Type:" + JSON.stringify(result.data));
           });

           accountService.getAllAccountLevels().then(function (result) {
               vm.levels = result.data;
               //console.log("Account  Level:" + JSON.stringify(result.data));
           });

           accountService.getAllAccountTypes().then(function (result) {
               vm.accountTypes = result.data;
             //.log("Accounts Types:" + JSON.stringify(result.data));
           });
       }

        function loadAllActiveAccounts() {
           accountService.getAllActiveAccount().then(function (result) {
               vm.parentAccounts = result.data;
             //console.log("accounts:"+JSON.stringify(result.data));

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
                        deleteAccount(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        acitvateAccount(value);
                        editAccount(value);
                        //swal(successMessage, "success");
                    }

                });
        }
    }
  
})();
