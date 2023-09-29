(function () 
{
    angular.module("app-vendor")
    .controller("vendorController", vendorController);
    vendorController.$inject = ['$location', '$http', '$scope', '$filter', '$window', '$timeout','commonService', 'vendorService'];

    function vendorController($location, $http, $scope, $filter, $window, $timeout, commonService, vendorService) {
        var vm = this;
        vm.vendors = [];
        vm.searchText = null;
        vm.editVendor = editVendor;
        vm.updateVendor = updateVendor;
        vm.activateVendor = activateVendor;
        vm.check = false;
    //    vm.getPageSize = getPageSize;
       // vm.searchVendors = searchVendors;
        vm.addNewAddress = addNewAddress;
        vm.removeRow = removeRow;
        vm.actionChanged = actionChanged;
        vm.pageSizes = ["10", "20","30","40"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.newAddressDialog = newAddressDialog;
        vm.newAddressShow = "false";
        vm.vendorCode = false;
        vm.vendorDebit = false;
        vm.vendorCredit = false;
        vm.vendorEmail = false;
        vm.vendorBtnText = "Save";
        //vm.checkInactive = checkInactive;
        vm.addressEdit = addressEdit;
        vm.showHideButtonText = "Add New Address";
        vm.hideVendorDebit = false;
        vm.hideVendorCredit = false;
        vm.vendorContactName = false;
        vm.isDefaults = [{ id: 1, name: 'true' }, { id: 2, name: 'false' }];
        vm.nameChange = nameChange;
        vm.checkVendorCode = checkVendorCode;
        vm.showItem = false;
        vm.addnewCardButton = "Add Card";
        vm.newAddress = {};
        vm.vendorAddressEmail = false;
        vm.addressCheck = false;
        vm.changeAddressCheck = changeAddressCheck;
        vm.activateAddress = activateAddress;
        vm.checkEmail = checkEmail;
        vm.countries = [{ id: 1, name: 'Nepal' }];
        vm.updateAddressButton = false;
        vm.updateAddress = updateAddress;
        vm.searchParamChanged = searchParamChanged;
        vm.totalVendor = 0;


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
                angular.forEach(vm.vendors, function (check) {
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
            vendorService.deletedSelected(selectedItemid).then(function (result) {
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
        function checkEmail(email)
        {
                if (email !== undefined) {
                vendorService.checkIfUserEmailExists(email).then(function (result) { 
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Vendor already exists");
                                editVendor(result.data, true);
                            } else {
                                yesNoDialog("Do you want to active your vendor?", "info", "This vendor is already exists but has been disabled. Do you want to activate this vendor?", "Active", "Your vendor has been activated.", "active", result.data);
                            }
                        }
                    }
                });
            }

        }

        
        function activateAddress(addressId) {
            vendorService.activateAddressById(addressId).then(function (result) {
                if (result.success) {
                    vendorService.GetById(result.data.vendorId).then(function (result) {
                        if (result.success) {
                            vm.editingItem.addresses = result.data.addresses;
                        }
                    });

                }
            });
        }

        function changeAddressCheck(check, vendorId) {
            if (check === true) {
                vendorService.getAllAddresses(vendorId).then(function (result) {
                    if (result.success) {
                        vm.editingItem.addresses = result.data;
                    }
                });
            } else {
                vendorService.GetById(vendorId).then(function (result) {
                    if (result.success) {
                        vm.editingItem.addresses = result.data.addresses;
                    }
                });

            }
        }

        init();

        loadDefaulValues();

        vm.open = function () {
            // $('#errorMessage').html('');
            vm.vendorForm.$setUntouched();
            vm.editingItem = {};
            vm.vendorCode = false;
            vm.vendorEmail = false;
            vm.hideVendorDebit = false;
            vm.hideVendorCredit = false;
            vm.vendorCredit = false;
            vm.vendorDebit = false;
            vm.vendorContactName = false;
            vm.newAddressShow = "false";
            vm.newAddress = {};
            vm.vendorCodeLength = 8;
            vm.vendorBtnText = "Save";
            vm.showHideButtonText = "Add New Address";
            vm.editingItem.addresses = [];
            vm.showModal = true;
            
            
           
        };

        vm.hide = function () {
            vm.vendorCode = false;
            vm.vendorEmail = false;
            vm.vendorDebit = false;
            vm.vendorCredit = false;
            vm.hideVendorDebit = false;
            vm.hideVendorCredit = false;
            vm.vendorContactName = false;
            vm.editingItem = null;
            vm.newAddress = null;
            vm.newAddressShow ="false";
            vm.showHideButtonText = "Add New Address";
            vm.activeItemnewAddress = null;
            vm.showModal = false;
        };

        function init() {
            vm.pageSize = 10;
            getVendorPageSize()
          //  getAllActiveVendors();
        }

        function loadDefaulValues() {
            loadAllTitles();
            loadAllSuffixes();
          //  loadAllCountries();
            loadAllAddressTypes();
            getAccountTerm();
        }

        function loadAllSuffixes() {

            vm.suffixes = [

                { id: 1, value: 'None' },
                { id: 2, value: 'Doctor' },
                { id: 3, value: 'Engineer' },
                { id: 4, value: 'Enterprenuer' },
                { id: 5, value: 'Lawyer' },
                { id: 6, value: 'Businessman' },
                { id: 7, value: 'Businesswoman' },
                { id: 8, value: 'Politician' },
                { id: 9, value: 'Freelancer' },
                { id: 10, value: 'MR' },
                { id: 11, value: 'Other' },
            ];
            //vendorService.getAllSuffixes()
            //        .then(function (result) {
            //            if (result.success) {
            //                vm.suffixes = result.data;
            //            } 
            //        });
        }

        //function loadAllCountries() {
        //    vendorService.getAllCountries()
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.countries = result.data;
        //            }
        //        });
        //}

        function loadAllTitles() {

            vm.titles = [

               { id: 1, value: 'None' },
               { id: 2, value: 'Mr' },
               { id: 3, value: 'Mrs' },
               { id: 4, value: 'Miss' },
               { id: 5, value: 'Shri' },
               { id: 6, value: 'Smt' },
               { id: 7, value: 'Shushri' },
               { id: 8, value: 'Other' }
               
            ];
            //vendorService.getAllTitles()
            //       .then(function (result) {
            //           if (result.success) {
            //               vm.titles = result.data;
            //           } 
            //       });
        }
        function loadAllAddressTypes() {
            vm.allAddressType = [

              { id: 1, value: 'Shipping' },
              { id: 2, value: 'Contact' }             

            ];

            //vendorService.getAllAddressTypes()
            //       .then(function (result) {
            //           if (result.success) {
            //               vm.allAddressType = [];
            //               for (var i = 0; i < result.data.length; i++) {
            //                   if (result.data[i].value === "Billing") {
            //                   } else {
            //                       vm.allAddressType.push(result.data[i]);
            //                   }
                               
            //               }
                           
            //           } 
            //       });
        }

        function getAccountTerm() {
            vendorService.getAccountTerm()
                .then(function(result) {
                    if (result.success) {
                        vm.onAccountOptions = result.data;
                    }
                });
        }

        function nameChange(firstName, lastName) {
            if (firstName && lastName) {
                vm.editingItem.contactName = firstName + ' ' + lastName;
            }
            else if (!firstName) {
                var emptyFirstName = "";
                vm.editingItem.contactName = emptyFirstName + ' ' + lastName;
            }
            else if (!lastName) {
                var emptyLastName = "";
                vm.editingItem.contactName = firstName + ' ' + emptyLastName;
            }

        }

        function addNewAddress(newAddress) {
            if (newAddress === null) {
                swal("please fillup all the required fields.");
                return;
            }
            else if (newAddress.addressType === undefined || newAddress.firstName === undefined || newAddress.lastName === undefined || newAddress.addressLine1 === undefined || newAddress.city === undefined || newAddress.state === undefined||
                newAddress.zipCode === undefined ||newAddress.countryId === undefined ||newAddress.email === undefined || newAddress.mobilePhone === undefined) {
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
                vm.showAddNew = false;
                vm.showUpdate = false;
            }
            
        }

        //function checkInactive(checked) {
        //    if (checked) {
        //        inactivevendor();
        //        vm.check = true;
        //    }
        //    else {
        //        vm.check = false;
        //        init();
        //    }
        //}

        //function inactivevendor() {
        //    vendorService.getInactivevendors(vm.currentPage, vm.pageSize)
        //            .then(function (result) {
        //                if (result.success) {
        //                    vm.vendors = result.data.items;
        //                    vm.pagingList = result.data.pageCount;
        //                    vm.pageNumber = result.data.pageNumber;
        //                }
        //        });
        //}

        function actionChanged(vendor, action) {
            if (Number(action) === 1) {
                vm.vendorCode = true;
                vm.vendorEmail = true;
                vm.hideVendorDebit = true;
                vm.hideVendorCredit = true;
                vm.vendorDebit = true;
                vm.vendorCredit = true;
                vm.vendorContactName = true;
                vm.vendorCodeLength = 10;
                vm.vendorBtnText = "Update";
                editVendor(vendor,vendor.active);
            }
            else if (Number(action) === 2) {
                //deletevendor(vendor);
                yesNoDialog("Are you sure?", "warning", "Vendor will be deactivated but still you can activate your vendor in future.", "Yes, delete it!", "Your vendor has been deactivated.", "delete", vendor);
            }
            else if (Number(action) === 3) {
                alert("you cannot use this item. We are working on it");
            }
            vm.action = null;
            
        }
        vm.newAddressShow === 'false';
        function newAddressDialog(text) {
            
            vm.updateAddressButton = false;
            vm.footerBar = true;
            vm.address.$setUntouched();

            if (text === "Cancel") {
                vm.showAddNew = false;
                vm.showUpdate = false;
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.newAddress = null;
                vm.vendorAddressEmail = false;
            }
            else {
                vm.showAddNew = true;
                vm.newAddress = null;
                vm.newAddressShow ="true";
                vm.showHideButtonText = "Cancel";
                
            }
            
        }

        function removeRow(address, index) {
            if (address.id) {
                address.active = false;
                vendorService.Update(vm.editingItem).then(function (result) {
                    if (result.success) {
                        vm.editingItem.addresses.splice(index, 1);
                    }
                });
            }

            else {
                vm.editingItem.addresses.splice(index, 1);
            }
        }

        //function searchVendors(text,checked) {
            
        //    if (checked === true) {
        //        if (text === "") {
        //            inactivevendor();
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

        //function searchTextChanged(text, checked) {
        //    vendorService.searchText(vm.currentPage, vm.pageSize, text, checked)
        //       .then(function (result) {
        //           if (result.success) {
        //               vm.vendors = result.data.items;
        //               vm.pagingList = result.data.pageCount;
        //               vm.pageNumber = result.data.pageNumber;
        //           }
        //       });
        //}

        vm.pageChanged = function (page) {
            $scope.selectAll = [];

            vm.currentPage = page;
            searchParamChanged();
          
        };

        function deleteVendor(successMessage, vendor) {
            vendorService.deletevendor(vendor.id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage, "success");
                        searchParamChanged();
                    }
                });
        }

        //function getPageSize(pageSize, checked) {
        //    vm.pageSize = pageSize;
        //    if (checked === true) {
        //        inactivevendor();
        //    } else {
        //        getAllActiveVendors();
        //    }
        //}

        //function getAllActiveVendors() {
        //    vendorService.Getvendors(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.vendors = result.data.items;
        //                vm.pagingList = result.data.pageCount;
        //                vm.pageNumber = result.data.pageNumber;
        //            }
        //        });

        //    vendorService.GetPaymentMethod()
        //        .then(function(result) {
        //            if (result.success) {
        //                vm.paymentMethod = result.data;
        //            } 
        //        });

        //}

        function editVendor(vendor,status) {
            if (status===true) {
                vendorService.GetById(vendor.id).then(function (result) {
                    if (result.success) {
                        vm.vendorCode = true;
                        vm.vendorEmail = true;
                        vm.editingItem = result.data;
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
                        //vm.vendorForm.$invalid = true;
                        
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
        function updateVendor(editingItem) {
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
            for (var k = 0; k < vm.countries.length; k++) {
                if (editingItem.billingAddress.countryId === vm.countries[k].id) {
                    editingItem.billingAddress.mobilePhone = commonService.addCode(vm.countries[k].code, editingItem.billingAddress.mobilePhone);
                    editingItem.addresses.telephone = commonService.addCode(vm.countries[k].code, editingItem.billingAddress.telephone);
                }
            }
            if (editingItem.billingAddress.id) {
                vendorService.Update(editingItem).then(function (result) {
                    if (result.success) {
                        vm.editingItem = {};
                        vm.showHideButtonText = "Add New Address";
                        vm.newAddressShow = "false";
                        vm.showModal = false;
                        searchParamChanged();

                    } else {
                        if(result.message.errors){
                            swal(result.message.errors[0]);
                        }
                        else {
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
                      
                    }
                });
            }
            else {
                editingItem.billingAddress.addressType = 1;
                if (editingItem.billingAddress.email) {
                    var result = checkVendorCode(editingItem.code);
                    if (editingItem.code === null || editingItem.code === "") {
                        editingItem.code = null;
                    }
                    else {
                        if (Number(result) === 0) {
                            editingItem.code = "V-" + editingItem.code;
                        }
                        else if (Number(result) === 1) {
                            swal("This vendor code already exists.");
                            vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                            vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                            if (vm.editingItem.addresses !== []) {
                                for (var l = 0; l < vm.editingItem.addresses.length; l++) {
                                    vm.editingItem.addresses[l].mobilePhone = commonService.removeCode(vm.editingItem.addresses[l].mobilePhone);
                                    vm.editingItem.addresses[l].telephone = commonService.removeCode(vm.editingItem.addresses[l].telephone);
                                }
                            }
                            return;
                        }

                        else if (Number(result) === 2) {
                            swal("Vendor code must be 8 characters.");
                            vm.editingItem.billingAddress.mobilePhone = commonService.removeCode(editingItem.billingAddress.mobilePhone);
                            vm.editingItem.billingAddress.telephone = commonService.removeCode(editingItem.billingAddress.telephone);
                            if (vm.editingItem.addresses !== []) {
                                for (var m = 0; m < vm.editingItem.addresses.length; m++) {
                                    vm.editingItem.addresses[m].mobilePhone = commonService.removeCode(vm.editingItem.addresses[m].mobilePhone);
                                    vm.editingItem.addresses[m].telephone = commonService.removeCode(vm.editingItem.addresses[m].telephone);
                                }
                            }
                            return;
                        }
                        else if (Number(result) === 3) {
                            editingItem.code = null;
                        }
                    }
                    vendorService.Create(editingItem).then(function (result) {
                        if (result.success) {
                            vm.editingItem = {};
                            searchParamChanged();
                            vm.showModal = false;
                        } else {
                            if(result.message.errors) {
                                swal(result.message.errors[0]);
                            }
                            else {
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
                            vm.editingItem.code = undefined;
                        }
                    });

                }

                else {
                    swal("Please fill up all required fields.");
                }
            }
        }

        function updateAddress(newAddress) {
            vm.newAddress = newAddress;
            vm.updateAddressButton = false;
            vm.footerBar = true;
            vm.newAddressShow = "false";
            vm.newAddressShow = "false";
            vm.showHideButtonText = "Add New Address";


        }
        
        function activateVendor(vendor) {
            vendorService.activatevendor(vendor.id)
                .then(function(result) {
                    if (result.success) {
                        swal("Vendor Activated");
                        searchParamChanged();
                    } 
                });
        };
        vm.footerBar = true;
        function addressEdit(address) {
           // vm.showUpdate = true;
            vm.updateAddressButton = true;
            vm.footerBar = false;
            var text = "Edit";
            if (text === "Cancel") {
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.editingItem.address = {};
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
                vm.vendorAddressEmail = false;
            }
            vm.showAddNew = false;
        }

        function checkVendorCode(code) {
            var data = 0;
            var vendorCode = "V-" + code;
          
            if (code === undefined || null) {
                data = 3;
            }
            if (code !== undefined) {
                if (code.length < 8 || code.length > 8) {
                    data = 2;
                } else {
                    vendorService.checkvendorCode(vendorCode).then(function (result) {
                        if (result.success) {
                            vendorService.checkIfVendorCodeExists(vendorCode).then(function (result) {
                                if (result.success) {
                                    if (result.data) {
                                        if (result.data.active) {
                                            swal("Vendor already exists");
                                            editVendor(result.data,true);
                                        } else {
                                            yesNoDialog("Do you want to active your vendor?", "info", "This vendor is already exists but has been disabled. Do you want to activate this vendor?", "Active", "Your vendor has been activated.", "active", result.data);
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
                        deleteVendor(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateVendor(value);
                        setTimeout(function() {
                            editVendor(value, true);
                        },3000);
                        //vm.vendorForm.$invalid = true;
                         
                        //swal(successMessage, "success");
                    }
                });
        }
        function getVendorPageSize() {
            vendorService.getPageSize().then(function (res) {
                if (res.success) {
                    if (res.data !== 0) {
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
            if (vm.searchText === undefined)
                vm.searchText = "";
            if (vm.totalVendor < vm.pageSize)
                vm.currentPage = 1;
            vendorService.searchTextForVendor(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.vendors = result.data.items;
                        vm.pagingList = result.data.pageCount;
                        vm.totalVendor = result.data.totalRecords;
                        vm.pageNumber = result.data.pageNumber;
                    }
                });
        }
    }

})();
