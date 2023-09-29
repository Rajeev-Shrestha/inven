(function() {
    "use strict";
    angular.module("common",[]).service('commonService', commonService);

    function commonService() {

        var service = {};
        service.addCode = addCode;
        service.removeCode = removeCode;
        service.getDateOnlyFromDateAndTime = getDateOnlyFromDateAndTime;
        service.getTimeOnlyFromDateAndTime = getTimeOnlyFromDateAndTime;
        return service;

        function addCode(code, number) {
            var result = "";
            if (number != undefined) {
                result = code + number;
            }
            return result;
        }

        function removeCode(number) {
            var result = "";
            if (number != undefined) {
                result = number.substring(number.indexOf("-") + 1);
            }
            return result;
        }

        function getDateOnlyFromDateAndTime(date) {

            var result;
            if (date != undefined) {
                result = moment(date).format("YYYY-MM-DD");
            }

            return result;
        }

        function getTimeOnlyFromDateAndTime(date) {
            var result;
            if (date != undefined) {
                result = moment(date).format("HH:mm");
            }

            return result;
        }
    }
           
     
})();

var app = angular.module('common', []);

app.service('commonService', [function () {

    this.addCode = function(code, number) {
            var result = "";
            if (number != undefined) {
                result = code +"-"+ number;
            }
            return result;
        }

    this.removeCode = function(number) {
            var result = "";
            if (number != undefined) {
                result = number.substring(number.indexOf("-") + 1);
            }
            return result;
        }

    this.removeCodeFromList = function (addresses) {
        var result = "";
        if (addresses !== []) {
            for (var i = 0; i < addresses.length; i++) {
                if (addresses[i].mobilePhone !== undefined) {
                    addresses[i].mobilePhone = addresses[i].mobilePhone.substring(addresses[i].mobilePhone.indexOf("-")+1);
                }
                if (addresses[i].telephone !== undefined) {
                    addresses[i].telephone = addresses[i].telephone.substring(addresses[i].telephone.indexOf("-") + 1);
                }
            }
            result = addresses;
        }
        return result;
    }

    this.getDateOnlyFromDateAndTime=function(date)
    {
        var result;
        if (date != undefined) {
            result = moment(date).format("YYYY-MM-DD");
        }

        return result;
    }

    this.getTimeOnlyFromDateAndTime = function (date) {
        var result;
        if (date != undefined) {
            result = moment(date).format("HH:mm");
        }

        return result;
    }
}]);