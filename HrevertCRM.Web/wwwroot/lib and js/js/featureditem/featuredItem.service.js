(function () {
    'use strict';

    angular
        .module('app-featuredItem')
        .service('featuredItemService', featuredItemService);

    featuredItemService.$inject = ['$http'];

    function featuredItemService($http) {
        var service = {};
        service.saveFeaturedItemBanner = saveFeaturedItemBanner;
        service.updateFeaturedItemBanner = updateFeaturedItemBanner;
        service.getFeaturedItemBanner = getFeaturedItemBanner;
        service.deleteFeaturedItemBanner = deleteFeaturedItemBanner;
        service.getFeaturedItemById = getFeaturedItemById;
        service.getImageTypes = getImageTypes;
        service.getBannerImagesByCategoryIdAndImageTypeId = getBannerImagesByCategoryIdAndImageTypeId;
        service.removeBannerImageFromBannerList = removeBannerImageFromBannerList;
        service.getAllParentProductCategory = getAllParentProductCategory;
        service.deletedSelected = deletedSelected;
        return service;

        function deletedSelected(selectedList) {
            return $http.post('/api/featureditem/bulkDelete', selectedList).then(handleSuccess, handleError);
        }
        function saveFeaturedItemBanner(featuredItem) {
            return $http.post('/api/featureditem/', featuredItem).then(handleSuccess, handleError);
        }

        function updateFeaturedItemBanner(featuredItem) {
            return $http.put('/api/featureditem/', featuredItem).then(handleSuccess, handleError);
        }

        function getFeaturedItemById(featuredItemId) {
            return $http.get('/api/featureditem/getfeatureditem/' + featuredItemId).then(handleSuccess, handleError);
        }

        function getFeaturedItemBanner() {
            return $http.get('/api/featureditem/getallactivefeatureditems').then(handleSuccess, handleError);
        }

        function deleteFeaturedItemBanner(bannerId) {
            return $http.delete('/api/featureditemmetadata/' + bannerId).then(handleSuccess, handleError);
        }

        function getImageTypes() {
            return $http.get('/api/featureditem/getimagetypes').then(handleSuccess, handleError);
        }

        function getBannerImagesByCategoryIdAndImageTypeId(imageTypeId, categoryId) {
            return $http.get('/api/featureditem/getBannerImagesByCategoryIdAndImageTypeId/' + imageTypeId + '/' + categoryId).then(handleSuccess, handleError);
        }

        function removeBannerImageFromBannerList(removeMetadataItem) {
            return $http.post('/api/FeaturedItemMetadata/removeBannerFromFeaturedItemMetadata', removeMetadataItem).then(handleSuccess, handleError);
        }

        function getAllParentProductCategory() {
            return $http.get('/api/ProductCategory/getallparentcategories').then(handleSuccess, handleError);
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