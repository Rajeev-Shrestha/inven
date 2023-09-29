(function () {
    angular.module("app-product")
        .controller("productController", productController);
    productController.$inject = ['$http', '$filter', '$scope','$state', 'ProductService'];
    function productController($http, $filter, $scope,$state, productService) {
        var vm = this;
        vm.selectedCategories = [];
        vm.products = [];
        vm.imageList = [];
        vm.activeItem = {};
        vm.saveItem = saveItem;
        vm.editItem = editItem;
        vm.getPageSize = getPageSize;
        vm.actionChanged = actionChanged;
        vm.activateProduct = activateProduct;
        vm.productTypeSelect = productTypeSelect;
        vm.checkIfExistName = checkIfExistName;
        vm.checkIfExistCode = checkIfExistCode;
        vm.searchParamChanged = searchParamChanged;
        vm.isLoading = true;
        vm.totalRecords = 0;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pageSize = 10;
        vm.pagingList = 1;
        vm.currentPage = 1;
        vm.filteredCount = 0;
        vm.totalRecords = 0;
      //  vm.searchText = searchText;
      //  vm.checkInactive = checkInactive;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.action = 4;
        vm.productType = [{ id: 1, name: 'Regular' }, { id: 2, name: 'Assembled' }, { id: 3, name: 'Kit' }];
        vm.productReference = false;
        vm.upload = [];
        vm.categories = [];
        vm.taxList = [];
        vm.removeFiles = removeFiles;
        vm.activeButton = false;
        vm.isActivatd = false;
        vm.productCode = false;
        vm.addProduct = addProduct;
        vm.showModal = false;
        vm.check = false;
        vm.cancel = true;
        vm.saveChanges = true;
        vm.editPanel = false;
        vm.btnProductSaveText = "Save";
        vm.productInformation = productInformation;
        vm.ImageInformation = ImageInformation;

       vm.shortDescriptionWidth = 'col-md-12';
       vm.disableUnitPriceAndTax = false;
       vm.deleteProductImage = deleteProductImage;


        //multiple select
      // vm.selectedAll = false;
       vm.checkAll = checkAll;
       vm.toggleSelection = toggleSelection;
       vm.deleteSelected = deleteSelected;
       vm.moveToCategory = moveToCategory;

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
               angular.forEach(vm.products, function (check) {
                   var index = $scope.selected.indexOf(check);
                   if (index >= 0) {
                      return true;
                  } else {
                      $scope.selected.push(check);
                  }
               })
           }else
           {
               $scope.selected = [];
           }
       }
       function deleteSelected(selectedItem) {
           var productsId = [];
           for (var i = 0; i < selectedItem.length; i++) {
                productsId.push(selectedItem[i].id);
           }
           productService.deletedSelected(productsId).then(function (result) {
               if (result.success) {
                   swal("Successfully Deleted!");
                   searchParamChanged();
                   $scope.selected = [];
                   $scope.selectAll = [];
                   vm.selectedRows = false;
                       
               } else {
                   alert("Unable to delete product.");
               }

           })
       }
         
       function moveToCategory(selectedProducts, selectedCategories) {
           var categories = [];
           for (var i = 0; i < selectedProducts.length; i++) {
               var productCategories = [];
               productCategories = selectedProducts[i].categories;
               selectedProducts[i].categories = [];
               for(var j = 0; j < selectedCategories.length; j++) {
                   categories.push(selectedCategories[j].id);
               }
               selectedProducts[i].newlyAssignedCats = categories;
               categories = [];
               var productCats = [];
               for (var k = 0; k < productCategories.length; k++) {
                   productCats.push(productCategories[k].id);
               }
               selectedProducts[i].categories = productCats;
             //  categories = [];
           }
           productService.assignToCategory(selectedProducts).then(function (result) {
               if (result.success) {
                   swal("Successfully Moved!");
                   searchParamChanged();
                   $scope.selected = [];
                   $scope.selectAll = [];
                   vm.activeItem.categories = "";


               } else {
                   alert("errors");
               }
           });
       }
        //delete product image
       function deleteProductImage(productId, imageUri) {
           var imageUrl = imageUri.split("/");
           var uri = imageUrl[3];
         if (productId !== null && imageUrl !== null) {
             productService.deleteProductImage(productId, uri).then(function (result) {
                   if (result.success) {
                       if (result.data) {
                           swal("Successfully removed!");
                         var imgList = [];
                           imgList = vm.imageList.slice();
                           for (var i = 0; i < imgList.length; i++) {
                               if (imgList[i].fileName === uri) {
                                   var index = vm.imageList.indexOf(imgList[i]);
                                   vm.imageList.splice(index, 1);
                               }
                           }
                       }
                       else {
                           swal("Failed to remove!");
                       }
                   }
               });
           }
       }
       function productInformation()
       {
           vm.cancel = true;       
           vm.saveChanges = true;
       }
       function ImageInformation() {

           if(vm.showImageEdit === true)
           {
               vm.cancel = false;
               vm.saveChanges = false;
           }

       }
        function checkIfExistCode(code) {
            if (code !== undefined) {
                productService.checkIfProductCodeExists(code).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Product already exists");
                                editItem(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your product?",
                                    "info",
                                    "This product is already exists but has been disabled. Do you want to activate this product?",
                                    "Active",
                                    "Your product has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    } else {
                        //do nothing
                    }
                });
            }
        }

        function checkIfExistName(name) {
            if (name !== undefined) {
                productService.checkIfProductNameExists(name).then(function(result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Product already exists");
                                editItem(result.data.id, true);
                            } else {
                                yesNoDialog("Do you want to active your product?",
                                    "info",
                                    "This product is already exists but has been disabled. Do you want to active this product?",
                                    "Active",
                                    "Your product has been activated.",
                                    "active",
                                    result.data);
                            }
                        }
                    } else {
                        //do nothing
                    }
                });
            }
        }

        function productTypeSelect(type) {
           if (Number(type) === 1) {
               vm.disableUnitPriceAndTax = false;
               vm.requird = '';
               vm.productReference = false;
               vm.shortDescriptionWidth = 'col-md-12';
           }
           else if (Number(type) === 2) {
               vm.disableUnitPriceAndTax = true;
               vm.activeItem.taxes = [];
               vm.activeItem.unitPrice = 0;
               vm.requird = 'required';
               vm.productReference = true;
               vm.shortDescriptionWidth = 'col-md-8';
           }
           else if (Number(type) === 3) {
               vm.disableUnitPriceAndTax = false;
               vm.requird = 'required';
               vm.productReference = true;
               vm.shortDescriptionWidth = 'col-md-8';
           }
       }

        // img cropping 
        vm.requird = '';
        $scope.myImage = '';
        $scope.myCroppedImage = '';
        $scope.$watch('myCroppedImage', function () {
            console.log($scope.myArray);
        });

        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.myImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        };
        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

        vm.showImageEdit = false;
        vm.imageList = [];
        vm.images = [];
        vm.editImageDialog = editImageDialog;
        vm.cancelEditImage = cancelEditImage;
        vm.saveImage = saveImage;
        vm.removeImage = removeImage;
       
        function editImageDialog(ev) {
            vm.images = [];
            //vm.imageList = [];
            vm.showImageEdit = true;
            vm.cancel = false;
            vm.saveChanges = false;
            //emptyEdit();
        }

        regularProductForReference();
        vm.productsForRefrence = [];
        function regularProductForReference() {
            productService.getRegularProduct().then(function(result) {
                if (result.success) {
                    for (var i = 0; i < result.data.length; i++) {
                        var obj = {
                            "id": result.data[i].id,
                            "name": result.data[i].name
                        }
                        vm.productsForRefrence.push(obj);
                    }
                }

                else {
                    //error code
                }
            });
        }

        vm.counter = 1;
        vm.listImage = [];
        vm.imageList = [];
        function saveImage() {
            var image = $scope.myArray;
            if (image === null || image === undefined)
            {
                swal("Please select Image.");
            } else {
                for (var i = 0; i < image.length; i++) {
                    image[i].fileName = "";

                    var startpos = image[i].dataURI.indexOf('/') + 1;
                    var endpos = image[i].dataURI.indexOf(';', startpos);
                    var extensition = image[i].dataURI.substring(startpos, endpos);

                    image[i].fileName = vm.counter + '.' + extensition;
                    image[i].imageBase64 = image[i].dataURI.substr(image[i].dataURI.indexOf(",") + 1);
                    vm.imageList.push(image[i]);
                    if (image[i].h === 100) {
                        image[i].ImageSize = 1;
                    }
                    else if (image[i].h === 300) {
                        image[i].ImageSize = 2;
                    }
                    else {
                        image[i].ImageSize = 3;
                    }
                }
                vm.counter = vm.counter + 1;
                vm.showImageEdit = false;
                vm.saveChanges = true;
                vm.cancel = true;
                $scope.myImage = '';
                document.getElementById("img").src = '';
                var input = $("#fileInput");
                input.replaceWith(input.val('').clone(true));
            }
        }
        function removeImage(index) {
            vm.listImage.splice(index, 1);
            vm.imageList.splice(index, 1);
        }

        vm.loadError = loadError;

        function loadError() {
            alert("Sorry we cannot load image");
        }
        function cancelEditImage() {
          //  $scope.showModal = false;
            $('#img').attr('src', '');
            var input = $("#fileInput");
            input.replaceWith(input.val('').clone(true));
            $scope.myImage = '';
            vm.cancel = true;
            vm.saveChanges = true;
            //if ($scope.myArray !== undefined) {
            //    if ($scope.myArray.length >= 0) {
            //        var image = $scope.myArray;
            //        for (var i = 0; i < image.length; i++) {
            //            if (image[i].productId !== undefined) {
            //            image[i].fileName = "";
            //            var startpos = image[i].dataURI.indexOf('/') + 1;
            //            var endpos = image[i].dataURI.indexOf(';', startpos);
            //            var extensition = image[i].dataURI.substring(startpos, endpos);

            //            image[i].fileName = vm.counter + '.' + extensition;
            //            image[i].imageBase64 = image[i].dataURI.substr(image[i].dataURI.indexOf(",") + 1);
            //            vm.imageList.push(image[i]);
            //            if (image[i].h === 100) {
            //                image[i].ImageSize = 1;
            //            }
            //            else if (image[i].h === 300) {
            //                image[i].ImageSize = 2;
            //            }
            //            else {
            //                image[i].ImageSize = 3;
            //            }
            //        }
            //    }
            //        vm.counter = vm.counter + 1;
            //    }
            //}
            vm.showImageEdit = false;
            //$scope.myImage = {};
            //$scope.myArray = [];
            //$scope.$apply();
          //  $ccope.myArray = '';
         
        }
        
        $scope.hide = function () {
            vm.editPanel = false;
            vm.productReference = false;
            vm.productCode = false;
            vm.activeItem = null;
            $files = 0;
            vm.disableUnitPriceAndTax = false;
            vm.upload = undefined;
            vm.productForm.$setUntouched();
        };

        loadTaxes();
        init();
        function init() {
            loadCategories();
            getProductsPageSize();
        }
        //function getInactives() {
        //    productService.getInactives(vm.currentPage, vm.pageSize)
        //        .then(function (result) {
        //            if (result.success) {
        //                vm.products = result.data.items;
        //                vm.pagingList = result.data.pageCount;
        //                vm.pageNumber = result.data.pageNumber;
                       
        //            } else {
        //                //Show error
        //            }
        //        });
        //}

        function activateProduct(product) {
            productService.activateProduct(product.id)
                .then(function (result) {
                    if (result.success) {
                        swal("Product Activated.");
                        searchParamChanged();
                    } else {
                        alert("Failed to activate product");
                    }
                });
        }

        //function checkInactive(active, checked) {
        //    if (checked) {
        //        getInactives();
        //        vm.check = true;
        //    }
        //    else {
        //        vm.check = false;
        //        getAllActiveProducts();
        //    }
        //}

        function deleteProduct(successMessage, id) {
            productService.Delete(id)
                .then(function (result) {
                    if (result.success) {
                        swal(successMessage, "success");
                        searchParamChanged();
                    }
                });
        }

        function actionChanged(ev, product, id) {
            if (Number(id) === 1) {
                editItem(product.id, product.active, ev);
            }
            else if (Number(id) === 2) {
                yesNoDialog("Are you sure?", "warning", "Product will be deactivated but still you can activate your product in future.", "Yes, delete it!", "Your product has been deactivated.", "delete", product);
            }
            else if (Number(id) === 3) {
                alert("Create report not working");
            }
            vm.action = null;
        }
        vm.deleteItem = deleteItem;
        function deleteItem(product) {
            yesNoDialog("Are you sure?", "warning", "Product will be deactivated but still you can activate your product in future.", "Yes, delete it!", "Your product has been deactivated.", "delete", product);
        }

        //function searchText(text) {
        //    if (vm.check) {
        //        if (text === "") {
        //            getInactives();
        //        } else {
        //            searchTextChanged(text, vm.check);
        //        }
        //    } else {
        //        if (text === "") {
        //            getAllActiveProducts();
        //        } else {
        //            searchTextChanged(text, vm.check);
        //        }
        //    }
        //}

        //function searchTextChanged(text, checked) {
        //    productService.searchText(vm.currentPage, vm.pageSize, text, checked)
        //       .then(function (result) {
        //           if (result.success) {
        //               vm.products = result.data.items;
        //               vm.pagingList = result.data.pageCount;
        //               vm.pageNumber = result.data.pageNumber;
        //           }
        //       });
        //}

        function loadCategories() {
            productService.getAllCategories().then(function (res) {
                if (res.success) {
                    for (var i = 0; i < res.data.length; i++) {
                        var obj = {
                            "id": res.data[i].id,
                            "name": res.data[i].name
                        }
                        vm.categories.push(obj);
                    }
                  
                }
                
            });
        }

        function loadTaxes() {
            //get all taxes controller
            productService.getAlltaxes().then(function (result) {
                if (result.success) {
                    for (var i = 0; i < result.data.length; i++) {
                        var obj = {
                            "id": result.data[i].id,
                            "name": result.data[i].taxCode
                        }
                        vm.taxList.push(obj);
                    }
                }
            });
        }
        function multiSelect(uiVariable, serverVariable) {
            var tempVariable = [];
            for (var i = 0; i < serverVariable.length; i++) {
                for (var j = 0; j < uiVariable.length; j++) {
                    if (uiVariable[j] === serverVariable[i].id) {
                        tempVariable.push(serverVariable[i]);
                    }
                }
            }
            serverVariable = tempVariable;
            return serverVariable;
        }

        function editItem(productId, active) {
            vm.btnProductSaveText = "Update";
            vm.imageList = [];
            if (active) {
                productService.GetById(productId).then(function (result) {
                    if (result.success) {
                        vm.activeItem = result.data;
                        if (result.data.categories) {
                            vm.activeItem.categories = multiSelect(result.data.categories, vm.categories);
                        }
                        if (result.data.taxes) {
                            vm.activeItem.taxes = multiSelect(result.data.taxes, vm.taxList);
                        }
                        if (result.data.productsReferencedByKitAndAssembledType) {
                            vm.activeItem.productsReferencedByKitAndAssembledType = multiSelect(result.data.productsReferencedByKitAndAssembledType, vm.productsForRefrence);
                        }
                        vm.productCode = true;
                        if (vm.activeItem.productType === 2 | vm.activeItem.productType === 3 ) {
                            vm.productReference = true;
                        }
                        if (vm.activeItem.largeImageUrls) {
                            // need to show images of images if there are images for the product alread2y
                            for (var i = 0; i < vm.activeItem.largeImageUrls.length; i++) {
                                var imageUrl = vm.activeItem.largeImageUrls[i].split("/");
                                var uri = imageUrl[3];
                                var obj = {
                                    "dataURI": vm.activeItem.largeImageUrls[i],
                                    "h": 300,
                                    "productId": productId,
                                    "fileName": uri
                                }
                                vm.imageList.push(obj);
                                //$scope.myArray = vm.imageList;
                            }
                        }
                        vm.editPanel = true;
                    }

                });
            }
            else {
               swal("You cannot edit this item. please active first.");
            }
        }
        $scope.range = function (n) {
            return new Array(n);
        };
        function getProductsPageSize() {
            productService.getPageSize().then(function (res) {
                if (res.success) {
                    if (res.data !== 0) {
                        vm.pageSize = res.data;
                        searchParamChanged(); 
                    }
                    else {
                        vm.pageSize = 10;
                        searchParamChanged();
                    }
                }
            });
        }
        //function getAllActiveProducts() {
        //    productService.GetAllActiveProducts(vm.currentPage, vm.pageSize).then(function (res) {
        //        vm.categoriesText = [];
        //        if (res.success) {
        //            vm.pagingList = res.data.pageCount;
        //            vm.pageNumber = res.data.pageNumber;
        //            vm.products = res.data.items;
        //            if (res.data.pageSize != 0) {
        //                vm.pageSize = res.data.pageSize;
        //            }
        //            else {
        //                vm.pageSize = 10;
        //            }
        //        }
        //    });
        //}

        //getAllActiveProducts();

        vm.pageChanged = function (page) {
            $scope.selectAll = [];
            vm.currentPage = page;
            if (vm.check === true) {
                searchParamChanged();
            } else {
                searchParamChanged();
            }
        };

        function getPageSize(pageSize) {
            vm.currentPage = 1;
            vm.pageSize = pageSize;
            searchParamChanged();
        }
        function saveItem(product) {
            vm.isLoading = true;
            var categories = [];
            var productsReferencedByKitAndAssembledType = [];
            var taxes = [];
            if (product.taxes !== null) {
                for (var j = 0; j < product.taxes.length; j++) {
                    if (product.taxes[j].id) {
                        taxes.push(product.taxes[j].id);
                    }
                }
            }
            for (var i = 0; i < product.categories.length;i++){
                if (product.categories[i].id) {
                    categories.push(product.categories[i].id);
                }
            }
            if (product.productsReferencedByKitAndAssembledType !== null || product.productsReferencedByKitAndAssembledType === "undefined") {
                for (var l = 0; l < product.productsReferencedByKitAndAssembledType.length; l++) {
                    if (product.productsReferencedByKitAndAssembledType[l].id) {
                        productsReferencedByKitAndAssembledType.push(product.productsReferencedByKitAndAssembledType[l].id);
                    }
                }
            }
            
            product.Images = {};
            for (var k = 0; k < vm.imageList.length; k++) {
                vm.imageList[k].fileName = product.name + '_' + vm.imageList[k].fileName;
                
            }
            product.Images = vm.imageList;
            if (categories.length > 0) {
                product.categories = categories;
            }
             if (taxes.length>0) {
                 product.taxes = taxes;
             }
             if (productsReferencedByKitAndAssembledType.length>0) {
                 product.productsReferencedByKitAndAssembledType = productsReferencedByKitAndAssembledType;
            }
            if (product.id) {
                productService.Update(product).then(function (result) {
                    if (result.success) {
                        searchParamChanged();
                        $scope.showModal = false;
                        categories = []; 
                        taxes = [];
                        productsReferencedByKitAndAssembledType = [];
                        vm.imageList = [];
                        vm.productForm.$setUntouched();
                        vm.activeItem = {};
                        vm.editPanel = false;
                    } else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                        }
                        else {
                           swal(result.message);
                        }
                    }
                    vm.isLoading = false;
                });
            }
            else {
                productService.Create(product).then(function (result) {
                    if (result.success) {
                        searchParamChanged();
                        $scope.showModal = false;
                        categories = [];
                        taxes = [];
                        productsReferencedByKitAndAssembledType = [];
                        vm.imageList = [];
                        vm.activeItem = {};
                        vm.productForm.$setUntouched();
                        vm.editPanel = false;
                    } else {
                        if (result.message.errors) {
                            swal(result.message.errors[0]);
                            vm.activeItem = {};
                        }
                        else {
                            swal(result.message);
                        }
                        vm.isLoading = false;
                    }
                });

            }
        }

        $scope.open = function () {
            vm.btnProductSaveText = "Save";
            vm.activeItem = {};
            vm.imageList = [];
            vm.showImageEdit = false;
            //if (vm.categories.length > 0) {
                vm.editPanel = true;
                vm.productCode = false;
                vm.activeItem.categories = [];
                vm.activeItem.taxes = [];
                vm.activeItem.productsReferencedByKitAndAssembledType = null;
            //}
            //else {
            //    messageBox("There is no caterogy. please add some category first",
            //        "app/productcategory#!/category",
            //        "Go");
            //}
            
        };

        function addProduct() {
            vm.productCode = false;
            vm.activeItem = null;
            vm.showModal = true;
        }

        function removeFiles(file) {
            var index = vm.upload.indexOf(file);
            vm.upload.splice(index, 1);
        }

        vm.fileSelect = fileSelect;

        function fileSelect(files) {
            if (files.length === 0) {
                vm.upload = [];
            }
            else {
                for (var i = 0; i < files.length; i++) {
                    vm.upload.push(files[i]);
                }
            }
        }
        function messageBox(result, link, text) {
            var message = '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">'
                           + '<div class="alert alert-info alert-dismissable">'
                           + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                           + result + '<div>' + '<a href="' + link + '">' + text + '</a>' + '</div>' + '</div>'
                           + '</div>';
            $('#message').html(message);
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
                    deleteProduct(successMessage, value.id);
                    //swal(successMessage, "success");
                }
                if (alertFor === 'active') {
                    activateProduct(value, false);
                    editItem(value.id, true);
                }
                
            });
        }
        // add new category
        vm.openCategoryDialog = openCategoryDialog;
        vm.loadCategoryForParentCategory = loadCategoryForParentCategory;
        vm.checkIfExistsName = checkIfExistsName;
        vm.hideCategoryDialog = hideCategoryDialog;
        vm.saveCategory = saveCategory;
        function openCategoryDialog()
        {
            $scope.showCategoryModal = true;
            loadCategoryForParentCategory();
            vm.categoryForm.$setUntouched();
            vm.activeCategoryItem = {};

        }

        function loadCategoryForParentCategory() {
            productService.getAllActiveCategoryForParent().then(function (result) {
                if (result.success) {
                    vm.parentCagtegories = result.data;
                }
            });
        }

        function checkIfExistsName(categoryName) {
            if (categoryName !== undefined) {
                productService.checkIfCategoryNameExists(categoryName).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Category already exists");                               
                            } else {
                                return true;
                            }
                        }
                    }
                });
            }
        }
        function hideCategoryDialog() {
            init();
            vm.showCategoryModal = false;
            vm.activeCategoryItem = {};
            loadCategories();
           
        }
        function saveCategory(category) {
            productService.createCategory(category).then(function (result) {
                if (result.success) {
                    vm.categories.push(result.data);
                    vm.activeItem.categories.push(result.data);
                   // $scope.$apply();
                    swal("New Category Successfully Added.");
                }
                else {
                    swal(result.errors, "error");
                }
            });
        }
        vm.productCategories = [];
        function searchParamChanged() {
            vm.isLoading = true;
            if (vm.searchText === undefined)
                vm.searchText = "";
            if (vm.totalRecords < vm.pageSize)
                vm.currentPage = 1;
            productService.searchTextForProduct(vm.searchText, !vm.check, vm.currentPage, vm.pageSize)
                .then(function (result) {
                    if (result.success) {
                        vm.products = result.data.items;
                        for (var i = 0; i < result.data.items.length; i++) {
                            var categories = result.data.items[i].categories;
                            result.data.items[i].categories = [];
                            var categoryList = [];
                            var productCategoryList = vm.categories.slice();
                            for (var j = 0; j < categories.length; j++) {
                                for (var k = 0; k < productCategoryList.length; k++) {
                                    if (categories[j] === productCategoryList[k].id) {
                                        var obj = {
                                            "id": productCategoryList[k].id,
                                            "name": productCategoryList[k].name
                                        }
                                        categoryList.push(obj);
                                    }
                                }
                            }
                            result.data.items[i].categories = categoryList;
                        }
                        vm.totalRecords = result.data.totalRecords;
                        vm.pagingList = result.data.pageCount;
                        vm.pageNumber = result.data.pageNumber;
                    }
                    vm.isLoading = false;
                });
        }

        // add new tax

        vm.taxes = [{ id: 1, name: 'Percent' }, { id: 2, name: 'Fixed' }];
        vm.saveTax = saveTax;
        vm.hideTaxDialog = hideTaxDialog;
        vm.checkIfTaxCodeExists = checkIfTaxCodeExists;
        vm.openTaxDialog = openTaxDialog;

        function openTaxDialog() {
            vm.tax = {};
            vm.taxForm.$setUntouched();
        }

        function hideTaxDialog() {
            vm.showCategoryModal = false;
            vm.tax = {};
            loadTaxes();

        }

        function checkIfTaxCodeExists(taxCode) {
            if (taxCode !== undefined) {
                productService.checkIfTaxCodeExists(taxCode).then(function (result) {
                    if (result.success) {
                        if (result.data) {
                            if (result.data.active) {
                                swal("Tax already exists");
                            } else {
                                return true;
                            }
                        }
                    }
                });
            }
        }

        function saveTax(tax) {
            tax.recoverableCalculationType = 1;
            productService.createTax(tax).then(function (result) {
                if (result.success) {
                    var obj = {
                        "id": result.data.id,
                        "name": result.data.taxCode
                    }
                    vm.taxList.push(obj);
                    vm.activeItem.taxes.push(obj);
                    swal("New Tax Successfully Added.");
                }
                else {
                    if (result.message.errors) {
                        swal(result.message.errors[0]);
                    } else {
                        swal(result.message);
                    }
                }
            });

        }
    }
})();

