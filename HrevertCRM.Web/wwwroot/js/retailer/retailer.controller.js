(function () {
    angular.module("app-retailer")
        .controller("retailerController", retailerController);
    retailerController.$inject = ['$http', '$scope', '$window', '$filter', 'retailerService'];

    function retailerController($http, $scope, $window, $filter, retailerService) {
        var vm = this;
        vm.searchRetailers = searchRetailers;
        vm.assignCompanyAsRetailer = assignCompanyAsRetailer;
        vm.removeRetailer = removeRetailer;
        vm.retailers = [{ id: 1, name: 'text' }, { id: 2, name: 'text2' }, { id: 3, name: 'text3' }, { id: 4, name: 'text4' }];
        //vm.companyList = [{ id: 1, name: 'text' }, { id: 2, name: 'text2' }, { id: 3, name: 'text3' }, { id: 4, name: 'text4' }];
        //init();

        function removeRetailer(retailer) {
            
        }
        function assignCompanyAsRetailer(retailer) {
            vm.retailerList = {};
            vm.retailerList.Retailers = [];
            for (var i = 0; i < vm.retailers.length; i++) {
                vm.retailerList.Retailers.push(vm.retailers[i].id);
            }
            retailerService.assignRetailer(vm.retailerList).then(function (result) {
                if (result.success) {
                    // show dialog say that retailer assigned
                    init();
                } else {
                    //error message
                }
            });
        }
        function searchRetailers(text) {
            retailerService.searchRetailer(text).then(function(result) {
                if (result.success) {
                    vm.companyList = result.data;
                } else {
                    vm.companyList = [];
                    //error code goes here
                }
            });
        }

        init();
        function init() {
            //retailerService.getRetailersById(4).then(function(result) {
            //    if (result.success) {
            //        vm.retailers = result.data;
            //    }
            //    else {
            //        //error code here
            //    }
            //});
            retailerService.getAllCompany().then(function (result) {
                if (result.success) {
                    vm.companyList = result.data;
                } else {
                    //error code here
                }
            });
        }
    }

})();

