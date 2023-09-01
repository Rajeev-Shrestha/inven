(function () {
    'use strict';

    angular
        .module('app-index')
        .factory('indexService', indexService);
    indexService.$inject = ['$http', '$sessionStorage', 'viewModelHelper'];

        function indexService($http, $sessionStorage,viewModelHelper) {
            var service = {};
            service.getCategories = getCategories;
            service.getItems = getItems;
            service.getProductById = getProductById; 
            service.getItemsByCategory = getItemsByCategory;
            service.getcategoryWithProduct = getcategoryWithProduct;
            service.crmLocation = crmLocation;
            service.addItemInCart = addItemInCart;
            service.getLoginUserId = getLoginUserId;
            service.getSliderImage = getSliderImage;
            service.getSettings = getSettings;
            service.getProductByCategoryId = getProductByCategoryId;
            service.getIndexProduct = getIndexProduct;
            service.getLatestProduct = getLatestProduct;
            service.getHotThisWeek = getHotThisWeek;
            service.getTopCategories = getTopCategories;
            service.getTrendingProducts = getTrendingProducts;
            service.getAllFeaturedItemsImages = getAllFeaturedItemsImages;
            service.getAllFeaturedItemsImagesByCategoryId = getAllFeaturedItemsImagesByCategoryId;
            service.getThemeSliderSettings = getThemeSliderSettings;

            //new codes
            service.getThemeGeneralSettings = getThemeGeneralSettings;
            service.getThemeLayoutSettings = getThemeLayoutSettings;


            return service;

            function getThemeSliderSettings() {
                var apiUrl = "/api/SlideSetting/getAll";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getLatestProduct(pageIndex, pageSize) {
                var apiUrl = '/api/Product/lastestproducts/' + buildUriForPaging(pageIndex, pageSize);
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            function getHotThisWeek() {
                var apiUrl = '/api/ShoppingCart/getHotThisWeek/';
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getTopCategories(list) {
                var urlString = '';
                for (var i = 0; i < list.length; i++) {
                    urlString = urlString + 'listOfCategoryId=' + list[i] + '&'
                }
                var newStr = urlString.substring(0, urlString.length - 1);
                var apiUrl = '/api/ProductCategory/getCategoriesByCategoryIdList?' + newStr;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getTrendingProducts() {
                var apiUrl = '/api/ShoppingCart/getTrendingProducts/';
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }



            function getThemeGeneralSettings() {
                var apiUrl = "/api/GeneralSetting/getAll";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            function getThemeLayoutSettings() {
                var apiUrl = "/api/LayoutSetting/getLayoutSetting";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            

            function getIndexProduct(customerId, categoryId, level) {
                var apiUrl = "/api/Product/GetProductsForStore/" + customerId + "/" + categoryId + "/" + level;
                //var apiUrl = "/api/Product/getstorefrontproducts/";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getProductByCategoryId(id) {
                //var apiUrl = "/api/Product/getstorefrontproductswithdiscounts/" + id;
                var apiUrl = "/api/Product/getProductsByCategoryId/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getSettings() {
                var apiUrl = "/api/EcommerceSetting/getactiveEcommerceSettings";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
                
            }
            function getSliderImage() {
                var apiUrl = "/api/Carousel/getactiveCarousels";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getLoginUserId() {
                return $http.get('/api/getloggedinuser/').then(handleSuccess, handleError);
            }
            function addItemInCart(item) {
                var apiUrl = "/api/ShoppingCart/addtocart";
                var dataToPost = { url: apiUrl, data: item }
                return $http.post('/api/PostWithData', dataToPost).then(handleSuccess, handleError);
            }

            function crmLocation() {
                return $http.get('/api/getcrmurl').then(handleSuccess, handleError);
            }

            function getcategoryWithProduct() {
                
                viewModelHelper.apiGet("/api/ProductCategory/getcategories",null,
                    function (result) {
                  return result.data;
               });

                //var apiUrl = "/api/ProductCategory/getcategories";
                //return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            function getItemsByCategory(id) {
                var apiUrl = "/api/ProductCategory/getproductcategory/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getProductById(id) {
                var apiUrl = "/api/Product/"+ id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            function getCategories() {
                var apiUrl = "/api/ProductCategory/getactivecategories";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
                
            }

            function getItems(pageIndex, pageSize) {
                var apiUrl = "/api/product/getactiveproducts" + buildUriForPaging(pageIndex, pageSize);
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            function buildUriForPaging(pageIndex, pageSize) {
                var uri = '?pagenumber=' + pageIndex + '&pageSize=' + pageSize;
                return uri;
            }

            function getAllFeaturedItemsImagesByCategoryId(id) {
                var apiUrl = "/api/featureditem/getAllBannerImagesByCategoryId/" + id;
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }
            
            function getAllFeaturedItemsImages() {
                var apiUrl = "/api/featureditem/getAllBannerImages";
                return $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(handleSuccess, handleError);
            }

            // private functions

            function handleSuccess(res) {
                return { success: true, data: res.data };
            }

            function handleError(error) {
                console.log(JSON.stringify(error.data));
                if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

                return { success: false, message: error.data };

            }


    }
})();