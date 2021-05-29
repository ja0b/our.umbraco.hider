(function () {
    angular.module('umbraco.services').config([
        '$httpProvider',
        function ($httpProvider) {
            $httpProvider.interceptors.push(['$q', '$injector', '$routeParams', function ($q, $injector, $routeParams) {
                return {
                    'request': function (request) {
                        var getByIdUrlRegex = /^\/umbraco\/backoffice\/UmbracoTrees\/ContentTree\/GetMenu/i;

                        if (getByIdUrlRegex.test(request.url)) {
                            var umbracoHiderResources = $injector.get('Umbraco.Hider.Resources');

                            umbracoHiderResources.getActionsButtonRule($routeParams.id)
                                .then(function (result) {
                                    if (result.data) {
                                        var $actionsButton = $('div [data-element="editor-actions"]');
                                        $actionsButton.addClass('hidden-button');
                                        $actionsButton.remove();
                                    }
                                });
                        }

                        return request || $q.when(request);
                    }
                };
            }]);
        }
    ]);
})();