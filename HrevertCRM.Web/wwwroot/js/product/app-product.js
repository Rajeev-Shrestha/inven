(function () {
    "use strict";
    angular.module("app-product", ['ui.router', 'ngSanitize', 'ngMessages', 'bw.paging', 'ngImgCrop', 'btorfs.multiselect', 'ui.select'])
    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', configRoutes]);
    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider) {
        $urlRouterProvider.otherwise('/product');

        $stateProvider
            .state('product',
            {
                url: '/product',
                templateUrl: '/templates/product/product.html',
                controller: 'productController',
                controllerAs: 'vm'

            });

        //.state('categoriestree',
        //    {
        //        url: '/categoriestree',
        //        templateUrl: '/templates/product/categoriestree.html',
        //        controller: 'CategoryController',
        //        controllerAs: 'vm'

        //    });
        //$locationProvider.html5Mode(true);
        configureHttp($httpProvider);
    };
  
   function configureHttp($httpProvider) {
       //initialize get if not there
       if (!$httpProvider.defaults.headers.get) {
           $httpProvider.defaults.headers.get = {};
       }

       //disable IE ajax request caching
       $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
       // extra
       $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
       $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
   };

   
})();