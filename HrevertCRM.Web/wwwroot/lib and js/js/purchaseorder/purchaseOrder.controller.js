(function () {
    angular.module("app-purchaseOrder")

        .controller("purchaseOrderController", purchaseOrderController);
    purchaseOrderController.$inject = ['$http', '$filter', '$scope', '$element', 'purchaseOrderService'];

    function purchaseOrderController($http, $filter, $scope, $element,purchaseOrderService) {
        var vm = this;
        //vm.searchTextChanged = searchTextChanged;
       // vm.checkInactive = checkInactive;
        //vm.getPageSize = getPageSize;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.currentPage = 1;
        vm.editPurchaseOrder = editPurchaseOrder;
        vm.methodActionChanged = methodActionChanged;
       // vm.activatePurchaseOrder = activatePurchaseOrder;
        vm.clearDialog = clearDialog;
        vm.savePurchaseOrder = savePurchaseOrder;
        vm.allPurchaseOrders = [];
        vm.pageChanged = pageChanged;
        vm.productNameSelect = productNameSelect;
        vm.editproduct = editproduct;
        vm.removeRow = removeRow;
        vm.addProductDetails = addProductDetails;
        vm.selectUser = selectUser;
        vm.taxRateItems = [];
      //  vm.searchTextChanged = searchTextChanged;
        vm.purchaseSalesOrderNumber = false;
        vm.activePurchaseOrder = {};
        vm.activePurchaseOrder.purchaseOrderLines = [];
        vm.showPurchaseCode = false;
        vm.check = false;
        vm.totalPurchaseOrder = 0;
      //  vm.getDueDateByTermIdAndDate = getDueDateByTermIdAndDate;
        vm.searchParamChanged = searchParamChanged;
        getPurchaseOrderPageSize();
        vm.showFullyPaid = showFullyPaid;
        vm.disableAllFields = false;
        vm.showPurchaseOrderCode = false;
        vm.getPurchaseOrderTypes = getPurchaseOrderTypes;
        vm.purchaseOrderBtnText = "Save";
        // loading required for the first time
        init();

        function getPurchaseOrderTypes() {

            purchaseOrderService.getPurchaseOrderTypes().then(function (result) {
                if (result.success) {
                    vm.purcaseOrderTypes = result.data;
                }
            });
        }

        function showFullyPaid(ordertype) {
            if (ordertype == 3) {
                vm.showCheckbox = true;
            } else {
                vm.showCheckbox = false;
            }
        }

        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;
        vm.disableInvoicedDate = false;
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
                angular.forEach(vm.allPurchaseOrders, function (check) {
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
            purchaseOrderService.deletedSelected(selectedItemid).then(function (result) {
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

        $scope.open = function () {
            vm.activePurchaseOrder = {};
            vm.activePurchaseOrder.purchaseOrderLines = [];
            vm.total = 0;
            vm.totalVatAmount = 0;
            vm.showCheckbox = false;
            $scope.disableRemoveIcon = false;
            vm.disableAllFields = false;
            vm.purchaseSalesOrderNumber = false;
            vm.showPurchaseOrderCode = false;
            vm.showPurchaseCode = false;
            addProductDetails(vm.activePurchaseOrder);
            loadDefaultValues();
            $scope.showButton = true;
            $scope.showModal = true;
            vm.disableInvoicedDate = false;
            vm.purchaseOrderBtnText = "Save";
            vm.purchaseOrderForm.$setUntouched();
            getPurchaseOrderTypes();
        }

        $scope.hide = function () {
            vm.purchaseSalesOrderNumber = false;
            vm.showPurchaseCode = false;
            vm.activePurchaseOrder = {};
            vm.activePurchaseOrder.purchaseOrderLines = [];
            $scope.showModal = false;
        }

        function init() {

            // loading Active Vendors
            
          purchaseOrderService.getAllActiveVendorsWithoutPaging().then(function (result) {
              if (result.success) {
                  vm.allVendors = result.data;
              }
          }, function (message) {
                console.log(message);
            });
          
            //loading Active Payment Term
            purchaseOrderService.getPaymentTerm().then(function (result) {
                if (result.success) {
                    vm.allTerms = result.data;
                }

            });

            // loading Active Delivery Methods
            purchaseOrderService.getAllActiveDeliveryMethods().then(function (result) {
                if (result.success) {
                    vm.allDeliveryMethod = result.data;
                }
            });

            // loading All Active Company
            purchaseOrderService.getCompanyUserWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allCompanyUserList = result.data;
                }
            });

            // loading All Active Product
            purchaseOrderService.getAllActiveProductWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allProductList = result.data;
                }
            });


            // for all tax data loading
          
            purchaseOrderService.getActiveTaxWithoutPaging().then(function (result) {
                if (result.success) {
                    vm.allTax = result.data;
                }

            });

          // loading Discount types

            purchaseOrderService.getAllDiscountTypes().then(function (result) {
                if (result.success) {
                    vm.discountTypeOption = result.data;
                }

            });

            // loading purchase order types
            getPurchaseOrderTypes();

            // loading purchase order status

            purchaseOrderService.getPurchaseOrderStatuses().then(function (result) {
                if (result.success) {
                    vm.purchaseOrderStatus = result.data;
                }
            });
        }

        function pageChanged(page) {
            $scope.selectAll = [];
            vm.currentPage = page;
          searchParamChanged();
        }

        //function loadAllActivePurchaseOrder() {
        //  purchaseOrderService.getActivePurchaseOrder(vm.currentPage, vm.pageSize).then(function (result) {
        //      if (result.success) {
        //          vm.allPurchaseOrders = result.data.items;
        //          vm.pageNumber = result.data.pageNumber;
        //          vm.totalPage = result.data.pageCount;
        //      }
        //    }, function (message) {
        //        console.log(message);
        //    });
        //}

        //function loadAllPurchaseOrder() {
        //  purchaseOrderService.getAllPurchaseOrder(vm.currentPage, vm.pageSize).then(function (result) {
        //      if (result.success) {
        //          vm.allPurchaseOrders = result.data.items;
        //          vm.pageNumber = result.data.pageNumber;
        //          vm.totalPage = result.data.pageCount;
        //      }
        //    }, function (message) {
        //        console.log(message);
        //    });
        //}
  
      //  function searchForText(text, checked) {
      //    purchaseOrderService.searchText(vm.currentPage, vm.pageSize, text, checked)
      //     .then(function (result) {
      //         if (result.success) {
      //             vm.allPurchaseOrders = result.data.items;
      //             vm.pageNumber = result.data.pageNumber;
      //             vm.totalPage = result.data.pageCount;
      //         }

      //     });
      //}

      //  function searchTextChanged(text, checked) {
      //    if (checked === true) {
             
      //       if (text === "") {
      //           searchParamChanged();
      //       } else {
      //           searchForText(text, checked);
      //       }
      //    } else {
      //        if (text === "") {
      //            searchParamChanged();
      //       } else {
      //           searchForText(text, checked);
      //       }
      //   }
      //}
             
      //  function checkInactive(active, checked) {
      //    if (checked) {
      //        loadAllPurchaseOrder();
        //        vm.check = true;

      //    }
      //    else {
      //        vm.check = false;
      //        searchParamChanged();
      //    }
      //}

      //  function getPageSize(pageSize) {
      //    vm.pageSize = pageSize;
      //    if (vm.check === true) {
      //        searchParamChanged();
      //    } else {
      //        searchParamChanged();
      //    }
         
      //}

        function editPurchaseOrder(purchaseOrder) {
            $scope.showModal = true;
            vm.disableInvoicedDate = true;
            vm.showPurchaseOrderCode = true;
            if (purchaseOrder.orderType === 3) {
                vm.showCheckbox = true;
                vm.disableAllFields = true;
                $scope.disableRemoveIcon = true;
                if (purchaseOrder.fullyPaid === false) {
                    $scope.showButton = true;
                    vm.disableFullyPaid = false;
                    if (purchaseOrder.active) {
                        purchaseOrderService.getPurchaseOrderById(purchaseOrder.id).then(function (result) {
                            if (result.success) {
                                vm.activePurchaseOrder = result.data;
                                vm.activePurchaseOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                                vm.activePurchaseOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                                vm.activePurchaseOrder.billingAddressId = result.data.billingAddressId;
                                vm.activePurchaseOrder.shippingAddressId = result.data.shippingAddressId;
                                vm.showPurchaseCode = true;
                                vm.purchaseSalesOrderNumber = true;
                                selectUser(purchaseOrder.vendorId);
                                purchaseOrderService.getActiveTax(vm.currentPage, vm.pageSize).then(function (result) {
                                    if (result.success) {
                                        vm.allTax = result.data;
                                        vm.total = 0;
                                        vm.totalVatAmount = 0;
                                        saveTaxRate(vm.activePurchaseOrder.purchaseOrderLines);
                                        if (vm.activePurchaseOrder.purchaseOrderLines === null) {
                                            vm.activePurchaseOrder.purchaseOrderLines = [];
                                            addProductDetails(vm.activePurchaseOrder);
                                        }
                                        else {
                                            editproduct(vm.activePurchaseOrder.purchaseOrderLines);
                                            
                                        }
                                    }
                                });
                               
                            }

                        }, function (message) {
                            console.log(message);
                        });
                    }

                } else {
                    $scope.showButton = false;
                    vm.disableFullyPaid = true;
                    if (purchaseOrder.active) {
                        purchaseOrderService.getPurchaseOrderById(purchaseOrder.id).then(function (result) {
                            if (result.success) {
                                vm.activePurchaseOrder = result.data;
                                vm.activePurchaseOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                                vm.activePurchaseOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                                vm.activePurchaseOrder.billingAddressId = result.data.billingAddressId;
                                vm.activePurchaseOrder.shippingAddressId = result.data.shippingAddressId;
                                vm.showPurchaseCode = true;
                                vm.purchaseSalesOrderNumber = true;
                                selectUser(purchaseOrder.vendorId);
                                purchaseOrderService.getActiveTax(vm.currentPage, vm.pageSize).then(function (result) {
                                    if (result.success) {
                                        vm.allTax = result.data;
                                        vm.total = 0;
                                        vm.totalVatAmount = 0;
                                        saveTaxRate(vm.activePurchaseOrder.purchaseOrderLines);
                                        if (vm.activePurchaseOrder.purchaseOrderLines === null) {
                                            vm.activePurchaseOrder.purchaseOrderLines = [];
                                            addProductDetails(vm.activePurchaseOrder);
                                        }
                                        else {
                                            editproduct(vm.activePurchaseOrder.purchaseOrderLines);

                                        }
                                    }
                                });
                                
                            }

                        }, function (message) {
                            console.log(message);
                        });
                    }

                } S

            } else {
                vm.disableAllFields = false;
                $scope.showButton = true;
                $scope.disableRemoveIcon = false;
                vm.showCheckbox = false;
                if (purchaseOrder.active) {
                    purchaseOrderService.getPurchaseOrderById(purchaseOrder.id).then(function (result) {
                        if (result.success) {
                            vm.activePurchaseOrder = result.data;
                            vm.activePurchaseOrder.dueDate = moment(result.data.dueDate).format("YYYY-MM-DD");
                            vm.activePurchaseOrder.invoicedDate = moment(result.data.invoicedDate).format("YYYY-MM-DD");
                            vm.activePurchaseOrder.billingAddressId = result.data.billingAddressId;
                            vm.activePurchaseOrder.shippingAddressId = result.data.shippingAddressId;
                            vm.showPurchaseCode = true;
                            vm.purchaseSalesOrderNumber = true;
                            selectUser(purchaseOrder.vendorId);
                            purchaseOrderService.getActiveTax(vm.currentPage, vm.pageSize).then(function (result) {
                                if (result.success) {
                                    vm.allTax = result.data;
                                    vm.total = 0;
                                    vm.totalVatAmount = 0;
                                    saveTaxRate(vm.activePurchaseOrder.purchaseOrderLines);
                                    if (vm.activePurchaseOrder.purchaseOrderLines === null) {
                                        vm.activePurchaseOrder.purchaseOrderLines = [];
                                        addProductDetails(vm.activePurchaseOrder);
                                    }
                                    else {
                                        editproduct(vm.activePurchaseOrder.purchaseOrderLines);
                                        //for (var i = 0; i < vm.activePurchaseOrder.purchaseOrderLines.length; i++) {
                                        //    vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].itemPrice * vm.activePurchaseOrder.purchaseOrderLines[i].itemQuantity;
                                        //    if (Number(vm.activePurchaseOrder.purchaseOrderLines[i].discountType) === 1) {
                                        //        vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].amount - (vm.activePurchaseOrder.purchaseOrderLines[i].itemPrice * vm.activePurchaseOrder.purchaseOrderLines[i].itemQuantity) * vm.activePurchaseOrder.purchaseOrderLines[i].discount / 100;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    }
                                        //    else if (Number(vm.activePurchaseOrder.purchaseOrderLines[i].discountType) === 2) {
                                        //        vm.activePurchaseOrder.purchaseOrderLines[i].amount = (vm.activePurchaseOrder.purchaseOrderLines[i].itemPrice * vm.activePurchaseOrder.purchaseOrderLines[i].itemQuantity) - vm.activePurchaseOrder.purchaseOrderLines[i].discount;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    }
                                        //    else if (Number(vm.activePurchaseOrder.purchaseOrderLines[i].discountType) === 3) {
                                        //        vm.activePurchaseOrder.purchaseOrderLines[i].discount = "";
                                        //        vm.activePurchaseOrder.purchaseOrderLines[i].amount = (vm.activePurchaseOrder.purchaseOrderLines[i].itemPrice * vm.activePurchaseOrder.purchaseOrderLines[i].itemQuantity) - 0;
                                        //        vm.totalVatAmount = vm.totalVatAmount +
                                        //            vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
                                        //    }
                                        //    vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
                                        //}
                                    }
                                }
                            });
                            // vm.purchaseOrderForm.$invalid = true;
                          
                        }

                    }, function (message) {
                        console.log(message);
                    });
                }
                else {
                    swal("You cann't edit this item.please activate it first.");
                }
            }
      }

        function methodActionChanged(data, id) {
          if (Number(id) === 1) {
              vm.purchaseSalesOrderNumber = "true";
              vm.purchaseOrderBtnText = "Update";
              editPurchaseOrder(data);
          }
          else if (Number(id) === 2) {
              yesNoDialog("Are you sure?", "warning", "Purchase Order will be deleted permanently.", "Yes, delete it!", "Your order has been deleted.", "delete", data);

          }
          else if (Number(id) === 3) {
              alert("Create report not working");
          }

          vm.action = null;

      }

      //  function activatePurchaseOrder(purchaseOrder) {
      //    purchaseOrderService.activePurchaseOrderById(purchaseOrder.id).then(function (result) {
      //        if (result.success) {
      //            swal("Purchase Order Activated.")
      //            searchParamChanged();
      //        }
      //    }, function (message) {
      //        console.log(message);
      //    });
      //}

        function savePurchaseOrder(purchaseOrder) {
            purchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("MM-DD-YYYY");
            if (purchaseOrder.id) {
                if (purchaseOrder.purchaseOrderLines === null || purchaseOrder.purchaseOrderLines === undefined || purchaseOrder.purchaseOrderLines.length === 0) {
                    vm.activePurchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                    yesNoDialog("Place Order first", "warning", "Please place any order to save else this information will be deleted.", "Yes, delete it!", "Your order has been deleted.", "delete", purchaseOrder);
                    return;
                } else {
                    for (var i = 0; i < purchaseOrder.purchaseOrderLines.length; i++) {
                        if (purchaseOrder.purchaseOrderLines[i].productId === "" || purchaseOrder.purchaseOrderLines[i].productId === null) {
                            purchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                            yesNoDialog("Place Order first", "warning", "Please place any order to save else this information will be deleted.", "Yes, delete it!", "Your order has been deleted.", "delete", purchaseOrder);
                            return;
                        }
                    }
                }
              purchaseOrderService.updatePurchaseOrder(purchaseOrder).then(function (result) {
                      if (result.success) {
                          searchParamChanged();
                          $scope.showModal = false;
                      } else {
                          if (result.message.errors) {
                              vm.activePurchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                              swal(result.message.errors[0]);
                          }
                          else {
                              vm.activePurchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                              swal(result.message);
                          }
                      }
                  },
                      function (error) {
                          swal(error.message);
                      });
          }
            else {
                if (purchaseOrder.purchaseOrderLines === null || purchaseOrder.purchaseOrderLines === undefined || purchaseOrder.purchaseOrderLines.length === 0) {
                    purchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                    swal("Please place your order first.");
                    return;
                } else
                {
                    for (var i = 0; i < purchaseOrder.purchaseOrderLines.length; i++) {
                        if (purchaseOrder.purchaseOrderLines[i].productId === "" || purchaseOrder.purchaseOrderLines[i].productId === null) {
                            purchaseOrder.invoicedDate = moment(purchaseOrder.invoicedDate).format("YYYY-MM-DD");
                            swal("Please place your order first.");
                            return;
                        }
                    }
                }
              purchaseOrderService.createPurchaseOrder(purchaseOrder).then(function (result) {
                  if (result.success) {
                      swal("Purchase Order successfully saved.");
                      
                      searchParamChanged();
                      $scope.showModal = false;
                  }
                  else {
                      if (result.message.errors) {
                          swal(result.message.errors[0]);
                      }
                      else
                      {
                          swal(result.message);
                      }   
                  }
                     

              }, function (error) {
                  swal(error.message);
                      });
          }
      }

        function deletePurchaseOrder(successMessage, data) {
          purchaseOrderService.deletePurchaseOrder(data.id).then(function (result) {
              if (result.success) {
                  swal(successMessage,"successfully deleted.");
                  searchParamChanged();
                  $scope.showModal = false;
                  //}
              }
             
          }, function (message) {
              console.log(message);
          });
      }
    
        function clearDialog()
      {
          vm.activePurchaseOrder = null;
      }

        // need to make some changes for modifications
        function productNameSelect(productDetails, orderList) {
          for (var i = 0; i < vm.allProductList.length; i++) {
              if (vm.allProductList[i].id === productDetails.productId) {

                  vm.activePurchaseOrder.purchaseOrderLines[productDetails.indexing - 1]
                      .description = vm.allProductList[i].shortDescription;
                  vm.activePurchaseOrder.purchaseOrderLines[productDetails.indexing - 1]
                      .itemPrice = vm.allProductList[i].unitPrice;
                  vm.activePurchaseOrder.purchaseOrderLines[productDetails.indexing - 1].itemQuantity = 1;
                  vm.activePurchaseOrder.purchaseOrderLines[productDetails.indexing - 1].discountType = 1;
              }
          }
          editproduct(orderList);
      }


        function editproduct(orderList) {
            vm.total = 0;
            vm.totalVatAmount = 0;
            saveTaxRate(orderList);

            for (var i = 0; i < orderList.length; i++) {
                vm.activePurchaseOrder.purchaseOrderLines[i].amount = orderList[i].itemPrice * orderList[i].itemQuantity;

                if (Number(orderList[i].discountType) === 1) {
                    vm.activePurchaseOrder.purchaseOrderLines[i].disableDiscountBox = true;
                    vm.activePurchaseOrder.purchaseOrderLines[i].discount = 0;
                    if (vm.taxRateItems.length === 0) {
                        vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].amount - (orderList[i].itemPrice * orderList[i].itemQuantity) * orderList[i].discount / 100;
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (totalAmount + (totalAmount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = orderList[i].itemPrice * orderList[i].itemQuantity;
                        }
                    }

                } else if (Number(orderList[i].discountType) === 2) {
                    vm.activePurchaseOrder.purchaseOrderLines[i].disableDiscountBox = false;
                    if (vm.taxRateItems.length === 0) {
                        vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].amount - (orderList[i].itemPrice * orderList[i].itemQuantity) * orderList[i].discount / 100;
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            var totalAfterDiscount = totalAmount - (totalAmount * Number(orderList[i].discount) / 100);
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (totalAfterDiscount + (totalAfterDiscount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = totalAmount - (totalAmount * Number(orderList[i].discount) / 100);
                        }

                    }

                } else if (Number(orderList[i].discountType) === 3) {
                    vm.activePurchaseOrder.purchaseOrderLines[i].disableDiscountBox = false;

                    if (vm.taxRateItems.length === 0) {
                    } else {
                        if (vm.taxRateItems[i]) {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            var totalAfterDiscount = totalAmount - Number(orderList[i].discount);
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (totalAfterDiscount + (totalAfterDiscount * vm.taxRateItems[i].rate) / 100);
                        }
                        else {
                            var totalAmount = orderList[i].itemPrice * orderList[i].itemQuantity;
                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = totalAmount - Number(orderList[i].discount);
                        }

                    }

                }
            }
            for (var j = 0; j < vm.activePurchaseOrder.purchaseOrderLines.length; j++) {
                vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[j].amount;
            }
        }





      //  function editproduct(purchaseOrderList) {
      //    purchaseOrderService.getActiveTaxWithoutPaging().then(function (result) {
      //            if (result.success) {
      //                vm.allTax = result.data;
      //                vm.total = 0;
      //                vm.totalVatAmount = 0;
      //                saveTaxRate(purchaseOrderList);
      //                for (var i = 0; i < purchaseOrderList.length; i++) {
      //                    vm.activePurchaseOrder.purchaseOrderLines[i].amount = purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity;
      //                    if (Number(purchaseOrderList[i].discountType) === 1) {
      //                        vm.activePurchaseOrder.purchaseOrderLines[i].discount = 0;
      //                        if (vm.taxRateItems.length === 0) {
      //                            vm.total = 0;
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].amount - (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity) * purchaseOrderList[i].discount / 100;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                        } else {
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = vm.activePurchaseOrder.purchaseOrderLines[i].amount - (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity) * purchaseOrderList[i].discount / 100;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                            vm.totalVatAmount = vm.totalVatAmount +
      //                                vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
      //                        }
      //                    } else if (Number(purchaseOrderList[i].discountType) === 2) {
      //                        if (vm.taxRateItems.length === 0) {
      //                            vm.total = 0;
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity) - purchaseOrderList[i].discount;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount ;
      //                        } else {
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity) - purchaseOrderList[i].discount;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                            vm.totalVatAmount = vm.totalVatAmount +
      //                                vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
      //                        }
      //                    } 
      //                    if (Number(purchaseOrderList[i].discountType) === 3) {
      //                        // no need
      //                        if (vm.taxRateItems.length === 0) {
      //                            vm.total = 0;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                        } else {
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity);
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                            vm.totalVatAmount = vm.totalVatAmount +
      //                                vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
      //                        }
      //                    }
      //                    else {
      //                        if (vm.taxRateItems.length === 0) {
      //                            vm.total = 0;
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                        } else {
      //                            vm.activePurchaseOrder.purchaseOrderLines[i].amount = (purchaseOrderList[i].itemPrice * purchaseOrderList[i].itemQuantity);
      //                            vm.total = vm.total + vm.activePurchaseOrder.purchaseOrderLines[i].amount;
      //                            vm.totalVatAmount = vm.totalVatAmount +
      //                                vm.activePurchaseOrder.purchaseOrderLines[i].amount * vm.taxRateItems[i].rate / 100;
      //                        }
      //                    }
      //                }
      //            }
      //        });
      //}

        function saveTaxRate(purchaseOrder) {
            vm.taxRateItems = [];
            for (var i = 0; i < purchaseOrder.length; i++) {
                for (var j = 0; j < vm.allTax.length; j++) {
                    if (Number(purchaseOrder[i].taxId) === vm.allTax[j].id) {
                        vm.taxRateItems.push({
                            id: purchaseOrder[i].taxId,
                            rate: vm.allTax[j].taxRate,
                            index: purchaseOrder[i].lineOrder
                        });
                    }
                }

            }

        }

      //  function saveTaxRate(purchaseOrder) {
      //    vm.taxRateItems = [];
      //    if (purchaseOrder === null) return;
      //    for (var i = 0; i<purchaseOrder.length; i++) {
      //            for (var j = 0; j < vm.allTax.length; j++) {
      //                if (Number(purchaseOrder[i].taxId) === vm.allTax[j].id) {
      //                    vm.taxRateItems.push({
      //                        id: purchaseOrder[i].taxId,
      //                        rate: vm.allTax[j].taxRate,
      //                        index: purchaseOrder[i].lineOrder
      //                    });
      //                }
      //            }
      //        }
      //}

        function removeRow(index, orderList) {
          purchaseOrderService.deletePurchaseOrderLines(orderList.id);
          vm.activePurchaseOrder.purchaseOrderLines.splice(index, 1);
          editproduct(vm.activePurchaseOrder.purchaseOrderLines);
         
      }  
    
        function addProductDetails(order) {
            if (order.purchaseOrderLines === null || order.purchaseOrderLines === []) {
              vm.activePurchaseOrder.purchaseOrderLines = [];
          }
         vm.activePurchaseOrder.purchaseOrderLines.push({ productId: "", itemPrice: "", itemQuantity: "" });
      }
 
        function selectUser(userId) {
            if (userId != null || userId != undefined) {
                purchaseOrderService.getDefaultValues(userId).then(function (result) {
                    if (result.success) {
                        vm.userBillingAddresses = result.data.billingAddresses;
                        vm.userShippingAddresses = result.data.shippingAddresses;
                        vm.activePurchaseOrder.billingAddressId = result.data.billingAddressId;
                        vm.activePurchaseOrder.shippingAddressId = result.data.shippingAddressId;
                        // vm.activePurchaseOrder.paymentTermId = result.data.paymentTermsId;
                        vm.activePurchaseOrder.email = result.data.billingAddresses[0].email;
                    }
                });
            }
            else {
                vm.userBillingAddresses = "";
                vm.userShippingAddresses = "";
                vm.activePurchaseOrder.billingAddressId = "";
                vm.activePurchaseOrder.shippingAddressId = "";
                // vm.activePurchaseOrder.paymentTermId = result.data.paymentTermsId;
                vm.activePurchaseOrder.email = "";
                swal("Please select vendor");
            }
        
      }

        //function getDueDateByTermIdAndDate(termId,date) {
        //  if (termId != null && date != undefined) {
        //      var obj = {
        //          dateTime: date,
        //          termId: termId
        //      };
        //      purchaseOrderService.getDueDateByTermId(obj).then(function (result) {
        //          if (result.success) {
        //              vm.activePurchaseOrder.dueDate = moment(result.data).format("YYYY-MM-DD");
        //          }
        //      });
        //  }
        //}
        $scope.onTimeSet = function (newDate, termId) {
            if (termId != null && newDate != undefined) {
                      var obj = {
                          dateTime: moment(newDate).format("MM-DD-YYYY"),
                          termId: termId
                      };
                      purchaseOrderService.getDueDateByTermId(obj).then(function (result) {
                          if (result.success) {
                              vm.activePurchaseOrder.dueDate = moment(result.data).format("YYYY-MM-DD");
                          }
                      });
                  }
        }
        function loadDefaultValues() {
            // get default delivery method assigned

            purchaseOrderService.getDefaultCompanyWebSetting().then(function (result) {
                if (result.success) {
                    if (result.data.deliveryMethodId != 0) {
                        vm.activePurchaseOrder.deliveryMethodId = result.data.deliveryMethodId;
                    }
                }
            });
        }
        function getPurchaseOrderPageSize() {
            purchaseOrderService.getPageSize().then(function (res) {
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
            if (vm.totalPurchaseOrder < vm.pageSize)
                vm.currentPage = 1;
            purchaseOrderService.searchTextForPurchaseOrder(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.allPurchaseOrders = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                        vm.totalPurchaseOrder = result.data.totalRecords;
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
                        deletePurchaseOrder(successMessage, value);
                    }
                });
        }
    }
})();