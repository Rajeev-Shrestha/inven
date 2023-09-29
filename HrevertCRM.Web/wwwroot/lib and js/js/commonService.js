angular.module('passData', [])

.service('dataService', function () {
    // private variable
    var _dataObj = {};

    this.dataObj = _dataObj;
});