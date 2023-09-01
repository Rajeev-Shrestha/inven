(function () {
    angular.module("app-compare")
        .controller("compareController", compareController);
    compareController.$inject = ['$scope', '$http', '$filter', '$location', '$window', '$cookies', 'compareService', 'viewModelHelper'];
    function compareController($scope, $http, $filter, $location, $window, $cookies, compareService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.addToCart = addToCart;
        vm.removeCompareItem = removeCompareItem;
        vm.itemIdList = [];
        vm.compareItemList = [];


        function getCompareItemId() {
            var item1 = $cookies.get('compareItem1');
            var item2 = $cookies.get('compareItem2');
            var item3 = $cookies.get('compareItem3');
            if (item1) {
                vm.itemIdList.push(item1);
            }
            if (item2) {
                vm.itemIdList.push(item2);
            }
            if (item3) {
                vm.itemIdList.push(item3);
            }
        }

        init();
        function init() {
            getCompareItemId();
            getCrmLocation();
            getLoginUser();
            getProductById(vm.itemIdList);
        }

        function getProductById(idList) {
            vm.compareItemList = [];
            for (var i = 0; i < idList.length; i++) {
                compareService.productById(idList[i])
                .then(function (result) {
                    if (result.success) {
                        vm.compareItemList.push(result.data);
                        
                    } else {
                        var message = {};
                        message.message = "get product by id, " + result.message + " in compare.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                    
                });
            }
            vm.class = 'loader loader-default';
        }

        function getLoginUser() {
            viewModelHelper.getLoginUser(

              function (result) {
                  var loginUserId = Number(result.data);
                  if (loginUserId > 0) {
                      vm.loginUser = loginUserId;
                  } else {
                      vm.loginUser = null;
                  }

              });
        }

        function getCrmLocation() {
            viewModelHelper.apiGetLocal("/api/getcrmurl",
                null,
                function (result) {
                    if (result.data) {
                        vm.crmLocation = result.data;
                    } else {
                        var message = {};
                        message.message = "get CRM location, " + result.message + " in checkout.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                    
                });
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
            compareService.addToCart(vm.cartItem)
                .then(function(result) {
                    if (result.success) {
                        vm.cartId = result.data.id;
                        $cookies.put('cartId', vm.cartId);
                    }
                    else {
                        var message = {};
                        message.message = "add to cart, " + result.message + " in checkout.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });

        }

        function removeCompareItem(product, index) {
            vm.itemIdList.splice(index, 1);
            getProductById(vm.itemIdList);
        }

    }
})();

