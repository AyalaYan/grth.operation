(function () {
    'use strict';
    angular.module('cmp.login', []);
    angular.module('cmp.directives', []);
    angular.module('cmp.employees', []);
    angular.module('cmp.customers', []);
    angular.module('cmp.settings', []);
    angular.module('cmp.projects', []);
    angular.module('cmp.services', []);
    angular.module('cmpApp', [
        // Angular modules 
        'ngRoute',
       'ngTouch', 'ui.grid',
'ui.grid.cellNav', 'ui.grid.edit', 'ui.grid.resizeColumns',
'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.moveColumns',
'ui.grid.exporter', 'ui.grid.grouping', 'ui.grid.pagination',
'multipleSelect', 'toaster',
        "ngAnimate",
        //'ngAria',
        //'ngMaterial',
        // Custom modules 
        'cmp.login',
        'cmp.employees',
        'cmp.customers',
        'cmp.settings',
        'cmp.projects',
        'cmp.services',
        'cmp.directives'
    ]).run(['$rootScope', 'cmpData', function ($rootScope, cmpData) {
        $rootScope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            //$rootScope.opened = true;
        };
        $rootScope.title = "CMPH";
        $rootScope.view = "login";

    }]).config(['$qProvider', '$httpProvider', function ($qProvider, $httpProvider) {
        $qProvider.errorOnUnhandledRejections(false);

        // Use x-www-form-urlencoded Content-Type
        $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

        /**
         * The workhorse; converts an object to x-www-form-urlencoded serialization.
         * @param {Object} obj
         * @return {String}
         */
        var param = function (obj) {
            var query = '', name, value, fullSubName, subName, subValue, innerObj, i;

            for (name in obj) {
                value = obj[name];

                if (value instanceof Array) {
                    for (i = 0; i < value.length; ++i) {
                        subValue = value[i];
                        fullSubName = name + '[' + i + ']';
                        innerObj = {};
                        innerObj[fullSubName] = subValue;
                        query += param(innerObj) + '&';
                    }
                }
                else if (value instanceof Object) {
                    for (subName in value) {
                        subValue = value[subName];
                        fullSubName = name + '[' + subName + ']';
                        innerObj = {};
                        innerObj[fullSubName] = subValue;
                        query += param(innerObj) + '&';
                    }
                }
                else if (value !== undefined && value !== null)
                    query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
            }

            return query.length ? query.substr(0, query.length - 1) : query;
        };

        // Override $http service's default transformRequest
        $httpProvider.defaults.transformRequest = [function (data) {
            return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
        }];
    }]);
})();