(function () {
    'use strict';

    angular
        .module('app-ecommerceSetting')
        .service('ecommerceSettingService', ecommerceSettingService);

    ecommerceSettingService.$inject = ['$http'];

    function ecommerceSettingService($http) {
        var service = {};

        service.saveSettings = saveSettings;
        service.getEcommerceSetting = getEcommerceSetting;
        service.saveImage = saveImage;
        service.getAllProduct = getAllProduct;
        service.getCarouselImage = getCarouselImage;
        service.removeCarouselImage = removeCarouselImage;
        service.updateEcommerceLogo = updateEcommerceLogo;
        service.saveEcommerceLogo = saveEcommerceLogo;
        service.removeLogoImage = removeLogoImage;
        return service;

        function removeLogoImage(url) {
            return $http.post('/api/EcommerceSetting/DeleteLogo/'+ url).then(handleSuccess, handleError);
        }
        function removeCarouselImage(id) {
            return $http.delete('/api/Carousel/' + id).then(handleSuccess, handleError);
        }

        function getCarouselImage() {
            return $http.get('/api/Carousel/searchCarousels/' + true + '/' + null).then(handleSuccess, handleError);
        }

        function getAllProduct() {
            return $http.get('/api/Product/getallactiveproducts').then(handleSuccess, handleError);
        }

        function saveImage(image) {
            return $http.post('/api/Carousel/', image).then(handleSuccess, handleError);
        }

        function saveSettings(setting) {
            return $http.put('/api/EcommerceSetting/', setting).then(handleSuccess, handleError);
        }

        function getEcommerceSetting() {
            return $http.get('/api/ecommerceSetting/getactiveEcommerceSettings/').then(handleSuccess, handleError);
        }

        // Ecommerce Logo
        function saveEcommerceLogo(image) {
            return $http.post('/api/ecommerceSetting/SaveEcommerceLogo', image).then(handleSuccess, handleError);
        }
        function updateEcommerceLogo(image) {
            return $http.post('/api/ecommerceSetting/updateEcommerceLogo', image).then(handleSuccess, handleError);
        }
        // private functions

        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";
            return { success: false, message: error.data };

        }
    }
})();