(function () {
    angular.module("app-items")
        .controller("allitemController", allitemController);
    allitemController.$inject = ['$http', '$filter', '$cookies', 'allitemService', 'viewModelHelper'];
    function allitemController($http, $filter, $cookies, allitemService, viewModelHelper) {
       
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        vm.getProductByCategory = getProductByCategory;
        vm.productList = [];

        allitemService.getcategoryTree()
              .then(function (result) {
                  if (result.success) {
                      vm.categoryTree = result.data;
                  } else {
                      var message = {};
                      message.message = "get category tree, " + result.data + " in all item.,";
                      viewModelHelper.bugReport(message,
                        function (result) {
                        });
                  }
              });

        function itemByCategoryId(id) {
            allitemService.getProductById(id)
              .then(function (result) {
                  if (result.success) {
                      vm.categoryTree = result.data;
                  } else {
                      var message = {};
                      message.message = "item by category id, " + result.data + " in all item.,";
                      viewModelHelper.bugReport(message,
                        function (result) {
                        });
                  }
              });
        }
        
        vm.allProduct = [];
        getProductByCategory($cookies.get('categoryId'));

        function getProductByCategory(category) {
            if (category === undefined) {
                getProducts($cookies.get('categoryId'));
                vm.categoryName = ($cookies.get('categoryName'));
            }
            else {
                if (category.currentNode === undefined) {
                    getProducts(category);
                }
                else {
                    getProducts(category.currentNode.id);
                    $cookies.put('categoryId', category.currentNode.id);
                    $cookies.put('categoryName', category.currentNode.name);
                    vm.categoryName = category.currentNode.name;
                }
                
            }
            vm.class = 'loader loader-default';
        }
        function getProducts(id) {
            allitemService.getProductByCategoryId(id)
                 .then(function (result) {
                     if (result.success) {
                         vm.allProduct = result.data;
                         
                     } else {
                         var message = {};
                         message.message = "get product by category id, " + result.data + " in all item.,";
                         viewModelHelper.bugReport(message,
                           function (result) {
                           });
                     }
                 });
        }
    }
})();