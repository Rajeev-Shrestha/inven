(function ()
{
    angular.module("app-customer")
    .controller("customersController", customersController);
    customersController.$inject = ['$location', '$http', '$scope', '$filter', '$window', '$timeout', 'commonService', 'customerService'];
    function customersController($location, $http, $scope, $filter, $window, $timeout, commonService, customerService) {
        var vm = this;
        vm.customerLevel = [];
        vm.customers = [];
        vm.filteredCustomers = [];
        vm.filteredCount = 0;
        vm.reverse = false;
        vm.searchText = null;
        vm.editCustomer = editCustomer;
        vm.updateCustomer = updateCustomer;
        vm.activateCustomer = activateCustomer;
        vm.nameChange = nameChange;
        vm.check = false;
       // vm.getPageSize = getPageSize;
        vm.addCardUi = addCardUi;
        //vm.searchcustomers = searchcustomers;
        vm.addNewAddress = addNewAddress;
        vm.selectedPaymentMethod = selectedPaymentMethod;
        vm.removeRow = removeRow;
        vm.actionChanged = actionChanged;
        vm.totalRecords = 0;
        vm.pageSizes = [ "10", "20","30","50"];
        vm.pageSize = "10";
        vm.currentPage = 1;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.activeItem = null;
        vm.newAddressDialog = newAddressDialog;
        vm.newAddressShow = "false";
        vm.customerCode = "false";
        vm.customerEmail = "false";
        vm.customerDisplayName = "false";
        vm.addressEdit = addressEdit;
        vm.showHideButtonText = "Add New Address";
        vm.validateCustomerCode = validateCustomerCode;
        vm.passwordValidator = passwordValidator;
      //  vm.checkInactive = checkInactive;
        vm.isDefaults = [{ id: 1, name: 'true' }, { id: 2, name: 'false' }];
        vm.showItem = false;
        vm.btnCustomerSaveText = "Save";
        vm.newAddress = {};
        vm.searchParamChanged = searchParamChanged;
        //vm.cancelImport = cancelImport;
        vm.newAddressEmail = false;
        vm.addressCheck = false;
        vm.changeAddressCheck = changeAddressCheck;
        vm.activateAddress = activateAddress;
        vm.buttonBar = true;
        vm.customerInformation = customerInformation;
        vm.addressInformation = addressInformation;
        vm.countries = [{ id: 1, name: 'Nepal', code: '+977' }];
        vm.updateAddressButton = false;
        vm.updateAddress = updateAddress;
        vm.showAddNew = false;
        vm.totalCustomer = 0;

        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;
        vm.customerTitles =
            [
                { id: 1, name: 'None' },
                { id: 2, name: 'Mr' },
                { id: 3, name: 'Mrs' },
                { id: 4, name: 'Miss' },
                { id: 5, name: 'Shri' },
                { id: 6, name: 'Smt' },
                { id: 7, name: 'Shushri' },
                { id: 8, name: 'Other' },

            ];
        vm.customerSuffixes =
           [
               { id: 1, name: 'None' },
               { id: 2, name: 'Doctor' },
               { id: 3, name: 'Engineer' },
               { id: 4, name: 'Enterprenuer' },
               { id: 5, name: 'Lawyer' },
               { id: 6, name: 'Businessman' },
               { id: 7, name: 'Businesswoman' },
               { id: 8, name: 'Politician' },
               { id: 9, name: 'Freelancer' },
               { id: 10, name: 'MR' },
               { id: 11, name: 'Other' }

           ];
        vm.customerAddressType =
       [
           { id: 2, name: 'Shipping' },
           { id: 3, name: 'Contact' }       

       ];
        $scope.selected = [];

        vm.exist = function (item) {
            return $scope.selected.indexOf(item) > -1;
        }

        function toggleSelection(item, event) {
            if (event.currentTarget.checked) {
                $scope.selected.push(item);
            } else {
                for (var i = 0; i < $scope.selected.length; i++) {
                    if ($scope.selected[i].id == item.id) {
                        $scope.selected.splice($scope.selected.indexOf($scope.selected[i]), 1);
                        $scope.selectAll = [];

                        return;
                    }
                }
            }
        }
       

        function checkAll() {
            if ($scope.selectAll) {
                angular.forEach(vm.customers, function (check) {
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
            customerService.deletedSelected(selectedItemid).then(function (result) {
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

        function customerInformation() {
            vm.buttonBar = true;
        }

        function addressInformation() {
            if(vm.newAddressShow === "true")
            {
                vm.buttonBar = false;
            } else {
                vm.buttonBar = true;
            }

        }
        function activateAddress(addressId) {
            customerService.activateAddressById(addressId).then(function (result) {
                if (result.success) {
                    customerService.GetById(result.data.customerId).then(function (result) {
                        if (result.success) {
                            vm.editingItem.addresses = result.data.addresses;
                        }
                    });
                   
                }
            });
        }

        function changeAddressCheck(check, customerId) {
            if (check === true) {
                customerService.getAllAddresses(customerId).then(function(result) {
                    if (result.success) {
                        vm.editingItem.addresses = result.data;
                    }
                });
            } else {
                customerService.GetById(customerId).then(function(result) {
                    if (result.success) {
                        vm.editingItem.addresses = result.data.addresses;
                    }
                });
              
            }
        }

        function cancelImport() {
            vm.csv = null;
            //vm.csv.result = false;
        }
        vm.csv = {
            content: null,
            header: true,
            headerVisible: false,
            separator: ',',
            separatorVisible: false,
            result: null,
            encoding: 'ISO-8859-1',
            encodingVisible: false
        };

        //vm.importUsers = function importUsers(result, updateExisting) {
        //    customerService.importUsers(vm.csv.result, updateExisting)
        //       .then(function (res) {
        //           if (res.success) {

        //           } 
        //       });
        //}

        loadAllTitles();
        loadAllSuffixes();
        loadAllAddressTypes();
        getZones();
        getAccountTerm();
        //loadAllCountries();
        init();

        function init() {
            getCustomersPageSize();
          //  getAllActiveCustomers();
        }

        //function loadAllCountries() {
        //    customerService.getAllCountries()
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.countries = result.data;
        //            }
        //        });
        //}

        function loadAllSuffixes() {
            customerService.getAllSuffixes()
                .then(function(result) {
                    if (result.success) {
                        vm.suffixes = result.data;
                    } 
                });
        }

        function getZones() {
            customerService.getActiveZones()
                .then(function(result) {
                    if (result.success) {
                        vm.zone = result.data;
                    }
                });
        }

        function loadAllTitles() {
            customerService.getAllTitles()
                .then(function(result) {
                    if (result.success) {
                        vm.titles = result.data;
                    } 
                });
        }

        function loadAllAddressTypes() {
            customerService.getAllAddressTypes()
                .then(function(result) {
                    if (result.success) {
                        vm.allAddressType = [];
                        for (var i = 0; i < result.data.length; i++) {
                            if (result.data[i].value === "Billing") {
                            } else {
                                vm.allAddressType.push(result.data[i]);
                            }
                        }
                    }
                    });
        }
    
        function nameChange(firstName, lastName) {
            if (firstName && lastName) {
                vm.editingItem.displayName = firstName + ' ' + lastName;
            } else if (!firstName) {
                var emptyFirstName = "";
                vm.editingItem.displayName = emptyFirstName + ' ' + lastName;
            } else if (!lastName) {
                var emptyLastName = "";
                vm.editingItem.displayName = firstName + ' ' + emptyLastName;
            }

        }

        function getAccountTerm() {
            customerService.getAccountTerm()
                .then(function(result) {
                    if (result.success) {
                        vm.onAccountOptions = result.data;
                    }
                });
        }

        function addNewAddress(newAddress) {
            if (newAddress === null) {
                swal("please fillup all the required fields.");
                return;
            }
            else if (newAddress.addressType === undefined || newAddress.firstName === undefined || newAddress.lastName === undefined || newAddress.addressLine1 === undefined || newAddress.city === undefined || newAddress.state === undefined ||
                newAddress.zipCode === undefined || newAddress.countryId === undefined || newAddress.email === undefined || newAddress.mobilePhone === undefined) {
                swal("please fillup all the required fields.");
                return;
            }
            else {
                vm.editingItem.addresses.push({
                    addressType: newAddress.addressType,
                    firstName: newAddress.firstName,
                    MiddleName: newAddress.middleName,
                    lastName: newAddress.lastName,
                    addressLine1: newAddress.addressLine1,
                    addressLine2: newAddress.addressLine2,
                    city: newAddress.city,
                    state: newAddress.state,
                    fax: newAddress.fax,
                    zipCode: newAddress.zipCode,
                    countryId: newAddress.countryId,
                    telephone: newAddress.telephone,
                    mobilePhone: newAddress.mobilePhone,
                    email: newAddress.email,
                    isDefault: newAddress.isDefault
                });
                vm.newAddress = {};
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.buttonBar = true;
            }
            
        }

        $scope.open = function () {
            vm.btnCustomerSaveText = "Save";
            vm.newAddressShow = "false";
            vm.showHideButtonText = "Add New Address";
            vm.newAddress = {};
            vm.editingItem = {};
            vm.customerCode = false;
            vm.customerEmail = false;
            vm.customerDisplayName = false;
            vm.customerConfirmPassword = true;
            vm.customerPassword = true;
            vm.customerCodeLength = 8;
            vm.editingItem.addresses = [];
            vm.showModal = true;
            vm.customerForm.$setUntouched();
        }

        vm.showImportModal = false;

        //vm.openImport = function () {
        //    vm.showImportModal = true;
        //}

        $scope.hide = function () {
            vm.customerCode = false;
            vm.customerEmail = false;
            vm.customerDisplayName = false;
            vm.editingItem = null;
            vm.newAddressShow = false;
            vm.newAddress = null;
            vm.customerConfirmPassword = true;
            vm.customerPassword = true;
            vm.newAddressShow = "false";
            vm.showHideButtonText = "Add New Address";
            vm.showModal = false;
        }

        //vm.hideImport = function () {
        //    cancelImport();
        //    vm.showImportModal = false;
            
        //}
        
        //function checkInactive(active, checked) {
        //    if (checked) {
        //        vm.check = true;
        //        inactiveCustomer();
           
        //    }
        //    else {
        //        vm.check = false;
        //        init();
        //    }
        //}

        //function inactiveCustomer() {
        //    customerService.getInactiveCustomers(vm.currentPage, vm.pageSize)
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.pageNumber = result.data.pageNumber;
        //                    vm.totalPage = result.data.pageCount;
        //                    vm.customers = result.data.items;
        //                } 
        //            });
        //}

        function actionChanged(customer, action) {
            if (Number(action) === 1) {
                editCustomer(customer.id, customer.active);
            }
            else if (Number(action) === 2) {
                yesNoDialog("Are you sure?", "warning", "Customer will be deactivated but still you can activate your customer in future.", "Yes, delete it!", "Your customer has been deactivated.", "delete", customer);
               
            }
            else if (Number(action) === 3) {
                alert("you cannot use this item. We are working on it");
            }
            vm.action = null;
            
        }
       
        function newAddressDialog(text) {
            if (text === "Cancel") {
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.newAddress = null;
                vm.newAddressEmail = false;
                vm.buttonBar = true;
            }
            else {
                vm.newAddress = null;
                vm.newAddressShow = "true";
                vm.showHideButtonText = "Cancel";
                vm.buttonBar = false;
                vm.showAddNew = true;
                vm.updateAddressButton = false;
                vm.address.$setUntouched();
            }
            
        }       

        function removeRow(address, index) {
            if (address.id) {
                address.active = false;
                vm.editingItem.confirmPassword = vm.editingItem.password;
                customerService.Update(vm.editingItem).then(function(result) {
                    if (result.success) {
                        vm.editingItem.addresses.splice(index, 1);
                        vm.editingItem.confirmPassword = "";
                    }
                });
            }

            else {
                vm.editingItem.addresses.splice(index, 1);
            }
        }
        
        //function searchcustomers(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            inactiveCustomer();
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
        //    }

        //function searchTextChanged(text, checked) {
        //    customerService.searchText(vm.currentPage, vm.pageSize, text, checked).then(function (result) {
        //        if (result.success) {
        //            vm.customers = result.data.items;
        //            vm.pageNumber = result.data.pageNumber;
        //            vm.totalPage = result.data.pageCount;
        //        }
        //    });
        //}

        vm.pageChanged = function(page) {
            $scope.selectAll = [];
            vm.currentPage = page;
            searchParamChanged();
          
        };

        function deleteCustomer(successMessage, customer) {
            customerService.deleteCustomer(customer.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage, "success");
                        searchParamChanged();
                    }
                });
        }      

        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    if (vm.check === true) {
        //        inactiveCustomer();
        //    } else {
        //        getAllActiveCustomers();
        //    }
            
        //}

        //function getAllActiveCustomers() {
        //    customerService.GetCustomers(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.customers = result.data.items;
        //                vm.pageNumber = result.data.pageNumber;
        //                vm.totalPage = result.data.pageCount;
        //            }
        //        });
        //    customerService.GetAllLevels()
        //        .then(function(result) {
        //            if (result.success) {
        //                vm.customerLevel = result.data;
        //            } 
        //        });

        //    customerService.GetPaymentMethod()
        //        .then(function(result) {
        //            if (result.success) {
        //                vm.paymentMethod = result.data;
        //            } 
        //        });

        //}
        
        function editCustomer(customerId, checked) {
            vm.btnCustomerSaveText = "Update";
            if (checked) {
                customerService.GetById(customerId)
                    .then(function (result) {
                        if (result.success) {
                            vm.customerCode = true;
                            vm.customerEmail = true;
                            vm.customerDisplayName = true;
                            vm.customerCodeLength = 10;
                            vm.customerConfirmPassword = false;
                            vm.customerPassword = false;
                            vm.editingItem = result.data;
                            vm.editingItem.confirmPassword = result.data.password;
                            vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(vm.editingItem.billingAddress.mobilePhone);
                            vm.editingItem.billingAddress.telephone = commonService.removeCode(vm.editingItem.billingAddress.telephone);
                       
                            if (vm.editingItem.addresses !== []) {
                                for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                                    vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                                    vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                                }
                            }
                           
                          
                          vm.showModal = true;
                          vm.newAddressShow = "false";
                          vm.customerForm.$invalid = true;
                        } else {
                            //show error
                            alert("sorry");
                        }
                    });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
        }

        function selectedPaymentMethod(paymentMethod) {
            vm.currentSelectedPaymentMethod = paymentMethod.methodCode;
        }

        function validateCustomerCode(code) {
            var data = 0;
            var customerCode = "C-" + code;
          
            if (code === undefined || null) {
                data = 3;
            }
            if (code !== undefined) {
                if (code.length < 8 || code.length > 8) {
                    data = 2;
                } else {
                    customerService.checkCustomerCode(customerCode).then(function (result) {
                        if (result.success) {
                            customerService.checkIfCustomerCodeExists(customerCode).then(function (result) {
                                if (result.success) {
                                    if (result.data) {
                                        if (result.data.active) {
                                            swal("Customer already exists");
                                            editCustomer(result.data.id, true);
                                        } else {
                                            yesNoDialog("Do you want to active your customer?", "info", "This customer is already exists but has been disabled. Do you want to activate this customer?", "Active", "Your customer has been activated.", "active", result.data);
                                        }
                                    }
                                }
                            });
                            data = 1;
                        }
                    });
                }
            }
                return data;

        }
   
        function updateCustomer(editingItem) {
                if (editingItem.addresses.length > 0) {
                for (var i = 0; i < editingItem.addresses.length; i++) {
                    editingItem.addresses[i].suffix = editingItem.billingAddress.suffix;
                    editingItem.addresses[i].title = editingItem.billingAddress.title;
                    for (var j = 0; j < vm.countries.length; j++) {
                        if (editingItem.addresses[i].countryId === vm.countries[j].id) {
                            editingItem.addresses[i].mobilePhone = commonService.addCode(vm.countries[j].code, editingItem.addresses[i].mobilePhone);
                            editingItem.addresses[i].telephone = commonService.addCode(vm.countries[j].code, editingItem.addresses[i].telephone);
                        }
                    }
                }
            }
            for (var j = 0; j < vm.countries.length; j++) {
                if (editingItem.billingAddress.countryId === vm.countries[j].id) {
                    editingItem.billingAddress.mobilePhone = commonService.addCode(vm.countries[j].code, editingItem.billingAddress.mobilePhone);
                    //editingItem.billingAddress.telephone = commonService.addCode(vm.countries[j].code, editingItem.billingAddress.telephone);
                    editingItem.addresses.telephone = commonService.addCode(vm.countries[j].code, editingItem.billingAddress.telephone);
                }
            }
            if (editingItem.billingAddress.id) {
               
                var data = passwordValidator(editingItem.password, editingItem.confirmPassword);
                if (data === true) {
                    swal("password don't match. try again!!");
                    vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                    vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                    if (vm.editingItem.addresses !== []) {
                        for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                            vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                            vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                        }
                    }

                    return;
                }
                else {
                    customerService.Update(editingItem).then(function (result) {
                        if (result.success) {
                            vm.editingItem = {};
                            vm.editingItem = null;
                            vm.showHideButtonText = "Add New Address";
                            vm.newAddressShow = "false";
                            searchParamChanged();
                            vm.showModal = false;

                        } else {
                            if (result.message.errors) {
                                swal(result.message.errors[0]);
                            } else {
                                swal(result.message);
                            }
                            vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                            vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                            if (vm.editingItem.addresses !== []) {
                                for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                                    vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                                    vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                                }
                            }
                            vm.showHideButtonText = "Add New Address";
                            vm.newAddressShow = "false";
                        }
                    });
                }
            }
            else {
                var result = passwordValidator(editingItem.password, editingItem.confirmPassword);
                var res = validateCustomerCode(editingItem.customerCode);

                if (editingItem.customerCode === null || "") {
                    editingItem.customerCode = null;
                } else {
                    if (Number(res) === 0) {
                        editingItem.customerCode ="C-"+editingItem.customerCode;
                    }
                    else if (Number(res) === 1) {
                        swal("This customer code already exists.");
                        vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                        vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                        if (vm.editingItem.addresses !== []) {
                            for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                                vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                                vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                            }
                        }
                        return;
                    }
                    else if (Number(res) === 2) {
                        swal("Customer code must be 8 digits");
                        vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                        vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                        if (vm.editingItem.addresses !== []) {
                            for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                                vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                                vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                            }
                        }
                        return;
                    }
                    else if (Number(res) === 3) {
                        editingItem.customerCode = null;
                    }
                }
                

                if (result === true) {
                    swal("password don't match. try again!!");
                    vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                    vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                    if (vm.editingItem.addresses !== []) {
                        for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                            vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                            vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                        }
                    }
                    return;
                }
                
                else {
                    editingItem.billingAddress.addressType = 1;
                    customerService.Create(editingItem).then(function (result) {
                                    if (result.success) {
                                        vm.editingItem = {};
                                        vm.editingItem = null;
                                        searchParamChanged();
                                        vm.showModal = false;
                                    } else {
                                        if (result.message.errors) {
                                            swal(result.message.errors[0]);
                                        }
                                        else {
                                            swal(result.message);
                                        }
                                        vm.editingItem.customerCode = undefined;
                                        vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                                        vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                                        if (vm.editingItem.addresses !== []) {
                                            for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                                                vm.editingItem.addresses[i].mobilePhone = commonService.removeCode(vm.editingItem.addresses[i].mobilePhone);
                                                vm.editingItem.addresses[i].telephone = commonService.removeCode(vm.editingItem.addresses[i].telephone);
                                            }
                                        }
                                    }
                                });

                }
            }
        }

        function activateCustomer(customer) {
            customerService.activatecustomer(customer.id)
                .then(function(result) {
                    if (result.success) {
                        swal("Customer Activated.");
                        searchParamChanged();                      
                    } 
                });
        };

        function addCardUi(addnewCardButton) {
            if (addnewCardButton === "Cancel") {
                vm.addnewCardButton = "Add Card";
                vm.showItem = false;
            } else {
                vm.addnewCardButton = "Cancel";
                vm.showItem = true;
            }
            
        };

        function updateAddress(newAddress) {
            vm.newAddress = newAddress;
            vm.updateAddressButton = false;
            vm.newAddressShow = "false";
            vm.buttonBar = "false";
            vm.showHideButtonText = "Add New Address";

        }
   
        function addressEdit(address) {
            vm.updateAddressButton = true;
            vm.showAddNew = false;
            var text = "Edit";
            if (text === "Cancel") {
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.editingItem.address = {};
                vm.buttonBar = true;
            }
            else {
                vm.newAddress = address;
                vm.newAddress.firstName = address.firstName;
                vm.newAddress.middleName = address.middleName;
                vm.newAddress.lastName = address.lastName;
                vm.newAddress.mobilePhone = address.mobilePhone;
                vm.newAddress.telephone = address.telephone;
                vm.newAddress.fax = address.fax;
                vm.newAddress.email = address.email;
                vm.newAddress.zipCode = address.zipCode;
                vm.newAddress.addressLine1 = address.addressLine1;
                vm.newAddress.addressLine2 = address.addressLine2;
                vm.newAddress.city = address.city;
                vm.newAddress.state = address.state;
                vm.newAddress.countryId = address.countryId;
                vm.newAddress.addressType = address.addressType;
                vm.newAddress.isDefault = address.isDefault;
                vm.newAddressShow = "true";
                vm.showHideButtonText = "Cancel";
                vm.newAddressEmail = true;
                vm.buttonBar = false;
            }
        }

        function passwordValidator(password, confirmPassword) {
                var result = false;
                if (password !== confirmPassword) {
                    result = true;
                }
                if (password === "" && confirmPassword === null) {
                    result = false;
                }
            return result;
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
                        deleteCustomer(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateCustomer(value);
                        editCustomer(value.id, true);
                        vm.customerForm.$invalid = true;
                        //swal(successMessage, "success");
                    }

                });
        }
        function getCustomersPageSize() {
            customerService.getPageSize().then(function (res) {
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
            if (vm.totalCustomer < vm.pageSize)
                vm.currentPage = 1;
            customerService.searchTextForCustomer(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.customers = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.totalCustomer = result.data.totalRecords;
                    }
                });

                customerService.GetAllLevels()
                    .then(function(result) {
                        if (result.success) {
                            vm.customerLevel = result.data;
                        } 
                    });

                customerService.GetPaymentMethod()
                    .then(function(result) {
                        if (result.success) {
                            vm.paymentMethod = result.data;
                        } 
                    });

        }
    
    }

})();
