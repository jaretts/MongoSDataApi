define('configuration/development', ['Mobile/Reports/ApplicationModule'], function() {

    return {
        modules: [
            new Mobile.Reports.ApplicationModule()
        ],
        connections: {
            'crm': {
                isDefault: true,
                offline: true,
                url: 'http://localhost:56056/sdata/',
                json: true
            }
        },
        enableUpdateNotification: true
    };

});