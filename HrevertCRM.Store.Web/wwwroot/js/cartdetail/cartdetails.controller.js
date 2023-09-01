(function () {
    angular.module("app-cartdetails")
        .filter('filterTreeItem', function(){
            function recursive(obj, newObj, level, itemId, isExpend) {
                console.log(obj, newObj, level, itemId, isExpend)
                angular.forEach(obj, function (o) {
                    if(o.children && o.children.length !=0){
                        o.level = level;
                        o.leaf = false;
                        newObj.push(o);
                        if(o.id == itemId) {
                            o.expend = isExpend;
                        }
                        if(o.expend == true) {
                            recursive(o.children, newObj, o.level + 1, itemId, isExpend);
                        }
                    } else {
                        o.level = level;
                        o.leaf = true;
                        newObj.push(o);
                        return false;
                    }
                });
            }

            return function (obj, itemId, isExpend) {
                var newObj = [];
                recursive(obj, newObj, 0, itemId, isExpend);
                return newObj;
            }
        })
        .directive("owlCarousel", function () {
                    return {
                        restrict: 'E',
                        transclude: false,
                        link: function (scope) {
                            scope.initCarousel = function (element) {
                                // provide any default options you want
                                var defaultOptions = {
                                };
                                var customOptions = scope.$eval($(element).attr('data-options'));
                                // combine the two options objects
                                for (var key in customOptions) {
                                    defaultOptions[key] = customOptions[key];
                                }
                                // init carousel
                                $(element).owlCarousel(defaultOptions);
                            };
                        }
                    };
                })
.directive('owlCarouselItem', [function () {
    return {
        restrict: 'A',
        transclude: false,
        link: function (scope, element) {
            // wait for the last item in the ng-repeat then call init
            if (scope.$last) {
                scope.initCarousel(element.parent());
            }
        }
    }
}])
    .controller("cartdetailsController", cartdetailsController);
    cartdetailsController.$inject = ['$scope', '$http', '$filter', '$location', '$window', '$cookies', 'cartdetailsService', 'viewModelHelper'];
    function cartdetailsController($scope, $http, $filter, $location, $window, $cookies, cartdetailsService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.removeItem = removeItem;
        vm.increaseQuantity = increaseQuantity;
        vm.decreaseQuantity = decreaseQuantity;
        vm.proceesToCheckout = proceesToCheckout;
        vm.changeQuantity = changeQuantity;
        vm.addItemToCart = addItemToCart;
        vm.grandTotal = 0;
        vm.total = 0;
        vm.totalDiscount = 0;
        vm.totalTax = 0;
        vm.resultMessage = 'Loading Items please wait ... ';

        function changeQuantity(item, number) {
            cartdetailsService.getItemsById(Number(vm.loginUser), item.id).then(function (result) {
                if (result.success) {
                    if (result.data.allowBackOrder) {
                        item.quantity = number;
                        updateCartitem(item);
                    }
                    else {
                        if (item.quantity > result.data.quantityOnHand) {
                            item.quantity = item.oldQuantity;
                            swal({
                                    title: 'There is only ' + result.data.quantityOnHand + ' items remaining in our stock.',
                                //timer: 2000,
                                    showConfirmButton: true
                            });
                        }
                        else {
                            item.quantity = number;
                            updateCartitem(item);
                    }
                }

            }
            });

        }
        getlatestProduct(1, 10);
        function getlatestProduct(pageNumber, pageSize) {
            cartdetailsService.getLatestProduct(pageNumber, pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.latestProduct = result.data.items;
                    } else {
                        var message = {};
                        message.message = "get latest product, " + result.data + " in item detail.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }
        function increaseQuantity(item) {
            cartdetailsService.getItemsById(Number(vm.loginUser), item.id).then(function (result) {
                if (result.success) {
                    if (result.data.allowBackOrder) {
                        item.quantity = item.quantity +1;
                        updateCartitem(item);
                    }
                    else {
                        if (item.quantity + 1 > result.data.quantityOnHand) {
                            swal({
                                    title: 'There is only ' + result.data.quantityOnHand + ' items remaining in our stock.',
                                //timer: 2000,
                                    showConfirmButton: true
                            });
                        }
                        else {
                            item.quantity = item.quantity +1;
                            updateCartitem(item);
                    }
                }

            }
            });

    }

        function proceesToCheckout(cartId) {
            if (cartId === undefined) {
                swal("Please place the order first.");
            } else {
                $cookies.put('cartId', cartId);
                $window.location.href = '/checkout';
                $window.location.target = '_self';
        }

    }

        function decreaseQuantity(item) {
            if (item.quantity == 1) { return; }
            else {
                item.quantity = item.quantity -1;
                updateCartitem(item);
        }
    }

        function updateCartitem(item) {
            vm.class = 'loader loader-default is-active';
            cartdetailsService.updateCartItem(item)
                .then(function (result) {
                    if (result.success) {
                        init();
                        viewModelHelper.refreshCart();
                    } else {
                        var message = {
                    };
                        message.message = "Update cart item, " + result.message + " Cart Detail.,";
                        viewModelHelper.bugReport(message,
                          function (result) { 
                        });
                }
            });
    }

        init();
        function init() {
            cartdetailsService.getLoginUser()
                .then(function (result) {
                    if (result.data > 0) {
                        vm.loginUser = result.data;
                    } else {
                        vm.loginUser = 0;
                }
                    cartdetailsService.getCartItems($cookies.get('cartId'), vm.loginUser, $cookies.get('guid'))
                    .then(function (result) {
                        if (result.success) {
                            if (result.data.id) {
                                vm.cartItems = result.data;
                                for (var j = 0; j < vm.cartItems.shoppingCartDetails.length; j++) {
                                    vm.cartItems.shoppingCartDetails[j].oldQuantity = vm.cartItems.shoppingCartDetails[j].quantity;
                                    vm.cartItems.shoppingCartDetails[j].totalCostWithTax = 0;
                                    var totaltax = 0;
                                    if (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit) {
                                        for (var k = 0; k < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit.length; k++) {

                                            for (var l = 0; l < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k].taxAndAmounts.length; l++) {
                                                totaltax += vm.cartItems.shoppingCartDetails[j]
                                                    .productsRefByAssembledAndKit[k].taxAndAmounts[l].taxAmount;
                                        }
                                            var total = (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .quantity *
                                                vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .productCost) +totaltax;

                                            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost = 0;
                                            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost += total;

                                            vm.cartItems.shoppingCartDetails[j].totalCostWithTax += vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost;
                                    }
                                    } else {
                                        var total = (vm.cartItems.shoppingCartDetails[j].quantity *
                                                vm.cartItems.shoppingCartDetails[j]
                                                .productCost) +totaltax;
                                        vm.cartItems.shoppingCartDetails[j].totalCostWithTax += total;
                                }

                            }
                                //vm.cartItems = [];
                                //for (var j = 0; j < result.data.shoppingCartDetails.length; j++) {
                                //    vm.cartItems.push(result.data.shoppingCartDetails[j]);
                                //    if (result.data.shoppingCartDetails[j].productsRefByAssembledAndKit) {
                                //        for (var k = 0; k < result.data.shoppingCartDetails[j].productsRefByAssembledAndKit.length; k++) {
                                //            vm.cartItems
                                //                .push(result.data.shoppingCartDetails[j]
                                //                    .productsRefByAssembledAndKit[k]);
                                //        }
                                //    }

                                //}
                                vm.totalDiscount = 0;
                                vm.totalTax = 0;
                                for (var i = 0; i < vm.cartItems.shoppingCartDetails.length; i++) {
                                    vm.totalDiscount = vm.totalDiscount + vm.cartItems.shoppingCartDetails[i].discount;
                                    vm.totalTax = vm.totalDiscount + vm.cartItems.shoppingCartDetails[i].taxAmount;
                                    vm.total = vm.total +
                                       (vm.cartItems.shoppingCartDetails[i].quantity *
                                           vm.cartItems.shoppingCartDetails[i].productCost);

                            }

                                //for (var i = 0; i < vm.cartItems.length; i++) {
                                //    vm.totalDiscount = vm.totalDiscount + vm.cartItems[i].discount;
                                //    vm.totalTax = vm.totalDiscount + vm.cartItems[i].taxAmount;
                                //    vm.total = vm.total +
                                //       (vm.cartItems[i].quantity *
                                //           vm.cartItems[i].productCost);

                                //}
                            } else {
                                vm.resultMessage = result.data;
                        }
                            vm.class = 'loader loader-default';
                        } else {

                            var message = {
                        };
                            message.message = "Update cart item, " + result.message + " Cart Detail.,";
                            viewModelHelper.bugReport(message,
                              function (result) { 
                            });
                    }
                    });
            });
            viewModelHelper.apiGetLocal("/api/getcrmurl",
                null,
                function (result) {
                    if (result.data) {
                        vm.crmLocation = result.data;
                    } else {
                        var message = {
                    };
                        message.message = "Update cart item, " + result.message + " Cart Detail.,";
                        viewModelHelper.bugReport(message,
                          function (result) { 
                        });
                }
            });
        }

        function addItemToCart(product, quantity) {
            cartdetailsService.getItemsById(vm.loginUser, Number(product.id))
                .then(function (result) {
                    if (result.success) {
                        if (!result.data.allowBackOrder && result.data.quantityOnHand < quantity) {
                            messageDialogForCart('There is only ' + result.data.quantityOnHand + ' items remaining in our stock.');
                        }
                        else {

                            var cartId = $cookies.get('cartId');
                            if (cartId !== null) {
                                vm.cartId = cartId;
                            } else {
                                vm.cartId = "0";
                            }
                            vm.cartItem =
                                {
                                    ShoppingCartId: vm.cartId,
                                    CustomerId: vm.loginUser,
                                    Guid: $cookies.get('guid'),
                                    ProductId: product.id,
                                    ProductCost: product.unitPrice,
                                    Quantity: quantity,
                                    Discount: product.discountPrice,
                                    //TaxAmount: '100',
                                    ShoppingDateTime: new Date()

                                };
                            viewModelHelper.addtoCart(vm.cartItem,

                              function (result) {
                                  if (result.data.id) {
                                      vm.cartId = result.data.id;
                                      $cookies.put('cartId', vm.cartId);
                                      messageDialogForCart('Product added to cart');
                                      viewModelHelper.refreshCart();
                                  } else {
                                      var message = {};
                                      message.message = "add to cart, " + result.data + " in item detail.,";
                                      viewModelHelper.bugReport(message,
                                        function (result) {
                                        });
                                  }

                              });

                        }
                    }
                });

        }

        function messageDialogForCart(message) {
            swal({
                title: message,
                timer: 1000,
                showConfirmButton: false
            });
        }
        //vm.viewModelHelper = viewModelHelper;

        //function init() {
        //    viewModelHelper.refreshCart();
        //    vm.class = 'loader loader-default';
        //    return;
        //    cartdetailsService.removeCartItem(item.id)
        //        .then(function (result) {
        //            if (result.success) {
        //                init();
        //            } else {
        //                var message = {};
        //                message.message = "remove item, " + result.message + " Cart Detail.,";
        //                viewModelHelper.bugReport(message,
        //                  function (result) {
        //                  });
        //            }
        //        });
        //    //for (var j = 0; j < vm.cartItems.shoppingCartDetails.length; j++) {
        //    //    vm.cartItems.shoppingCartDetails[j].totalCostWithTax = 0;
        //    //    var totaltax = 0;
        //    //    if (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit) {
        //    //        for (var k = 0; k < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit.length; k++) {
        //    //            for (var l = 0; l < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k].taxAndAmounts.length; l++) {
        //    //                totaltax += vm.cartItems.shoppingCartDetails[j]
        //    //                    .productsRefByAssembledAndKit[k].taxAndAmounts[l].taxAmount;
        //    //            }
        //    //            var total = (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
        //    //                .quantity *
        //    //                vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
        //    //                .productCost) + totaltax;
        //    //            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
        //    //                .perItemCost = 0;
        //    //            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
        //    //                .perItemCost += total;
        //    //            vm.cartItems.shoppingCartDetails[j].totalCostWithTax += vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
        //    //                .perItemCost;
        //    //        }
        //    //    } else {
        //    //        var total = (vm.cartItems.shoppingCartDetails[j].quantity *
        //    //                vm.cartItems.shoppingCartDetails[j]
        //    //                .productCost) + totaltax;
        //    //        vm.cartItems.shoppingCartDetails[j].totalCostWithTax += total;
        //    //    }
        //    //}
        //    //vm.totalDiscount = 0;
        //    //vm.totalTax = 0;
        //    //for (var i = 0; i < vm.cartItems.shoppingCartDetails.length; i++) {
        //    //    vm.totalDiscount = vm.totalDiscount + vm.cartItems.shoppingCartDetails[i].discount;
        //    //    vm.totalTax = vm.totalDiscount + vm.cartItems.shoppingCartDetails[i].taxAmount;
        //    //    vm.total = vm.total +
        //    //       (vm.cartItems.shoppingCartDetails[i].quantity *
        //    //           vm.cartItems.shoppingCartDetails[i].productCost);
        //    //}
        //    return;
        //    viewModelHelper.apiGetLocal("/api/getcrmurl",
        //        null,
        //        function (result) {
        //            if (result.data) {
        //                vm.crmLocation = result.data;
        //            } else {
        //                var message = {};
        //                message.message = "Update cart item, " + result.message + " Cart Detail.,";
        //                viewModelHelper.bugReport(message,
        //                  function (result) {
        //                  });
        //            }
        //        });
        //}
        function removeItem(item) {
            cartdetailsService.removeCartItem(item.id)
                .then(function (result) {
                    if (result.success) {
                        viewModelHelper.refreshCart();
                        init();
                    } else {
                        var message = {
                    };
                        message.message = "remove item, " + result.message + " Cart Detail.,";
                        viewModelHelper.bugReport(message,
                          function (result) { 
                        });
                }
            });
    }
}
})();

