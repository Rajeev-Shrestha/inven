(function () {
    "use strict";
    angular.module("app-category")
        .service("categoryService", categoryService);

    categoryService.$inject = ['$http'];

  


    function categoryService($http) {
        var service = {};
        service.getActiveCategory = getActiveCategory;
        service.getAllCategories = getAllCategories;
        service.getcategoryById = getcategoryById;
        service.updateCategory = updateCategory;
        service.createCategory = createCategory;
        service.deleteCategory = deleteCategory;
        service.getAllCategory = getAllCategory
        service.getProductByCategoryIdLevelAndNoofProducts = productByCategoryIdLevelAndNoofProducts;
        service.getAllActiveCategoryForParent = getAllActiveCategoryForParent;
        service.activateCategoryById = activateCategoryById;
        service.checkIfCategoryNameExists = checkIfCategoryNameExists;
        service.deleteOnlyCategory = deleteOnlyCategory;
        return service;

        function deleteOnlyCategory(id) {
            return $http.delete('/api/ProductCategory/deleteOnlyCategory/' + id).then(handleSuccess, handleError);
        }

        function checkIfCategoryNameExists(name) {
            return $http.get('/api/ProductCategory/CheckIfDeletedProductCategoryWithSameNameExists/' + name).then(handleSuccess, handleError);
        }

        function activateCategoryById(id) {
            return $http.get('/api/ProductCategory/activatecategory/'+id).then(handleSuccess, handleError);
        }

        function getAllActiveCategoryForParent() {
           return $http.get('/api/ProductCategory/getallactivecategories').then(handleSuccess, handleError);
        }
        function getAllCategory() {
            return $http.get('/api/ProductCategory/getactivecategories').then(handleSuccess, handleError);
        }

        function getAllCategories() { 
            return $http.get('/api/ProductCategory/allcategorytree').then(handleSuccess, handleError);
        }
        function deleteCategory(id) {
            return $http.delete('/api/ProductCategory/' + id).then(handleSuccess, handleError);
        }
        function createCategory(category) {
            return $http.post('/api/ProductCategory/createcategory/', category).then(handleSuccess, handleError);
        }
        function updateCategory(category) {
            return $http.put('/api/ProductCategory/updatecategory/', category).then(handleSuccess, handleError);
        }
        function getcategoryById(id) {
            return $http.get('/api/ProductCategory/getcategorywithchildren/' + id).then(handleSuccess, handleError);
        }
        function getActiveCategory(pageNo, pagesize) {
            //return $http.get('/api/category/getactivecategory/' + buildUriForPaging(pageNo, pagesize)).then(handleSuccess, handleError);
            return $http.get('/api/ProductCategory/categorytree/').then(handleSuccess, handleError);
        }


        function productByCategoryIdLevelAndNoofProducts(obj) {
            return $http.post('/api/ProductCategory/getproductbycategoryidlevelandnoofproducts/', obj).then(handleSuccess, handleError);
        }

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }
        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }
        function handleSuccess(res) {
            return { success: true, data: res.data };
        }

        function handleError(error) {
            if (error.data === "") error.data = "Server Error or Access Denied. Contact Administrator";

            return { success: false, message: error.data };

        }
    }

})();