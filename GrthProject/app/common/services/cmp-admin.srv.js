(function () {
    'use strict';

    angular
        .module('cmp.services')
        .service('cmpAdminData', cmpAdminData);

    cmpAdminData.$inject = ['$http', '$q'];


    function cmpAdminData($http, $q) {

        /****************************************************
                  VARIABLES
      **************************************************** */
        var vm = this;
        
        vm.employeesList= null,
        vm.getEmployees= getEmployees,
        vm.getEmployeeById= getEmployeeById,
        vm.setEmployee= setEmployee
     
      
        //role: {
        //        getRoles: {
        //            url: baseUrl + 'role',
        //            method: 'GET'
        //        },
        //    getRoleOptions: {
        //            url: baseUrl + 'role',
        //            method: 'GET'
        //    },
        //    UpdateRole: {
        //            url: baseUrl + 'role',
        //            method: 'POST'
        //    },
        //    InsertRole: {
        //            url: baseUrl + 'role',
        //            method: 'POST'
        //    }
        //},
        //user: {
        //        getUsers: {
        //            url: baseUrl + 'user',
        //            method: 'GET'
        //        },
        //    getUserOptions: {
        //            url: baseUrl + 'user',
        //            method: 'GET'
        //    },
        //    UpdateUser: {
        //            url: baseUrl + 'user',
        //            method: 'POST'
        //    },
        //    InsertUser: {
        //            url: baseUrl + 'user',
        //            method: 'POST'
        //    }
        //},

        /*****************************************************
       *                  METHODS                          *
       *****************************************************/
        function getEmployees() {
            if (!vm.employeesList) {
                var defered = $q.defer();
                $http.get('/api/').then(
                    function (response) {
                        vm.employeesList = response.data;
                        defered.resolve(response.data);
                    },
                    function (error) { defered.resolve(false); });
                return defered.promise;
            }
            else
                return $q.resolve(vm.employeesList)
           
        }

        function getEmployeeById(employeeId) {
            if (employeeId == 0) 
                return null; 
            return _.find(vm.employeesList.data, { id: employeeId });
        }

        function setEmployee(employee) {
            if (!employee.id) {   
                employee.id = "111"+employee.name;
                vm.employeesList.data.push(employee);
            }
        }

        /*****************************************************
    *               METHODS - PRIVATE                   *
    *****************************************************/

        function init() {

        }

        /*****************************************************
             *                  EXECUTIONS                       *
             *****************************************************/
        init();
    }
})();