(function() {
    angular
        .module("app-themeSetting")
        .controller("themeSettingController", themeSettingController);
    themeSettingController.$inject = ['$http', '$filter', '$scope', '$base64', '$timeout', 'Cropper', 'themeSettingService'];

    function themeSettingController($http, $filter, $scope, $base64, $timeout, Cropper, themeSettingService) {
        var vm = this;
        //general settings
        vm.getGeneralSettings = getGeneralSettings;
        vm.clearImage = clearImage;
        vm.saveGeneralSettings = saveGeneralSettings;
        vm.themes = [
            { id: 1, name: "Blue(Default)" },
            { id: 2, name: "Pink" }
        ];
        //general Code Section Start from here
        getGeneralSettings();
        function getGeneralSettings() {
            getGeneralColor();
            themeSettingService.getGeneralSetting().then(function(result) {
                if (result.success) {
                    vm.generalSettings = result.data;
                    if (result.data.logoUrl) {
                        $scope.storeLogoBase64 = '../../' + result.data.logoUrl;
                    }
                    if (result.data.faviconLogoUrl) {
                        $scope.fevIconBase64 = '../../' + result.data.faviconLogoUrl;
                    }
                } else {
                    console.log("Cannot get general settings");
                }
            });
        }
        function getGeneralColor() {
            themeSettingService.getThemeColor().then(function (result) {
                if (result.success) {
                    vm.ThemeColor = result.data;
                    // code remaining
                } else {
                    console.log("Cannot get theme Color");
                }
            });
        }

        var fevIconSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            vm.fevIconExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.fevIconBase64 = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        var storeLogoSelected = function (evt) {
            
            var file = evt.currentTarget.files[0];
            vm.storeLogoExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.storeLogoBase64 = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#fevIconFileInput')).on('change', fevIconSelected);
        angular.element(document.querySelector('#storeLogoFileInput')).on('change', storeLogoSelected);

        

        function saveGeneralSettings() {
            if ($scope.storeLogoBase64) {
                $scope.storeLogoImage = {
                        FileName: 'logo.' + vm.storeLogoExtensition,
                        ImageBase64: $scope.storeLogoBase64.substring($scope.storeLogoBase64.indexOf(",") + 1),
                        ImageType: 1
                }
            }
            if ($scope.fevIconBase64) {
                $scope.fevImage = {
                    FileName: 'favicon.' + vm.fevIconExtensition,
                    ImageBase64: $scope.fevIconBase64.substring($scope.fevIconBase64.indexOf(",") + 1),
                    ImageType: 2
                }
            }
            
            vm.generalSettings.logoImage = $scope.storeLogoImage;
            vm.generalSettings.faviconLogoImage = $scope.fevImage;
            if (vm.generalSettings.id) {
                themeSettingService.updateGeneralOptions(vm.generalSettings).then(function(result) {
                    if (result.success) {
                        alert("Genral Setting updated");
                        getGeneralSettings();
                    } else {
                        console.log("Cannot update general settings. error are: " + result.message);
                    }
                });
            } else {
                themeSettingService.saveGeneralOptions(vm.generalSettings).then(function(result) {
                    if (result.success) {
                        alert("Genral Setting saved");
                        getGeneralSettings();
                    } else {
                        console.log("Cannot create general settings. error are: " + result.message);
                    }
                });
            }
        }

        //general Code Section End from here
        vm.getHeaderSettings = getHeaderSettings;
        vm.addStoreInLocal = addStoreInLocal;
        vm.viewStoreDetails = viewStoreDetails;
        vm.updateStoreInLocal = updateStoreInLocal;
        vm.removeStoreDetails = removeStoreDetails;
        vm.saveHeaderSettings = saveHeaderSettings;
        vm.storeDataLocal = {};
        vm.storeLocalValue = [];
        function getHeaderSettings() {
            themeSettingService.getHeaderSetting().then(function(result) {
                if (result.success) {
                    vm.headerSettings = result.data;
                    if (!(result.data.storeLocatorViewModels.length > 0)) {
                        vm.headerSettings.storeLocatorViewModels = [];
                    }
                } else {
                    console.log("Error getting settings for header. Error are: " + result.message);
                }
            });
        }
        $scope.closeAddStoreDialog = function () {
            $('#addStore').modal('hide');
            vm.isStoreEditing = false;
        }
        $scope.openAddStoreDialog = function () {
            if (vm.headerSettings.storeLocatorViewModels) {
                if (vm.headerSettings.storeLocatorViewModels.length < vm.headerSettings.numberOfStores) {
                    $('#addStore').modal('show');
                    vm.storeDataLocal = null;
                } else {
                    alert("Sorry you have selected " + vm.headerSettings.numberOfStores + " store limit.");
                }
                vm.isStoreEditing = false;
            }
            else {
                $('#addStore').modal('show');
                vm.storeDataLocal = null;
            }
            
        }

        function addStoreInLocal(data) {
            if (vm.headerSettings.storeLocatorViewModels) {
                vm.headerSettings.storeLocatorViewModels.push(data);
            } else {
                vm.headerSettings.storeLocatorViewModels = [];
                vm.headerSettings.storeLocatorViewModels.push(data);
            }
            
            $scope.closeAddStoreDialog();
        }

        function viewStoreDetails(data, index) {
            vm.editedIndex = index;
            vm.storeDataLocal = data;
            vm.isStoreEditing = true;
            $('#addStore').modal('show');
        }

        function updateStoreInLocal(data) {
            vm.headerSettings.storeLocatorViewModels[vm.editedIndex] = data;
            $('#addStore').modal('hide');
        }

        function removeStoreDetails(index, id) {
            if (id) {
                themeSettingService.removeStoreLocator(id).then(function (result) {
                    if (result.success) {
                        getHeaderSettings();
                    } else {
                        console.log("Remove store locator error. Error are: " + result.message);
                    }
                });
            } else {
                vm.headerSettings.storeLocatorViewModels.splice(index, 1);
            }
        }

        function saveHeaderSettings(data) {
            if (data.id) {
                themeSettingService.updateHeaderSettings(data).then(function(result) {
                    if (result.success) {
                        alert("Header Settings updated");
                        getHeaderSettings();
                    } else {
                        console.log("Header Settings saved error. Error are: " + result.message);
                    }
                });
            } else {
                themeSettingService.saveHeaderSettings(data).then(function(result) {
                    if (result.success) {
                        alert("Header Settings saved");
                        getHeaderSettings();
                    }
                    else {
                        console.log("Header settings update error. Error are: " + result.message);
                    }
                });
            }
        }

        //footersettings code start from here
        vm.getFooterSettings = getFooterSettings;
        vm.saveFooterSettings = saveFooterSettings;
        vm.latestOrTrandingRadioList = [
            { id: 1, value: 1, text: "Show Trending Products" }, { id: 2, value: 2, text: "Show Latest Products" }
        ];

        function getFooterSettings() {
            themeSettingService.getAllFooterDetails().then(function(result) {
                if (result.success) {
                    vm.footerSettings = result.data;
                    if (result.data.themeBrandImageUrls) {
                        $scope.brandImagesBase64 = [];
                        for (var i = 0; i < result.data.themeBrandImageUrls.length; i++) {
                            $scope.brandImagesBase64.push('../../' + result.data.themeBrandImageUrls[i]);
                        }
                    }
                    if (result.data.footerLogoUrl) {
                        $scope.footerLogoBase64 = '../../' + result.data.footerLogoUrl;
                    }
                    
                } else {
                    console.log("get footer settings error. Errors are: " + result.message);
                }
            });
        }
        var brandImagesSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    if ($scope.brandImagesBase64) {
                        $scope.brandImagesBase64.push(evt.target.result);
                    } else {
                        $scope.brandImagesBase64 = [];
                        $scope.brandImagesBase64.push(evt.target.result);
                    }
                    if ($scope.tempBrandImagesBase64) {
                        $scope.tempBrandImagesBase64.push(evt.target.result);
                    } else {
                        $scope.tempBrandImagesBase64 = [];
                        $scope.tempBrandImagesBase64.push(evt.target.result);
                    }
                    $("#brandImagesFileInput").replaceWith($("#brandImagesFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        var footerLogoSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            vm.footerImagesExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.footerLogoBase64 = evt.target.result;
                    $("#footerLogoFileInput").replaceWith($("#footerLogoFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#brandImagesFileInput')).on('change', brandImagesSelected);
        angular.element(document.querySelector('#footerLogoFileInput')).on('change', footerLogoSelected);

        function saveFooterSettings(data) {
            if ($scope.footerLogoBase64) {
                data.footerImage = {
                    FileName: 'Footer_logo.' + vm.footerImagesExtensition,
                    ImageBase64: $scope.footerLogoBase64.substring($scope.footerLogoBase64.indexOf(",") + 1),
                    ImageType: 3
                }
            }
            if ($scope.tempBrandImagesBase64) {
                for (var j = 0; j < $scope.tempBrandImagesBase64.length; j++) {
                    var brandsImages = {
                        FileName: 'brand-' + Math.floor(Math.random()*90000) + 10000 + '.' + $scope.tempBrandImagesBase64[j].substring("data:image/".length, $scope.tempBrandImagesBase64[j].indexOf(";base64")),
                        ImageBase64: $scope.tempBrandImagesBase64[j].substring($scope.tempBrandImagesBase64[j].indexOf(",") + 1),
                        ImageType: 4
                    }
                    if (data.themeBrandImages) {
                        data.themeBrandImages.push(brandsImages);
                    } else {
                        data.themeBrandImages = [];
                        data.themeBrandImages.push(brandsImages);
                    }
                }
            }
            if (data.id) {
                themeSettingService.updateFooterSetting(data).then(function(result) {
                    if (result.success) {
                        alert("updated");
                        $scope.tempBrandImagesBase64 = null;
                        getFooterSettings();
                    } else {
                        console.log("Save footer settings error. Error are: " + result.message);
                    }
                });
            } else {
                themeSettingService.saveFooterSetting(data).then(function(result) {
                    if (result.message) {
                        alert("saved");
                        $scope.tempBrandImagesBase64 = null;
                        getFooterSettings();
                    }
                });
            }
        }

        //Layout settings code here
        vm.getLayoutSettings = getLayoutSettings;
        vm.saveLayoutSetting = saveLayoutSetting;
        vm.titleList = [
            { id: 1, value: 1, text: "Horizontal (Store items come below the title) " }, { id: 2, value: 2, text: "Inline (Store items come alongside the title) " }
        ];
        vm.getInspiredRadioList = [
            { id: 1, value: 1, text: "Background Image" }, { id: 2, value: 2, text: "Background Color" }
        ];
        function getLayoutSettings() {
            getCategories();
            themeSettingService.getLayoutSettings().then(function(result) {
                if (result.success) {
                    vm.layoutSettings = result.data;
                } else {
                    console.log("Error retriving layout settings. Error are: " + result.message);
                }
            });
        }

        function getCategories() {
            themeSettingService.getAllCategories().then(function(result) {
                if (result.success) {
                    vm.categories = result.data;
                }
                else {
                    console.log("Error gettings categories. Error are: " + result.message);
                }
            });
        }
        var trendingParallexSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            vm.trendingParallexImagesExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.trendingParallexBase64 = evt.target.result;
                    $("#trendingParallexFileInput").replaceWith($("#trendingParallexFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#trendingParallexFileInput')).on('change', trendingParallexSelected);

        var hotThisWeekParallexSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            vm.trendingParallexImagesExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.hotThisWeekParallexBase64 = evt.target.result;
                    $("#hotThisWeekParallexFileInput").replaceWith($("#hotThisWeekParallexFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#hotThisWeekParallexFileInput')).on('change', hotThisWeekParallexSelected);

        var latestProductParallexSelected = function (evt) {
            var file = evt.currentTarget.files[0];
            vm.trendingParallexImagesExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.latestProductParallexBase64 = evt.target.result;
                    $("#latestProductFileInput").replaceWith($("#latestProductFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#latestProductFileInput')).on('change', latestProductParallexSelected);

        //image crop trending image
        
        $scope.trendingImage = '';
        $scope.trendingImageBase64 = '';

        var trendingImageSelect = function (evt) {
            var file=evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function($scope){
                    $scope.cropFor = 'trending';
                    onfileSelect(file, evt.target.result);
                    $("#trendingImageFileInput").replaceWith($("#trendingImageFileInput").val('').clone(true));
                });
            };
            
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#trendingImageFileInput')).on('change', trendingImageSelect);

        //Image crop for hot this week
        $scope.hotThisWeekImage = '';
        $scope.hotThisWeekImageBase64 = '';

        var hotThisWeekImageSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.cropFor = 'hotThisWeek';
                    onfileSelect(file, evt.target.result);
                    $("#hotThisWeekImageFileInput").replaceWith($("#hotThisWeekImageFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#hotThisWeekImageFileInput')).on('change', hotThisWeekImageSelect);

        //latest products image crop
        $scope.latestProductsImage = '';
        $scope.latestProductsImageBase64 = '';

        var latestProductsImageSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.cropFor = 'latestProduct';
                    onfileSelect(file, evt.target.result);
                    $("#latestProductsFileInput").replaceWith($("#latestProductsFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#latestProductsFileInput')).on('change', latestProductsImageSelect);

        $scope.getInspiredImage = '';
        $scope.getInspiredImageBase64 = '';

        var getInspiredImageSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.cropFor = 'getInspired';
                    onfileSelect(file, evt.target.result);
                    $("#getInspiredFileInput").replaceWith($("#getInspiredFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#getInspiredFileInput')).on('change', getInspiredImageSelect);

        function saveLayoutSetting(data) {
            if (vm.recommendationList) {
                var personnalImages = [];
                for (var i = 0; i < vm.recommendationList.length; i++) {
                    var personnelDetails = vm.recommendationList[i];
                    var personalImage = {
                        FileName: 'Personal_Image.' + vm.recommendationList[i].personnelImage.substring("data:image/".length, vm.recommendationList[i].personnelImage.indexOf(";base64")),
                        ImageBase64: vm.recommendationList[i].personnelImage.substring(vm.recommendationList[i].personnelImage.indexOf(",") + 1),
                        ImageType: 9
                    }
                    personnelDetails.personnelImage = personalImage;
                    if (data.PersonnelSettingViewModel) {
                        data.PersonnelSettingViewModel.push(personnelDetails);
                    }
                    else {
                        data.PersonnelSettingViewModel = [];
                        data.PersonnelSettingViewModel.push(personnelDetails);
                    }

                }
            }
            if (data.id) {
                themeSettingService.updateLayoutSettings(data).then(function (result) {
                    if (result.success) {
                        getLayoutSettings();
                    } else {
                        console.log("Update layout settings error. Error are : " + result.message);
                    }
                });
            } else {
                themeSettingService.saveLayoutSettings(data).then(function (result) {
                    if (result.success) {
                        getLayoutSettings();
                    } else {
                        console.log("Save layout settings error. Error are : " + result.message);
                    }
                });
            }
        }

        //recomendation code from here
        vm.showPsRecommendationDialog = showPsRecommendationDialog;
        vm.hidePsRecommendationDialog = hidePsRecommendationDialog;
        vm.addRecommendation = addRecommendation;
        vm.viewRecommendationDetails = viewRecommendationDetails;
        vm.removeRecommendation = removeRecommendation;

        function showPsRecommendationDialog() {
            $('#addRecomendation').modal('show');
            vm.isRecomendationEditing = false;
        }
        function hidePsRecommendationDialog() {
            vm.recommendation = null;
            vm.hasReccomandationImage = false;
            $scope.reccomandationImage = '';
            $scope.reccomandationImageBase64 = '';
            $('#addRecomendation').modal('hide');
            vm.isRecomendationEditing = false;
            vm.recomendationIndex = null;
        }
        $scope.reccomandationImage = '';
        $scope.reccomandationImageBase64 = '';

        var reccomandationImageSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.cropFor = 'reccomandation';
                    recomendationCrop(file, evt.target.result);
                    $("#reccomandationFileInput").replaceWith($("#reccomandationFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#reccomandationFileInput')).on('change', reccomandationImageSelect);

        function addRecommendation(recomendation) {
            recomendation.personnelImage = $scope.reccomandationImageBase64;
            recomendation.layoutSettingId = vm.layoutSettings.id;
            if (vm.recomendationIndex >= 0) {
                vm.recommendationList[index] = recomendation;
            } else {
                if (vm.recommendationList) {
                    vm.recommendationList.push(recomendation);
                } else {
                    vm.recommendationList = [];
                    vm.recommendationList.push(recomendation);
                }
                
            }
            vm.recommendation = null;
            vm.hasReccomandationImage = false;
            $scope.reccomandationImage = '';
            $scope.reccomandationImageBase64 = '';
            $('#addRecomendation').modal('hide');
        }

        function viewRecommendationDetails(recomendation, index) {
            vm.recomendationIndex = index;
            vm.recommendation = recomendation;
            vm.hasReccomandationImage = true;
            $scope.reccomandationImageBase64 = recomendation.personnelImage;
            vm.isRecomendationEditing = true;
            $('#addRecomendation').modal('show');
        }

        function removeRecommendation(index) {
            vm.recommendationList.splice(index, 1);
        }

        vm.getSliderSettings = getSliderSettings;
        vm.saveSlideSetting = saveSlideSetting;
        vm.selectProduct = selectProduct;
        vm.saveConfiguration = saveConfiguration;
        vm.saveSliderSetting = saveSliderSetting;
        vm.viewSlideConfigurationDetails = viewSlideConfigurationDetails;
        vm.cancelConfiguration = cancelConfiguration;
        vm.slideConfigurationNumbers = [
            { id: 1, name: "One" }, { id: 2, name: "Two" }, { id: 3, name: "Three" }, { id: 4, name: "Four" },
            { id: 5, name: "Five" }
        ];
        vm.parallaxRadioList = [
            { id: 1, value: 1, text: "Parallax Background" }, { id: 2, value: 2, text: "Fixed Background" }
        ];
        function getSliderSettings() {
            getDiscountProducts();
            themeSettingService.getSlideSettings().then(function(result) {
                if (result.success) {
                    vm.slideConfigurationList = result.data.individualSlideSettingViewModels;
                    vm.slideSetting = result.data;
                } else {
                    console.log("Get slider settings error. Error are : " + result.message);
                }
            });
        }

        function getDiscountProducts() {
            themeSettingService.getAllDiscountProduct().then(function(result) {
                if (result.success) {
                    vm.discountProducts = result.data;
                } else {
                    console.log("Error retriving discount products. Error are : " + result.message);
                }
            });
        }


        var sliderParallaxSelected = function (evt) {

            var file = evt.currentTarget.files[0];
            vm.storeLogoExtensition = file.name.split('.').pop();
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.sliderParallaxBase64 = evt.target.result;
                    $("#sliderParallaxFileInput").replaceWith($("#sliderParallaxFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };

        angular.element(document.querySelector('#sliderParallaxFileInput')).on('change', sliderParallaxSelected);

        $scope.slideSettingImage = '';
        $scope.slideSettingBase64 = '';

        var slideSettingSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.cropFor = 'slideSetting';
                    onfileSelect(file, evt.target.result);
                    $("#slideSettingFileInput").replaceWith($("#slideSettingFileInput").val('').clone(true));
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#slideSettingFileInput')).on('change', slideSettingSelect);

        function saveSlideSetting() {
            vm.hasSlideSettingImage = true;
            $scope.slideSettingImage = '';
        }

        function selectProduct(productId) {
            themeSettingService.getProductDetails(productId).then(function(result) {
                if (result.success) {
                    if (result.data.originalPrice != null || result.data.originalPrice > 0) {
                        vm.slideSetting.individualSlideSettingViewModels.originalPrice = result.data.originalPrice;
                        vm.slideSetting.individualSlideSettingViewModels.originalPriceCheck = true;
                    }
                    if (result.data.discountPercentage !== null || result.data.discountPercentage > 0) {
                        vm.slideSetting.individualSlideSettingViewModels.discountPercentage = result.data.discountPercentage;
                        vm.slideSetting.individualSlideSettingViewModels.discountPercentageCheck = true;
                    }
                    vm.slideSetting.individualSlideSettingViewModels.salesPrice = result.data.salePrice;
                    vm.slideSetting.individualSlideSettingViewModels.IsExpires = result.data.expires;
                    vm.slideSetting.individualSlideSettingViewModels.expireDate = moment(result.data.expiryDate).format("YYYY-MM-DD");
                }
            });
        }

        vm.slideConfigurationList = [];
        vm.showConfigForm = false;
        function saveConfiguration(configuration, index) {
            
            if ($scope.slideSettingBase64) {
                configuration.SlideBackgroundImage = {
                    FileName: "Test",
                    ImageBase64: $scope.slideSettingBase64,
                    ImageType: 11
                }
            }
            if (vm.slideConfigurationList) {
                vm.slideConfigurationList.push(configuration);
            } else {
                vm.slideConfigurationList = [];
                vm.slideConfigurationList.push(configuration);
            }
            if (vm.slideConfigurationList.length >= vm.slideSetting.numberOfSlides) {
                vm.showConfigForm = false;
            } else {
                vm.showConfigForm = true;
            }
            vm.slideSetting.individualSlideSettingViewModels = {};
        }

        vm.sildeNumberSelect = sildeNumberSelect;

        function sildeNumberSelect(number) {
            vm.showConfigForm = true;
        }

        function saveSliderSetting(slideSetting) {
            slideSetting.individualSlideSettingViewModels = [];
            for (var i = 0; i < vm.slideConfigurationList.length; i++) {
                slideSetting.individualSlideSettingViewModels.push(vm.slideConfigurationList[i]);
            }
            if (slideSetting.id) {
                themeSettingService.updateSlideSetting(slideSetting).then(function(result) {
                    if (result.success) {
                        alert("Slide setting saved");
                        getSliderSettings();
                    }
                });
            } else {
                themeSettingService.saveSlideSetting(slideSetting).then(function (result) {
                    if (result.success) {
                        alert("Slide Setting saved");
                        getSliderSettings();
                    }
                });
            }
            
        }

        function viewSlideConfigurationDetails(data, index) {
            vm.showConfigForm = true;
            vm.slideSetting.individualSlideSettingViewModels = data;
        }
        function cancelConfiguration() {
            vm.showConfigForm = false;
            vm.slideSetting.individualSlideSettingViewModels = null;
        }
        function clearImage(name, id, index) {
            if (name === 'fevIcon') {
                if (id) {
                    themeSettingService.removeFavicon(id).then(function (result) {
                        if (result.success) {
                            $scope.fevIconBase64 = null;
                            getGeneralSettings();
                        } else {
                            console.log("removing favicon error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.fevIconBase64 = null;
                }
            }
            else if (name === 'storeLogo') {
                if (id) {
                    themeSettingService.removeLogoImage(id).then(function (result) {
                        if (result.success) {
                            $scope.storeLogoBase64 = null;
                            getGeneralSettings();
                        } else {
                            console.log("removing logo error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.storeLogoBase64 = null;
                }
            }
            else if (name === 'brandImage') {
                if (id) {
                    themeSettingService.deleteBrandImage(id, vm.footerSettings.themeBrandImageUrls[index])
                        .then(function(result) {
                            if (result.success) {
                                getFooterSettings();
                            } else {
                                console.log("Delete brand image error. Error are: " + result.message);
                            }
                        });
                } else {
                    $scope.brandImagesBase64.splice(index, 1);
                    $scope.tempBrandImagesBase64.splice(index, 1);
                }
            }
            else if (name === 'footerLogo') {
                if (id) {
                    themeSettingService.removeFooterLogo(id).then(function(result) {
                        if (result.success) {
                            getFooterSettings();
                        } else {
                            console.log("remove footer logo error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.footerLogoBase64 = null;
                }
            }
            else if (name === 'trendingParallex') {
                if (id) {
                    // API not ready yet
                } else {
                    $scope.trendingParallexBase64 = null;
                }
            }
            else if (name === 'hotThisWeekParallex') {
                if (id) {
                    // API not ready yet
                } else {
                    $scope.hotThisWeekParallexBase64 = null;
                }
            }
            else if (name === 'latestProductParallex') {
                if (id) {
                    // API not ready yet
                } else {
                    $scope.latestProductParallexBase64 = null;
                }
            }
            else if (name === 'trendingImage') {
                if (id) {
                    themeSettingService.removeTrendingImage(vm.layoutSettings.trendingItemsImageUrl, id).then(function (result) {
                        if (result.success) {
                            getFooterSettings();
                        } else {
                            console.log("remove footer logo error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.trendingImage = '';
                    $scope.trendingImageBase64 = '';
                    vm.hasTrendingImage = false;
                }
            }
            else if (name === 'hotThisWeekImage') {
                if (id) {
                    themeSettingService.removeHotImage(vm.layoutSettings.hotThisWeekImageUrl, id).then(function (result) {
                        if (result.success) {
                            getFooterSettings();
                        } else {
                            console.log("remove footer logo error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.hotThisWeekImage = '';
                    $scope.hotThisWeekImageBase64 = '';
                    vm.hasHotThisWeekImage = false;
                }
            }
            else if (name === 'latestProducts') {
                if (id) {
                    themeSettingService.removeProductItemImage(vm.layoutSettings.latestProductsImageUrl, id).then(function (result) {
                        if (result.success) {
                            getFooterSettings();
                        } else {
                            console.log("remove latest products logo error. Error are: " + result.message);
                        }
                    });
                } else {
                    $scope.latestProductsImage = '';
                    $scope.latestProductsImageBase64 = '';
                    vm.hasLatestProductsImage = false;
                }
            }
            else if (name === 'getInspired') {
                if (id) {
                    //API not ready
                } else {
                    $scope.getInspiredImage = '';
                    $scope.getInspiredImageBase64 = '';
                    vm.hasGetInspiredImage = false;
                }
            }
            else if (name === 'sliderParallax') {
                if (id) {
                    //API not ready
                } else {
                    $scope.sliderParallaxBase64 = null;
                }
            }
            else if (name === 'slideSetting') {
                if (id) {
                    //API not ready
                } else {
                    $scope.slideSettingImage = '';
                    $scope.slideSettingBase64 = '';
                    vm.hasSlideSettingImage = false;
                }
            }
            else if (name === 'reccomandation') {
                if (id) {
                    //API not ready
                } else {
                    $scope.dataUrl = null;
                    $scope.recomendationImage = '';
                    $scope.reccomandationImageBase64 = '';
                    vm.hasReccomandationImage = false;
                }
            }
        }
        var file, data;
        function onfileSelect(blob, fileUrl) {
            file = blob;
            $scope.dataUrl = fileUrl;
            $timeout(showCropper);  // wait for $digest to set image's src
            $('#imageCropDialog').modal('show');
        }
        function recomendationCrop(blob, fileUrl) {
            vm.hasReccomandationImage = true;
            file = blob;
            $scope.dataUrl = fileUrl;
            $timeout(showCropper);  // wait for $digest to set image's src
            //$('#imageCropDialog').modal('show');
        }

        vm.closeCropDialog = closeCropDialog;
        function closeCropDialog() {
            $scope.dataUrl = null;
            $('#imageCropDialog').modal('hide');
        }
        $scope.cropper = {};
        $scope.cropperProxy = 'cropper.first';
        vm.saveCropedImage = saveCropedImage;
        function saveCropedImage() {
            if (!file || !data) return;
            Cropper.crop(file, data).then(Cropper.encode).then(function (dataUrl) {
                if ($scope.cropFor === 'trending') {
                    vm.hasTrendingImage = true;
                    $scope.trendingImageBase64 = dataUrl;
                    closeCropDialog();
                }
                else if ($scope.cropFor === 'hotThisWeek') {
                    vm.hasHotThisWeekImage = true;
                    $scope.hotThisWeekImageBase64 = dataUrl;
                    closeCropDialog();
                }
                else if ($scope.cropFor === 'latestProduct') {
                    vm.hasLatestProductsImage = true;
                    $scope.latestProductsImageBase64 = dataUrl;
                    closeCropDialog();
                }
                else if ($scope.cropFor === 'getInspired') {
                    vm.hasGetInspiredImage = true;
                    $scope.getInspiredImageBase64 = dataUrl;
                    closeCropDialog();
                }
                else if ($scope.cropFor === 'slideSetting') {
                    vm.hasSlideSettingImage = true;
                    $scope.slideSettingBase64 = dataUrl;
                    closeCropDialog();
                }
                else if ($scope.cropFor === 'reccomandation') {
                    vm.hasReccomandationImage = true;
                    $scope.reccomandationImageBase64 = dataUrl;
                    $scope.dataUrl = '';
                    if (!$scope.cropper.first) return;
                    $scope.cropper.first('clear');
                }
            });
        }

        $scope.options = {
            maximize: true,
            aspectRatio: 2 / 1,
            crop: function (dataNew) {
                data = dataNew;
            }
        };
        $scope.showEvent = 'show';
        $scope.hideEvent = 'hide';
        function showCropper() { $scope.$broadcast($scope.showEvent); }
        function hideCropper() { $scope.$broadcast($scope.hideEvent); }
    }
})();