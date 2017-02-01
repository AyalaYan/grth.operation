(function () {
    'use strict';

    angular
        .module('cmp.services')
        .service('cmpData', cmpData);

    cmpData.$inject = ['$http', '$q', '$timeout', 'cmpApi'];


    function cmpData($http, $q, $timeout, cmpApi) {

        /****************************************************
                  VARIABLES
      **************************************************** */
        var vm = this;
        vm.employeesList = null;
        //Employees
        vm.getEmployees = _getEmployees;
        vm.getEmployeesOptions = _getEmployeesOptions;
        vm.updateEmployee = _updateEmployee;
        vm.insertEmployee = _insertEmployee;
        vm.getEmployeeById = _getEmployeeById;
        ////Jobs
       // vm.getJobs = _getJobs;
        vm.getJobsOptions = _getJobsOptions;
        //vm.updateJob = _updateJob;
        //vm.insertJob = _insertJob;
        //Company
       // vm.getCompanies = _getCompanies;
        vm.getCompanyOptions = _getCompanyOptions;
        //vm.updateCompany = _updateCompany;
        //vm.insertCompany = _insertCompany;
        ////Customer
        //vm.getCustomers = _getCustomers;
        //vm.getCustomerOptions = _getCustomerOptions;
        //vm.updateCustomer = _updateCustomer;
        //vm.insertCustomer = _insertCustomer;
        ////Department
        //vm.getDepartments = _getDepartments;
        //vm.getDepartmentOptions = _getDepartmentOptions;
        //vm.updateDepartment = _updateDepartment;
        //vm.insertDepartment = _insertDepartment;
        ////Experience
        vm.getExperiences = _getExperiences;
        //vm.updateExperience = _updateExperience;
        //vm.insertExperience = _insertExperience;
        ////FocalPoint
        //vm.getFocalPoints = _getFocalPoints;
        //vm.getFocalPointOptions = _getFocalPointOptions;
        //vm.updateFocalPoint = _updateFocalPoint;
        //vm.insertFocalPoint = _insertFocalPoint;
        ////Project
        //vm.getProject = _getProject;
        //vm.getProjectOptions = _getProjectOptions;
        //vm.updateProject = _updateProject;
        //vm.insertProject = _insertProject;
        ////Project Technology
        //vm.getProjectTechnologies = _getProjectTechnologies;
        //vm.getProjectTechnologyOptions = _getProjectTechnologyOptions;
        //vm.updateProjectTechnology = _updateProjectTechnology;
        //vm.insertProjectTechnology = _insertProjectTechnology;
        ////Technology
       //vm.getTechnologies = _getTechnologies;
        vm.getTechnologyOptions = _getTechnologyOptions;
        //vm.updateTechnology = _updateTechnology;
        //vm.insertTechnology = _insertTechnology;
        ////Project Types
        //vm.getProjectTypes = _getProjectTypes;
        //vm.getProjectTypeOptions = _getProjectTypeOptions;
        //vm.updateProjectType = _updateProjectType;
        //vm.insertProjectType = _insertProjectType;
        //States
        //vm.getStates = _getStates;
        vm.getStatesOptions = _getStatesOptions;
        //vm.updateState = _updateState;
        //vm.insertState = _insertState;
        //Cities
        //vm.getCities = _getCities;
        vm.getCitysOptions = _getCitysOptions;
        //vm.updateCity = _updateCity;
        //vm.insertCity = _insertCity;
        //Countries
        //vm.getCountries = _getCountries;
        vm.getCountryOptions = _getCountryOptions;
        //vm.updateCountry = _updateCountry;
        //vm.insertCountry = _insertCountry;
        /*****************************************************
       *                  METHODS                          *
       *****************************************************/
        
         /*
       * @description: get fields list and return dictionary list [{name:'' ,value: ''}]
       * @param: {list} array of objects
       */
        function _toOptions(list, key, name) {
            return _.map(list, function (obj) {
                return {
                    id: _.get(obj, key),
                    name: _.get(obj, name)
                }
            });
        }
        
        /*
         * @description: get employees
         */

        //employees
        function _getEmployees() {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.employees.getEmployees).then(function success(response) {
                vm.employeesList = response.data.Records;
                defer.resolve(vm.employeesList);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        /*
        * @description: get employees options
        * @param {option}: bool,  retrun as {DisplayText: ,Value:} dictionary 
        */
        function _getEmployeesOptions() {
            var params = {
                data: { "": true }
            };
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.employees.getEmployeesOptions, params).then(function success(response) {
                var list = response.data.Records;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        /*
        * @description: get employee by id from employees
        * @param {option}: bool,  retrun as {DisplayText: ,Value:} dictionary 
        */
        function _getEmployeeById(employeeId) {
            if (employeeId == 0)
                return { ID: 0 };
            return _.find(vm.employeesList, { ID: employeeId });
        }
        function _updateEmployee(EmployeeVM) {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.employees.updateEmployee, EmployeeVM)
            .then(function success(response) {
                var result = response.data;
                defer.resolve(result);
            }, function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                defer.reject(err);
            })
            return defer.promise;
        }

        function _insertEmployee(EmployeeVM) {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.employees.insertEmployee, EmployeeVM).then(
            function success(response) {
                var result = response.data;
                defer.resolve(result);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        //Cities
        /*
       * @description: get employees
       */
        function _getCitiesOptions() {
            var params = {
                sort: "ID"
            };
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.cities.getCities, params).then(function success(response) {
                var list = response.data.Records;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        /*
        * @description: get employees options
        * @param {option}: bool,  retrun as {DisplayText: ,Value:} dictionary 
        */
        function _getCitysOptions(iStateID) {
            var defer = $q.defer();
            if (iStateID == 0) {
                defer.resolve(null);
                return defer.promise;
            }
            else {
                cmpApi.getData(cmpApi.resources.cities.getCitysOptions, iStateID).then(function success(response) {
                    var list = response.data.Options;
                    defer.resolve(list);
                },
                function error() {
                    defer.reject(error);
                })
                return defer.promise;
            }
        }

        //States
        /*
       * @description: get states
       */
        function _getStates() {
            var params = {
                sort: "ID"
            };
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.states.getStates, params).then(function success(response) {
                var list = response.data.Records;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        /*
        * @description: get states options
        * @param {option}: bool,  retrun as {DisplayText: ,Value:} dictionary 
        */
        function _getStatesOptions(iCountryID) {
            var defer = $q.defer();
            if (iCountryID == 0) {
                defer.resolve(null);
                return defer.promise;
            }
            else {
                cmpApi.getData(cmpApi.resources.states.getStatesOptions, iCountryID).then(function success(response) {
                    var list = response.data.Options;
                    defer.resolve(list);
                },
                function error() {
                    defer.reject(error);
                })
                return defer.promise;
            }
        }

        //Countries
        /*
      * @description: get states
      */
        function _getCountries() {
            var params = {
                sort: "ID"
            };
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.countries.getCountries, params).then(function success(response) {
                var list = response.data.Records;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }
        /*
        * @description: get states options
        * @param {option}: bool,  retrun as {DisplayText: ,Value:} dictionary 
        */
        function _getCountryOptions() {

            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.countries.getCountryOptions).then(function success(response) {
                var list = response.data.Options;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;
        }

        //Experiences
        function _getExperiences(iEmployeeID) {
            var defer = $q.defer();
            if (iEmployeeID == 0) {
                defer.resolve(null);
                return defer.promise;
            }
            else {
                cmpApi.getData(cmpApi.resources.experience.getExperiences, iEmployeeID).then(function success(response) {
                    var list = response.data.Records;
                    defer.resolve(list);
                },
                function error() {
                    defer.reject(error);
                })
                return defer.promise;
            }
        }

        //Jobs
        function _getJobsOptions() {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.jobs.getJobsOptions, null).then(function success(response) {
                var list = response.data.Options;
                defer.resolve(list);
            },
            function error() {
                defer.reject(error);
            })
            return defer.promise;

        }

        //Technologies
        function _getTechnologyOptions() {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.technology.getTechnologyOptions, null).then(function success(response) {
                var list = _toOptions(response.data.Options, "Value", "DisplayText");
                defer.resolve(list);
            },
                function error() {
                    defer.reject(error);
                })
            return defer.promise;
        }

        //Companies
        function _getCompanyOptions() {
            var defer = $q.defer();
            cmpApi.getData(cmpApi.resources.company.getCompanyOptions, null).then(function success(response) {
                var list = response.data.Options;
                defer.resolve(list);
            },
                function error() {
                    defer.reject(error);
                })
            return defer.promise;
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