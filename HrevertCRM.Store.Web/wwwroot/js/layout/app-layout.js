/// <reference path="../../../views/shared/_layout.cshtml" />
(function () {
    "use strict";
    angular.module("common", ['ui.router', 'ngStorage', 'ui.bootstrap', 'ngCookies', 'angular.css.injector'])

    .config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$locationProvider', '$qProvider', configRoutes])
       .factory("viewModelHelper", ["$http", "$q", "$window", "$location", "$timeout", "$cookies","$rootScope",
           function ($http, $q, $window, $location, $timeout, $cookies, $rootScope) {
               return window.App.viewModelHelper($http, $q, $window, $location, $timeout, $cookies, $rootScope);
           }
       ]);
 
    angular.module("app-layout", ["common"]);
    function configRoutes($urlRouterProvider, $stateProvider, $httpProvider, $locationProvider,$qProvider) {
        //$urlRouterProvider.otherwise('/');

        //$stateProvider
        //    .state('/',
        //    {
        //        url: '/',
        //        templateUrl: '/templates/index.html',
        //        controller: 'indexController',
        //        controllerAs: 'vm'
        //    });
        $qProvider.errorOnUnhandledRejections(false);
        $locationProvider.html5Mode(true);
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
    window.App.viewModelHelper = function ($http, $q, $window, $location, $timeout, $cookies, $rootScope) {

        var self = this;
        $window.rootScopes = $window.rootScopes || [];
        $window.rootScopes.push($rootScope);

        self.cartItems = { totalItems: 0, items: [] };
       
        var selfUpdateItemCount = function () {
            
            var loginUser = null;
            var cartId = null;
            $http.get('/api/getloggedinuser').then(function (result) {
                
                if (result.status === 200) {
                    if (result.data > 0) {
                        loginUser = result.data;
                    } else {
                        loginUser = 0;
                    }
                    if ($cookies.get('cartId') > 0) {
                        cartId = $cookies.get('cartId');
                    } else {
                        cartId = 0;
                    }
                    //var cartId = $cookies.get('cartId');
                    var apiUrl = "/api/ShoppingCart/getshoppingcart/" + cartId + "/" + loginUser + "/" + $cookies.get('guid');
                    $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl)).then(function (result) {
                        if (result.status) {
                            self.cartItems = result.data;
                            $cookies.put('cartId', result.data.id);
                            callDigestforScope();

                        }
                    });
                }
            });
            
        };
      //  selfUpdateItemCount();
        self.refreshCart = selfUpdateItemCount; 
       
        function callDigestforScope() {
            angular.forEach($window.rootScopes, function (scope) {
                if (!scope.$$phase) {
                    scope.$apply();
                }
            });
        }

        self.getTotalItems = function () {
            return totalItems;
        }
        self.apiGet = function (uri, data, success, failure, always) {
            $http.get('/api/getdata?apiUrl=' + encodeURIComponent(uri), data)
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        } 
        self.apiGetLocal = function (uri, data, success, failure, always) {
            $http.get(uri, data)
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        }
        self.addtoCart = function addItemInCart(item, success, failure, always) {
            var apiUrl = "/api/ShoppingCart/addtocart";
            var dataToPost = { url: apiUrl, data: item }
            $http.post('/api/PostWithData', dataToPost)
               .then(function (result) {
                   self.cartItems = result.data;
                   $cookies.put('cartId', result.data.id);
                   callDigestforScope();
                   self.successAddToCartCallback(result, success, always);
               },
                   function (result) { self.errorCallback(result, failure, always); });
        }

        

        self.bugReport = function reportBug(item, success, failure, always) {
            return;
            var apiUrl = "/api/BugLogger/reportBug";
            var dataToPost = { url: apiUrl, data: item }
            $http.post('/api/PostWithData', dataToPost)
               .then(function (result) {
                   self.successCallback(result, success, always);
               },
                   function (result) { self.errorCallback(result, failure, always); });
        }


        self.apiPost = function (uri, data, success, failure, always) {
            $http.post(window.App.rootPath + uri, data)
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        }

        self.apiPut = function (uri, data, success, failure, always) {
            $http.put(window.App.rootPath + uri, data)
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        }

        self.apiDelete = function (uri, data, success, failure, always) {
            $http.delete(window.App.rootPath + uri, data)
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        }

        self.getLoginUser = function (uri, data, success, failure, always) {
            $http.get('/api/getloggedinuser/')
                .then(function (result) { self.successCallback(result, success, always); },
                    function (result) { self.errorCallback(result, failure, always); });
        }

        self.getTrendingProducts = function (uri, data, success, failure, always) {
            var apiUrl = '/api/Product/lastestproducts/' + buildUriForPaging(1, 10);
            $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl), data)
                .then(function (result) {
                    if (result.status === 200) {
                        self.trendingProducts = result.data.items;
                    }
                    
                },
                    function (result) { self.errorCallback(result, failure, always); });
        }
        self.getLatestProducts = function (uri, data, success, failure, always) {
            var apiUrl = '/api/Product/lastestproducts/' + buildUriForPaging(1, 10);
            $http.get('/api/getdata?apiUrl=' + encodeURIComponent(apiUrl), data)
                .then(function (result) {
                    if (result.status === 200) {
                        self.trendingProducts = result.data.items;
                    }

                },
                    function (result) { self.errorCallback(result, failure, always); });
        }

        self.getCartItems = function (uri, data, success, failure, always) {
            $http.get(uri)
                .then(function (result) {
                    self.successCallback(result, success, always);
                },
                    function (result) {
                        self.errorCallback(result, failure, always);
                    });
        }

        self.successCallback = function (result, success, always) {
            success(result);
            if (always != null) {
                always();
            }
        }


        self.successAddToCartCallback = function (result, success, always) {
            //selfUpdateItemCount();
            //callDigestforScope();
            success(result);
            if (always != null) {
                always();
            }
        }


        self.errorCallback = function (result, failure, always) {
            if (result.status < 0) {
                self.alerts.error("No internet connectivity detected. Please reconnect and try again.");
            } else {
                var message = result.status + ":" + result.statusText;
                if (result.data != null && result.data.message != null)
                    message += " - " + result.data.message;
                self.alerts.error(message);
                if (failure != null) {
                    failure(result);
                }
                if (always != null) {
                    always();
                }
            }
        }

        self.navigateTo = function (path) {
            $location.path(window.App.rootPath + path);
        }

        self.refreshPage = function (path) {
            $window.location.href = window.App.rootPath + path;
        }

        self.clone = function (obj) {
            return JSON.parse(JSON.stringify(obj));
        }
        function buildUriForPaging(pageIndex, pageSize) {
            var uri = '?pagenumber=' + pageIndex + '&pageSize=' + pageSize;
            return uri;
        }
      
        return this;
    };

    
})();