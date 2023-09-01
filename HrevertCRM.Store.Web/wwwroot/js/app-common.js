(function () {
    "use strict";

    angular
        .module("common", ["ngRoute"])
        .factory("viewModelHelper", ["$http", "$q", "$window", "$location", "$timeout",
            function($http, $q, $window, $location, $timeout) {
                return window.App.viewModelHelper($http, $q, $window, $location, $timeout);
            }
        ]);

    angular
        .module("main", ["common"]);

    //window.App.viewModelHelper = function ($http, $q, $window, $location) {

    //    var self = this;
     
    //    self.apiGet = function (uri, data, success, failure, always) {
    //        $http.get('/api/getdata?apiUrl=' + encodeURIComponent(uri), data)
    //            .then(function (result) { self.successCallback(result, success, always); },
    //                function (result) { self.errorCallback(result, failure, always); });
    //    }

    //    self.apiPost = function (uri, data, success, failure, always) {
    //        $http.post(window.App.rootPath + uri, data)
    //            .then(function (result) { self.successCallback(result, success, always); },
    //                function (result) { self.errorCallback(result, failure, always); });
    //    }

    //    self.apiPut = function (uri, data, success, failure, always) {
    //        $http.put(window.App.rootPath + uri, data)
    //            .then(function (result) { self.successCallback(result, success, always); },
    //                function (result) { self.errorCallback(result, failure, always); });
    //    }

    //    self.apiDelete = function (uri, data, success, failure, always) {
    //        $http.delete(window.App.rootPath + uri, data)
    //            .then(function (result) { self.successCallback(result, success, always); },
    //                function (result) { self.errorCallback(result, failure, always); });
    //    }

    //    self.successCallback = function (result, success, always) {
    //        success(result);
    //        if (always != null) {
    //            always();
    //        }
    //    }
      

    //    self.errorCallback = function (result, failure, always) {
    //        if (result.status < 0) {
    //            self.alerts.error("No internet connectivity detected. Please reconnect and try again.");
    //        } else {
    //            var message = result.status + ":" + result.statusText;
    //            if (result.data != null && result.data.message != null)
    //                message += " - " + result.data.message;
    //            self.alerts.error(message);
    //            if (failure != null) {
    //                failure(result);
    //            }
    //            if (always != null) {
    //                always();
    //            }
    //        }
    //    }

    //    self.navigateTo = function (path) {
    //        $location.path(window.App.rootPath + path);
    //    }

    //    self.refreshPage = function (path) {
    //        $window.location.href = window.App.rootPath + path;
    //    }

    //    self.clone = function (obj) {
    //        return JSON.parse(JSON.stringify(obj));
    //    }

    //    return this;
    //};
})();
