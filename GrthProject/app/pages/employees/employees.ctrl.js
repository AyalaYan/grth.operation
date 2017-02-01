(function () {
    'use strict';
    angular
        .module('cmp.employees')
        .controller('employeesCtrl', employeesCtrl);

    employeesCtrl.$inject = ['cmpData', '$location', '$scope', '$http', '$timeout',
        '$interval', 'uiGridConstants', 'uiGridGroupingConstants', '$anchorScroll', 'toaster'];

    function employeesCtrl(cmpData, $location, $scope, $http, $timeout, $interval, uiGridConstants, uiGridGroupingConstants, $anchorScroll, toaster) {

        /****************************************************
               VARIABLES
   **************************************************** */
        var vm = this;
        vm.action = "Add";
        vm.whenHttpRequestHasFinishedLoading = null;
        vm.employeeObj = null;
        vm.experiencesList = [{ ID: 0 }];
        //Drop Downs
        vm.countriesOptions = null;
        vm.citiesOptions = null;
        vm.statesOptions = null;
        vm.listJobs = null;
        vm.technologies = null;
        vm.listCompanies = null;
        vm.gridOptions = {
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableCellEditOnFocus: true,
            enableColumnResizing: true,
            enableFiltering: true,
            enableGridMenu: true,
            fastWatch: true,
            enableRowSelection: true,
            enableSelectAll: false,
            columnDefs: [
              { name: 'ID', displayName: 'ID', visible: false },
              { name: 'FirstName', displayName: 'First Name', enableSorting: true },
              { name: 'FirstFamilyName', displayName: 'First Family Name', enableSorting: true },
              { name: 'LastName', displayName: 'Last Name', enableSorting: true },
              { name: 'JobName', displayName: 'Job Name', enableSorting: true },
              { name: 'PhoneNumber', displayName: 'Phone', enableSorting: true },
              { name: 'Email', displayName: 'Email', enableSorting: true },
              { name: 'IsActive', displayName: 'Is Active', enableSorting: true },
              { name: 'Remarks', displayName: 'Remarks', enableSorting: true, visible: false },
              { name: 'Address', displayName: 'Address', enableSorting: true, visible: false },
              { name: 'CountryName', displayName: 'Country', enableSorting: true, visible: false },
              { name: 'StateName', displayName: 'State', enableSorting: true, visible: false },
              { name: 'CityName', displayName: 'City', enableSorting: true, visible: false },
            ],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
                vm.gridApi.grid.options.multiSelect = false;
                vm.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        vm.paginationOptions.sort = null;
                    } else {
                        vm.paginationOptions.sort = sortColumns[0].sort.direction;
                    }
                    vm.getEmployeeTableData();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    vm.paginationOptions.pageNumber = newPage;
                    vm.paginationOptions.pageSize = pageSize;
                    vm.getEmployeeTableData();
                });
                //set gridApi on scope
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    vm.employeeObj = cmpData.getEmployeeById(row.entity.ID);
                    vm.getExperiences(vm.employeeObj.ID);
                    if (vm.employeeObj && vm.employeeObj.CountryID) {
                        vm.getStatesOptions(vm.employeeObj.CountryID);
                    }
                    if (vm.employeeObj && vm.employeeObj.StateID) {

                        vm.getCitiesOptions(vm.employeeObj.StateID);
                    }
                    vm.addEditClick = true;
                    vm.collapsed = true;
                    vm.action = "Edit";
                    $anchorScroll();
                })
            }
        };

        vm.paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        /*****************************************************
  *                  METHODS                          *
  *****************************************************/
        vm.changeCountry = function () {
            if (vm.employeeObj && vm.employeeObj.CountryID) {
                vm.getStatesOptions(vm.employeeObj.CountryID);
            }
            else {
                vm.statesOptions = null;
            }
            vm.citiesOptions = null;
        }

        vm.changeState = function () {
            if (vm.employeeObj && vm.employeeObj.StateID) {

                vm.getCitiesOptions(vm.employeeObj.StateID);
            }
            else {
                vm.citiesOptions = null;
            }
        }

        vm.addEmployee = function () {
            vm.addEditClick = true;
            vm.collapsed = true;
            vm.action = "Add";
            vm.employeeObj = { ID: 0, IsActive: true }
            vm.setDefaultExperience();

        }

        vm.saveEmployee = function () {
            if (vm.employeeObj.ID === 0) {
                cmpData.insertEmployee(vm.employeeObj).then(function (data) {
                    if (data.Result === "OK") {
                        toaster.success('Insert Employee', data.Message);
                    }
                    else {
                        toaster.error('Insert Employee', data.Message);
                    }
                }, function (error) { });
            }
            else {
                cmpData.updateEmployee(vm.employeeObj).then(function (data) {
                    if (data.Result === "OK") {
                        toaster.success('Update Employee', data.Message);
                    }
                    else {
                        toaster.error('Update Employee', data.Message);
                    }
                }, function (error) { });
            }

            vm.addEditClick = false;
            vm.collapsed = false;
            vm.employeeObj = { ID: 0, IsActive: true }
            vm.setDefaultExperience();
            vm.getEmployeeTableData();

        }

        vm.addExperienceRecord = function () {
            vm.experiencesList.push({ ID: 0, IsActive: true });
        }


        /*****************************************************
    *               METHODS - PRIVATE                   *
    ******************************************************/
        vm.getEmployeeTableData = function () {

            cmpData.getEmployees().then(function (data) {
                vm.gridOptions.totalItems = data.length
                var firstRow = (vm.paginationOptions.pageNumber - 1) * vm.paginationOptions.pageSize;
                vm.gridOptions.data = data.slice(firstRow, firstRow + vm.paginationOptions.pageSize);

            }, function (error) { });
        };

        vm.getCountriesOptions = function () {
            cmpData.getCountryOptions().then(function (data) {
                vm.countriesOptions = data;
            }, function (error) { });
        };

        vm.getCitiesOptions = function (StateID) {
            cmpData.getCitysOptions(StateID).then(function (data) {
                vm.citiesOptions = data;
            }, function (error) { });

        };

        vm.getStatesOptions = function (CountryID) {
            cmpData.getStatesOptions(CountryID).then(function (data) {
                vm.statesOptions = data;
            }, function (error) { });
        };

        vm.getExperiences = function (iEmployeeId) {
            cmpData.getExperiences(iEmployeeId).then(function (data) {
                if (data && data.length > 0) { vm.experiencesList = data; }
                else { vm.setDefaultExperience(); }
            }, function (error) { });
        };

        vm.getJobsOptions = function () {
            cmpData.getJobsOptions().then(function (data) {
                vm.listJobs = data;
            }, function (error) { });
        };

        vm.getTechnologyOptions = function () {
            cmpData.getTechnologyOptions().then(function (data) {
                vm.technologies = data;
            }, function (error) { });
        };

        vm.getCompanyOptions = function () {
            cmpData.getCompanyOptions().then(function (data) {
                vm.listCompanies = data;
            }, function (error) { });
        };

        vm.setDefaultExperience = function () {
            vm.experiencesList = [];
            vm.experiencesList.push({ ID: 0, IsActive: true });
        }

        function init() {
            vm.getEmployeeTableData();
            vm.getCountriesOptions();
            if (vm.employeeObj && vm.employeeObj.CountryID) {
                vm.getStatesOptions(vm.employeeObj.CountryID);
            }
            if (vm.employeeObj && vm.employeeObj.StateID) {

                vm.getCitiesOptions(vm.employeeObj.StateID);
            }
            vm.getTechnologyOptions();
            vm.getCompanyOptions();
            vm.getJobsOptions();
        }
        /*****************************************************
            *                  EXECUTIONS                       *
         *****************************************************/
        init();
    }
})();
