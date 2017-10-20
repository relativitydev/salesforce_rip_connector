
//Relativity Application Guid
var ApplicationGuid = "FE38D634-A413-4EB1-A486-FC00D2B0F17C";

//Response Models
function SecurityResponse(val) {
    this.Value = val;
}

function encrypt_model(model) {
    var retVal = "";
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Relativity/CustomPages/GUID/Security/Encrypt".replace("GUID", ApplicationGuid),
        async: false,
        data: new SecurityResponse(encodeURIComponent(JSON.stringify(model))),
        success: function (data) {
            retVal = data;
        }
    });
    return retVal;
}

function decrypt_model(model) {
    var retVal = "";
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Relativity/CustomPages/GUID/Security/Decrypt".replace("GUID", ApplicationGuid),
        async: false,
        //data: new SecurityResponse(JSON.stringify(model)),
        data: new SecurityResponse(model),
        success: function (data) {
            retVal = JSON.parse(decodeURIComponent(data));
        }
    });
    return retVal;
}

// Copy the folloing Javascript files from the Relativity Integration Points SDK into the script folder:
// frame-messaging.js, jquery-1.8.2.js, jquery-postMessage.js
$(function () {
    //Create a new communication object that talks to the host page.
    var message = IP.frameMessaging();

    var getModel = function () {
        var model = {
            SalesforceURI: $("#SalesforceURI").val(),
            SalesforceUserID: $('#SalesforceUserID').val(),
            SalesforceUserPwd: $('#SalesforceUserPwd').val(),
            StartDate: $('#StartDate').val(),
            TicketType: $('#TicketType').val()
        }
        return model;
    };

    //An event raised when the user has clicked the Next or Save button.
    message.subscribe('submit', function () {
        //Execute save logic that persists the state
        var encryptedModel = new SecurityResponse(encrypt_model(getModel()));
        this.publish("saveState", JSON.stringify(encryptedModel));
        //Communicate to the host page that it to continue.
        this.publish('saveComplete', JSON.stringify(encryptedModel));
    });

    //An event raised when a user clicks the Back button.
    message.subscribe('back', function () {
        //Execute save logic that persists the state.
        var encryptedModel = new SecurityResponse(encrypt_model(getModel()));
        this.publish('saveState', JSON.stringify(encryptedModel));
    });

    //An event raised when the host page has loaded the current settings page.
    message.subscribe('load', function (model) {

        // Set field Value only when model contains value
        if (model.length > 0) {
            var encryptedModel = JSON.parse(model);
            var localModel = decrypt_model(encryptedModel.Value);
            $('#SalesforceURI').val(localModel.SalesforceURI);
            $('#SalesforceUserID').val(localModel.SalesforceUserID);
            $('#SalesforceUserPwd').val(localModel.SalesforceUserPwd);
            $('#StartDate').val(localModel.StartDate);
            $('#TicketType').val(localModel.TicketType);
        }
    });
});
