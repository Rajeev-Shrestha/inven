(function () {
    angular.module("app-search")
        .controller("searchController", searchController);
    searchController.$inject = ['$http', '$filter', '$scope', '$cookies', 'searchService', 'viewModelHelper'];
    function searchController($http, $filter, $scope, $cookies, searchService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.compareProduct = compareProduct;
        vm.addToCart = addToCart;
        vm.loginUser = null;
        vm.cartItemsLength = 0;
        init();
       
        function init() {
            var categoryId = $cookies.get('categoryId');
            var text = $cookies.get('text');
            vm.searchText = $cookies.get('text');
            console.log(vm.searchText);
            searchService.getLoginUserId()
                .then(function (result) {
                    if (result.success) {
                        var loginUserId = Number(result.data);
                        if (loginUserId > 0) {
                            vm.loginUser = loginUserId;
                        } else {
                            vm.loginUser = null;
                        }
                    } else {
                        var message = {};
                        message.message = "get login user id , " + result.message + " in search.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
            getCrmLocation();
            if (categoryId == undefined) {
                categoryId = null;
            }
            //console.log("text searched" + text);
            if (text === "undefined" || text === undefined) {
                text == "";
            }
            var searchViewModel = {};
            searchViewModel.categoryId = categoryId;
            searchViewModel.text = text;
            //console.log("text searched"+text);
            searchService.searchText(searchViewModel).then(function (result) {
                if (result.data) {
                    vm.searchResult = result.data;
                    vm.class = 'loader loader-default';
                } else {
                    var message = {};
                    message.message = "search text , " + result.message + " in search.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
                
            });

            }

        function getCrmLocation() {
            viewModelHelper.apiGetLocal("/api/getcrmurl",
                null,
                function(result) {
                    vm.crmLocation = result.data;
                });
        }

        function compareProduct(product) {
            if (!$cookies.get('compareItem1')) {
                $cookies.put('compareItem1', product.id);
            }
            else if (!$cookies.get('compareItem2')) {
                $cookies.put('compareItem2', product.id);
            }
            else if (!$cookies.get('compareItem3')) {
                $cookies.put('compareItem3', product.id);
            }
            else {
                $cookies.put('compareItem3', $cookies.get('compareItem2'));
                $cookies.put('compareItem2', $cookies.get('compareItem1'));
                $cookies.put('compareItem1', product.id);

            }
        }

        function addToCart(product) {
            var cartId = $cookies.get('cartId');
            if (cartId != null) {
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
                    Quantity: '1',
                    Discount: product.discountPrice,
                    //TaxAmount: '100',
                    ShoppingDateTime: new Date()

                };
            viewModelHelper.addtoCart(vm.cartItem,
              function (result) {
                  if (result.data.id) {
                      vm.cartId = result.data.id;
                      $cookies.put('cartId', vm.cartId);
                      messageDialogForCart(vm.cartId, vm.loginUser);
                  } else {
                      var message = {};
                      message.message = "add to cart , " + result.message + " in search.,";
                      viewModelHelper.bugReport(message,
                        function (result) {
                        });
                  }
                 
              });

        }

        function messageDialogForCart(cartId,customerId) {
            //BootstrapDialog.show({
            //    message: 'Product is successfully added to cart',
            //    buttons: [
            //        {
            //            label: 'Close',
            //            action: function (dialogItself) {
            //                dialogItself.close();
            //            //  alert("CartId:"+cartId+"&CustomerId:"+customerId);
            //                forCartItems(cartId, customerId);
            //            }
            //        }]
            //});
            swal({
                title: "Product added to cart.",
                timer: 1000,
                showConfirmButton: false
            });
            forCartItems(cartId, customerId);

        }

        function forCartItems(cartId, customerId) {
          //  var cartId = $cookies.get('cartId');
          //  var customerId = vm.loggedInUser;

            if (customerId === null) {
                customerId = 0;
            }
          //  console.log(customerId);
          //console.log(cartId);
            searchService.getCartItems(cartId, customerId).then(function (result) {
                if (result.success) {
                    if (result.data.shoppingCartDetails) {
                        $scope.cartItemsLength = result.data.shoppingCartDetails.length;
                        $scope.$broadcast('cartItemsChanged', { data: $scope.cartItemsLength });
                      //$scope.$emit('cartItemsChanged', { data: $scope.cartItemsLength });
                        console.log("Total CartItems :" + JSON.stringify($scope.cartItemsLength));
                    }
                 
                }
                else {
                    var message = {};
                    message.message = "get cart items , " + result.message + " in search.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
            
        }

    }
    
})();