(function () {
    angular.module("app-items")
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
        .config(config)
        .controller("itemDetailController", itemDetailController);
    function config(wipImageZoomConfigProvider) {
        wipImageZoomConfigProvider.setDefaults({
            style: 'box' // e.g.
        });
    };
    itemDetailController.$inject = ['$scope', '$http', '$filter', '$location', '$window', '$cookies', 'itemDetailService', 'viewModelHelper'];
    function itemDetailController($scope, $http, $filter, $location, $window, $cookies, itemDetailService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        var str = window.location.pathname;
        vm.selectCategory = selectCategory;
        vm.compareProduct = compareProduct;
        vm.productId = str.substring(str.lastIndexOf("/") + 1, str.length);
       
        vm.addToCard = addToCard;
        vm.caroselImageClick = caroselImageClick;
        vm.product = [];
        vm.totalRecords = 0;
        vm.filteredCount = 0;
        vm.shoppingCardId = null;
        vm.categoryList = [];
        vm.categories = "";
        vm.loginUser = 0;
        //vm.productId = $cookies.get('productId');
        
        function split(str) {
            var i = str.lastIndexOf("_");
            if (i > 0)
                return str.slice(0, i);
            else
                return "";
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

        function caroselImageClick(image) {
            //replacing folder name small with large
            vm.mainImageUrl = image.replace("Small", "Large");

        }

        function selectCategory(category) {
            $cookies.put('categoryId', category.id);
            $cookies.put('categoryName', category.name);
            $window.location.href = '/category';
        }

        function getlatestProduct(pageNumber, pageSize) {
            itemDetailService.getLatestProduct(pageNumber, pageSize)
                .then(function(result) {
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
        
        vm.imagesArray = [];
        init();
        function init() {
            if (vm.productId === "") {
                $window.location.href = "/error";
            } else {
                getlatestProduct(1, 5);
                itemDetailService.crmLocation()
                   .then(function (result) {
                       if (result.success) {
                           vm.crmLocation = result.data;
                       } else {
                           var message = {};
                           message.message = "get CRM location, " + result.data + " in item detail.,";
                           viewModelHelper.bugReport(message,
                             function (result) {
                             });
                       }
                   });

                itemDetailService.getCategoryTree()
                   .then(function (result) {
                       if (result.success) {
                           vm.categoryTree = result.data;
                       } else {
                           var message = {};
                           message.message = "get category tree, " + result.data + " in item detail.,";
                           viewModelHelper.bugReport(message,
                             function (result) {
                             });
                       }
                   });

                itemDetailService.getLoginUserId()
                .then(function (result) {
                    if (result.success) {
                        var loginUserId = Number(result.data);
                        if (loginUserId > 0) {
                            vm.loginUser = loginUserId;
                        } else {
                            vm.loginUser = 0;
                        }
                        vm.categories = "";
                        // Get Product By product ID
                        itemDetailService.getItemsById(vm.loginUser, Number(vm.productId))
                           .then(function (result) {
                               if (result.success) {
                                   vm.product = result.data;
                                   
                                   //for (var k = 0; k < result.data.smallImageUrls.length; k++) {
                                   //    vm.imagesArray[k] = {};
                                   //    vm.imagesArray[k].thumb = vm.crmLocation + '/' + result.data.smallImageUrls[k];
                                   //}

                                   //for (var l = 0; l < result.data.mediumImageUrls.length; l++) {
                                   //    //vm.imagesArray[l] = {};
                                   //    vm.imagesArray[l].medium = vm.crmLocation + '/' + result.data.mediumImageUrls[l];
                                   //}

                                   for (var m = 0; m < result.data.largeImageUrls.length; m++) {
                                       vm.imagesArray[m] = {};
                                       vm.imagesArray[m].thumb = vm.crmLocation + '/' + result.data.largeImageUrls[m];
                                       vm.imagesArray[m].medium = vm.crmLocation + '/' + result.data.largeImageUrls[m];
                                       vm.imagesArray[m].large = vm.crmLocation + '/' + result.data.largeImageUrls[m];
                                   }

                                   vm.refrenceProduct = result.data.productsRefByAssembledAndKit;
                                   try {
                                       vm.mainImageUrl = vm.product.largeImageUrls[0];
                                   } catch (e) {
                                    console.log(e);
                                   }
                                   itemDetailService.getAllCategory()
                                       .then(function (result) {
                                           if (result.success) {
                                               vm.categoryList = result.data;
                                               for (var i = 0; i < vm.product.categories.length; i++) {
                                                   for (var j = 0; j < vm.categoryList.length; j++) {
                                                       if (vm.product.categories[i] === vm.categoryList[j].id) {
                                                           vm.categories += vm.categoryList[j].name + ", ";
                                                       }
                                                   }
                                               }
                                               itemDetailService.getSettings()
                                                    .then(function (result) {
                                                        if (result.success) {
                                                            vm.settings = result.data;
                                                        } else {
                                                            var message = {};
                                                            message.message = "get setting, " + result.data + " in item detail.,";
                                                            viewModelHelper.bugReport(message,
                                                              function (result) {
                                                              });
                                                        }
                                                    });

                                           } else {
                                               var message = {};
                                               message.message = "get all category, " + result.data + " in item detail.,";
                                               viewModelHelper.bugReport(message,
                                                 function (result) {
                                                 });
                                           }
                                       });
                                   vm.class = 'loader loader-default';
                               } else {
                                   var message = {};
                                   message.message = "get item by id, " + result.data + " in item detail.,";
                                   viewModelHelper.bugReport(message,
                                     function (result) {
                                     });
                               }
                           });

                        //Get all category list
                    } else {
                        var message = {};
                        message.message = "get login user, " + result.data + " in item detail.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });

                
            }
            
            
        }

        vm.cartId = "0";
        //add to card button clicked
        function addToCard(product, quantity) {
            itemDetailService.getItemsById(vm.loginUser, Number(vm.productId))
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

        vm.changeQuantity = changeQuantity;
        vm.minus = "minus";
        vm.plus = "plus";
        vm.itemQuantity = 1;
        function changeQuantity(type) {
            if (type === 'minus') {
                        if (vm.itemQuantity > 0) {
                            vm.itemQuantity = vm.itemQuantity - 1;
                        }
            }
            else if (type === 'plus') {
                if (vm.product.allowBackOrder) {
                    vm.itemQuantity = vm.itemQuantity + 1;
                }
                else {
                    if (vm.product.quantityOnHand > vm.itemQuantity) {
                        vm.itemQuantity = vm.itemQuantity + 1;
                    }
                    else {
                        alert("Out Of stock");
                    }
                }


                //if (vm.product.allowBackOrder && vm.product.quantityOnHand > vm.itemQuantity) {
                //    vm.itemQuantity = vm.itemQuantity + 1;
                //}
                //else if (vm.product.quantityOnHand > vm.itemQuantity) {
                //    vm.itemQuantity = vm.itemQuantity + 1;
                //} 
                //else {
                //            alert("Out Of stock");
                //}
            } else {
                alert("error");
            }
        }

        vm.options = {
            zoomEnable: true,
            defaultIndex: 0, // Order of the default selected Image
            style: 'box', // inner or box
            boxPos: 'right-top', // e.g., right-top, right-middle, right-bottom, top-center, top-left, top-right ...
            boxW: 500, // Box width
            boxH: 500, // Box height
            method: 'lens', // fallow 'lens' or 'pointer'
            cursor: 'crosshair', // 'none', 'default', 'crosshair', 'pointer', 'move'
            lens: true, // Lens toggle
            zoomLevel: 3, // 0: not scales, uses the original large image size, use 1 and above to adjust.
            immersiveMode: '769', // false or 0 for disable, always, max width(px) for trigger
            immersiveModeOptions: {}, // can extend immersed mode options
            immersiveModeMessage: 'Click to Zoom', // Immersive mode message
            prevThumbButton: '&#9665;', // Prev thumb button (html)
            nextThumbButton: '&#9655;', // Next thumb button (html)
            thumbsPos: 'bottom', // Thumbs position: 'top', 'bottom'
            thumbCol: 3, // Thumb column count
            thumbColPadding: 4, // Padding between thumbs
            images: vm.imagesArray
    };

    }

   
})();
