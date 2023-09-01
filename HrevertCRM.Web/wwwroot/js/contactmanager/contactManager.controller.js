(function () {
    angular.module("app-contactmanager")
        .controller("contactManagerController", contactManagerController);
    contactManagerController.$inject = ['$http', '$filter', '$scope', '$timeout', 'contactManagerService'];
    function contactManagerController($http, $filter, $scope, $timeout, contactManagerService) {
        var vm = this;
        vm.activeContactManager = [];
        vm.saveCustomerContactGroup = saveCustomerContactGroup;
        vm.editCustomerContactGroup = editCustomerContactGroup;
        vm.sendEmailTo = sendEmailTo;
        vm.deleteContactGroup = deleteContactGroup;
        vm.actionChanged = actionChanged;
        vm.sendMail = sendMail;
        vm.remove = remove;
        vm.customerContactGroup = [];
        vm.pageSizes = [ "5", "10", "15", "20"];
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }, { id: 3, name: 'Email' }];
        vm.pageSize = "5";
        vm.currentPage = 1;
       // vm.searchTextChanged = searchTextChanged;
      //  vm.checkInactive = checkInactive;
      //  vm.getPageSize = getPageSize;
        vm.activeCustmerContactGroupById = activeCustmerContactGroupById;
        vm.pageChanged = pageChanged;
        vm.checkIfExistsGroupName = checkIfExistsGroupName;
        vm.customerContactGroup = [];
       // getAllActiveCostumerGroup();
        vm.upload = [];
        vm.fileSelect = fileSelect;
        vm.removeFiles = removeFiles;
        vm.customerListItems = [];
        vm.notAssignedCustomers = [];
        vm.mailItems = {};
        vm.emailList = [];
        vm.mailItems.files = [];
        vm.allActiveCustomers = [];
        vm.allMailAddress = [];
        vm.allCcEmailAddresses = [];
        vm.check = false;
        vm.searchParamChanged = searchParamChanged;
        vm.totalRecords = 0;
        function checkIfExistsGroupName(groupName) {
            if (groupName != undefined) {
                contactManagerService.checkIfExistsGroupName(groupName).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Customer ContactGroup already exists");
                                editCustomerContactGroup(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your Customer ContactGroup?",
                                    "info",
                                    "This customer contact groups is already exists but has been disabled. Do you want to active this group?",
                                    "Active",
                                    "Your product has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });

            }
        }

       // getAllActiveCustomers();
        getContactGroupPageSize();
        $scope.open = function () {
            getAllActiveCustomerAsNotAssignedCustomerForNewCustomerContactGroup();
            vm.activeCustomerContactGroup = null;
            vm.customerListItems = [];
            $scope.showModal = true;
            vm.contactGroupForm.$setUntouched();
            
        }

        $scope.hide = function () {
            vm.activeCustomerContactGroup = null;
            vm.mailItems = null;
            $scope.showModal = false;
        }

        $scope.hideEmailSend = function () {
            vm.mailItems = null;
            $scope.showEmailSendModal = false;
        }

        function pageChanged(currentPage) {
            vm.currentPage = currentPage;
            searchParamChanged();
        }

        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    if (vm.check === true) {
        //        getAllCostumerGroup();
        //    } else {
        //        getAllActiveCostumerGroup();
        //    }
        //}

        function remove(item) {
            contactManagerService.removeFiles(item.id)
                    .then(function (result) {
                        if (result.success) {
                            searchParamChanged();
                        } 
                    });
        }

        function actionChanged(contact, id) {
            if (Number(id) === 1) {
                editCustomerContactGroup(contact.id,contact.active);

            }
            else if (Number(id) === 2) {
                // deleteContactGroup(contact.id);
                yesNoDialog("Are you sure?", "warning", "Customer contact group will be deactivated but still you can activate your group in future.", "Yes, delete it!", "Your group has been deactivated.", "delete", contact);
            }
            else if (Number(id) === 3) {
                sendEmailTo(contact);
            }
            vm.action = null;
        }
     
        function deleteContactGroup(successMessage, id) {
            contactManagerService.deleteCustomerGroup(id)
                    .then(function (result) {
                        if (result.success) {
                            swal(successMessage, "success");
                            searchParamChanged();
                        } 
                        });
        }

        function saveCustomerContactGroup(customerContactGroup) {
            customerContactGroup.customerIdsInContactGroup = [];
            if (vm.customerListItems) {
                for (var i = 0; i < vm.customerListItems.length; i++) {
                    customerContactGroup.customerIdsInContactGroup[i] = vm.customerListItems[i].id;
                }
            }
           
            if (customerContactGroup.id) {
                contactManagerService.updateCustomerGroup(customerContactGroup)
                    .then(function (result) {
                        if (result.success) {
                            searchParamChanged();
                            //getAllActiveCostumerGroup();
                            getAllActiveCustomers();
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
                
            
            else {
                contactManagerService.createCustomerGroup(customerContactGroup)
                    .then(function (result) {
                        if (result.success) {
                            getAllActiveCustomers();
                            searchParamChanged();
                          //  getAllActiveCostumerGroup();
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

        function editCustomerContactGroup(contactGroupId, checked) {
            if (checked) {
                vm.customerListItems = [];
                contactManagerService.getCustomerGroupById(contactGroupId).then(function (result) {
                        if (result.success) {
                            vm.activeCustomerContactGroup = result.data;
                            for (var i = 0; i < vm.activeCustomerContactGroup.customerAndContactGroupList.length; i++) {
                                for (var j = 0; j < vm.allActiveCustomers.length; j++) {
                                    if (vm.allActiveCustomers[j].id ===
                                        vm.activeCustomerContactGroup.customerAndContactGroupList[i].customerId) {
                                        vm.customerListItems.push(vm.allActiveCustomers[j]);
                                    }
                                }
                            }

                            loadAllCustomerNotAssignedToGroup(vm.activeCustomerContactGroup.customerIdsInContactGroup);

                            $scope.showModal = true;
                        }
                    },
                function (error) {
                    console.log('error is:'+error);
                });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
              
            }
        }

        function loadAllCustomerNotAssignedToGroup(data) {
            getAllActiveCustomers();
            for (var l = 0; l < data.length; l++) {
            for (var k = 0; k < vm.allActiveCustomers.length; k++) {
                    if (vm.allActiveCustomers[k].id === data[l]) {
                        vm.allActiveCustomers.splice(vm.allActiveCustomers.indexOf(vm.allActiveCustomers[k]), 1);
                    }
                }
                
            }
            vm.notAssignedCustomers = vm.allActiveCustomers;
        }
      
        function sendEmailTo(data) {
            vm.mailItems = {};
            vm.mailItems.emailListTo = null;
            vm.mailItems.cc = null;
            $scope.showEmailSendModal = true;
            for (var i = 0; i < data.customerIdsInContactGroup.length; i++) {
                for (var j = 0; j < vm.allActiveCustomers.length; j++) {
                    if (data.customerIdsInContactGroup[i] === vm.allActiveCustomers[j].id) {
                        vm.emailList.push({
                            email: vm.allActiveCustomers[j].billingAddress.email,
                            id: vm.allActiveCustomers[j].id
                        });
                    }
                    
                }
            }

        }
             
        //function getAllActiveCostumerGroup() {
        //    contactManagerService.getAllActiveCustomerGroup(vm.currentPage, vm.pageSize).then(function (result) {
        //            if (result.success) {
        //                vm.customerContactGroup = result.data.items;
        //                vm.pageNumber = result.data.pageNumber;
        //                vm.totalPage = result.data.pageCount;

        //            }
        //        },
        //    function (error) {
        //        console.log("error"+error);
        //    });

        //}

        function getAllActiveCustomers() {
            contactManagerService.getAllActiveCustomersWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allActiveCustomers = result.data;
                    for (var i = 0; i < vm.allActiveCustomers.length; i++) {
                        var obj = {
                            "id": vm.allActiveCustomers[i].id,
                            "email": vm.allActiveCustomers[i].billingAddress.email
                        }
                        vm.allCcEmailAddresses.push(obj);
                    }
                }
            });
        }

        function getAllActiveCustomerAsNotAssignedCustomerForNewCustomerContactGroup() {
            contactManagerService.getAllActiveCustomersWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.notAssignedCustomers = result.data;
                }
               
            });
        }

        function fileSelect($files) {
            for (var i = 0; i < $files.length; i++) {
                vm.upload.push($files[i]);
            }
            
        }

        function removeFiles(file) {
            var index = vm.upload.indexOf(file);
            vm.upload.splice(index, 1);
        }

        function sendMail(mailItems) {
            vm.emailId = mailItems.emailSetting;
            var data = new FormData();
            for (var i = 0; i < vm.upload.length; i++) {
                data.append(vm.upload[i].name, vm.upload[i]);
            }
            
            vm.ccList = "";
            
            for (var j = 0; j < mailItems.cc.length; j++) {
                vm.ccList += mailItems.cc[j].email + ";";
            }

            vm.toList = "";

            for (var k = 0; k < mailItems.emailListTo.length;k++) {
                vm.toList += mailItems.emailListTo[k].email + ";";
            }
            
            var params = {
                emailSettingId : vm.emailId,
                cc: vm.ccList,
                mailTo: vm.toList,
                subject: mailItems.subject,
                message: mailItems.message
            };
  
            contactManagerService.sendEmail(data, params)
                    .then(function (result) {
                        if (result.statusText === "OK") {
                            vm.mailItems = null;
                            dialogSuccessMessage("Mail has been sent successfully.");
                            $timeout(function() {
                                $('#successMessage').html('');
                                $scope.showEmailSendModal = false;
                            }, 1000);

                          
                        } 
                    });


          
        }

        //function searchCustomer(text, checked) {
        //    contactManagerService.searchTextForCustomerContactGroup(text, checked)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.customerContactGroup = result.data;
        //                vm.pageNumber = result.data.pageNumber;
        //                vm.totalPage = result.data.pageCount;
        //            }
        //        });
        //}

        //function searchTextChanged(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getAllCostumerGroup();
        //        } else {
        //            searchCustomer(text, checked);
        //        }
        //    } else {
        //        if (text === "") {
        //            getAllActiveCostumerGroup();
        //        } else {
        //            searchCustomer(text, checked);
        //        }
        //    }
        //}

        //function checkInactive(active, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        getAllCostumerGroup();
        //    }
            
        //    else {
        //        vm.check = false;
        //        getAllActiveCostumerGroup();
        //    }
        //}

        //function getAllCostumerGroup() {
        //    contactManagerService.getAllCustomerGroup(vm.currentPage, vm.pageSize).then(function (result) {
        //        if(result.success){
        //        vm.customerContactGroup = result.data.items;
        //        vm.pageNumber = result.data.pageNumber;
        //        vm.totalPage = result.data.pageCount;
        //       }
        //    });
            
        //}

        function activeCustmerContactGroupById(contactGroup) {
            contactManagerService.activateCustomerContactGroupById(contactGroup.id).then(function (result) {
                if (result.success) {
                    swal("Customer contact group activated.");
                    searchParamChanged();
                }
           });
        }

        function dialogSuccessMessage(result) {
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
                        deleteContactGroup(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activeCustmerContactGroupById(value, false);
                        editCustomerContactGroup(value.id, true);
                        //swal(successMessage, "success");
                    }

                });
        }
        function getContactGroupPageSize() {
            contactManagerService.getPageSize().then(function (res) {
                if (res.success) {
                    if (res.data != 0) {
                        vm.pageSize = res.data;
                        getAllActiveCustomers();
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
            if (vm.totalRecords < vm.pageSize)
                vm.currentPage = 1;
            contactManagerService.searchTextForContactGroup(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.customerContactGroup = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.totalRecords = result.data.totalRecords;
                    }
                });
        }


    }
})();