var componentStore = (function () {

    var getComponents = function (callback) {

        $.ajax({
            url: 'api/components',
            dataType: 'json',
            success: callback,
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    };

    var getComponentById = function(id, callback) {
        $.ajax({
            url: 'api/components/' + id,
            dataType: 'json',
            success: callback,
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    };

    var addComponent = function (component, callback) {

        $.ajax({
            url: 'api/components',
            dataType: 'json',
            data: component,
            method: 'POST'//,
            //success: callback,
            //error: function (xhr, status, err) {
            //    console.error(this.props.url, status, err.toString());
            //}.bind(this)
        });
    };

    var sendCommand = function (component, command) {

        $.ajax({
            url: 'api/components/' + component.name + '/' + command,
            dataType: 'json',
            method: 'POST',
            //success: callback,
            error: function (xhr, status, err) {
                //console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    };

    return {
        getComponents: getComponents,
        getComponentById: getComponentById,
        addComponent: addComponent,
        sendCommand: sendCommand
    };

})();
