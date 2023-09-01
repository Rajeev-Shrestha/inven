(function () {
    angular.module("app-taskManager")
        .controller("taskManagerController", taskManagerController);
    taskManagerController.$inject = ['$http', '$filter', '$scope', 'commonService', 'taskManagerService'];

    function taskManagerController($http, $filter, $scope, commonService, taskManagerService) {
        var vm = this;
        vm.createTask = createTask;
        vm.task = {};
       // vm.updateReminder = updateReminder;
        //vm.showReminder = showReminder;
        vm.deleteTask = deleteTask;
        vm.priorities = [{ id: 1, name: "Normal" }, { id: 2, name: "High" }, { id: 3, name: "Urgent" }];
        vm.statuses = [
            { id: 1, name: "Not Started" }, { id: 2, name: "Started" }, { id: 3, name: "Pending" },
            { id: 4, name: "Committed" }, { id: 5, name: "Done" }
        ];
        vm.docType = [
            { id: 1, name: "Sales order" },
            { id: 2, name: "Purchase Order" }];
        vm.sortColumn = 'TaskTitle';
        vm.sortTasks = sortTasks;
        vm.sortDescending = false; 
        vm.changeWorkComplete = changeWorkComplete;
        vm.increase = increase;
        vm.decrease = decrease;
        vm.actionItems = [{ id: 1, name: 'Edit' }, { id: 2, name: 'Delete' }];
        vm.sortItems = [{ id: 1, name: 'Priority' }, { id: 2, name: 'Date and Priority' }];
        vm.sortTaskChanged = sortTaskChanged;
        vm.actionChanged = actionChanged;
        vm.tasks = [];
        vm.users = null;
        vm.pageSizes = ["10", "20", "50", "100"];
        vm.pagingList = [];
        vm.currentPage = 1;
        vm.pageSize = 10;
        vm.getPageSize = getPageSize;
        vm.searchText = '';
        vm.getTimeOnly = getTimeOnly;
        vm.getDateOnly = getDateOnly;
        vm.check = false;
       // vm.checkInactiveTasks = checkInactiveTasks;
        vm.activateTask = activateTask;
        vm.searchParamChanged = searchParamChanged;
        vm.onKeyPress = onKeyPress;
        vm.task = {};
        vm.task.completePercentage = 0;
        vm.displayCompletePercentage = displayCompletePercentage;
        vm.resetTask = resetTask;
        vm.getAllUsers = getAllUsers;
        vm.buttonText = "Set";
        vm.getUserRights = getUserRights;

     

        init();

        vm.displayPercentage = false;
        function displayCompletePercentage(id) {
            if(id===null || id===undefined || id===1){
                vm.displayPercentage = false;
            }
            else {
                vm.displayPercentage = true;
                vm.task.completePercentage = 0;
            }
        }

        function getAllUsers() {
            resetTask();            
            getAllApplicationUsers();
            getUserRights();
            vm.buttonText = "Set";
            vm.endDateTimeDisabled = false;
        }
        function resetTask() {
            // call api to find logged in user has right

            vm.task = { hasTaskAssignRights:true };
            vm.getAllApplicationUsers();
            vm.displayPercentage = false;
            vm.showTaskStartDateTime = false;
            vm.showTaskEndDateTime = false;
            vm.showDocId = false;
            vm.taskEndDateDisabled = true;

        }
        vm.enableEndDateTime = enableEndDateTime;
        function enableEndDateTime(date)
        {
            if(date === undefined)
            {
                vm.taskEndDateDisabled = true;
            } else
            {
                vm.taskEndDateDisabled = false;               
            }
        }

        function onKeyPress(event) {
            if (event.keyCode === 38) {
                if (vm.task.completePercentage > 99) {
                    //do nothing
                }
                else {
                    vm.task.completePercentage = vm.task.completePercentage + 1;
                }
            }
            else if (event.keyCode === 40) {
                if (vm.task.completePercentage < 1) {
                    //do nothing
                }
                else {
                    vm.task.completePercentage = vm.task.completePercentage - 1;
                }
            }
        }

        function getTimeOnly(datetime){
            var result = commonService.getTimeOnlyFromDateAndTime(datetime);
          return result;
           
        }

        function getDateOnly(datetime) {
            var result = commonService.getDateOnlyFromDateAndTime(datetime);
            return result;
        }

        function decrease(data) {
            if (data === 0) {
                vm.task.completePercentage = 0;
            } else {
                vm.task.completePercentage = data - 1;;
            }
        }

        function increase(data) {
            if (data === 100) {
                vm.task.completePercentage = 100;
            }
            {
                vm.task.completePercentage = data + 1;
            }
        }

        function init() {
            vm.task.completePercentage = 0;
            searchParamChanged();
          //  vm.taskEndDateDisabled = true;

          
        }
        function sortTasks(columnName) {
            vm.sortDescending = !vm.sortDescending;
            vm.sortColumn = columnName;
            searchParamChanged();
        }

        function changeWorkComplete(data) {
            if (data === undefined) {
                alert("invalid input");
                vm.task.completePercentage = 0;
                return;
            }
        }
        function getPageSize(pageSize) {
            vm.pageSize = pageSize;
        }
         vm.pageChanged = function (page) {
             vm.currentPage = page;
             searchParamChanged();
        };
         function createTask(task) {
            task.taskStartDateTime = moment(task.taskStartDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.taskEndDateTime = moment(task.taskEndDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.reminderStartDateTime = moment(task.reminderStartDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.reminderEndDateTime = moment(task.reminderEndDateTime).format("MM-DD-YYYY HH:mm:ss");
            //if (task.taskStartDateTime > task.taskEndDateTime || task.taskStartDateTime > task.reminderStartDateTime || task.reminderStartDateTime > task.reminderEndDateTime || task.taskEndDateTime < task.reminderStartDateTime)
            //{
            //    alert("Invalid Date.");
            //} else {
                if (!task.taskId) {
                    vm.buttonText = "Set";
                    taskManagerService.createNewTask(task).then(function (result) {
                        if (result.success) {
                            vm.tasks.push(result.data);
                            vm.task = {};
                            $('#task').modal('hide');
                            vm.task.completePercentage = 0;
                            searchParamChanged();
                        } else {
                            alert(errors);
                        }
                    });
                }
                else {

                    taskManagerService.updateTask(task).then(function (result) {
                        if (result.success) {
                            $('#task').modal('hide');
                            searchParamChanged();
                        } else {
                            alert(result.error);
                        }
                    });
                    // }
                }          
        }

        // Show task
         function showTask(task) {
            vm.buttonText = "Update";
            task.showDocId = null;
            task.taskStartDateTime = moment(task.taskStartDateTime).format("YYYY/MM/DD HH:mm");
            task.taskEndDateTime = moment(task.taskEndDateTime).format("YYYY/MM/DD HH:mm");
            task.reminderStartDateTime = moment(task.reminderStartDateTime).format("YYYY/MM/DD HH:mm");
            task.reminderEndDateTime = moment(task.reminderEndDateTime).format("YYYY/MM/DD HH:mm");
            getAllApplicationUsers();
            taskChecked(task.reminder);
            docTypeChanged(task.docType);
            displayCompletePercentage(task.status);
            vm.task = task;
        }

        // Get all Reminders
        function getAllActiveTasks() {
            taskManagerService.getAllActiveTasks().then(function (result) {
                if (result.success) {
                    vm.tasks = result.data;

                } else {
                    alert(result.error);
                }
            });
        }
        function getAllTasks() {
            taskManagerService.getAllTasks().then(function (result) {
                if (result.success) {
                    vm.tasks = result.data;
                } else {
                    alert(result.error);
                }
            });
        }

        //getActiveTask
        function activateTask(task) {
            task.Active = true;
            taskManagerService.updateTask(task)
                .then(function (result) {
                    if (result.success) {
                        swal("Task Activated.");
                        searchParamChanged();
                    } else {
                        console.log(result.message);
                    }
                });
        }
        // Edit task
        function updateTask(task) {
            task.taskStartDateTime = moment(task.taskStartDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.taskEndDateTime = moment(task.taskEndDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.taskStartDateTime = moment(task.taskStartDateTime).format("MM-DD-YYYY HH:mm:ss");
            task.taskEndDateTime = moment(task.taskEndDateTime).format("MM-DD-YYYY HH:mm:ss");
            taskManagerService.updateTask(task).then(function(result) {
                if (result.success) {
                    getAllActiveTasks();
                    //vm.task = null;
                    $('#task').modal('hide');
                } else {
                    alert(result.error);
                }
            });
        }
        vm.getAllApplicationUsers = getAllApplicationUsers;
        // Get all application users
        function getAllApplicationUsers() {
            taskManagerService.getAllApplicationUsers().then(function(result) {
                if (result.success) {
                    vm.users = result.data;
                } else {
                    vm.users = null;
                    alert(result.error);
                }
            });
        }

        function getUserRights() {
            taskManagerService.getUserRightsByLogin().then(function (result) {
                if (result.success) {
                    vm.loggedInUser = result.data;
                } else {
                    vm.users = null;
                    alert(result.error);
                }
            });
        }
        //check active Task
        function ctiveTasks(checked) {
            if (checked) {
                vm.check = true;
                getAllTasks();
            }
            else {
                vm.check = false;
                getAllActiveTasks();
            }
        }
        // Delete Task
        function deleteTask(successMessage, value) {
            taskManagerService.deleteTask(value.taskId).then(function(result) {
                if (result.status === 200) {
                    //vm.tasks.splice(vm.tasks.indexOf(task), 1);
                    swal(successMessage, "success");
                    searchParamChanged();

                    
                } else {
                    alert(result.message);
                }
            });
        }        
        vm.docTypeChanged = docTypeChanged;
        vm.showDocId = false;
        function docTypeChanged(id) {
            if (id === null || id===0) {
                vm.showDocId = false;
            }
            else {
                taskManagerService.getdocIdByDocType(id).then(function (result) {
                    if (result.success) {
                        vm.docId = result.data;
                        vm.showDocId = true;
                    } else {
                        alert(result.message);
                    }
                });
            }           
        }
        vm.taskChecked = taskChecked;
        vm.showTaskStartDateTime = false;
        vm.showTaskEndDateTime = false;
        function taskChecked(arg){
            if (arg === true) {
                vm.showTaskStartDateTime = true;
                vm.showTaskEndDateTime = true;
            }
            else {
                vm.showTaskStartDateTime = false;
                vm.showTaskEndDateTime = false;
            }
        }
        function actionChanged(task, id) {
            if (Number(id) === 1) {
                showTask(task);
                $('#task').modal('show');
            }
            else if (Number(id) === 2) {
                $('#task').modal('hide');
                yesNoDialog("Are you sure?", "warning", "Task will be deactivated", "Yes, delete it!", "Your task has been deactivated.", "delete", task);
            }
            
            vm.action = null;
        }

        function sortTaskChanged(id)
        {
            taskManagerService.sortTask(id).then(function (result) {
                if (result.success) {
                    vm.tasks = result.data;
                } else {
                    alert("error");
                }
            })
        }
        function searchParamChanged() {
            var sortOrder;
            if (vm.sortDescending)
                sortOrder = "ASC";
            else
                sortOrder = "DESC";
            taskManagerService.searchTextForTask(vm.searchText, !vm.check, vm.currentPage, vm.pageSize, vm.sortColumn, sortOrder)
                .then(function (result) {
                    if (result.success) {
                        vm.pageNumber = 1;
                        vm.tasks = result.data.items;
                        vm.totalPage = result.data.pageCount;
                        if (vm.totalPage === 1) {
                            vm.pageNumber = 1;
                        } else {
                            vm.pageNumber = result.data.pageNumber;
                        }
                    }
                });
        }
        function yesNoDialog(title, type, text, buttonText, successMessage, alertFor, value) {
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
                    if (alertFor === 'delete') {
                        deleteTask(successMessage, value);
                        //swal(successMessage, "success");
                    }
                    if (alertFor === 'active') {
                        activateTask(value);
                        //editCustomer(value.id, true);
                       // vm.customerForm.$invalid = true;
                        //swal(successMessage, "success");
                    }

                });
        }

        // bootstrap datetime picker
        vm.endDateBeforeRender = endDateBeforeRender 
        vm.endDateOnSetTime = endDateOnSetTime
        vm.startDateBeforeRender = startDateBeforeRender
        vm.startDateOnSetTime = startDateOnSetTime
        vm.endReminderDateBeforeRender = endReminderDateBeforeRender
        vm.endReminderDateOnSetTime = endReminderDateOnSetTime
        vm.startReminderDateBeforeRender = startReminderDateBeforeRender
        vm.startReminderDateOnSetTime = startReminderDateOnSetTime

        function startDateOnSetTime() {
            $scope.$broadcast('start-date-changed');
        }

        function endDateOnSetTime() {
            $scope.$broadcast('end-date-changed');
        }

        function startReminderDateOnSetTime() {
            $scope.$broadcast('start-reminder-date-changed');
        }

        function endReminderDateOnSetTime() {
            $scope.$broadcast('end-reminder-date-changed');
        }

        function startDateBeforeRender($dates) {
            if (vm.task.taskEndDateTime) {
                var activeDate = moment(vm.task.taskEndDateTime);

                $dates.filter(function (date) {
                    return date.localDateValue() >= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = false;
                })
            }
        }


        function startReminderDateBeforeRender($dates) {
            if (vm.task.taskReminderEndDateTime) {
                var activeDate = moment(vm.task.taskReminderEndDateTime);

                $dates.filter(function (date) {
                    return date.localDateValue() >= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = false;
                })
            }
        }
       

        function endDateBeforeRender($view, $dates) {
            if (vm.task.taskStartDateTime) {
                var activeDate = moment(vm.task.taskStartDateTime).subtract(1, $view).add(1, 'minute');

                $dates.filter(function (date) {
                    return date.localDateValue() <= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = false;
                })
            }
        }

        function endReminderDateBeforeRender($view, $dates) {
            if (vm.task.taskReminderStartDateTime) {
                var activeDate = moment(vm.task.taskReminderStartDateTime).subtract(1, $view).add(1, 'minute');

                $dates.filter(function (date) {
                    return date.localDateValue() <= activeDate.valueOf()
                }).forEach(function (date) {
                    date.selectable = false;
                })
            }
        }
      

      
        //end
    }
})();

