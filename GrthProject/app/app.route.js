(function () {
    'use strict';
    angular.module('cmpApp')
    .config(['$routeProvider', function config($routeProvider) {
        $routeProvider
           .when('/login', {
             controller: 'loginCtrl',
             templateUrl: 'pages/login/login.tpl.html',
             controllerAs: 'loginVm'
              })
               .when('/employees', {
                   controller: 'employeesCtrl',
                   controllerAs: 'employeesVm',
                   templateUrl: 'pages/employees/employees.tpl.html'
               })
            .when('/projects', {
                controller: 'projectsCtrl',
                templateUrl: 'pages/projects/projects.tpl.html',
                controllerAs: 'projectsVm'
            })
              .when('/customers', {
                  controller: 'customers',
                  templateUrl: 'pages/customers/customers.tpl.html',
                  controllerAs: 'customersVm'
              })
            .when('/settings', {
                controller: 'settings',
                templateUrl: 'pages/settings/settings.tpl.html',
                  controllerAs: 'settingsVm'
              })
            .otherwise({
                redirectTo: '/login'
            });

    }]);

})();
