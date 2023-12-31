/**
 * @file tree-view??
 * @author 862802759@qq.com
 */
angular.module('TreeView', [])
.provider('treeViewConfig', function () {
    var self = this;
    this.options = {
        iconLeaf: 'fa fa-file glyphicon glyphicon-file',
        iconExpand: 'fa fa-minus glyphicon glyphicon-minus',
        iconCollapse: 'fa fa-plus glyphicon glyphicon-plus',
        template: [
            '<ul class="tree-view-group">',
                '<li ng-repeat="data in data[children]"',
                    'ng-class="{expanded: isExpanded(data)}"',
                    'ng-if="!filterModel || isFiltered(data)">',
                    '<span class="tree-icon" ng-class="getExpandIcon(data)"',
                        'ng-click="toggleExpanded(data, $event)"></span>',
                    '<a ng-class="{checked: isChecked(data), indetermine: !isChecked(data) && childrenChecked(data)}"',
                        'ng-click="toggleChecked(data, $event)">',
                        '<tree-view-transclude></tree-view-transclude>',
                    '</a>',
                    '<tree-view-item',
                        'ng-if="data[children] && isExpanded(data)"',
                    '></tree-view-item>',
                '</li>',
            '</ul>'
        ].join(' ')
    };

    this.$get = function () {
        return self.options;
    };
})
.factory('treeViewService', function () {
    return {
        uniqueId: 0,
        applyChanges: function (source, type, changedItem) {
            if (type === 'addNewItem') {
                this.transformDataToTree(
                    changedItem,
                    source.$treeView.hashObject,
                    source.$treeView.valueProperty,
                    source.$treeView.childrenProperty
                );
            }
        },
        transformDataToTree: function (data, hashObject, valueProperty, childrenProperty) {
            if (!angular.isArray(data)) {
                data = [data];
            }
            var stack = [];
            // ????,?$treeView????????
            for (var i = 0; i < data.length; i++) {
                stack.push(data[i]);
            }
            while (stack.length) {
                var tempData = stack.pop();

                tempData.$treeView = tempData.$treeView || {};
                if (!(valueProperty in tempData)) {
                    tempData[valueProperty] = this.uniqueId++;
                }
                this.currentLength++;
                hashObject[tempData[valueProperty]] = tempData;

                if (tempData[childrenProperty] && tempData[childrenProperty].length) {
                    for (i = 0; i < tempData[childrenProperty].length; i++) {
                        var tempChild = tempData[childrenProperty][i];
                        tempChild.$treeView = tempChild.$treeView || {};
                        tempChild.$treeView.parentData = tempData;
                        stack.push(tempChild);
                    }
                }
            }
            return hashObject;
        }
    };
})
.directive('treeViewItem', function () {
    return {
        require: '^treeView',
        link: function (scope, element, attrs, treeViewCtrl) {
            treeViewCtrl.template(scope, function (clone) {
                element.empty().append(clone);
            });
        }
    };
})
.directive('treeViewTransclude', function () {
    return {
        link: function (scope, element) {
            scope.transcludeScope = scope.parentScopeOfTree.$new();
            scope.transcludeScope.node = scope.data;
            scope.transcludeScope.$index = scope.$index;
            scope.transcludeScope.$first = scope.$first;
            scope.transcludeScope.$middle = scope.$middle;
            scope.transcludeScope.$last = scope.$last;
            scope.transcludeScope.$odd = scope.$odd;
            scope.transcludeScope.$even = scope.$even;

            scope.$on('$destroy', function () {
                scope.transcludeScope.$destroy();
            });
            scope.$treeTransclude(scope.transcludeScope, function (clone) {
                element.empty().append(clone);
            });
        }
    };
})
.directive('treeView', function (treeViewConfig, treeViewService) {
    return {
        scope: {
            outputAllInfo: '=',
            datas: '=inputModel',
            ngModel: '=',
            filterModel: '=',
            recursionCheck: '=',
            outputDuplicate: '=',
            singleMode: '=',
            options: '=',
            hashObject: '=',
            enableCheck: '='
        },
        transclude: true,
        link: function (scope, element, attrs, treeCtrl, childTranscludeFn) {
            treeCtrl.template(scope, function (clone) {
                element.empty().append(clone);
            });
            scope.$treeTransclude = childTranscludeFn;
        },
        controller: function ($scope, $element, $attrs, $compile) {
            $scope.parentScopeOfTree = $scope.$parent;

            $scope.options = $scope.options || {};
            $scope.displayProperty = $scope.options.displayProperty || 'text';
            $scope.valueProperty = $scope.options.valueProperty || 'id';
            $scope.children = $scope.options.childrenProperty || 'children';

            $scope.iconLeaf = $scope.options.iconLeaf || treeViewConfig.iconLeaf;
            $scope.iconExpand = $scope.options.iconExpand || treeViewConfig.iconExpand;
            $scope.iconCollapse = $scope.options.iconCollapse || treeViewConfig.iconCollapse;
            $scope.iconCheck = $scope.options.iconCheck || treeViewConfig.iconCheck;
            $scope.iconUnCheck = $scope.options.iconUnCheck || treeViewConfig.iconUnCheck;

            this.template = $compile(treeViewConfig.template);

            var utils = {
                // ????
                arrayMinus: function (source, target) {
                    source = source || [];
                    target = target || [];
                    var result = [];
                    for (var i = 0; i < source.length; i++) {
                        if (this.find(target, source[i]) === -1) {
                            result.push(source[i]);
                        }
                    }
                    return result;
                },
                find: function (source, target) {
                    if (angular.isObject(target)) {
                        for (var i = 0; i < source.length; i++) {
                            if (target[$scope.valueProperty] === source[i][$scope.valueProperty]) {
                                return i;
                            }
                        }
                        return -1;
                    }
                    return source.indexOf(target);
                },
                shallowMinus: function (source, target) {
                    for (var i = 0; i < source.length; i++) {
                        if (target.indexOf(source[i]) !== -1) {
                            source.splice(i, 1);
                            i--;
                        }
                    }
                }
            };

            var treeView = {
                modelInited: false,
                inited: false,
                init: function (value) {
                    $element.addClass('tree-view');
                    value.$treeView = {
                        hashObject: this.hashObject,
                        valueProperty: $scope.valueProperty,
                        childrenProperty: $scope.children
                    };
                    this.transformDataToTree(value);
                    $scope.data = {};
                    $scope.data[$scope.children] = value;
                    if (!this.inited) {
                        this.bindEvents();
                    }
                    this.inited = true;
                    this.modelInited = false;
                },
                // ???????id,??object??
                unwrap: function (rawObjects) {
                    for (var i = 0; i < rawObjects.length; i++) {
                        rawObjects[i] = rawObjects[i][$scope.valueProperty];
                    }
                },
                // target ???check?????
                updateModelByCheck: function (changedItems, changeToResult, target) {
                    // ????????????(???id)
                    if (!$scope.outputAllInfo) {
                        this.unwrap(changedItems);
                    }
                    // ??model
                    if (changeToResult === true) {
                        $scope.ngModel = $scope.ngModel || [];
                        Array.prototype.push.apply($scope.ngModel, changedItems);
                    }
                    else {
                        utils.shallowMinus($scope.ngModel, changedItems);
                    }
                    // ???????????????
                    // ???????
                    if (!$scope.outputDuplicate) {
                        if (changeToResult === true) {
                            this.deleteDuplicated($scope.ngModel);
                        }
                        if (changeToResult === false) {
                            this.fillResult($scope.ngModel, changedItems, target);
                        }
                    }
                },
                // ?????????
                // ?????????,?????????
                fillResult: function (result, deleted, target) {
                    var pairs = [];
                    // ??????
                    var cloneTarget = target;
                    while (cloneTarget.$treeView.parentData) {
                        var parentItem = this.getParentItem(cloneTarget);
                        if (deleted.indexOf(parentItem) !== -1) {
                            pairs.push(cloneTarget);
                            cloneTarget = cloneTarget.$treeView.parentData;
                        }
                        else {
                            break;
                        }
                    }

                    if (!pairs.length) {
                        return;
                    }
                    for (var i = 0; i < pairs.length; i++) {
                        var resultToConcat = [].concat(pairs[i].$treeView.parentData[$scope.children]);
                        resultToConcat.splice(resultToConcat.indexOf(pairs[i]), 1);
                        if (!$scope.outputAllInfo) {
                            this.unwrap(resultToConcat);
                        }
                        Array.prototype.push.apply(result, resultToConcat);
                    }
                },
                // ?????????
                // ??????,????????
                deleteDuplicated: function (result) {
                    var deleted = [];
                    for (var i = 0; i < result.length; i++) {
                        var parentObject;
                        if (!$scope.outputAllInfo) {
                            parentObject = this.hashObject[result[i]].$treeView.parentData;
                            parentObject = parentObject && parentObject[$scope.valueProperty];
                        }
                        else {
                            parentObject = result[i].$treeView.parentData;
                        }
                        if (result.concat(deleted).indexOf(parentObject) !== -1) {
                            deleted = deleted.concat(result.splice(i, 1));
                            i--;
                        }
                    }
                },
                // ???????,????????,????
                // ??:????????????
                updateStateByModelChange: function (newValue, oldValue) {
                    var added;
                    if (this.modelInited) {
                        added = utils.arrayMinus(newValue, oldValue);
                    }
                    else {
                        added = newValue || [];
                    }
                    var deleted = utils.arrayMinus(oldValue, newValue);
                    for (var i = 0; i < added.length; i++) {
                        var currentData = $scope.outputAllInfo ? added[i] : this.hashObject[added[i]];
                        currentData.$treeView.isChecked = true;
                        this.expandUp(currentData);
                        if (!this.modelInited) {
                            if (!$scope.outputDuplicate) {
                                this.calculateDown(currentData);  
                            }
                        }
                    }
                    for (i = 0; i < deleted.length; i++) {
                        currentData = $scope.outputAllInfo ? deleted[i] : this.hashObject[deleted[i]];

                        // ????????????(outputDuplicate?false?)
                        // ?????,???deleteDuplicate?,???????
                        if (!$scope.outputDuplicate
                                && currentData.$treeView.parentData
                                && currentData.$treeView.parentData.$treeView.isChecked) {
                            continue;
                        }
                        currentData.$treeView.isChecked = false;
                        // ?????,?????????(??)
                        // ????????,??deleted.length === 1
                        // ????added
                        // ???????,??newValue??
                        if (!$scope.outputDuplicate
                                && (!newValue.length
                                        || (deleted.length === 1 && !added.length))) {
                            this.calculateDown(currentData);
                        }
                    }
                    // ?????
                    // ???,????
                    if (!$scope.outputDuplicate && newValue.length === this.currentLength) {
                        this.deleteDuplicated($scope.ngModel);
                    }
                    this.modelInited = true;
                },
                expandUp: function (data) {
                    var parentItem = data.$treeView.parentData;
                    if (parentItem && !parentItem.$treeView.isExpanded) {
                        if (parentItem.$treeView.isChecked) {
                            this.expandUp(parentItem);
                        }
                        else {
                            parentItem.$treeView.isExpanded = true;
                            this.expandUp(parentItem);
                        }
                    }
                },
                getParentItem: function (data) {
                    var parentItem;
                    var currentObject = angular.isObject(data) ? data : this.hashObject[data];
                    if (!$scope.outputAllInfo) {
                        parentItem = currentObject.$treeView.parentData;
                        parentItem = parentItem && parentItem[$scope.valueProperty];
                    }
                    else {
                        parentItem = data.$treeView.parentData;
                    }
                    return parentItem;
                },
                calculateChecked: function (data, valueChangedItems) {
                    this.calculateDown(data, valueChangedItems);
                    this.calculateUp(data, valueChangedItems);
                },
                calculateDown: function (data, valueChangedItems) {
                    if (data[$scope.children]) {
                        for (var i = 0; i < data[$scope.children].length; i++) {
                            
                            if (data[$scope.children][i].$treeView.isChecked !== data.$treeView.isChecked) {
                                valueChangedItems && valueChangedItems.push(data[$scope.children][i]);
                                data[$scope.children][i].$treeView.isChecked = data.$treeView.isChecked;
                                // ?????????????????
                                // ?????????????
                                this.calculateDown(data[$scope.children][i], valueChangedItems);
                            }
                        }
                    }
                },
                calculateUp: function (data, valueChangedItems) {
                    if (data.$treeView.parentData
                            && data.$treeView.parentData.$treeView.isChecked !== data.$treeView.isChecked) {
                        var preValue = !!data.$treeView.parentData.$treeView.isChecked;
                        var checked = 0;
                        for (var i = 0; i < data.$treeView.parentData[$scope.children].length; i++) {
                            if (data.$treeView.parentData[$scope.children][i].$treeView.isChecked) {
                                checked++;
                            }
                        }
                        // ??????????
                        data.$treeView.parentData.$treeView.isChecked = (checked === i);
                        if (data.$treeView.parentData.$treeView.isChecked !== preValue) {
                            valueChangedItems && valueChangedItems.push(data.$treeView.parentData);
                            // ?????????????????
                            // ??????????????
                            this.calculateUp(data.$treeView.parentData);
                        }
                    }
                },
                hashObject: {},
                currentLength: 0,
                transformDataToTree: function (data) {
                    treeViewService.transformDataToTree(data, this.hashObject, $scope.valueProperty, $scope.children);
                    if ($attrs.hashObject) {
                        $scope.hashObject = this.hashObject;
                    }
                    return;
                },
                bindEvents: function () {
                    var self = this;

                    if ($scope.singleMode) {
                        $scope.$watch('ngModel', function (newValue, oldValue) {
                            self.updateStateByModelChange(newValue, oldValue);
                        });
                    }
                    else {
                        $scope.$watchCollection('ngModel', function (newValue, oldValue) {
                            newValue = newValue || [];
                            oldValue = oldValue || [];
                            self.updateStateByModelChange(newValue, oldValue);
                        });
                    }

                    $scope.$watch('filterModel', function (newValue, oldValue) {
                        if (!newValue) {
                            return;
                        }
                        var result = [];
                        angular.forEach(self.hashObject, function (item, key) {
                            if (item[$scope.displayProperty].toString().indexOf(newValue) !== -1) {
                                result.push(item);
                            }
                            else {
                                item.$treeView.isFiltered = false;
                            }
                        });

                        for (var i = 0; i < result.length; i++) {
                            result[i].$treeView.isFiltered = true;
                            self.filtUp(result[i]);
                            self.expandUp(result[i]);
                        }
                    });
                },
                filtUp: function (data) {
                    if (data.$treeView.parentData && !data.$treeView.parentData.isFiltered) {
                        data.$treeView.parentData.isFiltered = true;
                        this.filtUp(data.$treeView.parentData);
                    }
                }
            };

            $scope.getExpandIcon = function (data) {
                if (!(data[$scope.children] && data[$scope.children].length)) {
                    return $scope.iconLeaf;
                }
                if (data.$treeView.isExpanded) {
                    return $scope.iconExpand;
                }
                return $scope.iconCollapse;
            };

            $scope.toggleExpanded = function (data, event) {
                data.$treeView.isExpanded = !data.$treeView.isExpanded;
                event.stopPropagation();
            };

            $scope.isExpanded = function (data) {
                return !!(data.$treeView && data.$treeView.isExpanded);
            };

            $scope.isChecked = function (data) {
                return !!(data.$treeView && data.$treeView.isChecked);
            };

            $scope.childrenChecked = function (data) {
                if (!data[$scope.children] || !data[$scope.children].length) {
                    return false;
                }
                var stack = [];
                var children = data[$scope.children];
                for (var i = 0; i < children.length; i++) {
                    stack.push(children[i]);
                }
                while (stack.length) {
                    var tempData = stack.pop();
                    if (tempData.$treeView.isChecked) {
                        return true;
                    }
                    if (!tempData.children || !tempData.children.length) {
                        continue;
                    }
                    for (i = 0; i < tempData.children.length; i++) {
                        stack.push(tempData.children[i]);
                    }
                }
                return false;
            };

            $scope.toggleChecked = function (data) {
                var preValue = $scope.isChecked(data);
                var valueChangedItems = [data];
                if ($scope.singleMode) {
                    treeView.initCheck();
                }
                data.$treeView.isChecked = !preValue;
                // ??????check,???check
                // ????check????????check
                if ($scope.recursionCheck) {
                    treeView.calculateChecked(data, valueChangedItems);
                }
                treeView.updateModelByCheck(valueChangedItems, data.$treeView.isChecked, data);
            };

            $scope.isFiltered = function (data) {
                return !!data.$treeView.isFiltered;
            };

            $scope.isCheckable = function () {
                return $scope.enableCheck !== false;
            };

            $scope.$watch('datas', function (newValue, oldValue) {
                if (angular.isDefined(newValue)) {
                    treeView.init(newValue);
                }
            });
        }
    };
});