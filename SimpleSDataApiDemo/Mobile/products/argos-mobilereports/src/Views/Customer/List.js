define('Mobile/Reports/Views/Customer/List', ['Sage/Platform/Mobile/List'], function () {

    return dojo.declare('Mobile.Reports.Views.Customer.List', [Sage.Platform.Mobile.List], {
        //Templates
        /* feed doesn't have a `$key` property */
        /*
        rowTemplate: new Simplate([
            '<li data-action="activateEntry" data-key="{%= $.Id %}" data-descriptor="{%: $.customername %}">',
            '<div data-action="selectEntry" class="list-item-selector"></div>',
            '{%! $$.itemTemplate %}',
            '</li>'
        ]),
        */
        itemTemplate: new Simplate([
            '<h3>{%: $.customername %}</h3>',
            '<h4>{%: $.telephoneno %}</h4>',
            '<h4>Open Order Balance: {%: $.openorderamt %}</h4>'
        ]),


        /*
        public String _id { get; set; }
        public String customername { get; set; }
        public String addressline1 { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zipcode { get; set; }
        public String telephone { get; set; }
        public Double openorderamt { get; set; }
        */

        //Localization
        titleText: 'Customers',

        //View Properties
        detailView: 'customer_detail',
        insertView: 'customer_edit',
        expose: true,
        icon: 'content/images/icons/reports_24.png',
        id: 'customer_list',
        queryOrderBy: 'state asc, customername asc',
        querySelect: [
            'Id',
            'customername',
            'telephone',
            'state',
            'openorderamt'
        ],
        resourceKind: 'Customer',
        /*
        createToolLayout: function () {
            return this.tools || (this.tools = {
                'tbar': []
            });
        },
        */
        /*
        queryWhere: function () {
            return dojo.string.substitute('UserID eq ${0}', [App.context['user']['$key']]);
        },
        */
        /*
        getGroupForEntry: function (entry) {
            return {
                tag: entry['state'],
                title: entry['state']
            };
        },
        */
        formatSearchQuery: function (query) {
            return dojo.string.substitute('customername like "${0}%"', [this.escapeSearchQuery(query)]);
        }
    });

});