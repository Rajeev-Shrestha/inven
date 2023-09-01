(function () {
    angular.module("app-category")
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
    };
}])
        .controller("categoryController", categoryController);
    categoryController.$inject = ['$http', '$filter', '$cookies', 'categoryService', 'viewModelHelper'];
    function categoryController($http, $filter, $cookies, categoryService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.addToCart = addToCart;
        vm.selectCategory = selectCategory;
        vm.compareProduct = compareProduct;
        vm.categorySelected = categorySelected;
        vm.categoryName = $cookies.get('categoryName');
        //vm.listCategory = [{ id: $cookies.get('categoryId'), name: $cookies.get('categoryName') }];
        vm.listCategory = [];
        vm.productCategoryList = [];
        vm.categoryList = [];
        vm.productCats = [];
        function categorySelected(category, checked) {
            if (checked) {
                vm.listCategory.push({ id: category.id, name: category.name });
                vm.categoryList.push(category.id);
            } else {
                vm.productCats = vm.listCategory.slice();
                for (var i = 0; i < vm.productCats.length; i++) {
                    if (vm.productCats[i].id == category.id) {
                        vm.listCategory.splice(i, 1);
                        vm.categoryList.splice(i, 1);
                    }
                }
            }
            //vm.catName = vm.catName.substring(0, vm.catName.length - 1);
            if (vm.categoryList.length <= 0) {
                getProductByCategory($cookies.get('categoryId'));
            } else {
                getProductsByCategoryList();
            }
            //for (var i = 0; i < vm.listCategory.length; i++) {
            //    //vm.productCategoryList.push(vm.listCategory[i].id);
            //    getProductByCategory(vm.listCategory[i].id);
            //}
            
        }

        function getProductsByCategoryList() {
            categoryService.getItemsByCategoryIdList(vm.categoryList).then(function (result) {
                if (result.success) {
                    vm.product = result.data;
                }
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

        function productsByCategoryList() {
            categoryService.getItemsByCategoryIdList(vm.productCategoryList)
                .then(function(result) {
                    if (result.success) {
                        vm.product = result.data;
                    } else {
                        var message = {};
                        message.message = "Product by category list, " + result.message + " Category.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        init();
        function init() {
            categoryService.getCrmLocation()
                .then(function (result) {
                    if (result.success) {
                        vm.crmLocation = result.data;
                    } else {
                        var message = {};
                        message.message = "Get CRM location, " + result.message + " Category.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
            getProductByCategory($cookies.get('categoryId'));
            categoryService.getcategoryTree()
                .then(function(result) {
                    if (result.success) {
                        vm.category = result.data;
                        vm.class = 'loader loader-default';
                       // console.log(JSON.stringify(result.data));
                    } else {
                        var message = {};
                        message.message = "Get category tree, " + result.message + " Category.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        vm.product = [];
        function getProductByCategory(categoryId) {
            categoryService.getItemsByCategoryId(categoryId)
                .then(function (result) {
                    if (result.success) {
                        if (vm.product.length === 0) {
                            vm.product = result.data;
                        } else {
                            
                            for (var j = 0; j < result.data.length; j++) {
                                    for (var k = 0; k < vm.product.length; k++) {
                                        if (result.data[j].id !== vm.product[k].id) {
                                        vm.product.push(result.data[j]);
                                        }
                                    }
                                }
                            }
                    } else {
                        var message = {};
                        message.message = "Get product by category, " + result.message + " Category.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        function selectCategory(category) {
            $cookies.put('categoryId', category.id);
            $cookies.put('categoryName', category.name);
            vm.categoryName = $cookies.get('categoryName');
            getProductByCategory($cookies.get('categoryId'));
            //$window.location.href = '/category';
        }

        function addToCart(product) {
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
                    Quantity: '1',
                    Discount: product.discountPrice,
                    //TaxAmount: '100',
                    ShoppingDateTime: new Date()

                };
            if (vm.loginUser > 0) {
                vm.cartItem =
                {
                    ShoppingCartId: vm.cartId,
                    CustomerId: vm.loginUserId,
                    ProductId: product.id,
                    ProductCost: product.unitPrice,
                    Quantity: '1',
                    Discount: product.discountPrice,
                    TaxAmount: '100',
                    ShoppingDateTime: new Date()

                };
            }
            viewModelHelper.addtoCart(vm.cartItem,

              function (result) {
                  if (result.data.id) {
                      vm.cartId = result.data.id;
                      $cookies.put('cartId', vm.cartId);
                      messageDialogForCart();
                      viewModelHelper.refreshCart();
                  } else {
                      var message = {};
                        message.message = "Add to cart, " + result.message + " Category.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                  }
                  
              });
            //categoryService.addItemInCart(vm.cartItem).then(function (result) {
            
            //    vm.cartId = result.data.id;
            //    $cookies.put('cartId', vm.cartId);
            //    messageDialogForCart();
            //    //setTimeout(function () {
            //    //    window.location.reload(1);
            //    //}, 1000);
            //});
        }

        function messageDialogForCart() {
            swal({
                title: "Product added to cart",
                timer: 1000,
                showConfirmButton: false
            });
        }

    }
})();