(function () {
    "use strict";
    angular.module("app-myaccount")
        .controller("myaccountController", myaccountController);
    myaccountController.$inject = ['$http', '$filter', '$scope','$location', 'myaccountService', 'viewModelHelper'];
    function myaccountController($http, $filter, $scope, $location, myaccountService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active'; editUser
        vm.editUser = editUser;
        vm.updateCustomer = updateCustomer;
        vm.addUserShippingAddress = addUserShippingAddress;
        vm.newAddressDialog = newAddressDialog;
        vm.orders = [{ id: 1, invoiceNumber: '0125684', status: 'Active', date: new Date() }, { id: 2, invoiceNumber: '98561465', status: 'Active', date: new Date() },{ id: 3, invoiceNumber: '4567400', status: 'Active', date: new Date() }];
        vm.removeRow = removeRow;
        vm.viewHosory = viewHosory;
        vm.viewOrderHistoryById = viewOrderHistoryById;
        vm.editUserShippingAddress = editUserShippingAddress;
        vm.countries = [{ id: 1, name: "Nepal", code:"+977" }];
        vm.optionList = [
        {
             id: 1, name: 'Account Dashboard'
        }, {
            id: 2, name: 'Address Book'
        }, {
            id: 3, name: 'My Orders'
        },
        //{
        //    id: 4, name: 'My Returns'
        //}, 
        {
            id: 5, name: 'Recent History'
        }
        //, {
          //  id: 6, name: 'Gift Card'
       // }
        ];
        vm.updateAddress = updateAddress;    
        vm.isDefaults = [{ id: 1, name: 'true' }, { id: 2, name: 'false' }];
        vm.newAddressShow = "false";
        vm.newAddressEmail = false;
        vm.listClick = listClick;
        vm.showItem = 1;
        vm.loginUser = '';
        vm.user = null;
        init();

        function editUser(customer) {
            vm.profileForm.$setUntouched();
            $('#errorMessage').html('');
            vm.editingItem = {};
            vm.customerCode = true;
            vm.customerEmail = true;
            var telephone = customer.billingAddress.telephone;
            var mobNo = customer.billingAddress.mobilePhone;

            customer.billingAddress.mobilePhone = mobNo.substring(mobNo.indexOf("-") + 1);
            if (customer.billingAddress.telephone) {
                customer.billingAddress.telephone = telephone.substring(telephone.indexOf("-") + 1);
            }
            if(customer.addresses!==[])
            for (var i = 0; i < customer.addresses.length; i++) {
                var teleNo = customer.addresses[i].telephone;
                var mobileNo = customer.addresses[i].mobilePhone;
                customer.addresses[i].mobilePhone = mobileNo.substring(mobileNo.indexOf("-") + 1);
                if (customer.addresses[i].telephone) {
                    customer.addresses[i].telephone = teleNo.substring(teleNo.indexOf("-") + 1);
                }
                
            }
            vm.editingItem = customer;
            $scope.showModal = true;
        }
       
        function newAddressDialog(text) {
            if (text === "Cancel") {
                vm.newAddressShow = "false";
                vm.showHideButtonText = "Add New Address";
                vm.newAddress = {};
            }
            else {
                vm.newAddressShow = "true";
                vm.showHideButtonText = "Cancel";
            }

        }

        function removeRow(user, address, index) {
            if (address.id) {
                address.active = false;
                user.confirmPassword = user.password;
                myaccountService.Update(user).then(function (result) {
                    if (result.success) {
                        user.addresses.splice(index, 1);
                    }

                });
            }
            else {
                user.addresses.splice(index, 1);
            }
        }

        function editCustomer(customer) {
            myaccountService.GetById(vm.loginUser)
                .then(function (result) {
                    if (result.success) {
                        vm.editingItem = result.data;
                        for (var i = 0; i < vm.editingItem.addresses.length; i++) {
                            if (vm.editingItem.addresses[i].addressType === 1) {
                                vm.editingItem.addresses[i].addressType = "shipping";
                            }
                            else if (vm.editingItem.addresses[i].addressType === 2) {
                                vm.editingItem.addresses[i].addressType = "contact";
                            }

                        }
                        $scope.showModal = true;
                    } else {
                        var message = {};
                        message.message = "get customer by id , " + result.message + " in myaccount.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        function addUserShippingAddress(user) {
            vm.shippingAddressForm.$setUntouched();
            $('#errorMessageForAddress').html('');
            vm.newAddress = null;
            vm.newAddressEmail = false;
            vm.user = user;
        }

        function editUserShippingAddress(user,address) {
            $('#errorMessageForAddress').html('');
            vm.newAddressEmail = true;
            vm.user = user;
            vm.newAddress = address;
            var teleNo = vm.newAddress.telephone;
            var mobileNo = vm.newAddress.mobilePhone;
            vm.newAddress.mobilePhone = mobileNo.substring(mobileNo.indexOf("-") + 1);
            vm.newAddress.telephone = teleNo.substring(teleNo.indexOf("-") + 1);
            
        }

        function updateCustomer(editingItem) {

            var message = validatePassword(editingItem.password, editingItem.confirmPassword);

            if (message !== "Okay") {
                dialogMessage(message);
                return;
            }
            if(message===undefined){
                var result = "enter confirm password.";
                dialogMessage(result);
                return;
            }
            else {
                myaccountService.Update(editingItem).then(function (result) {
                    if (result.success) {
                        $scope.showModal = false;
                        $('#addressBookModal').modal('hide');
                    } else {
                        if (result.data.errors) {
                            dialogMessage(result.data.errors);
                        } else {
                            dialogMessage(result.message);
                            var message = {};
                            message.message = "update customer , " + result.message + " in myaccount.,";
                            viewModelHelper.bugReport(message,
                                function(result) {
                                });
                            vm.showHideButtonText = "Add New Address";
                            vm.newAddressShow = "false";
                        }
                    }
                });
            }
        }

        function updateAddress(address) {
            if (address == null) return;
            if (address.id == undefined) {
               
                vm.user.addresses.push(address);
            }
            for (var j = 0; j < vm.user.addresses.length; j++) {
                for (var i = 0; i < vm.countries.length; i++) {
                    if (vm.user.addresses[j].countryId === vm.countries[i].id) {
                        vm.user.addresses[j].mobilePhone = vm.countries[i].code + "-" + vm.user.addresses[j].mobilePhone;
                    }
                }
            }
            vm.user.confirmPassword = vm.user.password;
            myaccountService.Update(vm.user).then(function(result) {
                if (result.success) {
                    $('#userDetailAddressModal').modal('hide'); // success

                }
                else {
                if (result.data.errors) {
                        dialogMessageForAddress(result.data.errors);
                    }
                else {
                        dialogMessageForAddress(result.data);
                    }
                }
            });
        }

        function listClick(option) {
            vm.showItem = option.id;
            switch (option.id.toString()) {
                case '1':
                    accountDashboard();
                    break;
                case '2':
                    addressBook();
                    break;
                case '3':
                    myOrder();
                    break;
                //case '4':
                //    myReturn();
                //    break;
                case '5':
                    history();
                    break;
                case '6':
                    giftCard();
                    break;
                default:

            }
        }

        function accountDashboard() {
            //alert("this is dashboard");
          
        }

        function addressBook(id) {
            //myaccountService.getCustomerAddressById(id).then(function(result) {
            //    if (result.success) {
            //        //
            //    }
            //});
        }

        function myOrder() {
            myaccountService.getMyOrders(vm.userDetails.id).then(function (result) {
                if (result.success) {
                    vm.myOrder = result.data;
                } else {
                    var message = {};
                    message.message = "my order , " + result.message + " in myaccount.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }

        function myReturn() {
            //alert("this is myReturn");
        }

        var addresss = [];
        function init() {
            myaccountService.GetAllLevels().then(function (result) {
                 if (result.success) {
                        vm.customerLevel = result.data;
                        myaccountService.getAllCountries().then(function (result) {
                            if (result.success) {
                               // vm.countries = result.data;
                                myaccountService.getAllTitles().then(function (result) {
                                     if (result.success) {
                                         vm.titles = result.data;
                                         myaccountService.getAllSuffixes().then(function (result) {
                                             if (result.success) {
                                                 vm.suffixes = result.data;
                                                 myaccountService.getActiveZones().then(function (result) {
                                                     if (result.success) {
                                                         vm.zone = result.data;
                                                         myaccountService.getAllAddressTypes().then(function (result) {
                                                             if (result.success) {
                                                                 for (var i = 0; i < result.data.length; i++) {
                                                                     if (result.data[i].value != "Billing") {
                                                                         addresss.push(result.data[i]);
                                                                     }
                                                                 }
                                                                 vm.allAddressType = addresss;
                                                                 vm.class = 'loader loader-default';
                                                                 try {
                                                                    myaccountService.getLoginUserId(result.data)
                                                                                .then(function (result) {
                                                                                    if (result.success) {
                                                                                        vm.loginUser = result.data;
                                                                                        myaccountService.GetById(vm.loginUser).then(function (result) {
                                                                                            if (result.success) {
                                                                                                vm.userDetails = result.data;
                                                                                            } else {
                                                                                                var message = {};
                                                                                                message.message = "get customer by id , " + result.message + " in myaccount.,";
                                                                                                viewModelHelper.bugReport(message,
                                                                                                  function (result) {
                                                                                                  });
                                                                                            }

                                                                                            myaccountService.getMyOrders(vm.userDetails.id).then(function (result) {
                                                                                                if (result.success) {
                                                                                                    vm.myOrder = result.data;
                                                                                                } else {
                                                                                                    var message = {};
                                                                                                    message.message = "my order , " + result.message + " in myaccount.,";
                                                                                                    viewModelHelper.bugReport(message,
                                                                                                      function (result) {
                                                                                                      });
                                                                                                }
                                                                                            });

                                                                                        });
                                                                                    } else {
                                                                                        var message = {};
                                                                                        message.message = "get login user id , " + result.message + " in myaccount.,";
                                                                                        viewModelHelper.bugReport(message,
                                                                                          function (result) {
                                                                                          });
                                                                                    }
                                                                                });

                                                                } catch (e) {
                                                                }
                                                             } else {
                                                                 var message = {};
                                                                 message.message = "get all addressTypes , " + result.message + " in myaccount.,";
                                                                 viewModelHelper.bugReport(message,
                                                                   function (result) {
                                                                   });
                                                             }
                                                         });
                                                     } else {
                                                         var message = {};
                                                         message.message = "get all zones , " + result.message + " in myaccount.,";
                                                         viewModelHelper.bugReport(message,
                                                           function (result) {
                                                           });
                                                     }
                                                 });
                                             }
                                             else {
                                                 var message = {};
                                                 message.message = "get all suffixes , " + result.message + " in myaccount.,";
                                                 viewModelHelper.bugReport(message,
                                                   function (result) {
                                                   });
                                             }
                                         });
                                     }
                                     else {
                                         var message = {};
                                         message.message = "get all titles , " + result.message + " in myaccount.,";
                                         viewModelHelper.bugReport(message,
                                           function (result) {
                                           });
                                     }
                                 });
                            }
                            else {
                                var message = {};
                                message.message = "get all countries , " + result.message + " in myaccount.,";
                                viewModelHelper.bugReport(message,
                                    function (result) {
                                    });
                            }
                        });
                    } else {
                        var message = {};
                        message.message = "get all level , " + result.message + " in myaccount.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                 }

                

                 

                 

                 

                 
                 

                    
                });
        }

        function viewHosory(ev, order) {
            vm.salesOrderDetails = order;
            openDialog(ev, '#viewHistory');
        }

        vm.crmLocation = "";
        function history() {
            myaccountService.getOrderSummary(vm.loginUser)    // usser login id
                .then(function (result) {
                    if (result.success) {
                        vm.recentHistory = result.data;
                    } else {
                        var message = {};
                        message.message = "get order summary , " + result.message + " in myaccount.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
            vm.history = [{ imageUrls: 'CompanyMM/1/product-1-1.png', name: 'Dell inspirion', viewedOn: new Date() }, { imageUrls: 'CompanyMM/1/product-2-1.png', name: 'Samsung galaxy', viewedOn: new Date() }, { imageUrls: 'CompanyMM/1/product-3-1.png', name: 'Moter bike', viewedOn: new Date() }];
            myaccountService.crmLocation()
                   .then(function (result) {
                       if (result.success) {
                           vm.crmLocation = result.data + '/';
                       } else {
                           var message = {};
                           message.message = "get CRM location , " + result.message + " in myaccount.,";
                           viewModelHelper.bugReport(message,
                             function (result) {
                             });
                       }
                   });
            //alert("this is history");
        }

        function viewOrderHistoryById(id, ev) {
            myaccountService.salesOrderById(id)
                   .then(function (result) {
                       if (result.success) {
                           vm.activeSalesOrder = result.data;
                           vm.activeSalesOrder.dueDate = new Date(vm.activeSalesOrder.dueDate);
                           vm.activeSalesOrder.invoicedDate = new Date(vm.activeSalesOrder.invoicedDate);
                           calculateValues(vm.activeSalesOrder);
                           
                           initHistory();
                           openDialog(ev, '#viewItemHistory');
                       } else {
                           var message = {};
                           message.message = "get sales order by id , " + result.message + " in myaccount.,";
                           viewModelHelper.bugReport(message,
                             function (result) {
                             });
                       }
                   });
        }

        function calculateValues(order) {
            vm.totalVatAmount = 0;
            vm.totalBeforeVatAmount = 0;
            for (var i = 0; i < order.salesOrderLines.length; i++) {
                if (order.salesOrderLines[i].discountType === 1) {
                    var total = order.salesOrderLines[i].itemQuantity * order.salesOrderLines[i].itemPrice;
                    order.salesOrderLines[i].amount = total - (total * order.salesOrderLines[i].discount) / 100;
                } else if (order.salesOrderLines[i].discountType === 2) {
                    var total = order.salesOrderLines[i].itemQuantity * order.salesOrderLines[i].itemPrice;
                    order.salesOrderLines[i].amount =  order.salesOrderLines[i].itemQuantity * order.salesOrderLines[i].itemPrice - order.salesOrderLines[i].discount;
                } else {
                    order.salesOrderLines[i].amount = order.salesOrderLines[i].itemQuantity * order.salesOrderLines[i].itemPrice;
                }
                vm.totalBeforeVatAmount = vm.totalBeforeVatAmount + order.salesOrderLines[i].amount;
                vm.totalVatAmount = vm.totalVatAmount + calculateTax(order.salesOrderLines[i].taxAmount, order.salesOrderLines[i].amount);
            }
        }

        function calculateTax(value, total) {

            return (total * value) / 100;
        }

        function initHistory() {
            myaccountService.getAllUser()
                .then(function (result) {
                    if (result.success) {
                        vm.allUserList = result.data;
                    } else {
                        var message = {};
                        message.message = "get all user , " + result.message + " in myaccount.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }

                });
            myaccountService.getDeliveryMethods().then(function (result) {
                if (result.success) {
                    //console.log("delivery methods:" + JSON.stringify(result.data));
                    vm.allDeliveryMethod = result.data;
                } else {
                    var message = {};
                    message.message = "get delivery methods , " + result.message + " in myaccount.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }

            });
            myaccountService.getPaymentTerm().then(function (result) {
                if (result.success) {
                    vm.allTerms = result.data;
                } else {
                    var message = {};
                    message.message = "get payment term , " + result.message + " in myaccount.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }

            });
            //myaccountService.getCompanyUser().then(function (result) {
            //    if (result.success) {
            //        vm.allCompanyUserList = result.data;
            //    }
            //});
            myaccountService.getAllProduct().then(function (result) {
                if (result.success) {
                    vm.allProductList = result.data;
                } else {
                    var message = {};
                    message.message = "get all product , " + result.message + " in myaccount.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }

            });
            myaccountService.getActiveTax().then(function (result) {
                if (result.success) {
                    vm.allTax = result.data;
                } else {
                    var message = {};
                    message.message = "get active tax , " + result.message + " in myaccount.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }

            });
            vm.discountTypeOption = [{ name: "None", id: 3 }, { name: "Discount %", id: 1 }, { name: "Discount value", id: 2 }];
            
        }

        function giftCard() {
            //alert("this is giftCard");
        }

        function validatePassword(password, confirmPassword) {
            var message = "";
            if (password === undefined || confirmPassword === undefined||password===null||confirmPassword===null) { return; }
            if (password.length < 6) {
                message = "Password cannot be less 6 than digit.";
            }

            if (password === confirmPassword) {
                message = "Okay";
            }
            if(password!== confirmPassword){
                message = "Password donot match.";
            }
            return message;
        }

        function dialogMessage(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-danger alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '</div>'
                           + '</div>';
            $('#errorMessage').html(message);
        }

        function dialogMessageForAddress(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-danger alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '</div>'
                           + '</div>';
            $('#errorMessageForAddress').html(message);
        }

        vm.hide = hide;

        function hide() {
            vm.editingItem = null;
            $scope.showModal = false;
       }
    }
})();