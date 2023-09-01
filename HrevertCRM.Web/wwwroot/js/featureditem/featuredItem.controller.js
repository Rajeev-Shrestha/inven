(function () {
    angular.module("app-featuredItem")
        .controller("featuredItemController", featuredItemController);
    featuredItemController.$inject = ['$http', '$filter', '$scope', 'featuredItemService'];
    function featuredItemController($http, $filter, $scope ,featuredItemService) {
        var vm = this;
        vm.addFeaturedItemBannerImage = addFeaturedItemBannerImage;
        vm.activeFeaturedItem = null;
        vm.saveFeatureItemBannerImage = saveFeatureItemBannerImage;
        $scope.fileNameChangedForBanner = fileNameChangedForBanner;
        vm.removeFeatureItemBanner = removeFeatureItemBanner;
        vm.featuredItemObj = {};
        vm.imageList = [];
        vm.imageListForBanner = [];
        vm.saveImagesForBanner = saveImagesForBanner;
        vm.changeImageSelectOption = changeImageSelectOption;
        vm.showCloseBannerBtn = false;
        vm.showSaveBannerBtn = false;
        vm.showImageCloseBtn = true;
        vm.showImageAddBtn = true;
        vm.bannerImagesForCategory = [];
        vm.bannercategoryId = 0;
        vm.ImageSizeForImagesOption = [{ id: 1, w: 1040, h: 180, aspectratio: 5.7 },
                                       { id: 2, w: 570, h: 120, aspectratio: 4.7 },
                                       { id: 3, w: 285, h: 120, aspectratio: 2.37 }];
        vm.editFeaturedItem = editFeaturedItem;
        vm.imageSaveForm = true;
        vm.showBannerImages = false;
        vm.imageTypeOption = false;
        vm.productCategoryOption = false;
        vm.featuredItemBtnText = "Save";
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];

        init();


        //multiple select
        vm.checkAll = checkAll;
        vm.toggleSelection = toggleSelection;
        vm.deleteSelected = deleteSelected;

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
                angular.forEach(vm.allFeaturedItemList, function (check) {
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
            featuredItemService.deletedSelected(selectedItemid).then(function (result) {
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
            vm.activeFeaturedItem = null;
            vm.showImageCloseBtn = true;
            vm.showImageAddBtn = true;
            vm.imageSaveForm = true;
            vm.showSaveBannerBtn = false;
            vm.featuredItemBtnText = "Save";
            vm.showCloseBannerBtn = false;
            $scope.showModal = true;
        }

        $scope.hide = function () {
            vm.activeFeaturedItem = null;
            vm.imageSaveForm = true;
            vm.imageTypeOption = false;
            vm.productCategoryOption = false;
            $scope.showModal = false;
        }

        function getAllParentCategory() {
            featuredItemService.getAllParentProductCategory()
                .then(function (result) {
                    if (result.success) {
                        vm.parentProductCategory = result.data;

                    } else {
                        console.log(result.message);
                    }
                });
        }

        function loadAllImageTypes() {
            featuredItemService.getImageTypes().then(function (result) {
                if (result.success) {
                    vm.imageOptionForBanners = result.data;
                } else {
                    console.log(result.message);
                }

            });
        }

        function init() {
            loadAllActiveFeaturedItem();
            getAllParentCategory();
            loadAllImageTypes();
        }
        function openDialogForFeaturedItemBanner(ev) {
            openDialog(ev, '#featuredItemBanner');
        }

        function addFeaturedItemBannerImage(ev) {
            vm.imageTypeOption = false;
            vm.productCategoryOption = false;
            vm.activeFeaturedItem = null;
            openDialogForFeaturedItemBanner(ev);

        }

        //crop for bannerImage
        $scope.bannerImage = '';
        $scope.bannerCroppedImage = '';
        vm.featuredItemName = '';
        $scope.$watch('bannerCroppedImage', function () {
        });

        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.bannerImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#fileInputForBannerImage')).on('change', handleFileSelect);

        function fileNameChangedForBanner(file) {
            vm.featuredItemName = file[0];
        }


        function changeImageSelectOption(imagetype, categoryId) {

            if (categoryId === undefined || imagetype === undefined) {
                return;
            } else {
                if (imagetype === 1) {
                    vm.resultImageSize = vm.ImageSizeForImagesOption[0];

                } else if (imagetype === 2) {
                    vm.resultImageSize = vm.ImageSizeForImagesOption[1];
                } else if (imagetype === 3) {
                    vm.resultImageSize = vm.ImageSizeForImagesOption[2];
                }
                loadBannerImagesIfExistsByProductCategoryId(imagetype, categoryId);
            }
        }

        function saveImagesForBanner(image, imageOption) {
            var spiltString = image.substring(0, 22);
            vm.imageForBanner = {
                FileName: vm.featuredItemName.name,
                ImageBase64: image.replace(spiltString, ''),
                ImageType: imageOption
            };
            vm.imageListForBanner.push(vm.imageForBanner);

            if (imageOption === 1) {
                if (vm.imageListForBanner.length === 1) {
                    vm.showCloseBannerBtn = true;
                    vm.showSaveBannerBtn = true;
                    vm.showImageCloseBtn = false;
                    vm.showImageAddBtn = false;
                }
                else {
                    swal("upload photo.");
                }
            }
            else if (imageOption === 2) {
                if (vm.imageListForBanner.length === 1 && vm.showBannerImages === true) {
                    vm.showCloseBannerBtn = true;
                    vm.showSaveBannerBtn = true;
                    vm.showImageCloseBtn = false;
                    vm.showImageAddBtn = false;
                }
                else if (vm.imageListForBanner.length === 1 && vm.showBannerImages === false) {
                    swal("upload remaining photo.");
                }
                else if (vm.imageListForBanner.length === 2) {
                    vm.showCloseBannerBtn = true;
                    vm.showSaveBannerBtn = true;
                    vm.showImageCloseBtn = false;
                    vm.showImageAddBtn = false;
                }
                else {
                    swal("upload remaining photo.");
                }
            }
            else if (imageOption === 3) {
                if (vm.imageListForBanner.length < 4 && vm.showBannerImages === true) {
                    vm.showCloseBannerBtn = true;
                    vm.showSaveBannerBtn = true;
                    vm.showImageCloseBtn = false;
                    vm.showImageAddBtn = false;
                }
                else if (vm.imageListForBanner.length < 4 && vm.showBannerImages === false) {
                    swal("upload remaining photo.");
                }
                else if (vm.imageListForBanner.length === 4) {
                    vm.showCloseBannerBtn = true;
                    vm.showSaveBannerBtn = true;
                    vm.showImageCloseBtn = false;
                    vm.showImageAddBtn = false;
                }
                else {
                    swal("upload remaining photo.");
                }
            }

            else {
                vm.showCloseBannerBtn = true;
                vm.showSaveBannerBtn = true;
                vm.showImageCloseBtn = false;
                vm.showImageAddBtn = false;
            }

        }

        function saveFeatureItemBannerImage(featuredItem) {
            featuredItem.bannerImage = vm.imageListForBanner;

            if (featuredItem.id === undefined) {
                featuredItemService.saveFeaturedItemBanner(featuredItem).then(function (result) {
                    if (result.success) {
                        $scope.bannerImage = '';
                        $scope.bannerCroppedImage = [];
                        vm.imageListForBanner.length = 0;
                        $scope.showModal = false;
                        loadAllActiveFeaturedItem();
                        vm.activeFeaturedItem = null;
                        document.getElementById("img").src = '';
                        var input = $("#fileInputForBannerImage");
                        input.replaceWith(input.val('').clone(true));
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        }
                        else {
                            swal(result.message);
                        }
                    }
                });
            }
            else {
                featuredItem.fullWidthImageUrls = null;
                featuredItem.halfWidthImageUrls = null;
                featuredItem.quaterWidthImageUrls = null;
                featuredItemService.updateFeaturedItemBanner(featuredItem).then(function (result) {
                    if (result.success) {
                        $scope.bannerImage = '';
                        vm.bannerCroppedImage = [];
                        vm.imageListForBanner.length = 0;
                        $scope.showModal = false;
                        loadAllActiveFeaturedItem();
                        vm.activeFeaturedItem = null;
                        document.getElementById("img").src = '';
                        var input = $("#fileInputForBannerImage");
                        input.replaceWith(input.val('').clone(true));
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        }
                        else {
                            swal(result.message);
                        }
                    }
                });
            }
        }

        function loadAllActiveFeaturedItem() {
            featuredItemService.getFeaturedItemBanner().then(function (result) {
                if (result.success) {
                    vm.allFeaturedItemList = result.data;
                }
            });


        }

        function removeFeatureItemBanner(featuredItemId, imageType, imageUrl) {
            var spiltString = imageUrl.substring(0, 6);
            var imageurl = imageUrl.replace(spiltString, '');
            var removedata = {
                featuredItemId: featuredItemId,
                imageTypeId: imageType,
                imageUrl: imageurl
            };
            featuredItemService.removeBannerImageFromBannerList(removedata).then(function (result) {
                if (result.success) {
                    vm.activeFeaturedItem = null;
                    vm.imageListForBanner = null;
                    $scope.showModal = false;
                    loadAllActiveFeaturedItem();
                } else {
                    console.log(result.message);
                  //  dialogMessage(result.message);
                }

            });

        }

        function loadBannerImagesIfExistsByProductCategoryId(imageType, categoryId) {
            featuredItemService.getBannerImagesByCategoryIdAndImageTypeId(imageType, categoryId).then(function (result) {
                if (result.success) {
                    if (result.data !== "") {
                        if (result.data[0].imageType === 1) {
                            if (result.data[0].fullWidthImageUrls) {
                                vm.showBannerImages = false;
                                swal("There is already banner images for this product. choose different products and category.");
                            }
                        }
                        else if (result.data[0].imageType === 2) {
                            if (result.data[0].halfWidthImageUrls) {
                                vm.showBannerImages = false;
                                swal("There is already banner images for this product. choose different products and category.");
                            }
                        }
                        else if (result.data[0].imageType === 3) {
                            if (result.data[0].quaterWidthImageUrls) {
                                vm.showBannerImages = false;
                                swal("There is already banner images for this product. choose different products and category.");
                            }
                        }
                    }
                    vm.bannercategoryId = categoryId;
                }
            });
        }

        function editFeaturedItem(featuredItem, checked) {
            vm.featuredItemBtnText = "Update";
            // for imagetype size
            if (featuredItem.imageType === 1) {
                vm.resultImageSize = vm.ImageSizeForImagesOption[0];
            }
            else if (featuredItem.imageType === 2) {
                vm.resultImageSize = vm.ImageSizeForImagesOption[1];
            }
            else if (featuredItem.imageType === 3) {
                vm.resultImageSize = vm.ImageSizeForImagesOption[2];
            }
            // data loading
            if (checked) {
                featuredItemService.getFeaturedItemById(featuredItem.id).then(function (result) {
                    vm.activeFeaturedItem= [];
                    if (result.success) {
                        if (result.data.imageType === 1) {
                            if (result.data.fullWidthImageUrls) {
                                for (var i = 0; i < result.data.fullWidthImageUrls.length; i++) {
                                    vm.activeFeaturedItem.fullWidthImageUrls = result.data.fullWidthImageUrls[i];
                                }
                                vm.showBannerImages = true;
                                vm.imageSaveForm = false;
                                vm.showImageCloseBtn = false;
                                vm.showImageAddBtn = false;
                                vm.showCloseBannerBtn = true;
                                vm.showSaveBannerBtn = true;
                            }
                            else {
                                vm.showBannerImages = false;
                                vm.imageSaveForm = true;
                                vm.showImageCloseBtn = true;
                                vm.showImageAddBtn = true;
                                vm.showCloseBannerBtn = false;
                                vm.showSaveBannerBtn = false;
                            }
                        }

                        else if (result.data.imageType === 2) {
                            if (result.data.halfWidthImageUrls) {
                                for (var j = 0; j < result.data.halfWidthImageUrls.length; j++) {
                                    vm.activeFeaturedItem.halfWidthImageUrls = result.data.halfWidthImageUrls[j];
                                }
                                vm.showBannerImages = true;
                                vm.imageSaveForm = false;
                                vm.showImageCloseBtn = false;
                                vm.showImageAddBtn = false;
                                vm.showCloseBannerBtn = true;
                                vm.showSaveBannerBtn = true;
                                if (result.data.halfWidthImageUrls.length === 1) {
                                    vm.showBannerImages = true;
                                    vm.imageSaveForm = true;
                                    vm.showImageCloseBtn = true;
                                    vm.showImageAddBtn = true;
                                    vm.showCloseBannerBtn = false;
                                    vm.showSaveBannerBtn = false;
                                }
                            }

                            else {
                                vm.showBannerImages = false;
                                vm.imageSaveForm = true;
                                vm.showImageCloseBtn = true;
                                vm.showImageAddBtn = true;
                                vm.showCloseBannerBtn = false;
                                vm.showSaveBannerBtn = false;
                            }
                        }
                        else if (result.data.imageType === 3) {
                            if (result.data.quaterWidthImageUrls) {
                                for (var k = 0; k < result.data.quaterWidthImageUrls.length; k++) {
                                    vm.activeFeaturedItem.quaterWidthImageUrls[k] = '../../' + result.data.quaterWidthImageUrls[k];
                                }
                                vm.showBannerImages = true;
                                vm.imageSaveForm = false;
                                vm.showImageCloseBtn = false;
                                vm.showImageAddBtn = false;
                                vm.showCloseBannerBtn = true;
                                vm.showSaveBannerBtn = true;

                                if (result.data.quaterWidthImageUrls.length < 4) {
                                    vm.showBannerImages = true;
                                    vm.imageSaveForm = true;
                                    vm.showImageCloseBtn = true;
                                    vm.showImageAddBtn = true;
                                    vm.showCloseBannerBtn = false;
                                    vm.showSaveBannerBtn = false;
                                }
                            }

                            else {
                                vm.showBannerImages = false;
                                vm.imageSaveForm = true;
                                vm.showImageCloseBtn = true;
                                vm.showImageAddBtn = true;
                                vm.showCloseBannerBtn = false;
                                vm.showSaveBannerBtn = false;
                            }
                        }
                        vm.activeFeaturedItem = result.data;
                        $scope.showModal = true;
                        vm.imageTypeOption = true;
                        vm.productCategoryOption = true;
                    } else {
                        console.log(result.message);;
                    }
                });
            }
            else {
                swal("You cann't edit this item.please activate it first.");
            }
        }
    }
})();