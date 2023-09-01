(function () {
    'use strict';

    angular
        .module('app-themeSetting')
        .service('themeSettingService', themeSettingService);

    themeSettingService.$inject = ['$http'];

    function themeSettingService($http) {
        var service = {};
        service.saveGeneralOptions = saveGeneralOptions;
        service.getGeneralSetting = getGeneralSetting;
        service.updateGeneralOptions = updateGeneralOptions;
        service.removeLogoImage = removeLogoImage;
        service.removeFavicon = removeFavicon;
        service.getThemeColor = getThemeColor;
        //services for footer settings
        service.saveFooterSetting = saveFooterSetting;
        service.getAllFooterDetails = getAllFooterDetails;
        service.updateFooterSetting = updateFooterSetting;
        service.deleteBrandImage = deleteBrandImage;
        service.removeFooterLogo = removeFooterLogo;
        service.getAllProduct = getAllProduct;
        

        //services for header settings
        service.getHeaderSetting = getHeaderSetting;
        service.saveHeaderSettings = saveHeaderSettings;
        service.updateHeaderSettings = updateHeaderSettings;
        service.removeStoreLocator = removeStoreLocator;

        //services for layout settings
        service.updateLayoutSettings = updateLayoutSettings;
        service.saveLayoutSettings = saveLayoutSettings;
        service.getAllCategories = getAllCategories;
        service.getLayoutSettings = getLayoutSettings;
        service.removeTrendingImage = removeTrendingImage;
        service.removeHotImage = removeHotImage;
        service.removeTrendingParallaxImage = removeTrendingParallaxImage;
        service.removeHotParallaxImage = removeHotParallaxImage;
        service.removeProductParallaxImage = removeProductParallaxImage;
        service.removerecommendImage = removerecommendImage;
        service.removeProductItemImage = removeProductItemImage;
            
        //services for slide settings
        service.updateSlideSetting = updateSlideSetting;
        service.saveSlideSetting = saveSlideSetting;
        service.getProductDetails = getProductDetails;
        service.getAllDiscountProduct = getAllDiscountProduct;
        service.getSlideSettings = getSlideSettings;
        


        return service;

        //general options setting
        function getGeneralSetting() {
            return $http.get('/api/GeneralSetting/getAll').then(handleSuccess, handleError);
        }
        function getThemeColor() {
            return $http.get('/api/GeneralSetting/getThemeColor').then(handleSuccess, handleError);
        }

        function getAllProduct() {
            return $http.get('/api/Product/getallactiveproducts').then(handleSuccess, handleError);
        }
        function removeLogoImage(id) {
            return $http.put('/api/GeneralSetting/removeLogo/' + id).then(handleSuccess, handleError);
        }
        function removeFavicon(id) {
            return $http.put('/api/GeneralSetting/removeFavicon/' + id).then(handleSuccess, handleError);
        }
        function saveGeneralOptions(items) {
            return $http.post('/api/GeneralSetting/create', items).then(handleSuccess, handleError);
        }
        function updateGeneralOptions(items) {
            return $http.put('/api/GeneralSetting/update', items).then(handleSuccess, handleError);
        }
        //header settings

        function saveHeaderSettings(headerSetting) {
            return $http.post('/api/HeaderSetting/create', headerSetting).then(handleSuccess, handleError);
        }
        function updateHeaderSettings(items) {
            return $http.put('/api/HeaderSetting/update', items).then(handleSuccess, handleError);
        }
        function getHeaderSetting() {
            return $http.get('/api/HeaderSetting/getHeaderSetting').then(handleSuccess, handleError);
        }

        function removeStoreLocator(id) {
            return $http.put('/api/HeaderSetting/deleteStoreLocator/' + id).then(handleSuccess, handleError);
        }
        
       //footer settings

        function saveFooterSetting(items) {
            return $http.post('/api/FooterSetting/Create', items).then(handleSuccess, handleError);
        }
        function getAllFooterDetails() {
            return $http.get('/api/FooterSetting/getAll').then(handleSuccess, handleError);
        }
        function updateFooterSetting(items) {
            return $http.put('/api/FooterSetting/update', items).then(handleSuccess, handleError);
        }
        function deleteBrandImage(footerSettingId, imageUrl) {
            return $http.delete('/api/FooterSetting/deleteImage/' + footerSettingId + '/' + imageUrl).then(handleSuccess, handleError);
        }
        function removeFooterLogo(footerSettingId) {
            return $http.put('/api/FooterSetting/removeFooterLogo/' + footerSettingId).then(handleSuccess, handleError);
        }
        // layout settings
        function getAllCategories() {
            return $http.get('/api/productcategory/getactivecategories').then(handleSuccess, handleError);
        }
        function saveLayoutSettings(items) {
            return $http.post('/api/LayoutSetting/create', items).then(handleSuccess, handleError);
        }
        function updateLayoutSettings(items) {
            return $http.put('/api/LayoutSetting/update', items).then(handleSuccess, handleError);
        }
        function getLayoutSettings() {
            return $http.get('/api/LayoutSetting/getLayoutSetting').then(handleSuccess, handleError);
        }
        function removeTrendingImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removerecommendImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removeTrendingParallaxImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removeHotParallaxImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removeProductParallaxImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removeHotImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        function removeProductItemImage(imageUri, id, imageType) {
            return $http.delete('/api/LayoutSetting/removeLayoutImage/' + imageUri + "/" + id + "/" + imageType).then(handleSuccess, handleError);
        }
        //slide settings
        function getAllDiscountProduct() {
            return $http.get('/api/Discount/getDiscountsProductWise').then(handleSuccess, handleError);
        }
        function saveSlideSetting(slideSetting) {
            return $http.post('/api/SlideSetting/create', slideSetting).then(handleSuccess, handleError);
        }
        function updateSlideSetting(slideSetting) {
            return $http.put('/api/SlideSetting/update', slideSetting).then(handleSuccess, handleError);
        }

        function getProductDetails(productId) {
            return $http.get('/api/Discount/getDiscountOfProductForSlide/' + productId).then(handleSuccess, handleError);
        }
        function getSlideSettings() {
            return $http.get('/api/SlideSetting/getAll').then(handleSuccess, handleError);
        }
        
        // private functions

        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.statusCode === 401) error.data = error.data;
            if (error.statusCode === 500) error.data = error.data;
            if (error.data === "" || error.data === null) error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }
})();