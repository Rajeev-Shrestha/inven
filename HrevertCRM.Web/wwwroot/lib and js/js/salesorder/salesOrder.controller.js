(function () {
    angular.module("app-salesOrder")
        
        .controller("salesOrderController", salesOrderController);
    salesOrderController.$inject = ['$http', '$filter', '$scope', '$element', 'salesOrderService'];

    function salesOrderController($http, $filter, $scope, $element,salesOrderService) {
        var vm = this;
        vm.allUserList = [];
        vm.newProductDetails = [];
        vm.selectUser = selectUser;
        vm.addProductDetails = addProductDetails;
        vm.productNameSelect = productNameSelect;
        vm.removeRow = removeRow;
        //vm.getPageSize = getPageSize;
        vm.productDiscountTypeSelected = productDiscountTypeSelected;
        vm.editSalesOrder = editSalesOrder;
        vm.deleteSalesOrder = deleteSalesOrder;
        vm.editproduct = editproduct;
        vm.updateSalesOrder = updateSalesOrder;
      //  vm.checkInactive = checkInactive;
        vm.actionChanged = actionChanged;
       // vm.activeSalesOrderById = activeSalesOrderById;
       // vm.searchTextChanged = searchTextChanged;
        vm.selectedProductDetails = "";
        vm.check = false;
        vm.totalVatAmount = 0;
        vm.taxTypeChange = false;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.taxRateItems = [];
        vm.pageChanged = pageChanged;
        vm.activeSalesOrder = {};
        vm.activeSalesOrder.salesOrderLines = [];
        vm.actionType = [{ action: "Edit" }, { action: "Delete" }, { action: "Create invoice" }];
        vm.selected = [];
        vm.salesPurchaseOrderNumber = false;
        vm.showSalesOrderCode = false;
        vm.totalSalesOrder = 0;
        vm.disableInvoicedDate = false;
        vm.disableAllFields = false;
        vm.showFullyPaid = showFullyPaid;
        vm.showCheckbox = false;
        vm.disableFullyPaid = false;
        vm.salesOrderBtnText = "Save";
        vm.orderStatus =
            [
                { id: 1, name: 'Sales Quote' },
                { id: 2, name: 'Sales Order' },
                { id: 3, name: 'Sales Invoice' },
                { id: 4, name: 'Credit Quote' },
                { id: 5, name: 'Credit Order' },
                { id: 6, name: 'Credit Memo' }
            ];
        vm.orderTypes =
           [
               { id: 1, name: 'Order' },
               { id: 2, name: 'Quote' },
               { id: 3, name: 'Invoice' }
           ];
        vm.discountTypes =
            [
                { id: 1, name: 'None' },
                { id: 2, name: 'Percent' },
                { id: 3, name: 'Fixed' }
            ];
    //    vm.getDueDateByTermIdAndDate = getDueDateByTermIdAndDate;
        vm.searchParamChanged = searchParamChanged;
        $element.find('input').on('keydown', function (ev) {
            ev.stopPropagation();
        });

        init();

        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;

        $scope.selected = [];

        vm.exist = function (item) {
            return $scope.selected.indexOf(item) > -1;
        }

        function showFullyPaid(ordertype) {
            if (ordertype == 3) {
                vm.showCheckbox = true;
            } else {
                vm.showCheckbox = false;
            }
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
                angular.forEach(vm.salesOrder, function (check) {
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
            var allSelectedid = [];
            for (var i = 0; i < selectedItem.length; i++) {
                allSelectedid.push(selectedItem[i].id);
            }            
            salesOrderService.deletedSelected(allSelectedid).then(function (result) {
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
        getSalesOrderPageSize();

        $scope.open = function () {
            vm.disableAllFields = false;
            vm.showCheckbox = false;
            vm.disableFullyPaid = false;
            vm.activeSalesOrder = {};
            vm.activeSalesOrder.salesOrderLines = [];
            vm.total = 0;
            vm.totalVatAmount = 0;
            vm.salesPurchaseOrderNumber = false;
            vm.showSalesOrderCode = false;
            addProductDetails(vm.activeSalesOrder);
            loadDefaultValues();
            $scope.showButton = true;
            $scope.disableRemoveIcon = false;            
            $scope.showModal = true;
            vm.salesForm.$setUntouched();
            vm.disableInvoicedDate = false;
            vm.salesOrderBtnText = "Save";
            getSalesOrderTypes();
        }
        getAllCustomerWithoutPaging();
        function getAllCustomerWithoutPaging() {
              salesOrderService.getAllCustomerWithoutPaging().then(function (result) {
                  if (result.success) {
                      vm.allUserList = [];
                    for (var i = 0; i < result.data.length; i++) {
                        var obj = {
                            "id": result.data[i].id,
                            "displayName": result.data[i].displayName
                        }
                        vm.allUserList.push(obj);
                    }
                }

            });
        }
        $scope.hide = function () {
            vm.salesPurchaseOrderNumber = false;
            vm.showSalesOrderCode = false;
            vm.total = "";
            vm.totalVatAmount = "";
            vm.activeSalesOrder = {};
            vm.activeSalesOrder.salesOrderLines = [];
        //    vm.salesForm.$setUntouched();
            $scope.showModal = false;
        }

    
//        function loadAllActiveSalesOrder() {
//            salesOrderService.getOrderList(vm.currentPage, vm.pageSize).then(function (result) {
//                if (result.success) {
//                    vm.salesOrder = result.data.items;
//                    vm.pageNumber = result.data.pageNumber;
//                    vm.totalPage = result.data.pageCount;
//                    salesOrderService.getAllCustomerWithoutPaging().then(function (result) {
//                        if (result.success) {
//                            vm.customerDetails = result.data;
//                            for (var i = 0; i < vm.salesOrder.length; i++) {
//                                for (var j = 0; j < vm.customerDetails.length; j++) {
//                                    if (vm.customerDetails[j].id === vm.salesOrder[i].customerId) {
//                                        vm.salesOrder[i].displayName = vm.customerDetails[j].displayName;
//                                    }
//                                }
//                            }
//                        }
//                    });
//                }
//            });
//}
        function getSalesOrderTypes() {
            salesOrderService.getSalesOrderTypes().then(function (result) {
                if (result.success) {
                    vm.salesOrderTypes = result.data;
                }

            });
        }
        function init() {
            salesOrderService.getActiveTaxWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allTax = result.data;
                }

            });

            salesOrderService.getDeliveryMethods().then(function (result) {
                if (result.success) {
                    vm.allDeliveryMethod = result.data;
                }

            });

            salesOrderService.getPaymentMethod().then(function (result) {
                if (result.success) {
                    vm.paymentMethod = result.data;
                }
            });
       
            salesOrderService.getCompanyUserWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allCompanyUserList = result.data;
                }
            });

            salesOrderService.getAllProductWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allProductList = result.data;
                }

            });

            salesOrderService.getPaymentTerm().then(function (result) {
                if (result.success) {
                    vm.allTerms = result.data;
                }

            });

            salesOrderService.getAllDiscountTypes().then(function (result) {
                if (result.success) {
                    vm.discountTypeOption = result.data;
                }

            });
            getSalesOrderTypes();
            salesOrderService.getSalesOrderTypes().then(function (result) {
                if (result.success) {
                    vm.salesOrderTypes = result.data;
                    vm.listOfSalesOrderType = vm.salesOrderTypes;
                }

            });

            salesOrderService.getSalesOrderStatuses().then(function (result) {
                if (result.success) {
                    vm.salesOrderStatus = result.data;
                }

            });

        }

        function actionChanged(order,id) {
            if (Number(id) === 1) {
                vm.salesPurchaseOrderNumber = "true";
                vm.salesOrderBtnText = "Update";
                editSalesOrder(order);
            }
            else if (Number(id) === 2) {
                yesNoDialog("Are you sure?", "warning", "Sales Order will be deleted permanently.", "Yes, delete it!", "Your order has been deleted.", "delete", order);
            }
            else if (Number(id) === 3) {
                alert("Create report not working");
            }
            vm.action = null;
        }

        //function checkInactive(active, checked) {
        //    if (checked===true) {
        //        getInactiveOrder();
        //        vm.check = true;
        //    }
        //    else {
        //        vm.check = false;
        //        loadAllActiveSalesOrder();
        //    }
        //}

        //function activeSalesOrderById(order) {
        //    salesOrderService.activeSalesOrder(order.id)
        //       .then(function (result) {
        //           if (result.success) {
        //               swal("Sales Order Activated.");
        //               searchParamChanged();
        //           }

        //       });
        //}

        //function getInactiveOrder(){
        //    salesOrderService.getAllOrderList(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.salesOrder = result.data.items;
        //                vm.pageNumber = result.data.pageNumber;
        //                vm.totalPage = result.data.pageCount;
        //                salesOrderService.getCustomer(vm.currentPage, vm.pageSize).then(function (result) {
        //                        if (result.success) {
        //                            vm.customerDetails = result.data.items;
        //                            for (var i = 0; i < vm.salesOrder.length; i++) {
        //                                for (var j = 0; j < vm.customerDetails.length; j++) {
        //                                    if (vm.customerDetails[j].id === vm.salesOrder[i].customerId) {
        //                                        vm.salesOrder[i].displayName = vm.customerDetails[j].displayName;
        //                                        vm.salesOrder[i].email = vm.customerDetails[j].email;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    });
        //            }
        //        });
        //}
       
        function saveTaxRate(salesOrder) {
            vm.taxRateItems = [];
            for (var i = 0; i < salesOrder.length; i++) {
                for (var j = 0; j < vm.allTax.length; j++) {
                    if (Number(salesOrder[i].taxId) === vm.allTax[j].id) {
                        vm.taxRateItems.push({
                            id: salesOrder[i].taxId,
                            rate: vm.allTax[j].taxRate,
                            index: salesOrder[i].lineOrder
                        });
                    }
                }
                
            }
            
        }

        //function searchTextChanged(text, checked) {
        //    if (checked === true) {
        //        if (text === "") {
        //            getInactiveOrder();
        //        } else {
        //            searchText(text, checked);
        //        }
        //    } else {
        //        if (text === "") {
        //            loadAllActiveSalesOrder();
        //        } else {
        //            searchText(text, checked);
        //        }
        //    }
        //}

        //function searchText(text, checked) {
        //   salesOrderService.searchText(vm.currentPage, vm.pageSize, text, checked)
        //      .then(function (result) {
        //          if (result.success) {
        //              vm.salesOrder = result.data.items;
        //              vm.pageNumber = result.data.pageNumber;
        //              vm.totalPage = result.data.pageCount;
        //              salesOrderService.getAllCustomerWithoutPaging().then(function (result) {
        //                  if (result.success) {
        //                      vm.customerDetails = result.data;
        //                      for (var i = 0; i < vm.salesOrder.length;i++){
        //                          for (var j = 0; j < vm.customerDetails.length; j++) {
        //                              if (vm.customerDetails[j].id === vm.salesOrder[i].customerId) {
        //                                  vm.salesOrder[i].displayName = vm.customerDetails[j].displayName;
        //                              }
        //                          }
        //                      }
        //                  }
        //              });
        //          }
        //      });
        //}
        getActiveTaxes();
        function getActiveTaxes() {
            salesOrderService.getActiveTax(vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.allTax = result.data;
                    }
                    else {
                        swal(result.message);
                    }
                });
        }

        function editproduct(orderList) {
            vm.total = 0;
            vm.totalVatAmount = 0;
            saveTaxRate(orderList);

            for (var i = 0; i < orderList.length; i++) {
                vm.activeSalesOrder.salesOrderLines[i].amount = orderList[i].itemPrice * orderList[i].itemQuantity;

                if (Number(orderList[i].discountType) === 1) {
                    vm.activeSalesOrder.salesOrderLines[i].disableDiscountBox = true;
                    vm.activeSalesOrder.salesOrderLines[i].discount = 0;
                    if (vm.taxRateItems.length === 0) {
                        //vm.total = 0;
                        vm.activeSalesOrder.salesOrderLines[i].amount = vm.activeSalesOrder.salesOrderLines[i].amount - (orderList[i].itemPrice * orderList[i].itemQuantity) * orderList[i].discount / 100;
                        //vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[i].amount;
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activeSalesOrder.salesOrderLines[i].amount = (totalAmount + (totalAmount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            vm.activeSalesOrder.salesOrderLines[i].amount = orderList[i].itemPrice * orderList[i].itemQuantity;
                        }
                        
                        //vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[i].amount;
                    }

                } else if (Number(orderList[i].discountType) === 2) {
                    vm.activeSalesOrder.salesOrderLines[i].disableDiscountBox = false;
                    if (vm.taxRateItems.length === 0) {
                        //vm.total = 0;
                        vm.activeSalesOrder.salesOrderLines[i].amount = vm.activeSalesOrder.salesOrderLines[i].amount - (orderList[i].itemPrice * orderList[i].itemQuantity) * orderList[i].discount / 100;
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            var totalAfterDiscount = totalAmount - (totalAmount * Number(orderList[i].discount) / 100);
                            vm.activeSalesOrder.salesOrderLines[i].amount = (totalAfterDiscount + (totalAfterDiscount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activeSalesOrder.salesOrderLines[i].amount = totalAmount - (totalAmount * Number(orderList[i].discount) / 100);
                        }
                        
                    }

                } else if (Number(orderList[i].discountType) === 3) {
                    vm.activeSalesOrder.salesOrderLines[i].disableDiscountBox = false;

                    if (vm.taxRateItems.length === 0) {
                        //vm.total = 0;
                        //vm.activeSalesOrder.salesOrderLines[i].amount = (vm.total + vm.activeSalesOrder.salesOrderLines[i].amount) - vm.activeSalesOrder.salesOrderLines[i].discount;
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            var totalAfterDiscount = totalAmount - Number(orderList[i].discount);
                            vm.activeSalesOrder.salesOrderLines[i].amount = (totalAfterDiscount + (totalAfterDiscount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activeSalesOrder.salesOrderLines[i].amount = totalAmount - Number(orderList[i].discount);
                        }
                        
                    }

                }
                else {
                    //if (vm.taxRateItems.length === 0) {
                    //    vm.total = 0;
                    //    vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[i].amount;
                    //} else {
                    //    vm.activeSalesOrder.salesOrderLines[i].amount = (orderList[i].itemPrice * orderList[i].itemQuantity);
                    //    vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[i].amount;
                    //    vm.totalVatAmount = vm.totalVatAmount +
                    //        vm.activeSalesOrder.salesOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                    //}
                }
                
            }
            for (var j = 0; j < vm.activeSalesOrder.salesOrderLines.length; j++) {
                vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[j].amount;
            }
        }

        function productDiscountTypeSelected(type, amount, index, product) {
            if (!amount) {
                vm.newProductDetails[index].amount = "";
                vm.newProductDetails[index].amount = Number(product.qty) * Number(product.unitPrice);
            } else {

                vm.newProductDetails[index].amount = "";
                vm.newProductDetails[index].amount = Number(product.qty) * Number(product.unitPrice);
                if (amount !== "") {
                    if (type === 'Discount %') {
                        vm.newProductDetails[index]
                            .amount = vm.newProductDetails[index].amount -
                            (vm.newProductDetails[index].amount * amount) / 100;
                    } else if (type === 'Discount value') {
                        vm.newProductDetails[index].amount = vm.newProductDetails[index].amount - amount;
                    }
                    else if (type === 'None') {
                        vm.newProductDetails.discount[index] = 0;
                        vm.newProductDetails[index].amount = vm.newProductDetails[index].amount - 0;
                    }
                }
            }
        }

        function deleteSalesOrder(successmessage, sellsOrder) {
            salesOrderService.deleteSalesOrder(sellsOrder.id)
                .then(function(result) {
                    if (result.success) {
                        swal(successmessage,"success");
                        searchParamChanged();
                        $scope.showModal = false;
                    }

                });
        }
        function editSalesOrder(salesOrder) {
            vm.listOfSalesOrderType = vm.salesOrderTypes.slice();
            if (salesOrder.orderType === 2) {
                vm.orderTypes.splice(2, 1);
            } else {
                getSalesOrderTypes();
            }
            if (salesOrder.orderType === 3) {
                vm.showCheckbox = true;
                vm.disableAllFields = true;
                $scope.disableRemoveIcon = true;
                if (salesOrder.fullyPaid === false) {
                    $scope.showButton = true;
                    vm.disableFullyPaid = false;
                    if (salesOrder.active) {                      
                        salesOrderService.getById(salesOrder.id).then(function (result) {
                            if (result.success) {
                                vm.activeSalesOrder = result.data;
                                vm.activeSalesOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                                vm.activeSalesOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                                vm.activeSalesOrder.deliveryMethodId = result.data.deliveryMethodId;
                                vm.activeSalesOrder.billingAddressId = result.data.billingAddressId;
                                vm.activeSalesOrder.shippingAddressId = result.data.shippingAddressId;
                                vm.showSalesOrderCode = true;
                                vm.salesPurchaseOrderNumber = true;
                                getAllCustomerWithoutPaging();
                                selectUser(salesOrder.customerId);
                                salesOrderService.getActiveTaxWithoutPaging().then(function (result) {
                                    if (result.success) {
                                        vm.allTax = result.data;
                                        vm.total = 0;
                                        vm.totalVatAmount = 0;
                                        getUserAddressById(salesOrder.customerId);
                                        saveTaxRate(vm.activeSalesOrder.salesOrderLines);
                                        if (vm.activeSalesOrder.salesOrderLines === null) {
                                            vm.activeSalesOrder.salesOrderLines = [];
                                            addProductDetails(vm.activeSalesOrder);
                                        }
                                        else {
                                            editproduct(vm.activeSalesOrder.salesOrderLines);
                                        }

                                    }
                                });
                                $scope.showModal = true;
                                vm.disableInvoicedDate = true;
                            }
                        });
                    }
                } else {
                    if (salesOrder.active) {
                        vm.disableFullyPaid = true;
                        salesOrderService.getById(salesOrder.id).then(function (result) {
                            if (result.success) {
                                vm.activeSalesOrder = result.data;
                                vm.activeSalesOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                                vm.activeSalesOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                                vm.activeSalesOrder.deliveryMethodId = result.data.deliveryMethodId;
                                vm.activeSalesOrder.billingAddressId = result.data.billingAddressId;
                                vm.activeSalesOrder.shippingAddressId = result.data.shippingAddressId;
                                vm.showSalesOrderCode = true;
                                vm.salesPurchaseOrderNumber = true;
                                getAllCustomerWithoutPaging();
                                selectUser(salesOrder.customerId);
                                salesOrderService.getActiveTaxWithoutPaging().then(function (result) {
                                    if (result.success) {
                                        vm.allTax = result.data;
                                        vm.total = 0;
                                        vm.totalVatAmount = 0;
                                        getUserAddressById(salesOrder.customerId);
                                        saveTaxRate(vm.activeSalesOrder.salesOrderLines);
                                        if (vm.activeSalesOrder.salesOrderLines === null) {
                                            vm.activeSalesOrder.salesOrderLines = [];
                                            addProductDetails(vm.activeSalesOrder);
                                        }
                                        else {
                                            editproduct(vm.activeSalesOrder.salesOrderLines);
                                          
                                        }

                                    }
                                });
                                $scope.showModal = true;
                                vm.disableInvoicedDate = true;
                                $scope.showButton = false;
                            }
                        });
                    }
                }
            } else {
                vm.disableAllFields = false;
                $scope.showButton = true;
                $scope.disableRemoveIcon = false;
                if (salesOrder.active) {
                    vm.showCheckbox = false;
                    salesOrderService.getById(salesOrder.id).then(function (result) {
                        if (result.success) {
                            vm.activeSalesOrder = result.data;
                            vm.activeSalesOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                            vm.activeSalesOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                            vm.activeSalesOrder.deliveryMethodId = result.data.deliveryMethodId;
                            vm.activeSalesOrder.billingAddressId = result.data.billingAddressId;
                            vm.activeSalesOrder.shippingAddressId = result.data.shippingAddressId;
                            vm.showSalesOrderCode = true;
                            vm.salesPurchaseOrderNumber = true;
                            getAllCustomerWithoutPaging();
                            selectUser(salesOrder.customerId);
                            salesOrderService.getActiveTaxWithoutPaging().then(function (result) {
                                if (result.success) {
                                    vm.allTax = result.data;
                                    vm.total = 0;
                                    vm.totalVatAmount = 0;
                                    getUserAddressById(salesOrder.customerId);
                                    saveTaxRate(vm.activeSalesOrder.salesOrderLines);
                                    if (vm.activeSalesOrder.salesOrderLines === null) {
                                        vm.activeSalesOrder.salesOrderLines = [];
                                        addProductDetails(vm.activeSalesOrder);
                                    }
                                    else {
                                        editproduct(vm.activeSalesOrder.salesOrderLines);
                                        //for (var i = 0; i < vm.activeSalesOrder.salesOrderLines.length; i++) {
                                        //    vm.activeSalesOrder.salesOrderLines[i].amount = vm.activeSalesOrder.salesOrderLines[i].itemPrice * vm.activeSalesOrder.salesOrderLines[i].itemQuantity;

                                        //    if (Number(vm.activeSalesOrder.salesOrderLines[i].discountType) === 1) {
                                        //        vm.activeSalesOrder.salesOrderLines[i].amount = vm.activeSalesOrder.salesOrderLines[i].amount - (vm.activeSalesOrder.salesOrderLines[i].itemPrice * vm.activeSalesOrder.salesOrderLines[i].itemQuantity) * vm.activeSalesOrder.salesOrderLines[i].discount / 100;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activeSalesOrder.salesOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    } else if (Number(vm.activeSalesOrder.salesOrderLines[i].discountType) === 2) {

                                        //        vm.activeSalesOrder.salesOrderLines[i].amount = (vm.activeSalesOrder.salesOrderLines[i].itemPrice * vm.activeSalesOrder.salesOrderLines[i].itemQuantity) - vm.activeSalesOrder.salesOrderLines[i].discount;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activeSalesOrder.salesOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    } else if (Number(vm.activeSalesOrder.salesOrderLines[i].discountType) === 3) {

                                        //        vm.activeSalesOrder.salesOrderLines[i].amount = (vm.activeSalesOrder.salesOrderLines[i].itemPrice * vm.activeSalesOrder.salesOrderLines[i].itemQuantity) - 0;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activeSalesOrder.salesOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    }

                                        //    vm.total = vm.total + vm.activeSalesOrder.salesOrderLines[i].amount;
                                        //}
                                    }

                                }
                            });
                            $scope.showModal = true;
                            vm.disableInvoicedDate = true;
                            vm.salesOrderForm.$valid = true;
                           
                        }
                    });
                }
                else {
                    swal("You cannot edit this item.please activate it first.");
                }
            }
            
        }

        function getUserAddressById(user) {
            salesOrderService.getUserAddressById(user)
                .then(function(result) {
                    if (result.success) {
                        vm.selectedUserAddress = result.data.addresses;
                    }

                });
        }
       
        function selectUser(userId) {
            if (userId != null || userId != undefined) {
                salesOrderService.getdefaults(userId)
           .then(function (result) {
               if (result.success) {
                   vm.defaultValues = result.data;
                   vm.userBillingAddresses = result.data.billingAddresses;
                   vm.userShippingAddresses = result.data.shippingAddresses;
                   vm.activeSalesOrder.billingAddressId = result.data.billingAddressId;
                   vm.activeSalesOrder.shippingAddressId = result.data.shippingAddressId;
                   //  vm.activeSalesOrder.paymentTermsId = result.data.paymentTermsId;
                   vm.activeSalesOrder.email = result.data.billingAddresses[0].email;

               }

           });
                vm.userSelected = "true";
            }
            else {
                vm.userBillingAddresses = "";
                vm.userShippingAddresses = "";
                vm.activeSalesOrder.billingAddressId = "";
                vm.activeSalesOrder.shippingAddressId ="";
                //  vm.activeSalesOrder.paymentTermsId = result.data.paymentTermsId;
                vm.activeSalesOrder.email = "";
                swal("Please select customer")
            }
        }

        function addProductDetails(order) {
            if (order.salesOrderLines === null || order.salesOrderLines.length === 0 || order.salesOrderLines === []) {
                vm.activeSalesOrder.salesOrderLines = [];
            }
             vm.activeSalesOrder.salesOrderLines.push({ productId: "", itemPrice: "", itemQuantity: "" });
        }

        function productNameSelect(productDetails, orderList) {
            for (var i = 0; i < vm.allProductList.length; i++) {
                if (vm.allProductList[i].id === productDetails.productId) {

                    vm.activeSalesOrder.salesOrderLines[productDetails.indexing - 1]
                        .description = vm.allProductList[i].shortDescription;
                    vm.activeSalesOrder.salesOrderLines[productDetails.indexing - 1]
                        .itemPrice = vm.allProductList[i].unitPrice;
                    vm.activeSalesOrder.salesOrderLines[productDetails.indexing - 1].itemQuantity = 1;
                    vm.activeSalesOrder.salesOrderLines[productDetails.indexing - 1].discountType = 1;

                }
            }
            editproduct(orderList);
        }

        function removeRow(index, orderList) {
            salesOrderService.deleteSalesOrderLines(orderList.id);
            vm.activeSalesOrder.salesOrderLines.splice(index, 1);
            editproduct(vm.activeSalesOrder.salesOrderLines);
        }

        function pageChanged(page) {
            $scope.selectAll = [];
            vm.currentPage = page;
            searchParamChanged();
        }

        //function getPageSize(pageSize) {
        //    vm.pageSize = pageSize;
        //    if (vm.check === true) {
        //        getInactiveOrder();
        //    } else {
        //        loadAllActiveSalesOrder();
        //    }
        //}
    
        function updateSalesOrder(orderDetails) {
            //orderDetails.invoicedDate = moment(orderDetails.invoicedDate).format("MM-DD-YYYY");
            //if (orderDetails.salesOrderLines === null || orderDetails.salesOrderLines === undefined) {
            //    vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
            //    swal("Add Product to list.");
            //    return;
            //}
            //for (var i = 0; i < orderDetails.salesOrderLines.length; i++) {
            //        if (!orderDetails.salesOrderLines[i].productId< 0) {
            //            vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
            //            swal("Add Product to list.");
            //            return;
            //        }
            //    }
            if (orderDetails.id) {
                if (orderDetails.salesOrderLines === null || orderDetails.salesOrderLines === undefined || orderDetails.salesOrderLines.length === 0) {
                    vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                    yesNoDialog("Place Order first", "warning", "Please place any order to save else this information will be deleted.", "Yes, delete it!", "Your sales order has been deleted.", "delete", orderDetails);
                    return;
                } else {
                    for (var i = 0; i < orderDetails.salesOrderLines.length; i++) {
                        if (orderDetails.salesOrderLines[i].productId === "" || orderDetails.salesOrderLines.productId === null) {
                            vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                            yesNoDialog("Place Order first", "warning", "Please place any order to save else this information will be deleted.", "Yes, delete it!", "Your sales order has been deleted.", "delete", orderDetails);
                            return;
                        }

                    }
                }
                salesOrderService.updateSalesOrder(orderDetails)
                    .then(function (result) {
                        if (result.success) {
                            vm.salesForm.$setUntouched();
                            swal("Sales Order successfully updated.");
                            searchParamChanged();
                            $scope.showModal = false;
                        } else {
                            if (result.message.errors) {
                                vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                                swal(result.message.errors[0]);
                            }
                            else {
                                vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                                swal(result.message);
                            }
                        }

                    });
                }
            else {
                if (orderDetails.salesOrderLines === null || orderDetails.salesOrderLines === undefined || orderDetails.salesOrderLines.length === 0) {
                    vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                    swal("Please place your order first.");
                    return;
                } else {
                    for (var i = 0; i < orderDetails.salesOrderLines.length; i++) {
                        if (orderDetails.salesOrderLines[i].productId === "" || orderDetails.salesOrderLines.productId === null) {
                            vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");
                            //yesNoDialog("Place Order first", "warning", "Please place any order to save else this information will be deleted.", "Yes, delete it!", "Your sales order has been deactivated.", "delete", orderDetails);
                            swal("Please place your order first.");
                            return;
                        }

                    }
                }
                salesOrderService.createSalesOrder(orderDetails).then(function (result) {
                    if (result.success) {
                        vm.salesForm.$setUntouched();
                        searchParamChanged();
                        swal("Sales Order successfully created.");
                        $scope.showModal = false;

                    }
                    else {
                        if (result.message.errors) {
                            sweetAlert(result.message.errors[0]);
                            vm.activeSalesOrder.invoicedDate = moment(orderDetails.invoicedDate).format("YYYY-MM-DD");

                        }
                        else {
                            swal(result.message);
                        }
                    }

                });
            }
        }

        //$scope.getDueDateByTermIdAndDate=  function(termId, date) {
        //    if (termId != null && date != undefined) {
        //        var obj = {
        //            dateTime: date,
        //            termId: termId
        //        };
        //        salesOrderService.getDueDateByTermId(obj).then(function (result) {
        //            if (result.success) {
        //                vm.activeSalesOrder.dueDate = moment(result.data).format("YYYY-MM-DD");
        //            }
        //        });
        //    }
        //}

        $scope.onTimeSet = function (newDate, termId) {
            if (termId != null && newDate != undefined) {
                var obj = {
                    dateTime: moment(newDate).format("MM-DD-YYYY"),
                    termId: termId
                };
                salesOrderService.getDueDateByTermId(obj).then(function (result) {
                    if (result.success) {
                        vm.activeSalesOrder.dueDate = moment(result.data).format("YYYY-MM-DD");
                    }
                });
            }         
        }
        function loadDefaultValues() {
            // get default payment method assigned

            salesOrderService.getDefaultCompanyWebSetting().then(function (result) {
                if (result.success) {
                    if (result.data.paymentMethodId != 0) {
                        vm.activeSalesOrder.paymentMethodId = result.data.paymentMethodId;
                    }

                    if (result.data.deliveryMethodId != 0) {
                        vm.activeSalesOrder.deliveryMethodId = result.data.deliveryMethodId;
                    }
                }
            });
        }
        function getSalesOrderPageSize() {
            salesOrderService.getPageSize().then(function (res) {
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
                        deleteSalesOrder(successMessage, value);
                    }
                  
                });
        }
        function searchParamChanged() {
            if (vm.searchText == undefined)
                vm.searchText = "";
            if (vm.totalSalesOrder < vm.pageSize)
                vm.currentPage = 1;
            salesOrderService.searchTextForSalesOrder(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.salesOrder = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.totalSalesOrder = result.data.totalRecords;
                    }
                });
        }

    }

})();