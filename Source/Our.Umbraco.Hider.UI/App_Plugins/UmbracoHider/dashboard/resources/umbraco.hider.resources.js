(function () {
    'use strict';

    angular
        .module('umbraco.resources')
        .factory('Umbraco.Hider.Resources', UmbracoHiderResources);

    UmbracoHiderResources.$inject = ['$http'];

    function UmbracoHiderResources($http) {
        var API_ROOT = '/umbraco/backoffice/UmbracoHider/UmbracoHiderApi/';

        var service = {
            getConfig: getConfig,
            saveRules: saveRules,
            getActionsButtonRule: getActionsButtonRule
        };

        return service;

        function getConfig() {
            return $http.get(API_ROOT + 'GetConfig');
        }

        function saveRules(configContent) {
            return $http.post(API_ROOT + 'SaveRules',
                { Rules: configContent });
        }

        function getActionsButtonRule() {
            return $http.get(API_ROOT + 'IsActionsButtonHidden');
        }
    }
})();