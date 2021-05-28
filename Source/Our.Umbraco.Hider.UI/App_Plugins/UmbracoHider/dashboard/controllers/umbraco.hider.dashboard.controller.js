(function () {
    angular
        .module('umbraco')
        .controller('Umbraco.Hider.Dashboard.Controller', umbracoHiderDashboardController);

    umbracoHiderDashboardController.$inject = [
        'Umbraco.Hider.Resources',
        'notificationsService'
    ];

    function umbracoHiderDashboardController(umbracoHiderResources, notificationsService) {
        var vm = this;

        /////////////////////////////Dashboard/////////////////////////////////
        vm.dashboard = {};
        vm.dashboard.rules = [];
        vm.dashboard.actionInProgress = false;
        vm.dashboard.propertiesEditorswatchers = [];
        vm.dashboard.rowObject = {};
        vm.dashboard.propertiesOrder = [];
        vm.dashboard.columnsData = {
            'columns': [
                {
                    'title': 'Rule type',
                    'alias': 'Type',
                    'type': 'dropdown',
                    'props':
                    {
                        'options': [
                            { 'text': 'Hide Tabs', 'value': 'HideTabs' },
                            { 'text': 'Hide Properties', 'value': 'HideProperties' },
                            { 'text': 'Hide Buttons', 'value': 'HideButtons' },
                            { 'text': 'Hide Content Apps', 'value': 'HideContentApps' }
                        ]
                    }
                },
                { 'title': 'Enabled', 'alias': 'Enabled', 'type': 'checkbox', 'props': {} },
                { 'title': 'Names', 'alias': 'Names', 'type': 'textarea', 'props': {} },
                { 'title': 'Users', 'alias': 'Users', 'type': 'textarea', 'props': {} },
                { 'title': 'User Groups', 'alias': 'UserGroups', 'type': 'textarea', 'props': {} },
                { 'title': 'Content Ids', 'alias': 'ContentIds', 'type': 'textarea', 'props': {} },
                { 'title': 'Parent content Ids', 'alias': 'ParentContentIds', 'type': 'textarea', 'props': {} },
                { 'title': 'Content Types', 'alias': 'ContentTypes', 'type': 'textarea', 'props': {} },
                { 'title': 'Description', 'alias': 'Description', 'type': 'textarea', 'props': {} }
            ]
        };

        getTableLayout();
        getConfig();

        function getTableLayout() {
            vm.dashboard.rowObject = {};
            vm.dashboard.propertiesOrder = [];

            angular.forEach(vm.dashboard.columnsData.columns,
                function (value) {
                    vm.dashboard.propertiesOrder.push(value.alias);
                });
        };

        function getConfig() {
            umbracoHiderResources.getConfig()
                .then(function (response) {
                    if (response.data) {
                        var rules = response.data.Rules;
                        vm.dashboard.rules = rules;
                    }
                }, function (error) {
                    notificationsService.error('Error retrieving rules from the json config file, please check the logs');
                });
        }

        /////////////////////////////Save Rule/////////////////////////////////
        vm.dashboard.saveRules = function () {
            vm.dashboard.actionInProgress = true;

            umbracoHiderResources.saveRules(vm.dashboard.rules)
                .then(function (result) {
                    if (result.data && result.data !== '') {
                        notificationsService.success('Rules saved successfully');
                    } else {
                        notificationsService.error('Error saving rules, please check the logs');
                    }
                    vm.dashboard.actionInProgress = false;
                }, function (error) {
                    notificationsService.error('Error saving rules, please check the logs');
                    vm.dashboard.actionInProgress = false;
                });
        };

        /////////////////////////////Add Rule/////////////////////////////////
        vm.dashboard.addRule = function () {
            vm.dashboard.rules.push(angular.copy(vm.dashboard.rowObject));
            var newRowIndex = vm.dashboard.rules.length - 1;
            vm.dashboard.rules[newRowIndex];
        }

        /////////////////////////////Remove Rule/////////////////////////////////
        vm.dashboard.removeRule = function (index) {
            vm.dashboard.rules.splice(index, 1);
        }

        /////////////////////////////Sort grid/////////////////////////////////
        vm.dashboard.sortableOptions = {
            axis: 'y',
            cursor: 'move',
            handle: '.sortHandle',
            tolerance: 'pointer',
            containment: 'parent',
            start: function (event, ui) {
                var curTh = ui.helper.closest('table').find('thead').find('tr');
                var itemTds = ui.item.children('td');

                curTh.find('th').each(function (ind, obj) {
                    itemTds.eq(ind).width($(obj).width());
                });
            }
        };
    }
}
)();