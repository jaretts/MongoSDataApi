define('Mobile/Reports/Views/Customer/Edit', ['Sage/Platform/Mobile/Edit'], function() {

    return dojo.declare('Mobile.Reports.Views.Customer.Edit', [Sage.Platform.Mobile.Edit], {
        //Localization
        customerNameText: 'customer name',
        divisionText: 'division',
        customerNoText: 'customer',
        phoneText: 'phone',
        addressLine1Text: 'address',
        cityText: 'city',
        stateText: 'state',
        postalCodeText: 'postal code',
        openOrderAmtText: 'open order amount',

        //View Properties
        entityName: 'Customer',
        id: 'customer_edit',
        insertSecurity: true,
        updateSecurity: true,
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
        resourceKind: 'Customer',
/*
        divisionText: 'divisionn no',
        customerNoText: 'customer no',
        addressLine1Text: 'address line 1',
        cityText: 'city',
        stateText: 'state',
        postalCodeText: 'postal code',
*/
        createLayout: function() {
            return this.layout || (this.layout = [{
                label: this.customerNameText,
                name: 'Name',
                property: 'customername',
                type: 'text',
                validator: Mobile.Reports.Validator.hasText
            },{
                label: this.divisionText,
                name: 'Division',
                property: 'ardivisionno',
                type: 'text',
                validator: Mobile.Reports.Validator.hasText
            },{
                label: this.customerNoText,
                name: 'CustomerNo',
                property: 'customerno',
                type: 'text',
                validator: Mobile.Reports.Validator.hasText
            }, {
                label: this.phoneText,
                name: 'Phone',
                property: 'telephoneno',
                type: 'text',
                validator: Mobile.Reports.Validator.hasText
            },{
                label: this.addressLine1Text,
                name: 'AddressLine1',
                property: 'addressline1',
                type: 'text',
                validator: Mobile.Reports.Validator.hasText
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
            }]);
        },

        /* no-request tweak for demo purposes */
        /*
        createRequest: function() {
            return {
                create: function(entry, options) {
                    if (options.success) options.success.call(options.scope || this, entry);
                },
                update: function(entry, options) {
                    var view = options.scope;
                    if (view && view.options) dojo.mixin(view.options.entry, entry);
                    if (options.success) options.success.call(options.scope || this, entry);
                }
            };
        }
        */
    });
});