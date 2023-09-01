(function () {
    "use strict";
    angular.module("app-category").controller("categoryController", categoryController);
    categoryController.$inject = ['$scope', '$http', '$filter', '$rootScope', '$sessionStorage', 'categoryService'];
    function categoryController($scope, $http, $filter, $rootScope, $sessionStorage, categoryService) {


        var vm = this;
        vm.categoryById = categoryById;
        vm.actionChanged = actionChanged;
        vm.saveCategory = saveCategory;
        vm.addCategoryDialog = false;
        vm.btnSaveCategoryText = "Save";
        //var item = $sessionStorage.get('activeMenu');
        //checkActive();
        //function checkActive() {
        //    for (var k = 0; k < item.length; k++) {
        //        item[k].active = false;
        //        if (item[k].children) {
        //            for (var l = 0; l < item[k].children.length; l++) {
        //                item[k].children[l].active = false;
        //            }
        //        }

        //    }
        //    item[0].active = true;
        //    $sessionStorage.put('activeMenu', item);
        //}
        vm.checkIfExistsName = checkIfExistsName;
        vm.showDetailForm = false;
        vm.activeCategory = {};
        vm.pageSizes = ["4", "5", "10", "20"];
        vm.currentPage = 1;
        vm.pageSizes = 4;
        vm.action = null;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.activeCategoryItem = {};
        vm.categoryId = null;
        vm.showForm = false;
        vm.check = false;
        vm.hide = hide;
        vm.openCategory = openCategory;
        vm.nodeClickedForEdit = nodeClickedForEdit;
        vm.deleteDialog = deleteDialog;
        vm.checkInactive = checkInactive;
        vm.searchText = searchText;
        vm.newSubCategory = newSubCategory;
        vm.activateCategory = activateCategory;
        vm.deactivateCategory = deactivateCategory;
        vm.deleteOnlyCategory = deleteOnlyCategory;
        vm.deleteCategoryWithChildCategory = deleteCategoryWithChildCategory;
        init();

        loadCategoryForParentCategory();

        function checkIfExistsName(categoryName) {
           if (categoryName !== undefined) {
                categoryService.checkIfCategoryNameExists(categoryName).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Category already exists");
                                vm.activeCategoryItem.name = "";
                              //  edit(result.data);
                            } else {
                                yesNoDialog("Do you want to activate your Category?",
                                    "info",
                                    "This category is already exists but has been disabled. Do you want to active this category?",
                                    "Active",
                                    "Your category has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    }
                });
            }
        }

        function hide() {
            vm.showModal = false;
            vm.showDeleteModal = false;
            vm.activeCategoryItem = {};
        }

        function openCategory() {
            vm.showModal = true;
            loadCategoryForParentCategory();
        }
       
        function newSubCategory() {
            vm.btnSaveCategoryText = "Save";
            vm.activeCategoryItem = null;
            vm.categoryForm.$setUntouched();
            openCategory();
        }
   
        function init() {
            categoryService.getActiveCategory()
                   .then(function(result) {
                       if(result.success) {
                           vm.category = result.data;
                       } 
                   });
        }

        function categoryById(item) {
            categoryService.getcategoryById(item.id)
                .then(function (result) {
                    if (result.success) {
                        if (result.data.childrens.length === 0) {
                            vm.showForm = true;
                        } else {
                            vm.showForm = false;
                        }
                        vm.activeCategory = result.data;
                        vm.parentId = result.data.parentId;
                        vm.parentCategoryName = result.data.name;
                        vm.categoryId = result.data.id;
                        vm.showDetailForm = true;
                    };

                });
        }

        function edit(category) {
            categoryService.getcategoryById(category.id)
                .then(function (result) {
                    if (result.success) {
                        vm.activeCategoryItem = result.data;
                        openCategory();
                    };
                });
        }

        function getAllCategories() {
            categoryService.getAllCategories()
                           .then(function (result) {
                               if (result.success) {
                                   vm.category = result.data;

                               };

                           });
        }

        function actionChanged(category, action) {
            if (Number(action) === 1) {
                nodeClickedForEdit(category);
            }
            else if (Number(action) === 2) {
               // deleteDialog(category);
               // yesNoDialog("Are you sure?", "warning", "Do you want to deactivate subcategories as well.", "Yes, delete it!", "Your category has been deactivated.", "delete", category);
            }
            vm.action = null;
        }

        function saveCategory(category) {
            if (category.id) {
                categoryService.updateCategory(category).then(function(result) {
                    if (result.success) {
                        init();
                        vm.categoryForm.$setUntouched();
                        vm.showModal = false;
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        } else {
                            swal(result.message);
                        }
                    }
                });
            } else {
                categoryService.createCategory(category).then(function (result) {
                    if (result.success) {
                        init();
                        vm.categoryForm.$setUntouched();
                        loadCategoryForParentCategory();
                        vm.showModal = false;
                    }
                    else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                            vm.categoryForm.$setUntouched();
                            loadCategoryForParentCategory();
                            vm.showModal = false;
                        } else {
                           swal(result.message);
                        }
                    }
                });
            }
            
        }

        function deactivateCategory() {
            vm.showDeleteModal = true;
            categoryService.deleteCategory(vm.deleteCategory.id)
                .then(function(result) {
                    if (result.success) {
                        if (vm.check === true) {
                            getAllCategories();
                        }
                        else {
                            init();

                        }
                        vm.activeCategory = null;
                        vm.showDeleteModal = false;
                    }
                });
        }

        function loadCategoryForParentCategory() {
            categoryService.getAllActiveCategoryForParent().then(function (result) {
                if (result.success) {
                    vm.parentCagtegories = result.data;

                    vm.cats = vm.parentCagtegories;
                }
            });
        }
        vm.cats = [];
        function nodeClickedForEdit(item) {
            vm.btnSaveCategoryText = "Update";
            categoryService.getcategoryById(item.id)
                    .then(function (result) {
                        if (result.success) {
                            vm.activeCategoryItem = result.data;
                            vm.cats = vm.parentCagtegories.slice();
                            for (var i = 0; i < vm.cats.length; i++) {
                                if (vm.cats[i].id === item.id) {
                                     vm.cats.splice(i, 1);
                                }
                            }
                            vm.showModal = true;
                        };
                    });
        }
        //vm.optionsFilter = optionsFilter;
        //function optionsFilter(id) {
        //    if (option.id == 'Active') {
        //        return false;
        //    }
        //    return true;
        //}
        function checkInactive(ev,checked){
           if (checked === true) {
               vm.check = true;
               getAllCategories();
           }
           else {
               vm.check = false;
               init();
           }
        }
      
        function searchText(text,status) {

            if (status === true) {
                if (text === "") {
                    getAllCategories();
                } else {
                    searchCategoryByText(text, status);
                }
            } else {
                if (text === "") {
                    init();
                } else {
                    searchCategoryByText(text, status);
                }
            }
        }

        function activateCategory(item) {

            categoryService.activateCategoryById(item.id).then(function (result) {
                if (result.success) {
                    swal("Category Activated.");
                    getAllCategories();
                    vm.showModal = false;
                }
            });
        }
        vm.ok = Ok;
        vm.categoryId =null;
        $scope.open = function (data) {
            $scope.showCategoryModal = true;
            vm.categoryId = data.id;
        };
        function Ok(deleteCheck) {
            if (deleteCheck === true)
            {
                deleteCategoryWithChildCategory(vm.categoryId);
               // $scope.showCategoryModal = false;
            } else {
                deleteOnlyCategory(vm.categoryId);
               // $scope.showCategoryModal = false;
            }
        };

        $scope.cancel = function () {
            $scope.showCategoryModal = false;
        };

        function deleteOnlyCategory(categoryid) {
            categoryService.deleteOnlyCategory(categoryid)
                .then(function (result) {
                    if (result.success) {
                        if (vm.check === true) {
                            getAllCategories();
                        }
                        else {
                            init();
                        }
                        vm.activeCategory = null;
                        $scope.showCategoryModal = false;
                        vm.showDeleteModal = false;
                    }
                });
        }

        function deleteDialog(data) {
            //vm.message = "";
            //if (data.children && data.children.length >0) {
              
            //    vm.message = "Are you sure? By deactivating, every children category of this category will be deactivated.";
            //}
            //else {
            //    vm.message = "Are you sure?";
            //}
            //vm.showDeleteModal = true;
            //vm.deleteCategory = data;
            vm.showModal = true;

           // yesNoDialog("Are you sure?", "warning", "Category will be deactivated but still you can activate your category in future.", "Yes, delete it!", "Your category has been deactivated.", "delete", data);
        }
        
        function deleteCategoryWithChildCategory(id) {
            categoryService.deleteCategory(id)
                .then(function (result) {
                    if (result.success) {
                       // swal(successMessage, "success");
                        if (vm.check === true) {
                            getAllCategories();
                        }
                        else {
                            init();
                        }
                        vm.activeCategory = null;
                        $scope.showCategoryModal = false;
                    }
                });
        }

        function searchCategoryByText(text, status) {
            categoryService.searchAction(text, status).then(function (result) {
                if (result.success) {
                    vm.accounts = result.data;
                }
            });
        }

        function yesNoDialog(title, type, text, buttonText, successMessage, alertFor, value) {
            swal({
                    title: title,
                    text: text,
                    type: type,
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: buttonText,
                    closeOnConfirm: false
                },
                function () {
                    if (alertFor === 'delete') {
                        deleteCategory(successMessage, value.id);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateCategory(value, false);
                        nodeClickedForEdit(value, true);
                        //swal(successMessage, "success");
                    }

                });
        }
       
    }
  
})();
