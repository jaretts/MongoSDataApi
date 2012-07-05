/// <reference path="../../../../../argos-sdk/libraries/ext/ext-core-debug.js"/>
/// <reference path="../../../../../argos-sdk/libraries/sdata/sdata-client-debug"/>
/// <reference path="../../../../../argos-sdk/libraries/Simplate.js"/>
/// <reference path="../../../../../argos-sdk/src/View.js"/>
/// <reference path="../../../../../argos-sdk/src/Detail.js"/>

define('Mobile/SalesLogix/Views/Customer/Detail', ['Sage/Platform/Mobile/Detail'], function () {

    return dojo.declare('Mobile.SalesLogix.Views.Customer.Detail', [Sage.Platform.Mobile.Detail], {
        //Localization
        cutomerNameText: 'customer',
        customerIDText: 'customer ID',
        addressText: 'address',
        phoneText: 'phone',
        lastPymtText: 'last payment',
        lastPymtDateText: 'last payment date',
        amountDueText: 'amount due',
        overDueText: 'over due',
        currentDueText: 'current due',
        over30DaysText: '    over 30',
        over60DaysText: '    over 60',
        over90DaysText: '    over 90',
        over120DaysText: '   over 120',
        openOrdAmtText: 'open order amount',
        credHoldText: 'credit hold',
        credLimitText: 'credit limit',
        activityTypeText: {
            'atPhoneCall': 'Phone Call'
        },
        actionsText: 'Quick Actions',
        relatedActivitiesText: 'Activities',
        relatedContactsText: 'Contacts',
        relatedHistoriesText: 'Notes/History',
        relatedItemsText: 'Related Items',
        relatedNotesText: 'Notes',
        relatedOpportunitiesText: 'Opportunities',
        relatedTicketsText: 'Tickets',
        statusText: 'status',
        subTypeText: 'subtype',
        titleText: 'Account',
        typeText: 'type',
        webText: 'web',
        callMainNumberText: 'Call main number',
        scheduleActivityText: 'Schedule activity',
        addNoteText: 'Add note',
        viewAddressText: 'View address',
        moreDetailsText: 'Financial Info',
        calledText: 'Called ${0}',

        //View Properties
        id: 'customer_detail',
        //editView: 'account_edit',
        //historyEditView: 'history_edit',
        //noteEditView: 'history_edit',
        //security: 'Entities/Account/View',
        querySelect: [
            'customername',
            'ardivisionno',
            'customerno',
            'addressline1',
            'addressline2',
            'city',
            'state',
            'zipcode',
            'countrycode',
            'telephoneno',
            'datelastpayment',
            'lastpaymentamt',
            'currentbalance',
            'agingcategory1',
            'agingcategory2',
            'agingcategory3',
            'agingcategory4',
            'openorderamt',
            'credithold',
            'creditlimit'
        ],
        resourceKind: 'Customer',
        callMainPhone: function () {
            //  this.recordCallToHistory(function() {
            App.initiateCall(this.entry['telephoneno']);
            // }.bindDelegate(this));
        },
        checkMainPhone: function (entry, value) {
            return !value;
        },
        /*
        tmpAddress: function (tmpAddr1, tmpCity, tmpState, tmpZipCode) {

            this.AddressLine1 = tmpAddr1;
            this.AddressLine2 = tmpAddr2;
            this.City = tmpCity;
            this.State = tmpState;
            this.ZipCode = tmpZipCode;
        },
        viewAddress: function () {

            var swmAddress = new this.tmpAddress(this.entry['addressline1'], this.entry['city'], this.entry['state'], this.entry['zipcode']);

            App.showMapForAddress(Mobile.SalesLogix.Format.address(swmAddress, true, ' '));
        },
        checkAddress: function (entry, value) {
            return !Mobile.SalesLogix.Format.address(value, true, '');
        },
        scheduleActivity: function () {
            App.navigateToActivityInsertView();
        },
        */
        /*
        amountDue: function (entry) {
            var ad = 0;
            ad = Number(entry.CurrentBalance) + Number(entry.AgingCategory1) + Number(entry.AgingCategory2) + Number(entry.AgingCategory3) + Number(entry.AgingCategory4);
            return Mobile.SalesLogix.Format.currency(ad);
        },
        overDue: function (o) {
            var od = 0;
            od = Number(o.AgingCategory1) + Number(o.AgingCategory2) + Number(o.AgingCategory3) + Number(o.AgingCategory4);
            return Mobile.SalesLogix.Format.currency(od);
        },
        */
        formatRelatedInvQuery: function (o) {
            return "ARDivisionNo eq '" + this.entry.ARDivisionNo + "' and CustomerNo eq '" + this.entry.CustomerNo + "' and Balance ne 0";
        },



        createLayout: function () {
            return this.layout || (this.layout = [{
                title: this.actionsText,
                list: true,
                cls: 'action-list',
                name: 'QuickActionsSection',
                children: [{
                    name: 'CallMainPhoneAction',
                    property: 'TelephoneNo',
                    label: this.callMainNumberText,
                    icon: 'content/images/icons/Dial_24x24.png',
                    action: 'callMainPhone',
                    disabled: this.checkMainPhone,
                    renderer: Mobile.SalesLogix.Format.phone.bindDelegate(this, false)
                }, {
                    name: 'ViewAddressAction',
                    property: 'Address',
                    label: this.viewAddressText,
                    icon: 'content/images/icons/Map_24.png',
                    action: 'viewAddress',
                    disabled: this.checkAddress,
                    renderer: Mobile.SalesLogix.Format.address.bindDelegate(this, true, ' ')
                }]
            }, {
                title: 'Customer Info:',
                name: 'DetailsSection',
                children: [{
                    name: 'Customer ID',
                    property: 'customerno',
                    label: 'customer ID',
                    type: 'text',
                    tpl: new Simplate(['<h3>{%: this.entry.ardivisionno+"-"+this.entry.customerno %}</h3>'])
                }, {
                    name: 'CustomerName',
                    property: 'customername',
                    label: 'customer',
                    type: 'text'
                }]
            }, {
                title: this.moreDetailsText,
                collapsed: true,
                name: 'FinancialInfoSection',
                children: [{
                    name: 'AmountDue',
                    property: 'CurrentBalance',
                    label: this.amountDueText,
                    tpl: new Simplate([
                        '<h3>{%: this.amountDue(this.entry) %}</h3>'
                    ])

                }, {
                    name: 'OverDue',
                    property: 'CurrentBalance',
                    label: this.overDueText,
                    tpl: new Simplate([
                        '<h3 style="color:red">{%: this.overDue(this.entry) %}</h3>'
                    ])

                }, {
                    name: 'CurrentBalance',
                    property: 'CurrentBalance',
                    label: this.currentDueText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'AgingCategory1',
                    property: 'AgingCategory1',
                    label: this.over30DaysText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'AgingCategory2',
                    property: 'AgingCategory2',
                    label: this.over60DaysText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'AgingCategory3',
                    property: 'AgingCategory3',
                    label: this.over90DaysText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'AgingCategory4',
                    property: 'AgingCategory4',
                    label: this.over120DaysText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'LastPaymentAmt',
                    property: 'LastPaymentAmt',
                    label: this.lastPymtText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'DateLastPayment',
                    property: 'DateLastPayment',
                    label: this.lastPymtDateText,
                    renderer: Sage.Platform.Mobile.Format.date
                }, {
                    name: 'OpenOrderAmt',
                    property: 'OpenOrderAmt',
                    label: this.openOrdAmtText,
                    renderer: Mobile.SalesLogix.Format.currency

                }, {
                    name: 'CreditLimit',
                    property: 'CreditLimit',
                    label: this.credLimitText,
                    renderer: Mobile.SalesLogix.Format.currency
                }, {
                    name: 'CreditHold',
                    property: 'CreditHold',
                    label: this.credHoldText,
                    renderer: Sage.Platform.Mobile.Format.yesNo
                }]

            }, {

                title: 'Related Items',
                list: true,
                children: [{
                    icon: 'content/images/icons/Opportunity_Won_24.png',
                    label: 'Open Invoices',
                    where: this.formatRelatedInvQuery(this.entry),
                    view: 'openInvoice_list'
                }]


            }]);
        }
    });
});