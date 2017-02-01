(function () {
    'use strict';
    angular
        .module('cmp.services')
        .service('cmpApi', cmpApi);

    cmpApi.$inject = ['$http'];
    var baseUrl = '/api/'
    function cmpApi($http) {
        this.resources = {
            employees: {
                getEmployees: {
                    url: baseUrl + 'employees',
                    method: 'GET'
                },
                getEmployeesOptions: {
                    url: baseUrl + 'employees',
                    method: 'GET'
                },
                 updateEmployee: {
                    url: baseUrl + 'employees',
                    method: 'POST'
                },
                 insertEmployee: {
                    url: baseUrl + 'employees',
                    method: 'POST'
                }
            },
            jobs: {
                getJobs: {
                    url: baseUrl + 'jobs',
                    method: 'GET'
                },
                getJobsOptions: {
                    url: baseUrl + 'jobs',
                    method: 'GET'
                },
                updateJob: {
                    url: baseUrl + 'jobs',
                    method: 'POST'
                },
                insertJob: {
                    url: baseUrl + 'jobs',
                    method: 'POST'
                }
            },
            company: {
                getCompanies: {
                    url: baseUrl + 'company',
                    method: 'GET'
                },
                getCompanyOptions: {
                    url: baseUrl + 'company',
                    method: 'GET'
                },
                updateCompany: {
                    url: baseUrl + 'company',
                    method: 'POST'
                },
                insertCompany: {
                    url: baseUrl + 'company',
                    method: 'POST'
                }
            },
            customer: {
                getCustomers: {
                    url: baseUrl + 'customer',
                    method: 'GET'
                },
                getCustomerOptions: {
                    url: baseUrl + 'customer',
                    method: 'GET'
                },
                updateCustomer: {
                    url: baseUrl + 'customer',
                    method: 'POST'
                },
                insertCustomer: {
                    url: baseUrl + 'customer',
                    method: 'POST'
                }
            },
            department: {
                getDepartments: {
                    url: baseUrl + 'department',
                    method: 'GET'
                },
                getDepartmentOptions: {
                    url: baseUrl + 'department',
                    method: 'GET'
                },
                updateDepartment: {
                    url: baseUrl + 'department',
                    method: 'POST'
                },
                insertDepartment: {
                    url: baseUrl + 'department',
                    method: 'POST'
                }
            },
            experience: {
                getExperiences: {
                    url: baseUrl + 'experience',
                    method: 'GET'
                },
                getExperiencesOptions: {
                    url: baseUrl + 'experience',
                    method: 'GET'
                },
                updateExperience: {
                    url: baseUrl + 'experience',
                    method: 'POST'
                },
                insertExperience: {
                    url: baseUrl + 'experience',
                    method: 'POST'
                }
            },
            focalPoint: {
                getFocalPoints: {
                    url: baseUrl + 'focalpoint',
                    method: 'GET'
                },
                getFocalPointOptions: {
                    url: baseUrl + 'focalpoint',
                    method: 'GET'
                },
                updateFocalPoint: {
                    url: baseUrl + 'focalpoint',
                    method: 'POST'
                },
                insertFocalPoint: {
                    url: baseUrl + 'focalpoint',
                    method: 'POST'
                }
            },
            project: {
                getProject: {
                    url: baseUrl + 'project',
                    method: 'GET'
                },
                getProjectOptions: {
                    url: baseUrl + 'project',
                    method: 'GET'
                },
                updateProject: {
                    url: baseUrl + 'project',
                    method: 'POST'
                },
                insertProject: {
                    url: baseUrl + 'project',
                    method: 'POST'
                }
            },
            projectTechnology: {
                getProjectTechnologies: {
                    url: baseUrl + 'projecttechnology',
                    method: 'GET'
                },
                getProjectTechnologyOptions: {
                    url: baseUrl + 'projecttechnology',
                    method: 'GET'
                },
                updateProjectTechnology: {
                    url: baseUrl + 'projecttechnology',
                    method: 'POST'
                },
                insertProjectTechnology: {
                    url: baseUrl + 'projecttechnology',
                    method: 'POST'
                }
            },
            technology: {
                getTechnologies: {
                    url: baseUrl + 'technology',
                    method: 'GET'
                },
                getTechnologyOptions: {
                    url: baseUrl + 'technology',
                    method: 'GET'
                },
                updateTechnology: {
                    url: baseUrl + 'technology',
                    method: 'POST'
                },
                insertTechnology: {
                    url: baseUrl + 'technology',
                    method: 'POST'
                }
            },
            projectTypes: {
                getProjectTypes: {
                    url: baseUrl + 'projecttypes',
                    method: 'GET'
                },
                getProjectTypeOptions: {
                    url: baseUrl + 'projecttypes',
                    method: 'GET'
                },
                updateProjectType: {
                    url: baseUrl + 'projecttypes',
                    method: 'POST'
                },
                insertProjectType: {
                    url: baseUrl + 'projecttypes',
                    method: 'POST'
                }
            },
            states: {
                getStates: {
                    url: baseUrl + 'states',
                    method: 'GET'
                },
                getStatesOptions: {
                    url: baseUrl + 'states',
                    method: 'GET'
                },
                updateState: {
                    url: baseUrl + 'states',
                    method: 'POST'
                },
                insertState: {
                    url: baseUrl + 'states',
                    method: 'POST'
                }
            },
            cities: {
                getCities: {
                    url: baseUrl + 'cities',
                    method: 'GET'
                },
                getCitysOptions: {
                    url: baseUrl + 'cities',
                    method: 'GET'
                },
                updateCity: {
                    url: baseUrl + 'cities',
                    method: 'POST'
                },
                insertCity: {
                    url: baseUrl + 'cities',
                    method: 'POST'
                }
            },
            countries: {
                getCountries: {
                    url: baseUrl + 'countries',
                    method: 'GET'
                },
                getCountryOptions: {
                    url: baseUrl + 'countries',
                    method: 'GET'
                },
                updateCountry: {
                    url: baseUrl + 'countries',
                    method: 'POST'
                },
                insertCountry: {
                    url: baseUrl + 'countries',
                    method: 'POST'
                }
            },
            role: {
                getRoles: {
                    url: baseUrl + 'role',
                    method: 'GET'
                },
                getRoleOptions: {
                    url: baseUrl + 'role',
                    method: 'GET'
                },
                updateRole: {
                    url: baseUrl + 'role',
                    method: 'POST'
                },
                insertRole: {
                    url: baseUrl + 'role',
                    method: 'POST'
                }
            },
            user: {
                getUsers: {
                    url: baseUrl + 'user',
                    method: 'GET'
                },
                getUserOptions: {
                    url: baseUrl + 'user',
                    method: 'GET'
                },
                updateUser: {
                    url: baseUrl + 'user',
                    method: 'POST'
                },
                insertUser: {
                    url: baseUrl + 'user',
                    method: 'POST'
                }
            },
        };

        //TODO: check giledAjax, if can use with http - cahnge to use with resource
        //TODO: add mock to porject
        this.getData = function (urlResource, params) {
            if (urlResource.method == 'POST') {            
               return $http.post(urlResource.url, params);
            }
            else {
                if (params) {
                    return $http.get(urlResource.url + "/" + params, params);
                }
                else { return $http.get(urlResource.url); }
                
            }
        }
    }
})();