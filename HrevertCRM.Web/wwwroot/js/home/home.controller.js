(function () {
    angular.module("app-home")
        .controller("homeController", homeController);
    homeController.$inject = ['$http', '$scope', '$interval', '$window', '$filter', 'homeService'];
    function homeController($http, $scope, $interval, $window, $filter, homeService) {
        var vm = this;
        vm.isLoading = false;
        vm.today = new Date();

        var tick = function () {
            vm.clock = Date.now();
        }
        tick();
        $interval(tick, 1000);
        getCurrentFiscalYear();
        var swalFunction = function (item) {
            swal({
                title: item,
                text: "You will not be able to recover this imaginary file!",
                type: "info",
                showCancelButton: false,
                showConfirmButton: false
            });
        };
        function testMessageDialog() {
            swalExtend({
                swalFunction: swalFunction,
                swalFunctionParams: "Are you sure?",
                hasCallback: true,
                hasCancelButton: true,
                buttonNum: 3,
                buttonNames: ["delete", "yes", "No"],
                clickFunctionList: [
                    function () {
                        console.debug('ONE BUTTON');
                    },
                    function () {
                        console.debug('TWO BUTTON');
                    },
                    function () {
                        console.debug('Three BUTTON');
                    }
                ]
            });
        }
        checkFirstLogin();
        function checkFirstLogin() {
            vm.isLoading = true;
            homeService.checkLogin().then(function (result) {
                if (result.success) {
                    if (result.data.isCompanyInitialized === false) {
                        $window.location.href = 'app/setup#!/setup';
                    }
                    else {
                        if (result.data.isEstoreInitialized === false) {
                            $window.location.href = 'app/companywebSetting#!/companywebSetting';
                        }
                        else {
                            vm.isLoading = false;
                        }
                        vm.isLoading = false;
                    }

                }
                else {
                    //code here
                }
            });

        }
        vm.reloadChart = reloadChart;
        vm.fiscalYearChange = fiscalYearChange;
        vm.fileInput = fileInput;


        //function companyWebSetting(id) {
        //    homeService.getCompanyWebSetting(id).then(function (result) {
        //        if (result.success) {
        //            if(result.data.fiscalYearFormat === 0){
        //                $window.location.href = 'app/setup#!/setup';
        //            }
        //        }
        //    });
        //}
        function fileInput(file) {
            //console.log("test");
        }

        vm.chartLineSeries = ['Total Order', 'Total Sales'];


        vm.chartLineOptions = {
            maintainAspectRatio: false,
            responsive: true
        };

        vm.chartLineColours = ['#494750', '#cc3321'];

        function fiscalYearChange(fiscalYear) {
            vm.isLoading = true;
            getDashboardValue(fiscalYear);
        }
        getFiscalYear();
        function reloadChart(fiscalYear) {
            vm.isLoading = true;
            getDashboardValue(fiscalYear);
        }
        function getFiscalYear() {
            homeService.getFiscalYear().then(function (result) {
                if (result.success) {
                    //companyWebSetting(result.data[0].companyId);
                    vm.fiscalYear = result.data;
                    //vm.activefiscalYear = vm.fiscalYear[0].id;
                    // getDashboardValue(vm.fiscalYear[0].id);
                }
                else {
                    //console.log("Oops, Fiscal year retrive problem :(")
                }
            });
        }
        vm.chartPieLabels = [];
        vm.chartPieCharts = [];

        function getDashboardValue(fiscalYear) {
            vm.isLoading = true;
            homeService.getDashboardValue(fiscalYear).then(function (result) {
                if (result.success) {
                    vm.dashboardValue = result.data;
                    vm.chartLineCharts = [];
                    vm.chartLineLabels = [];
                    vm.chartLineCharts[0] = [];
                    vm.chartLineCharts[1] = [];
                    if (result.data.orderFiscalPeriodWise === null) {
                        vm.noChartData = "Data not found";
                    } else {
                        for (var i = 0; i < result.data.orderFiscalPeriodWise.length; i++) {
                            vm.chartLineCharts[0].push(result.data.orderFiscalPeriodWise[i].orderCount);
                        }
                    }
                    if (result.data.salesFiscalPeriodWise === null) {
                        vm.noChartData = "Data not found";
                    } else {
                        for (var j = 0; j < result.data.salesFiscalPeriodWise.length; j++) {
                            //this is for chart
                            vm.chartLineLabels.push(result.data.salesFiscalPeriodWise[j].fiscalPeriodName);
                            vm.chartLineCharts[1].push(result.data.salesFiscalPeriodWise[j].salesCount);
                            //this is for donut
                            //vm.chartPieLabels.push(result.data.salesFiscalPeriodWise[i].fiscalPeriodName);
                            //vm.chartPieCharts.push(result.data.salesFiscalPeriodWise[i].salesCount)
                        }
                    }


                    vm.isLoading = false;
                } else {
                    //console.log("Error getting dashboard value :(");
                }
            });
        }
        vm.reportbug = reportbug;
        function reportbug(bug) {
            var bugViewModel = {};
            bugViewModel.Message = bug;
            homeService.report(bugViewModel).then(function (result) {
                if (result.success) {
                    //  vm.model =  'data-dismiss="modal"';
                    //alert("Your problem is reported");
                    $('#myModal').modal('hide');
                }
                else {

                }
            });
        }
        function getCurrentFiscalYear() {
            homeService.getCurrentFiscalYear().then(function (result) {
                if (result.success && result.data != null) {
                    var currentFiscalYearData = result.data;
                    vm.activefiscalYear = currentFiscalYearData.id;
                    homeService.getDashboardValue(currentFiscalYearData.id).then(function (result) {
                        if (result.success) {
                            vm.dashboardValue = result.data;
                            vm.chartLineCharts = [];
                            vm.chartLineLabels = [];
                            vm.chartLineCharts[0] = [];
                            vm.chartLineCharts[1] = [];
                            if (result.data.orderFiscalPeriodWise === null) {
                                vm.noChartData = "Data not found";
                            } else {
                                for (var i = 0; i < result.data.orderFiscalPeriodWise.length; i++) {
                                    vm.chartLineCharts[0].push(result.data.orderFiscalPeriodWise[i].orderCount);
                                }
                            }
                            if (result.data.salesFiscalPeriodWise === null) {
                                vm.noChartData = "Data not found";
                            } else {
                                for (var j = 0; j < result.data.salesFiscalPeriodWise.length; j++) {
                                    //this is for chart
                                    vm.chartLineLabels.push(result.data.salesFiscalPeriodWise[j].fiscalPeriodName);
                                    vm.chartLineCharts[1].push(result.data.salesFiscalPeriodWise[j].salesCount);
                                    //this is for donut
                                    //vm.chartPieLabels.push(result.data.salesFiscalPeriodWise[i].fiscalPeriodName);
                                    //vm.chartPieCharts.push(result.data.salesFiscalPeriodWise[i].salesCount)
                                }
                            }


                            vm.isLoading = false;
                        } else {
                            //console.log("Error getting dashboard value :(");
                        }
                    });
                }
                else {
                    if (vm.activefiscalYear != null) {
                        // alert("Fiscal Year does not exist for current date, Please add a new Fiscal Year for Current Date");
                        vm.activefiscalYear = vm.fiscalYear[0].id
                    }
                    else {
                        vm.noChartData = "Fiscal Year does not exist, Please add a new Fiscal Year";
                    }
                }
            });
        }
    }

})();

