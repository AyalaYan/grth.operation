(function () {
    'use strict';
    angular
        .module('cmp.customers')
        .controller('customersCtrl', customersCtrl);

    customersCtrl.$inject = ['cmpData', '$location', '$routeParams'];

    function customersCtrl(cmpData, $location, $routeParams) {
        /****************************************************
               VARIABLES
   **************************************************** */
        var vm = this;
        vm.listoperation = null;
        vm.employeeId = $routeParams.employeeId;
        vm.AddNewemployee = false;
        /*****************************************************
    *                  METHODS                          *
    *****************************************************/
        vm.saveemployeeData = function () {
           cmpData.setEmployee(vm.employeeObj);
           $location.path('/list');
        }
        /*****************************************************
    *               METHODS - PRIVATE                   *
    *****************************************************/
        function init() {
            vm.employeeObj = cmpData.getEmployeeById(vm.employeeId);
            if (vm.employeeObj) {
                vm.AddNewemployee = true;
            }
        }
        /*****************************************************
            *                  EXECUTIONS                       *
            *****************************************************/
        init();
    }
})();
