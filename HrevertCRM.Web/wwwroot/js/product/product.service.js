(function () {
    'use strict';

    angular
        .module('app-product')
        .service('ProductService', productService);

    productService.$inject = ['$http'];

    function productService($http) {
        var service = {};

      //  service.GetAllActiveProducts = getAllActiveProducts;
        service.GetById = getById;
     //   service.searchText = searchText;
        service.Create = create;
        service.Update = update;
        service.Delete = Delete;
        service.getAllCategories = getAllCategories;
        service.getCategoryTree = getCategoryTree;
       // service.getInactives = getInactives;
        service.activateProduct = activateProduct;
       // service.getAllProduct = getAllProduct;
        service.getAlltaxes = getAlltaxes;
        service.getRegularProduct = getRegularProduct;
        service.checkIfProductCodeExists = checkIfProductCodeExists;
        service.checkIfProductNameExists = checkIfProductNameExists;
        service.deleteProductImage = deleteProductImage;
        service.getAllActiveCategoryForParent = getAllActiveCategoryForParent;
        service.checkIfCategoryNameExists = checkIfCategoryNameExists;
        service.createCategory = createCategory;
        service.createTax = createTax;
        service.checkIfTaxCodeExists = checkIfTaxCodeExists;
        service.deletedSelected = deletedSelected;
        service.assignToCategory = assignToCategory;
        

        //CheckIfDeletedTaxWithSameTaxCodeExists

        service.getPageSize = getPageSize;
        service.searchTextForProduct = searchTextForProduct;
        return service;

        function assignToCategory(productWithNewlyAssignedCats) {
            return $http.post('/api/product/assignToCategory/', productWithNewlyAssignedCats).then(handleSuccess, handleError);
        }
        function deletedSelected(productsId) {
            return $http.post('/api/Product/bulkDelete', productsId).then(handleSuccess, handleError);
        }

        function checkIfCategoryNameExists(name) {
            return $http.get('/api/ProductCategory/CheckIfDeletedProductCategoryWithSameNameExists/' + name).then(handleSuccess, handleError);
        }
        function checkIfTaxCodeExists(code) {
            return $http.get('/api/tax/CheckIfDeletedTaxWithSameTaxCodeExists/' + code).then(handleSuccess, handleError);
        }
        function getAllActiveCategoryForParent() {
            return $http.get('/api/ProductCategory/getallactivecategories').then(handleSuccess, handleError);
        }

        function deleteProductImage(productId, imageUri) {
            return $http.delete('/api/product/deleteImage/' + productId + '/' + imageUri).then(handleSuccess, handleError);
        }
        function checkIfProductCodeExists(code) {
            return $http.get('/api/Product/CheckIfDeletedProducWithSameCodeExists/' + code).then(handleSuccess, handleError);
        }
        function createCategory(category) {
            return $http.post('/api/ProductCategory/createcategory/', category).then(handleSuccess, handleError);
        }
        function createTax(tax) {
            return $http.post('/api/tax/createTax/', tax).then(handleSuccess, handleError);
        }

        function checkIfProductNameExists(name) {
            return $http.get('/api/Product/CheckIfDeletedProducWithSameNameExists/' + name).then(handleSuccess, handleError);
        }
        function getRegularProduct() {
            return $http.get('/api/product/GetRegularProductsOnly').then(handleSuccess, handleError);
        }
        function getAlltaxes() {
            return $http.get('/api/tax/getallactivetaxes').then(handleSuccess, handleError);
        }
        //function getAllProduct() {
        //    return $http.get('/api/product/getallactiveproducts').then(handleSuccess, handleError);
        //}
        function activateProduct(id) {
            return $http.get('/api/product/activateproduct/' + id).then(handleSuccess, handleError);
        }

        function getCategoryTree() {
            return $http.get('/api/ProductCategory/categorytree/').then(handleSuccess, handleError);
        }

        //function getAllActiveProducts(pageIndex, pageSize) {
        //    return $http.get('/api/product/getactiveproducts' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        //function getInactives(pageIndex, pageSize) {
        //    return $http.get('/api/product/getallproducts' + buildUriForPaging(pageIndex, pageSize)).then(handleSuccess, handleError);
        //}

        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }

        function buildUriForSearchPaging(pageIndex, pageSize, text, active) {
            var uri = '?pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&text=' + text + '&active=' + active;
            return uri;
        }

        //function searchText(pageIndex, pageSize, text, active) {
        //    if (active === 'false'|| active === false) {
        //        return $http.get('/api/Product/searchactiveproducts/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
        //    }
        //    else
        //    {
        //        return $http.get('/api/Product/searchallproducts/' + buildUriForSearchPaging(pageIndex, pageSize, text, active)).then(handleSuccess, handleError);
        //    }
           
        //}

        function getAllCategories() {
            return $http.get('/api/productcategory/getactivecategories').then(handleSuccess, handleError);
        }

        function getById(id) { 
            return $http.get('/api/Product/GetProduct/' + id).then(handleSuccess, handleError);
        }

        function create(data) {
            return $http.post('/api/product/createproduct/', data).then(handleSuccess, handleError);
            return $http.post("/api/product/createproduct/",
                data,
                {

                    withCredentials: false,
                    headers: { 'Content-Type': undefined },
                
                    transformRequest: angular.identity,
                  
                    responseType: "arryabuffer"
                }).then(handleSuccess, handleError);
        } 

        function update(data) {
            return $http.put('/api/product/', data).then(handleSuccess, handleError);
            return $http.post("/api/product/",
               data,
               {
                   withCredentials:false,
                   headers: { 'Content-Type': undefined },
                   transformRequest: angular.identity,
                   responseType:"arraybuffer"
               }).then(handleSuccess, handleError);

           
        }

        function Delete(id) {
            return $http.delete('/api/product/' + id).then(handleSuccess, handleError);
        }
        //Get Page size
        function getPageSize() {
            return $http.get('/api/product/getPageSize').then(handleSuccess, handleError);
        }
        //search products
        function searchTextForProduct(text, checked, pageIndex, pageSize) {

            return $http.get('/api/product/searchProducts/' + buildUriForSearchPaging(pageIndex, pageSize, text, checked))
                    .then(handleSuccess, handleError);

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