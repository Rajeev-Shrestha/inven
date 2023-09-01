(function () {
    angular.module("app-salesOpportunitySetting")
        .controller("salesOpportunitySettingController", salesOpportunitySettingController);
    salesOpportunitySettingController.$inject = ['$http', '$filter', '$scope', 'salesOpportunitySettingService'];
    function salesOpportunitySettingController($http, $filter, $scope, salesOpportunitySettingService) {

        var vm = this;

        // stage controller
        vm.buttonText = "Set";
        vm.createStage = createStage;
        vm.getAllActiveStages = getAllActiveStages;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.actionChanged = actionChanged;
        vm.deleteStage = deleteStage;
        vm.checkInActiveStage = checkInActiveStage;
        vm.getAllStages = getAllStages;
        vm.activateStage = activateStage;
        vm.resetGrade = resetGrade;
        vm.resetStageform = resetStageform;
        // vm.checkInActiveStage = checkInActiveStage;

        init();
        function init() {
            getAllActiveStages();
            getAllActiveSources();
            getAllActiveGrades();
            getAllActiveReasons();
        }
        function resetStageform() {
            vm.stage = {};
        }
        //getAllActiveStage
        function getAllActiveStages() {
            salesOpportunitySettingService.getAllActiveStages().then(function (result) {
                if (result.success) {
                    vm.stages = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //getAllStage
        function getAllStages() {
            salesOpportunitySettingService.getAllStages().then(function (result) {
                if (result.success) {
                    vm.stages = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //checkInActiveStage
        function checkInActiveStage(checked) {
            if (checked) {
                vm.check = true;
                getAllStages();
            }
            else {
                vm.check = false;
                getAllActiveStages();
            }
        }

        //activate Stage
        function activateStage(stage) {
            stage.Active = true;
            salesOpportunitySettingService.updateStage(stage)
                .then(function (result) {
                    if (result.success) {
                        swal("Stage Activated.");
                        getAllStages();
                    } else {
                        //console.log(result.message);
                        alert("Error");
                    }
                });
        }      

        //show stage
        function showStage(stage) {
            vm.buttonText = "Update";
            // taskChecked(task.reminder);
            vm.stage = stage;
        }

        //create and update stage
        function createStage(stage) {
            if (!stage.id) {
                vm.buttonText = "Set";
                salesOpportunitySettingService.createStage(stage).then(function (result) {
                    if (result.success) {
                        vm.stages.push(result.data);
                        vm.stage = {};
                        $('#addStage').modal('hide');
                    } else {
                        alert(errors);
                    }
                });
            }
            else {
                vm.buttonText = "Update";
                salesOpportunitySettingService.updateStage(stage).then(function (result) {
                    if (result.success) {
                        vm.getAllActiveStages;
                        $('#addStage').modal('hide');
                    } else {
                        alert(result.error);
                    }
                });
            }
        }

        //delete Stage
        function deleteStage(successMessage, value) {
            salesOpportunitySettingService.deleteStage(value.id).then(function (result) {
                if (result.success === true) {
                    //vm.tasks.splice(vm.tasks.indexOf(task), 1);
                    swal(successMessage, "success");
                    getAllActiveStages();


                } else {
                    alert(result.message);
                }
            });
        }

        // end stage Controller

        // source controller
        vm.getAllSources = getAllSources;
        vm.getAllActiveSources = getAllActiveSources;
        vm.createSource = createSource;
        vm.checkInActiveSource = checkInActiveSource;
        vm.deleteSource = deleteSource;
        vm.showSource = showSource;
        vm.resetSource = resetSource;
        // reset source form fields
        function resetSource() {
            vm.source= {};
        }
        // get all active sources
        function getAllActiveSources() {
            salesOpportunitySettingService.getAllActiveSources().then(function (result) {
                if (result.success) {
                    vm.sources = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //getAllStage
        function getAllSources() {
            salesOpportunitySettingService.getAllSources().then(function (result) {
                if (result.success) {
                    vm.sources = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //create and update source   
        function createSource(source) {
            if (!source.id) {
                vm.buttonText = "Set";
                salesOpportunitySettingService.createSource(source).then(function (result) {
                    if (result.success) {
                        vm.sources.push(result.data);
                        vm.source = {};
                        $('#addSource').modal('hide');
                    } else {
                        alert(result.error);
                    }
                });
            } else {
                vm.buttonText = "Update";
                salesOpportunitySettingService.updateSource(source).then(function (result) {
                    if (result.success) {
                        getAllActiveSources();
                        $('#addSource').modal('hide');

                    } else {
                        alert(result.error);
                    }
                });

            }

        }

        //delete Source
        function deleteSource(successMessage, value) {
            salesOpportunitySettingService.deleteSource(value.id).then(function (result) {
                if (result.success === true) {
                    swal(successMessage, "success");
                    getAllActiveSources();
                } else {
                    alert(result.message);
                }
            });
        }

        // check active Source

        function checkInActiveSource(checked) {
            if (checked) {
                vm.checkSource = true;
                getAllSources();
            }
            else {
                vm.checkSource = false;
                getAllActiveSources();
            }
        }

        //show source
        function showSource(source) {
            vm.source = source;
            vm.buttonText = "Update";
        }

        //activate Source
        vm.activateSource = activateSource;
        function activateSource(source) {
            source.Active = true;
            salesOpportunitySettingService.updateSource(source)
                .then(function (result) {
                    if (result.success) {
                        swal("source Activated.");
                        getAllSources();
                    } else {
                        //console.log(result.message);
                        alert("Error");
                    }
                });
        }

        //end Source Controller

        // Grade Controller        
        vm.getAllActiveGrades = getAllActiveGrades;
        vm.getAllGrades = getAllGrades;
        vm.createGrade = createGrade;
        vm.checkInActiveGrade = checkInActiveGrade;
        vm.deleteGrade = deleteGrade;
        vm.showGrade = showGrade;
        vm.activateGrade = activateGrade;

        //get all active grade
        function getAllActiveGrades() {
            salesOpportunitySettingService.getAllActiveGrades().then(function (result) {
                if (result.success) {
                    vm.grades = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //get All Grade
        function getAllGrades() {
            salesOpportunitySettingService.getAllGrades().then(function (result) {
                if (result.success) {
                    vm.grades = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        // create and update Grade
        function createGrade(grade) {
            if (!grade.id) {
                vm.buttonText = "Set";
                salesOpportunitySettingService.createGrade(grade).then(function (result) {
                    if (result.success) {
                        vm.grades.push(result.data);
                        vm.grade = {};
                        $('#addGrade').modal('hide');
                    } else {
                        alert(result.error);
                    }
                });
            } else {
                vm.buttonText = "Update";
                salesOpportunitySettingService.updateGrade(grade).then(function (result) {
                    if (result.success) {
                        getAllActiveGrades();
                        $('#addGrade').modal('hide');

                    } else {
                        alert(result.error);
                    }
                });

            }

        }

        //delete grade
        function deleteGrade(successMessage, value) {
            salesOpportunitySettingService.deleteGrade(value.id).then(function (result) {
                if (result.success === true) {
                    swal(successMessage, "success");
                    getAllActiveGrades();
                } else {
                    alert(result.message);
                }
            });
        }

        // check active grade
        function checkInActiveGrade(checked) {
            if (checked) {
                vm.checkGrade = true;
                getAllGrades();
            }
            else {
                vm.checkGrade = false;
                getAllActiveGrades();
            }
        }

        //show grade
        function showGrade(grade) {
            vm.grade = grade;
            vm.buttonText = "Update";
        }

        //activate grade
        function activateGrade(grade) {
            grade.Active = true;
            salesOpportunitySettingService.updateGrade(grade)
                .then(function (result) {
                    if (result.success) {
                        swal("grade Activated.");
                        getAllGrades();
                    } else {
                        alert("Error");
                    }
                });
        }

        // end grade

        // ReasonClosed Controller

        vm.getAllActiveReasons = getAllActiveReasons;
        vm.getAllReasons = getAllReasons;
        vm.createReason = createReason;
        vm.checkInActiveReason = checkInActiveReason;
        vm.deleteReason = deleteReason;
        vm.showReason = showReason;
        vm.activateReason = activateReason;
        vm.resetReason = resetReason;
        // reset reason form fields
        function resetReason() {
            vm.reason = {};
        }
        //get all active reason
        function getAllActiveReasons() {
            salesOpportunitySettingService.getAllActiveReasons().then(function (result) {
                if (result.success) {
                    vm.reasons = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //get All reason
        function getAllReasons() {
            salesOpportunitySettingService.getAllReasons().then(function (result) {
                if (result.success) {
                    vm.reasons = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        // create and update Grade
        function createReason(reason) {
            if (!reason.id) {
                vm.buttonText = "Set";
                salesOpportunitySettingService.createReason(reason).then(function (result) {
                    if (result.success) {
                        vm.reasons.push(result.data);
                        vm.reason = {};
                        $('#addReason').modal('hide');
                    } else {
                        alert(result.error);
                    }
                });
            } else {
                vm.buttonText = "Update";
                salesOpportunitySettingService.updateReason(reason).then(function (result) {
                    if (result.success) {
                        getAllActiveReasons();
                        $('#addReason').modal('hide');

                    } else {
                        alert(result.error);
                    }
                });

            }

        }

        //delete grade
        function deleteReason(successMessage, value) {
            salesOpportunitySettingService.deleteReason(value.id).then(function (result) {
                if (result.success === true) {
                    swal(successMessage, "success");
                    getAllActiveReasons();
                } else {
                    alert(result.message);
                }
            });
        }

        // check active grade
        function checkInActiveReason(checked) {
            if (checked) {
                vm.checkReason = true;
                getAllReasons();
            }
            else {
                vm.checkReason = false;
                getAllActiveReasons();
            }
        }

        //show grade
        function showReason(reason) {
            vm.reason = reason;
            vm.buttonText = "Update";
        }

        //activate grade
        function activateReason(reason) {
            reason.Active = true;
            salesOpportunitySettingService.updateReason(reason)
                .then(function (result) {
                    if (result.success) {
                        swal("Reason Activated.");
                        getAllReasons();
                    } else {
                        alert("Error");
                    }
                });
        }

        // end reason closed

        //common


        // action changed
        function actionChanged(data, id, modalName) {
            if (Number(id) === 1 && modalName === "stage") {
                showStage(data);
                $('#addStage').modal('show');
            }
            else if (Number(id) === 1 && modalName === "source") {
                showSource(data);
                $('#addSource').modal('show');

            }
            else if (Number(id) === 1 && modalName === "grade") {
                showGrade(data);
                $('#addGrade').modal('show');

            }
            else if (Number(id) === 1 && modalName === "reason") {
                showReason(data);
                $('#addReason').modal('show');

            }
            else if (Number(id) === 2 && modalName === "stage") {
                $('#addStage').modal('hide');
                yesNoDialog("Are you sure?", "warning", "Task will be deactivated", "Yes, delete it!", "Your task has been deactivated.", "delete", data, modalName);
            }
            else if (Number(id) === 2 && modalName === "source") {
                $('#addSource').modal('hide');
                yesNoDialog("Are you sure?", "warning", "Task will be deactivated", "Yes, delete it!", "Your task has been deactivated.", "delete", data, modalName);
            }
            else if (Number(id) === 2 && modalName === "grade") {
                $('#addGrade').modal('hide');
                yesNoDialog("Are you sure?", "warning", "Task will be deactivated", "Yes, delete it!", "Your task has been deactivated.", "delete", data, modalName);
            }

            else if (Number(id) === 2 && modalName === "reason") {
                $('#addReason').modal('hide');
                yesNoDialog("Are you sure?", "warning", "Task will be deactivated", "Yes, delete it!", "Your task has been deactivated.", "delete", data, modalName);
            }
            vm.action = null;
        }

        //yes no dialog box
        function yesNoDialog(title, type, text, buttonText, successMessage, alertFor, value, modalName) {
            swal({
                title: title,
                text: text,
                type: type,
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: buttonText,
                closeOnConfirm: false
            },
                function () {
                    if (alertFor === 'delete' && modalName === 'stage') {

                        deleteStage(successMessage, value);
                    }
                    else if (alertFor === 'delete' && modalName === 'source') {
                        deleteSource(successMessage, value);
                    }
                    else if (alertFor === 'delete' && modalName === 'grade') {
                        deleteGrade(successMessage, value);
                    }
                    else if (alertFor === 'delete' && modalName === 'reason') {
                        deleteReason(successMessage, value);
                    }
                });
        }
        // clear all grade form fields
        function resetGrade() {
            vm.grade = {};
        }
        //end
    }
})();