(function () {
    angular.module("app-layout")
        .controller("layoutController", layoutController);
    layoutController.$inject = ['$http', '$scope', '$filter', '$window', '$sessionStorage', '$timeout', '$cookies', 'layoutService'];

    function layoutController($http, $scope, $filter, $window, $sessionStorage, $timeout, $cookies, layoutService) {
        var vm = this;
        vm.productPage = productPage;
        vm.salesOrder = salesOrder;

        vm.activeTabOnclick = activeTabOnclick;
        
        getLoggedinUserDetails();
        vm.menuItemWithoutSecurity = [
            {
                id: 1, name: 'Dashboard', icon: 'dashboard', url: '/', active: false
            },
            {
                id: 2, name: 'Settings', icon: 'cogs', url: '', active: false, children: [
                    {
                        id: 1, name: 'Company Settings', icon: 'users', url: '', active: false, children: [
                            {
                                id: 1, name: 'User', icon: 'users', url: '/app/user#/user', children: null, active: true
                            },
                           
                            {
                                id: 3, name: 'Fiscal Year', icon: 'list', url: '/app/fiscalyear#/fiscalyear', children: null, active: false
                            },
                            //{
                            //    id: 4, name: 'Retailer Setting', icon: 'list', url: '/app/retailer#/retailer', active: false
                            //},
                            {
                                id: 5, name: 'Email Setting', icon: 'share', url: '/app/emailSetting#/emailSetting', active: false
                            },
                            {
                                id: 6, name: 'Company Setup', icon: 'users', url: '/app/companySetup#/companySetup', active: false
                            },


                        ]
                    },
                    {
                        id: 2, name: 'Product Settings', icon: 'th', url: '', active: false, children: [

                            {
                                id: 1, name: 'Category', icon: 'bars', url: '/app/productcategory#/productcategory', children: null, active: false
                            },
                            {
                                id: 2, name: 'Item Measure', icon: 'bars', url: '/app/itemMeasure#/itemMeasure', children: null, active: false
                            },
                            {
                                id: 3, name: 'Measure Unit', icon: 'bars', url: '/app/measureUnit#/measureUnit', children: null, active: false
                            },
                            
                        ]
                    },


                {
                    id: 4, name: 'Ecommerce Settings', icon: 'shopping-cart', url: '', active: false, children: [
                        {
                            id: 1, name: 'Vendor', icon: 'user-plus', url: '/app/vendor#/vendor', children: null, active: false
                        },
                        {
                            id: 2, name: 'Customer', icon: 'user-plus', url: '/app/customer#/customer', children: null, active: false
                        },
                        {
                            id: 3, name: 'Payment Method', icon: 'bars', url: '/app/paymentMethod#/paymentMethod', active: false
                        },
                        {
                            id: 4, name: 'Delivery Method', icon: 'bars', url: '/app/deliveryMethod#/deliveryMethod', active: false
                        },
                        {
                            id: 5, name: 'Payment Term', icon: 'bars', url: '/app/paymentTerm#/paymentTerm', active: false
                        },
                        {
                            id: 6, name: 'Ecommerce Setting', icon: 'cog', url: '/app/ecommerceSetting#/ecommerceSetting', active: false
                        },
                        {
                            id: 7, name: 'Discount Setting', icon: 'cog', url: '/app/discountSetting#/discountSetting', active: false
                        },
                        {
                            id: 8, name: 'Zone Setting', icon: 'cog', url: '/app/zoneSetting#/zoneSetting', active: false
                        },
                        {
                            id: 9, name: 'Web Setting', icon: 'cog', url: '/app/companywebSetting#/companywebSetting', active: false
                        },
                        {
                            id: 10, name: 'Theme Setting', icon: 'themeisle', url: '/app/themeSetting#/themeSetting', children: null, active:true
                        },
                        {
                            id: 11, name: 'Banner Setting', icon: 'bars', url: '/app/featuredItem#/featuredItem', active: false
                        },
                        {
                            id: 12, name: 'Delivery Rate', icon: 'bars', url: '/app/deliveryRate#/deliveryRate', active: false
                        },
                        {
                            id: 13, name: 'Tax Setting', icon: 'cog', url: '/app/taxSetting#/taxSetting', active: false
                        },
                        {
                            id: 14, name: 'Sales Opportunity', icon: 'bars', url: '/app/salesOpportunities#/salesOpportunities', active: false
                        }
                    ]
                },
                ]
            },

             {
                 id: 3, name: 'Products', icon: 'th', url: '/app/product#/product', children: null, active: false
             },

             {
                 id: 4, name: 'Tools', icon: 'wrench', url: '', active: false, children: [
                    {
                        id: 1, name: 'Contact Manager', icon: 'user-o', url: '/app/contactManager#/contactManager', children: null
                    },
                    //{
                    //    id: 2, name: 'Task Manager', icon: 'layers', url: '/app/taskManager#/taskManager', children: null
                    //},
                    //{
                    //    id: 3, name: 'Sales Opportunities', icon: 'layers', url: '/app/salesOpportunities#/salesOpportunities', children: null
                    //},
                    //{
                    //    id: 4, name: 'Tools Settings', icon: 'text_fields', url: '', active: false, children: [
                    //        {
                    //            id: 1, name: 'Sales Opportunity Setting', icon: 'layers', url: '/app/salesOpportunitySetting#/salesOpportunitySetting', children: null
                    //        }
                    //    ]
                    //},
                 ]
             },

             {
                 id: 6, name: 'Accounts Settings', icon: 'money', url: '', active: false, children: [
                 { id: 1, name: 'Accounts', icon: 'money', url: '/app/account#/account', children: null },
                        { id: 2, name: 'Sales Order', icon: 'list', url: '/app/salesOrder#/salesOrder', children: null },
            { id: 3, name: 'Purchase Order', icon: 'list', url: '/app/purchaseOrder#/purchaseOrder', children: null },
                    {
                        id: 4, name: 'Journal Master', icon: 'list', url: '/app/journalMaster#/journalMaster', active: false
                    },
                 ]
             },
            {
                id: 7, name: 'Page Settings', icon: 'money', url: '', active: false
            }



        ];
              vm.menuItemWithSecurity = [
            {
                id: 1, name: 'Dashboard', icon: 'dashboard', url: '/', active: false
            },
            {
                id: 2, name: 'Settings', icon: 'cogs', url: '', active: false, children: [
                    {
                        id: 1, name: 'Company Settings', icon: 'users', url: '', active: false, children: [
                            {
                                id: 1, name: 'User', icon: 'users', url: '/app/user#/user', children: null, active: false
                            },
                            {
                                id: 2, name: 'Security Rights', icon: 'security', url: '/app/securityright#/securityright', children: null, active: false
                            },

                            {
                                id: 3, name: 'Fiscal Year', icon: 'list', url: '/app/fiscalyear#/fiscalyear', children: null, active: false
                            },
                            //{
                            //    id: 4, name: 'Retailer Setting', icon: 'list', url: '/app/retailer#/retailer', active: false
                            //},
                            {
                                id: 5, name: 'Email Setting', icon: 'share', url: '/app/emailSetting#/emailSetting', active: false
                            },
                            {
                                id: 6, name: 'Company Setup', icon: 'users', url: '/app/companySetup#/companySetup', active: false
                            },


                        ]
                    },
                    {
                        id: 2, name: 'Product Settings', icon: 'th', url: '', active: false, children: [

                            {
                                id: 1, name: 'Category', icon: 'bars', url: '/app/productcategory#/productcategory', children: null, active: false
                            },
                            {
                                id: 2, name: 'Item Measure', icon: 'bars', url: '/app/itemMeasure#/itemMeasure', children: null, active: false
                            },
                            {
                                id: 3, name: 'Measure Unit', icon: 'bars', url: '/app/measureUnit#/measureUnit', children: null, active: false
                            },
                            {
                                id: 4, name: 'Theme Setting', icon: 'themeisle', url: '/app/themeSetting#/themeSetting', children: null
                            },
                        ]
                    },
                     

                {
                         id: 4, name: 'Ecommerce Settings', icon: 'shopping-cart', url: '', active: false, children: [
                             {
                                 id: 1, name: 'Vendor', icon: 'user-plus', url: '/app/vendor#/vendor', children: null, active: false
                             },
                             {
                                 id: 2, name: 'Customer', icon: 'user-plus', url: '/app/customer#/customer', children: null, active: false
                             },
                             {
                                 id: 3, name: 'Payment Method', icon: 'bars', url: '/app/paymentMethod#/paymentMethod', active: false
                             },
                             {
                                 id: 4, name: 'Delivery Method', icon: 'bars', url: '/app/deliveryMethod#/deliveryMethod', active: false
                             },
                             {
                                 id: 5, name: 'Payment Term', icon: 'bars', url: '/app/paymentTerm#/paymentTerm', active: false
                             },
                             {
                                 id: 6, name: 'Ecommerce Setting', icon: 'cog', url: '/app/ecommerceSetting#/ecommerceSetting', active: false
                             },
                             {
                                 id: 7, name: 'Discount Setting', icon: 'cog', url: '/app/discountSetting#/discountSetting', active: false
                             },
                             {
                                 id: 8, name: 'Zone Setting', icon: 'cog', url: '/app/zoneSetting#/zoneSetting', active: false
                             },
                             {
                                 id: 9, name: 'Web Setting', icon: 'cog', url: '/app/companywebSetting#/companywebSetting', active: false
                             },
                             {
                                 id: 10, name: 'Banner Setting', icon: 'bars', url: '/app/featuredItem#/featuredItem', active: false
                             },
                             {
                                 id: 12, name: 'Delivery Rate', icon: 'bars', url: '/app/deliveryRate#/deliveryRate', active: false
                             },
                             {
                                 id: 13, name: 'Tax Setting', icon: 'cog', url: '/app/taxSetting#/taxSetting', active: false
                             },
                             {
                                 id: 14, name: 'Sales Opportunity', icon: 'bars', url: '/app/salesOpportunities#/salesOpportunities', active: false
                             }
                         ]
                     },
                ]
            },
           
             {
                 id: 3, name: 'Products', icon: 'th', url: '/app/product#/product', children: null, active: false
             },

             {
                 id: 4, name: 'Tools', icon: 'wrench', url: '', active: false, children: [
                    {
                        id: 1, name: 'Contact Manager', icon: 'user-o', url: '/app/contactManager#/contactManager', children: null
                    },
                    //{
                    //    id: 2, name: 'Task Manager', icon: 'layers', url: '/app/taskManager#/taskManager', children: null
                    //},
                    //{
                    //    id: 3, name: 'Sales Opportunities', icon: 'layers', url: '/app/salesOpportunities#/salesOpportunities', children: null
                    //},
                    //{
                    //    id: 4, name: 'Tools Settings', icon: 'text_fields', url: '', active: false, children: [
                    //        {
                    //            id: 1, name: 'Sales Opportunity Setting', icon: 'layers', url: '/app/salesOpportunitySetting#/salesOpportunitySetting', children: null
                    //        }
                    //    ]
                    //},
                 ]
             },

             {
                 id: 6, name: 'Accounts Settings', icon: 'money', url: '', active: false, children: [
                 { id: 1, name: 'Accounts', icon: 'money', url: '/app/account#/account', children: null },
                        { id: 2, name: 'Sales Order', icon: 'list', url: '/app/salesOrder#/salesOrder', children: null },
            { id: 3, name: 'Purchase Order', icon: 'list', url: '/app/purchaseOrder#/purchaseOrder', children: null },
                    {
                        id: 4, name: 'Journal Master', icon: 'list', url: '/app/journalMaster#/journalMaster', active: false
                    },
                 ]
             },
                  {
                      id: 7, name: 'Page Settings', icon: 'money', url: '/app/pagesetting#/pagesetting', active: false
                  },
                   {
                       id: 8, name: 'Media Library', icon: 'file-image-o', url: '/app/medialibrary#/medialibrary', active: false
                   }
             


        ];

        //$scope.activeMenu = vm.menuItems[0].name;

        if (!$sessionStorage.get('activeMenu')) {
            $sessionStorage.put('activeMenu', vm.menuItems, 30);
        }
        else {
            vm.menuItems = $sessionStorage.get('activeMenu');
        }

        var item = $sessionStorage.get('activeMenu');
        var noActive;
        //vm.activeTab = null;
        //vm.activeSubTab = null;
        //checkActiveTab($sessionStorage.get('activeMenu'));
        companyLogo();
        function companyLogo() {
            layoutService.getCompanyLogo().then(function (result) {
                if (result.success) {
                    vm.companyLogo = result.data.mediaUrl;
                }
            });
        }
        function checkActiveTab(item) {
            for (var i = 0; i < item.length; i++) {
                if (item[i].children) {
                    for (var j = 0; j < item[i].children.length; j++) {
                        if (item[i].children[j].active) {
                            noActive = true;
                            vm.activeSubTab = item[i].children[j];
                            return;
                        }
                        else {
                            noActive = false;
                        }
                    }
                }
                else {
                    if (item[i].active) {
                        noActive = true;
                        vm.activeTab = item[i];
                        return;
                    }
                    else {
                        noActive = false;
                    }
                }
            }
            if (!noActive) {
                vm.menuItems[0].active = true;
            }
        }

        function productPage() {
            $window.location.href = '/product';

        }

        function salesOrder() {
            $window.location.href = '/salesOrder';
        }

        function removeActive(item) {
            for (var i = 0; i < item.length; i++) {
                if (item[i].children) {
                    for (var j = 0; j < item[i].children.length; j++) {
                        item[i].children[j].active = false;
                    }
                    item[i].active = false;
                }
                else {
                    if (item[i].active) {
                        item[i].active = false;
                    }
                    else {

                    }
                }

            }
        }
        function activeTabOnclick(menuItem, isChildren) {
            removeActive($sessionStorage.get('activeMenu'));
            var menuList = $sessionStorage.get('activeMenu');
            if (isChildren) {
                for (var i = 0; i < menuItem.children.length; i++) {
                    if (menuItem.children[i].id === menuItem.id) {
                        //if (menuItem.children[i].active) {
                        //    menuItem.children[i].active = false;
                        //    $sessionStorage.remove('activeMenu');
                        //    menuList.children = menuItem.children;
                        //    $sessionStorage.put('activeMenu', menuList, 30);
                        //} else {
                        //    menuItem.children[i].active = true;
                        //    $sessionStorage.remove('activeMenu');
                        //    menuList.children = menuItem.children;
                        //    $sessionStorage.put('activeMenu', menuList, 30);
                        //    $window.location.href = isChildren.url;
                        //}


                        //test code for here
                        menuItem.children[i].active = true;
                        $sessionStorage.remove('activeMenu');
                        menuList.children = menuItem.children;
                        $sessionStorage.put('activeMenu', menuList, 30);
                        $window.location.href = isChildren.url;
                    }
                }

            }
            else {
                for (var i = 0; i < menuList.length; i++) {
                    if (menuList[i].id === menuItem.id) {
                        if (menuList[i].active) {
                            menuList[i].active = false;
                            $sessionStorage.remove('activeMenu');
                            $sessionStorage.put('activeMenu', menuList, 30);
                        } else {
                            menuList[i].active = true;
                            $sessionStorage.remove('activeMenu');
                            $sessionStorage.put('activeMenu', menuList, 30);
                            if (menuItem.url) {
                                $window.location.href = menuItem.url;
                            }
                        }


                    }
                    else {

                    }
                }
            }

        }
        checkloginStatus();
        function checkloginStatus() {
            layoutService.checkLogin().then(function (result) {
                if (result.success) {
                    vm.companyDetails = result.data;
                    if (result.data.isCompanyInitialized === false) {
                        vm.disableMenu = true;
                    }
                    else {
                        vm.companyDetails = result.data;
                        if (result.data.isEstoreInitialized === false) {
                            vm.disableMenu = true;
                        }
                        else {
                            //vm.disableMenu = false;
                        }
                        //vm.isLoading = false;
                    }

                }
                else {
                    vm.disableMenu = true;
                }
            });
        }

        vm.test = test;

        function test() {
            console.log("Test");
        }
        function getLoggedinUserDetails() {
            layoutService.getLoggedinUserDetails().
            then(function (result) {
                if (result.success) {
                    vm.userDetails = result.data.hasAuthorityToAssignRight;
                }
                else {
                    alert(result.error);
                }
            });
        }

    }
})();

