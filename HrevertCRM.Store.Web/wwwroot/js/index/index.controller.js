(function () {
    "use strict";
    angular.module("app-index")
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
        .controller("indexController", indexController);
    indexController.$inject = ['$http', '$filter', '$location', '$timeout', '$window', '$scope', '$cookies', 'indexService', 'viewModelHelper'];
    function indexController($http, $filter, $location, $timeout, $window, $scope, $cookies, indexService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.loading = true;
        vm.loadComplete = false;
        vm.addToCart = addToCart;
        vm.goToCategory = goToCategory;
        vm.getProductByCategory = getProductByCategory;
        vm.selectCategory = selectCategory;
        vm.viewMoreProduct = viewMoreProduct;
        vm.compareProduct = compareProduct;
        vm.loadAllProduct = loadAllProduct;
        vm.addWishlist = addWishlist;
        vm.guid = $cookies.get('guid');
        vm.loginUser = null;

        function addWishlist() {
            messageDialogForCart('Sorry, this function is not avaliable right now.');
        }


        function loadAllProduct(category) {
            indexService.getIndexProduct(500, category.id, 0)
                .then(function(result) {
                    if (result.success) {
                        category.children[0].productList = result.data;
                        //console.log("Categories Children"+JSON.stringify(category));
                    } else {
                        var message = {};
                        message.message = "load All Product, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {

                          });
                    }
                });
        }

        function getlatestProduct(pageNumber, pageSize) {
            indexService.getLatestProduct(pageNumber, pageSize)
                .then(function (result) {
                    if (result.data.items) {
                        vm.latestProduct = result.data.items;
                    } else {
                        var message = {};
                        message.message = "Get Latest product, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {

                          });
                    }
                });
        }

        function getHotThisWeek() {
            indexService.getHotThisWeek()
                .then(function (result) {
                    if (result.data) {
                        vm.hotThisWeek = result.data;
                        
                    } else {
                        var message = {};
                        message.message = "Get Hot This Week, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {

                          });
                    }
                });
        }

        function getTopCategories(list) {
            indexService.getTopCategories(list)
                .then(function (result) {
                    if (result.data) {
                        vm.topCategories = result.data;
                    } else {
                        var message = {};
                        message.message = "Get Latest product, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {

                          });
                    }
                });
        }

        function getTrendingProduct() {
            indexService.getTrendingProducts()
                .then(function (result) {
                    if (result.data) {
                        vm.trendingProducts = result.data;
                        
                    } else {
                        var message = {};
                        message.message = "Get Latest product, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {

                          });
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
            messageDialogForCart('Product Added to compare list');

        }

        function selectCategory(category) {
            $cookies.put('categoryId', category.id);
            $cookies.put('categoryName', category.name);
            $window.location.href = '/category';
        }

        function goToCategory() {
            $window.location.href = '/items';
        }

        function viewMoreProduct() {

        }
        vm.cartId = "0";
        function addToCart(product, ev) {
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
                    //CategoryId: product.categoryId,
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
                      messageDialogForCart('Product added to cart');
                      //  viewModelHelper.refreshCart();
                  } else {
                      var message = {};
                      message.message = "Add to cart, " + result.data + " in index.,";
                      viewModelHelper.bugReport(message,
                        function (result) {
                        });
                  }
                  
              });
        }
        init();
        vm.ItemLoadComplete = false;
        vm.slider = [];
        var bannerImages = [];
        vm.bannerImagesForCategory = [];
        vm.active = true;
        vm.searchText = null;
        function init() {

            
            indexService.getLoginUserId()
                .then(function (result) {
                    if (result.success) {
                        var loginUserId = Number(result.data);
                        if (loginUserId > 0) {
                            vm.loginUser = loginUserId;
                        } else {
                            vm.loginUser = null;
                        }
                    } else {
                        
                    }
                });
            viewModelHelper.apiGetLocal("/api/getcrmurl", null, function (result) {
                if (result.data) {
                    vm.crmLocation = result.data;
                } else {
                    var message = {};
                    message.message = "Get CRM URL, " + result.data + " in index.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
                viewModelHelper.apiGet("/api/Carousel/searchCarousels/" + true + "/" + null + "/", null, function (result) {
                    if (result.data) {
                        vm.slider = result.data;
                        for (var i = 0; i < vm.slider.length; i++) {
                            vm.slider[i].imageUrl = vm.crmLocation + '/' + vm.slider[i].imageUrl;
                        }
                    } else {
                        var message = {};
                        message.message = "Get CRM URL, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
                getlatestProduct(1, 10);
                getTrendingProduct();
                getHotThisWeek();
                    indexService.getAllFeaturedItemsImages().then(function (records) {
                        if (records.success) {
                            for (var i = 0; i < records.data.length; i++)
                            {
                                if (records.data[i].bannerImageUrls.length > 0) {
                                    for (var j = 0; j < records.data[i].bannerImageUrls.length; j++) {
                                        // alert(records.data[i].bannerImageUrls[j]);
                                        records.data[i].bannerImageUrls[j] = vm.crmLocation + '/' + records.data[i].bannerImageUrls[j];

                                    }
                                }

                                if (records.data[i].fullWidthUrls) {
                                    for (var k = 0; k < records.data[i].fullWidthUrls.length; k++) {
                                        records.data[i].fullWidthUrls[k] = vm.crmLocation + '/' + records.data[i].fullWidthUrls[k];
                                    }
                                }

                                if (records.data[i].halfWidthUrls) {
                                    for (var l = 0; l < records.data[i].halfWidthUrls.length;l++){
                                        records.data[i].halfWidthUrls[l] = vm.crmLocation + '/' + records.data[i].halfWidthUrls[l];
                                    }
                                }
                                if (records.data[i].quaterWidthUrls) {
                                    for (var m = 0; m < records.data[i].quaterWidthUrls.length; m++) {
                                        records.data[i].quaterWidthUrls[m] = vm.crmLocation + '/' + records.data[i].quaterWidthUrls[m];
                                    }
                                }
                            }
                            //console.log("Banner Images: "+JSON.stringify(records.data));
                            vm.bannerImagesForCategory = records.data;
                        }
                        viewModelHelper.apiGet("/api/ProductCategory/categorytree", null, function (result) {
                            if (result.data) {
                                vm.products = result.data;
                                angular.forEach(vm.products, function (cateogryValue, cateogryKey) {
                                    angular.forEach(vm.products[cateogryKey].children, function (subCategoryValue, subCategoryKey) {
                                        if (vm.products[cateogryKey].children[0].productList) {
                                            angular.forEach(vm.products[cateogryKey].children[subCategoryKey]
                                                .productViewModels,
                                                function (productValue, productKey) {
                                                    vm.products[cateogryKey].children[0].productList
                                                        .push(vm.products[cateogryKey].children[subCategoryKey]
                                                            .productViewModels[productKey]);
                                                });
                                        }
                                        else {
                                            vm.products[cateogryKey].children[0].productList = [];
                                            angular.forEach(vm.products[cateogryKey].children[subCategoryKey]
                                                .productViewModels,
                                                function (productValue, productKey) {
                                                    vm.products[cateogryKey].children[0].productList
                                                        .push(vm.products[cateogryKey].children[subCategoryKey]
                                                            .productViewModels[productKey]);
                                                });
                                        }

                                    });
                                    cateogryValue.categoryUrls = [];
                                    

                                });
                                vm.class = 'loader loader-default';
                                for (var i = 0; i < vm.products.length; i++) {
                                    var fullWidth = [];
                                    var halfWidth = [];
                                    var quaterWidth = [];
                                    bannerImages = [];
                                    if (vm.products[i].id == vm.bannerImagesForCategory[i].id) {
                                        // if (vm.bannerImagesForCategory[i].bannerImageUrls.length > 0) {
                                        //    vm.products[i].categoryUrls = vm.bannerImagesForCategory[i].bannerImageUrls;
                                        //}

                                        if (vm.bannerImagesForCategory[i].fullWidthUrls) {
                                            for (var k = 0; k < vm.bannerImagesForCategory[i].fullWidthUrls.length; k++) {

                                                fullWidth.push(vm.bannerImagesForCategory[i].fullWidthUrls[k]);
                                            }
                                        }

                                        if (vm.bannerImagesForCategory[i].halfWidthUrls) {
                                            for (var j = 0; j < vm.bannerImagesForCategory[i].halfWidthUrls.length; j++) {
                                                halfWidth.push(vm.bannerImagesForCategory[i].halfWidthUrls[j]);
                                            }
                                        }

                                        if (vm.bannerImagesForCategory[i].quaterWidthUrls) {
                                            for (var j = 0; j < vm.bannerImagesForCategory[i].quaterWidthUrls.length; j++) {
                                                quaterWidth.push(vm.bannerImagesForCategory[i].quaterWidthUrls[j]);
                                            }
                                        }

                                        var dataForFullWidth = {
                                            imageTypeId: 1,
                                            imageUrl: fullWidth
                                        };
                                        var dataForHalfWidth = {
                                            imageTypeId: 2,
                                            imageUrl: halfWidth
                                        };
                                        var dataForQuaterWidth = {
                                            imageTypeId: 3,
                                            imageUrl: quaterWidth
                                        };
                                        if (fullWidth.length > 0) {
                                            bannerImages.push(dataForFullWidth);
                                        }
                                        if(halfWidth.length>0){
                                            bannerImages.push(dataForHalfWidth);
                                        }
                                        if(quaterWidth.length>0){
                                            bannerImages.push(dataForQuaterWidth);
                                        }
                                    
                                      

                                    }
                                    vm.products[i].categoryUrls = bannerImages;
                                    //console.log("1st category Image Urls:" + JSON.stringify(bannerImages));

                                }
                                
                            } else {
                                var message = {};
                                message.message = "Get category tree with product, " + result.data + " in index.,";
                                viewModelHelper.bugReport(message,
                                  function (result) {
                                  });
                            }
                        });
                        
                    });
                    viewModelHelper.apiGet("/api/EcommerceSetting/getactiveEcommerceSettings", null, function (result) {
                        if (result.data) {
                            vm.settings = result.data;
                        } else {
                            var message = {};
                            message.message = "Get Ecommerce setting, " + result.data + " in index.,";
                            viewModelHelper.bugReport(message,
                              function (result) {
                              });
                        }
                    });
                    
            });
        }
        getThemeSettings();
        themeLayoutSettings();
        getSliderSettings();
        function getThemeSettings() {
            indexService.getThemeGeneralSettings().then(function (result) {
                if (result.success) {
                    vm.generalSettings = result.data;
                }
                else {
                    var message = {};
                    message.message = "Get general settings, " + result.data + " in index.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }

        function getSliderSettings() {
            indexService.getThemeSliderSettings().then(function (result) {
                if (result.success) {
                    vm.sliderSettings = result.data;

                }
                else {
                    var message = {};
                    message.message = "Get Slider settings, " + result.data + " in index.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }

        function themeLayoutSettings() {
            indexService.getThemeLayoutSettings().then(function (result) {
                if (result.success) {
                    var listCategory = [];
                    result.data.latestProductsImageUrl = vm.crmLocation + "/" + result.data.latestProductsImageUrl;
                    result.data.hotThisWeekImageUrl = vm.crmLocation + "/" + result.data.hotThisWeekImageUrl;
                    result.data.trendingItemsImageUrl = vm.crmLocation + "/" + result.data.trendingItemsImageUrl;
                    vm.layoutSettings = result.data;
                    if (result.data.categoryOne) {
                        listCategory.push(result.data.categoryOne);
                    }
                    if (result.data.categoryTwo) {
                        listCategory.push(result.data.categoryTwo);
                    }
                    if (result.data.categoryThree) {
                        listCategory.push(result.data.categoryThree);
                    }
                    if (result.data.categoryFour) {
                        listCategory.push(result.data.categoryFour);
                    }
                    getTopCategories(listCategory);
                }
                else {
                    var message = {};
                    message.message = "Get general settings, " + result.data + " in index.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }


        function getProductByCategory(subCategory, category) {
            subCategory.productList = [];
           
            indexService.getProductByCategoryId(subCategory.id)
                .then(function (result) {
                    if (result.success) {
                        category.children[0].productList = result.data;
                        return;
                        subCategory.productList = [];
                        //return;
                        subCategory.productList = result.data;
                        vm.SelectedCategoryId = subCategory.id;
                    } else {
                        var message = {};
                        message.message = "Get product by category ID, " + result.data + " in index.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        vm.redirectToItem = redirectToItem;

        function redirectToItem(slider) {
            $window.location.href = '/items/' + slider.itemId;
            $window.location.target = '_self';
        }


        $scope.onTransitionStart = function (swiper) {
            //console.log("The current index is : " + swiper.activeIndex);
        };

        $scope.setSlide = function (index) {
            $scope.instance.slideTo(index, 300);
        };

        function messageDialogForCart(message) {
           swal({
               title: message,
               timer: 1000,
               showConfirmButton: false
           });
        }
        function startLoading(name) {
            $loading.start(name);
        }
        function finishLoading(name) {
            $loading.finish(name);
        }

        //$scope.productLists = partition([1, 2, 3, 4, 5, 6, 7, 8], 3);

        //function partition(coll, size) {
        //    var groups = [];
        //    for (var groupIndex = 0, numGroups = Math.ceil(coll.length / size) ;
        //         groupIndex < numGroups;
        //         groupIndex++) {
        //        var startIndex = groupIndex * size;
        //        groups.push(coll.slice(startIndex, startIndex + size));
        //    }
        //    return groups;
        //}
        $scope.myInterval = 5000;
        var slides = $scope.slides = [];
        $scope.addSlide = function () {
            var newWidth = 600 + slides.length + 1;
            slides.push({
                image: 'http://placekitten.com/' + newWidth + '/300',
                text: ['More', 'Extra', 'Lots of', 'Surplus'][slides.length % 4] + ' ' +
                  ['Cats', 'Kittys', 'Felines', 'Cutes'][slides.length % 4]
            });
        };
        for (var i = 0; i < 4; i++) {
            $scope.addSlide();
        }
    }
})();