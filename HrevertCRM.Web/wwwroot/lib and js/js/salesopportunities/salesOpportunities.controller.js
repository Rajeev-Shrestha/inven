(function () {
    angular.module("app-salesOpportunities")
        .controller("salesOpportunitiesController", salesOpportunitiesController);
    salesOpportunitiesController.$inject = ['$http', '$filter', '$scope', 'salesOpportunitiesService'];
    function salesOpportunitiesController($http, $filter, $scope, salesOpportunitiesService) {
        var vm = this;
        vm.customers = null;
        vm.salesRepresentatives = null;
        vm.stages = null;
        vm.sources = null;
        vm.grades = null;
        vm.reasons = null;
        vm.getAllCustomers = getAllCustomers;
        vm.getAllSalesRepresentatives = getAllSalesRepresentatives;
        vm.getAllStages = getAllStages;
        vm.loadAllData = loadAllData;

        vm.getAllSources = getAllSources;
        vm.getAllReasonsClosed = getAllReasonsClosed;
        vm.getAllGrades = getAllGrades;
        vm.createOpportunity = createOpportunity;
        vm.salesPriorities = [{ id: 0, name: 'High' }, { id: 1, name: 'Low' }, { id: 2, name: 'Normal' }, { id: 3, name: 'Followup' }];
        vm.getAllAcitveSalesOpportunities = getAllAcitveSalesOpportunities;
        vm.showSalesOpportunity = showSalesOpportunity;
        vm.disableStage = false;
        vm.salesOpportunity = {};
        vm.salesOpportunity.stageId = null;
        vm.buttonText = "Set";
        vm.reset = reset;
        vm.deleteOpportunity = deleteOpportunity;
        vm.check = false;
        vm.checkInActivateCard = checkInActivateCard;
        vm.getAllSalesOpportunities = getAllSalesOpportunities;
        vm.activateOpportunity = activateOpportunity;
        init();

        function init() {
            getAllAcitveSalesOpportunities();
            getAllStages();
        }

        function loadAllData(stage)
        {
            getAllCustomers();
            getAllSalesRepresentatives();           
            getAllSources();
            getAllReasonsClosed();
            getAllGrades();
            vm.salesOpportunity.stageId = stage.id;
            vm.disableStage = true;
        }

        function reset()
        {
            vm.salesOpportunity = {};
            vm.buttonText = "Set";
        }

        //function loadAllData(stage) {
        //    salesOpportunitiesService.getStageById(stage.id).then(function (result) {
        //        if (result.success) {
                    
        //            getAllCustomers();
        //            getAllSalesRepresentatives();
        //            getAllSources();
        //            getAllReasonsClosed();
        //            getAllGrades();
        //            vm.stages = result.data;
        //            vm.disableStage = true;

        //        } else {

        //            alert(result.error);
        //        }
        //    });
        //}

        // vm.createSalesOpportunities = createSalesOpportunities;

        function activateOpportunity(sale) {
            sale.Active = true;
            salesOpportunitiesService.updateOpportunity(sale).then(function (result) {
                if (result.success) {
                    vm.salesOpportunities = result.data;
                    vm.salesOpportunity = {};
                    $('#myModal').modal('hide');
                    getAllSalesOpportunities();
                } else {
                    alert(result.error);
                }
            });

        }

        function getAllCustomers() {
            salesOpportunitiesService.getAllCustomers().then(function (result) {
                if (result.success) {
                    vm.customers = result.data;
                } else {

                    alert(result.error);
                }
            });
        }
        function getAllSalesOpportunities() {
            salesOpportunitiesService.getAllSalesOpportunities().then(function (result) {
                if (result.success) {
                    vm.salesOpportunities = result.data;
                } else {

                    alert(result.error);
                }
            });
        }
        function getAllSalesRepresentatives() {
            salesOpportunitiesService.getAllSalesRepresentatives().then(function (result) {
                if (result.success) {
                    vm.salesRepresentatives = result.data;
                    //vm.salesPriorities= 
                } else {
                    alert(result.error);
                }
            });
        }
        function getAllStages() {
            salesOpportunitiesService.getAllStages().then(function (result) {
                if (result.success) {
                    vm.stages = result.data;
                    $scope.containerInnerStyle={
                        width: 250 * vm.stages.length
                    }
                } else {
                    alert(result.error);
                }
            });
        }
        function getAllSources() {
            salesOpportunitiesService.getAllSources().then(function (result) {
                if (result.success) {
                    vm.sources = result.data;
                } else {
                    alert(result.error);
                }
            });
        }
        function getAllReasonsClosed() {
            salesOpportunitiesService.getAllReasonsClosed().then(function (result) {
                if (result.success) {
                    vm.reasons = result.data;
                } else {
                    alert(result.error);
                }
            });
        }
        function getAllGrades() {
            salesOpportunitiesService.getAllGrades().then(function (result) {
                if (result.success) {
                    vm.grades = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //function createSalesOpportunities() {
        //    if (result.success) {
        //        alert("success");
        //    } else {
        //        alert("try again!");
        //    }
        //}

        function createOpportunity(opportunity) {
            opportunity.closingDate = moment(opportunity.closingDate).format("MM-DD-YYYY HH:mm:ss");
            opportunity.closedDate = moment(opportunity.closedDate).format("MM-DD-YYYY HH:mm:ss");

            if (!opportunity.id) {
                vm.buttonText = "Set";
                salesOpportunitiesService.createOpportunity(opportunity).then(function (result) {
                    if (result.success) {
                        vm.salesOpportunities = result.data;
                        vm.salesOpportunity = {};
                        $('#myModal').modal('hide');
                        getAllAcitveSalesOpportunities();
                    } else {
                        alert(errors);
                    }
                });
            }
            else {
                salesOpportunitiesService.updateOpportunity(opportunity).then(function (result) {
                    if (result.success) {
                        vm.salesOpportunities = result.data;
                        vm.salesOpportunity = {};
                        $('#myModal').modal('hide');
                        getAllAcitveSalesOpportunities();
                    } else {
                        alert(result.error);
                    }
                });
            }
        }

        function deleteOpportunity(sale) {
            salesOpportunitiesService.deleteOpportunity(sale.id).then(function (result) {
                if (result.success) {
                     $('#myModal').modal('hide');
                    getAllAcitveSalesOpportunities();
                } else {
                    alert(result.error);
                }
            });
        }
        vm.color = "";
        function getAllAcitveSalesOpportunities() {
            salesOpportunitiesService.getAllAcitveSalesOpportunities().then(function (result) {
                if (result.success) {
                    vm.salesOpportunities = result.data;
                } else {
                    alert("error");
                }
            });
        }

        function showSalesOpportunity(sale) {
            vm.disableStage = false;
            vm.buttonText = "Update";
            vm.salesOpportunity = sale;
            loadAllData();
            getAllStages();
        }

        function checkInActivateCard(checked) {
            if (checked) {
                vm.check = true;
                getAllSalesOpportunities();
            } else {
                vm.check = false;
                getAllAcitveSalesOpportunities();
            }
           

        }

    }

})();