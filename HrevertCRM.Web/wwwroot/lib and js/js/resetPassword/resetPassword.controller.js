(function () {
    angular.module("app-resetPassword")
        .controller("resetPasswordController", resetPasswordController);
    resetPasswordController.$inject = ['$http', '$scope', '$filter', '$window', 'resetPasswordService'];

    function resetPasswordController($http, $scope, $filter, $window, resetPasswordService) {
        var url
        var vm = this;
        var str = 'http://localhost:11703/Account/ConfirmEmail?userId=72469a70-466f-44d5-91d6-e5ae06434351&code=CfDJ8FEL4QswamFBlL7G1K568GnJfUX94mVQ%2FBwaA20UXz1KDMJ8WciCih7ye52A%2FWFiNwRCpAhTfXp6FI03pX5g7v%2FRn7a%2BfW4izry5ACIQJktY0qoOqs0RXkW0oWffXQZMFf2Qw7UtPPEGxBgJxjskFk%2FtpdjCmJBIUBKG428F%2BuJU8WKEcxkk3xvJKzvtIQY6fTUlOcjNyMvz1R2E6zLsoqMOb8VscVuEC5XLKOMJp6QOM0MSpioPiXNnWl96WLIeRw%3D%3D';
        var userId = str.substring(str.lastIndexOf("userid=") + 1, str.lastIndexOf("&"));
        var code = str.substring(str.lastIndexOf("code="));
    }

})();

