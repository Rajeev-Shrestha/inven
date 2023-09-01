(function () {
    angular.module("app-placeorder")
        .controller("placeorderController", placeorderController);
    placeorderController.$inject = ['$http', '$filter', '$cookies', 'placeorderService', 'viewModelHelper'];
    function placeorderController($http, $filter, $cookies, placeorderService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active';
        viewModelHelper.refreshCart();
        vm.shippingCost = 50;
        vm.grandTotal = 0;
        vm.cartId = $cookies.get('cartId');
        placeorderService.crmLocation()
               .then(function (result) {
                   if (result.success) {
                       vm.crmLocations = result.data;
                   } else {
                       var message = {};
                       message.message = "get CRM location , " + result.message + " in placeorder.,";
                       viewModelHelper.bugReport(message,
                         function (result) {
                         });
                   }
               });
        init();
        vm.loginUser = null;
        function init() {
            placeorderService.getloginUser()
            .then(function (result) {
                if (result.data > 0) {
                    vm.loginUser = result.data;
                    //init(cartId);
                } else {
                    vm.loginUser = 0;
                }
                placeorderService.getCartItems(vm.cartId, vm.loginUser)
               .then(function (result) {
                   if (result.success) {
                       vm.cartDetails = result.data;
                       for (var i = 0; i < vm.cartDetails.shoppingCartDetails.length; i++) {
                           vm.grandTotal = vm.grandTotal +
                               (vm.cartDetails.shoppingCartDetails[i].quantity *
                                   vm.cartDetails.shoppingCartDetails[i].productCost) +
                               vm.shippingCost;
                       }
                       placeorderService.getAddressById(vm.cartDetails.shippingAddressId)
                               .then(function (result) {
                                   if (result.success) {
                                       vm.billingAddressDetails = result.data;
                                   } else {
                                       var message = {};
                                       message.message = "get shipping address by id , " + result.message + " in placeorder.,";
                                       viewModelHelper.bugReport(message,
                                         function (result) {
                                         });
                                   }
                                   placeorderService.getAddressById(vm.cartDetails.billingAddressId)
                                   .then(function (result) {
                                       if (result.success) {
                                           vm.shippingAddressDetails = result.data;
                                           vm.class = 'loader loader-default';
                                       } else {
                                           var message = {};
                                           message.message = "get billing address by id , " + result.message + " in placeorder.,";
                                           viewModelHelper.bugReport(message,
                                             function (result) {
                                             });
                                       }
                                       //placeorderService.getPaymentInformation(vm.cartDetails.paymentInformationId)
                                       //.then(function (result) {
                                       //    if (result.success) {
                                       //        vm.paymentInformation = result.data;
                                       //    }
                                       //});
                                   });
                               });

                       
                       $cookies.remove('cartId');
                       $cookies.remove('guid');
                       viewModelHelper.refreshCart();
                   } else {
                       var message = {};
                       message.message = "get cart item , " + result.message + " in placeorder.,";
                       viewModelHelper.bugReport(message,
                         function (result) {
                         });
                   }
               });
            });
            
        }
    }
})();