define('Mobile/Reports/Views/Customer/Detail', ['Sage/Platform/Mobile/Detail'], function() {

    return dojo.declare('Mobile.Reports.Views.Customer.Detail', [Sage.Platform.Mobile.Detail], {
        //Localization
        //actionsText: 'Quick Actions',
        //detailsText: 'Customer Info',
        //Localization
        cutomerNameText: 'customer',
        customerIDText: 'customer ID',
        divisionText: 'division',
        phoneText: 'phone',
        addressLine1Text: 'address',
        cityText: 'city',
        stateText: 'state',
        postalCodeText: 'postal code',
        openOrderAmtText: 'open order amount',
        /*
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
        */
        //View Properties
        id: 'customer_detail',
        resourceKind: 'Customer',
        editView: 'customer_edit',
        

        querySelect: [
            'Id',
            'customername',
            'ardivisionno',
            'customerno',
            'addressline1',
            'city',
            'state',
            'zipcode',
            'telephoneno',
            'openorderamt',
        ],


        createLayout: function() {
            return this.layout || (this.layout = [{

                title: 'Customer Detail',
                name: 'Details Section',
                list: false,
                children: [{
                    name: 'Customer ID',
                    property: 'customerno',
                    label: 'customer ID',
                    type: 'text'
                },{
                    name: 'CustomerName',
                    property: 'customername',
                    label: 'customer',
                    type: 'text'
                },{
                    label: this.divisionText,
                    name: 'Division',
                    property: 'ardivisionno',
                    type: 'text',
                },{
                    label: this.phoneText,
                    name: 'Phone',
                    property: 'telephoneno',
                    type: 'text',
                },{
                    label: this.addressLine1Text,
                    name: 'AddressLine1',
                    property: 'addressline1',
                    type: 'text',
                },{
                    label: this.cityText,
                    name: 'City',
                    property: 'city',
                    type: 'text'
                },{
                    label: this.stateText,
                    name: 'State',
                    property: 'state',
                    type: 'text'
                }, {
                    label: this.postalCodeText,
                    name: 'ZipCode',
                    property: 'zipcode',
                    type: 'text'
                },{
                    label: this.openOrderAmtText,
                    name: 'Open Order',
                    property: 'openorderamt',
                    type: 'text'
                }]
            }]);
        }

    });
});