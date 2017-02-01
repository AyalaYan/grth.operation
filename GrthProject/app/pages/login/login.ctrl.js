(function () {
    'use strict';
    angular
        .module('cmp.login')
        .controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['cmpData', '$location'];

    function loginCtrl(cmpData, $location) {
        /****************************************************
               VARIABLES
   **************************************************** */
        var vm = this;
        vm.countoperation = null;
        /*****************************************************
        *                  METHODS                          *
        *****************************************************/
        vm.goToListOfoperation = function () {
            $location.path('/list');
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
