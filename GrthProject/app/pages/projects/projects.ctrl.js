(function () {
    'use strict';
    angular
        .module('cmp.projects')
        .controller('projectsCtrl', projectsCtrl);

    projectsCtrl.$inject = ['cmpData', '$location'];

    function projectsCtrl(cmpData, $location) {
        /****************************************************
               VARIABLES
   **************************************************** */
        var vm = this;
        vm.listoperation = null;
        /*****************************************************
    *                  METHODS                          *
    *****************************************************/
        vm.addNewemployee = function () {
            $location.path('/set/0');
       };
        
        /*****************************************************
        *               METHODS - PRIVATE                   *
        ******************************************************/
        function init() {
            cmpData.getoperation().then(
              function (operation) {                
                   vm.listoperation=operation.data
              });
           
        }
        /*****************************************************
            *                  EXECUTIONS                       *
            *****************************************************/
        init();
    }
})();
