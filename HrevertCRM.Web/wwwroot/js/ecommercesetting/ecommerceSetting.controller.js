(function () {
    angular.module("app-ecommerceSetting")
        .controller("ecommerceSettingController", ecommerceSettingController);
    ecommerceSettingController.$inject = ['$http', '$filter', '$scope','ecommerceSettingService'];
    function ecommerceSettingController($http, $filter, $scope, ecommerceSettingService) {

        var vm = this;
        vm.sliderImage = [{ id: 1, name: '3' }, { id: 2, name: '5' }, { id: 3, name: '7' }];
        vm.saveSettings = saveSettings;
        vm.saveCarouselImage = saveCarouselImage;
        vm.removeCarousel = removeCarousel;
        vm.carouselResultImageSize = { id: 1, w: 1200, h: 400, aspectratio: 3 };
        vm.logoImageSize = { id: 1, w: 170, h: 120, aspectratio: 3 };

        vm.myCroppedImage = [];
        vm.images = [];
        vm.quantityStatus = [{ id: 1, name: 'In-Stock/Out-Stock' }, { id: 2, name: 'Exact Available' }];
        vm.includeQuantityInSalesOrder = false;
        vm.settingsData = {};
        vm.settingsData.includeQuantityInSalesOrder = false;
        vm.settingsData.displayOutOfStockItems = false;
        vm.loadSettings = loadSettings;
        vm.listImage = {};
        vm.saveEcommerceLogo = saveEcommerceLogo;
        vm.removeLogo = removeLogo;
        vm.imageForLogo = [];
        loadSettings();

        $scope.open = function () {
            vm.ecommerceMethodImageForm.$setUntouched();
            getAllProducts();
            $scope.showModal = true;
        }

        $scope.openImageDialog = function () {
            $scope.showEcommerceModal = true;
        }

        $scope.closeCompanyModal = function()
        {
            $scope.showEcommerceModal = false;
        }

        $scope.hide = function () {
            $scope.showModal = false;
            $('#img').attr('src', '');
            var input = $("#fileInput");
            input.replaceWith(input.val('').clone(true));
            $scope.myImage = '';
        }


        function getAllProducts() {
            ecommerceSettingService.getAllProduct().then(function (result) {
                if (result.success) {
                    vm.products = result.data;
                } else {
                    console.log(result.message);
                }
            });
        }
        function saveCarouselImage(image, itemId) {
            if (image === null || image === undefined) {
                swal("Please select Image.");
            }
            else{
                vm.images = {
                    FileName: vm.itemName.name,
                    ImageBase64: image[0].dataURI.substring(image[0].dataURI.lastIndexOf(",") + 1, image[0].dataURI.length)
                };

                //vm.listImage.push(vm.images);
                vm.listImage.productOrCategory = '1';
                vm.listImage.itemId = itemId;
                vm.listImage.image = vm.images;
                //vm.imageList.push(image);
                ecommerceSettingService.saveImage(vm.listImage).then(function (result) {
                    if (result.success) {
                        var input = $("#fileInput");
                        input.replaceWith(input.val('').clone(true));
                        vm.listImage = {};
                        vm.myCroppedImage = [];
                        loadSettings();
                        $scope.myImage = '';
                        vm.products = [];
                        $scope.showModal = false;
                    } else {
                        console.log(result.message);
                    }
                });
            }
        }

        function saveSettings(settings) {
            settings.image = vm.imageForLogo;
            if (settings.image === "" || settings.image === null || settings.image === undefined) {
                swal("Please select e-commerce logo!");
            }
            else {
                ecommerceSettingService.saveSettings(settings).then(function (result) {
                    if (result.success) {
                        loadSettings();
                        swal("E-Commerce Setting has been saved successfully.");
                        setTimeout(function () {
                            $('#successMessage').html('');
                        }, 3000);
                    } else {
                        swal(result.message);
                    }

                });
            }
        }

        function loadSettings() {
            $('#successMessage').html('');
            ecommerceSettingService.getEcommerceSetting().then(function (result) {
                if (result.success) {
                    vm.settingsData = result.data;
                    if (vm.settingsData.imageUrl) {
                        vm.settingsData.imageUrl = '../../' + vm.settingsData.imageUrl;
                    }
                    
                } else {
                    swal(result.message);
                }

            });


            ecommerceSettingService.getCarouselImage().then(function (result) {
                if (result.success) {
                    if (result.data.length !== 0) {
                        for (var i = 0; i < result.data.length; i++) {
                            result.data[i].imageUrl = '../../' + result.data[i].imageUrl;
                        }
                        vm.allImageList = result.data;
                    }
                    else {
                             vm.allImageList = result.data;
                    }
                } else {
                    swal(result.message);
                }

            });
        }

        function removeLogo(url) {
            var result = url.substring(url.lastIndexOf('/') + 1);
            ecommerceSettingService.removeLogoImage(result).then(function (result) {
                if (result.success) {
                    loadSettings();
                } else {
                    console.log(result.message);
                }

            });
        }

        function removeCarousel(image) {
            ecommerceSettingService.removeCarouselImage(image.id).then(function (result) {
                if (result.success) {
                    loadSettings();
                } else {
                    console.log(result.message);
                }

            });
        }

        //Image upload for carosel
        $scope.myImage = '';
        $scope.myCroppedImage = '';
        $scope.$watch('myCroppedImage', function () {
            // console.log($scope.myArray);
        });

        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.myImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };

        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

        $scope.fileNameChanged = fileNameChanged;

        function fileNameChanged(file) {
            vm.itemName = file[0];
        }
        var handleLogoSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.logoImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#fileInput1')).on('change', handleLogoSelect);
        function dialogMessageForSuccess(result) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                + '<div class="alert alert-success alert-dismissable">'
                + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                + result + '</div>'
                + '</div>';
            $('#successMessage').html(message);
        }
        //save e-commerce logo

        $scope.closeEcommerceModal = function () {
            $scope.showEcommerceModal = false;
            $('#logoImage').attr('src', '');
            var input = $("#fileInput1");
            input.replaceWith(input.val('').clone(true));
            $scope.myImage = '';
        }
        function saveEcommerceLogo(image) {
            vm.settingsData.imageUrl = image;
            if (image === "") {
                swal("upload E-Commerce logo.");
                return;
            }

            if (image !== undefined) {
                var spiltString = image.substring(0, 22);
                vm.imageForLogo = {
                    FileName: "logo" + ".jpg",
                    ImageBase64: image.replace(spiltString, ''),

                };
                $scope.showEcommerceModal = false;
                $('#logoImage').attr('src', '');
                var input = $("#fileInput1");
                input.replaceWith(input.val('').clone(true));
                $scope.myImage = '';

            }
            //var ecommerceLogo = {
            //    image: vm.imageForLogo
            //}
            //if (ecommerceLogo.id == undefined) {
            //    ecommerceSettingService.saveEcommerceLogo(ecommerceLogo).then(function (result) {
            //        if (result.success) {
            //            $scope.showEcommerceModal = false;

            //        } else {
            //            if (result.message.errors) {
            //                swal(result.messsage.errors[0]);
            //            }
            //            else {
            //                swal(result.messsage);
            //            }
            //        }

            //    });
            //}
            //else {
            //    ecommerceSettingService.updateEcommerceLogo(ecommerceLogo).then(function (result) {
            //        if (result.success) {
            //            $scope.showEcommerceModal = false;
            //        }
            //        else {
            //            if (result.message.errors) {
            //                swal(result.messsage.errors);
            //            }
            //            else {
            //                swal(result.messsage);
            //            }
            //        }
            //    });
            //}
        }
    }
})();