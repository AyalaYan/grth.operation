
(function () {
    'use strict';
    angular
        .module('cmp.directives')
        .directive('employeeExperience', employeeExperience);

    employeeExperience.$inject = ['$window'];

    function employeeExperience($window) {

        var directive = {
            restrict: 'EA',
            templateUrl: '/app/common/directives/experience/cmp.experience.dirct.html',
            scope: { experience: '=', technologies: '=', companies: "=" },
             link: function($scope, element, attrs, ctrl) {
              if ($scope.experience.IsActive===true) {
                    $scope.collapsed = true;
                }
                else { $scope.collapsed = false; }            
                $scope.saveExperience = function () {

                }
                $scope.cancelSaveExperience = function () {
                }

            }   
        };
        return directive;
    }
})();