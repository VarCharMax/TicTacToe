
function CheckEmailConfirmationStatus(email) {
    $.get("/CheckEmailConfirmationStatus?email=" + email, function (data) {
        if (data === "OK") {
            if (interval !== null) {
                clearInterval(interval);
            }
            window.location.href = "/GameInvitation?email=" + email;
        }
    });
}

function CheckGameInvitationConfirmationStatus(id) {
    $.get("/GameInvitationConfirmation?id=" + id, function () {
        if (data.Result === "OK") {
            if (interval !== null) {
                clearInterval(interval);
            }
            window.location.href = "/GameSession/Index/" + id;
        }
    });
}

var openSocket = function (parameter, strAction) {
    if (interval !== null) clearInterval(interval);
    
    var protocol = location.protocol === "https" ? "wss:" : "wss:";
    var operation = "";
    var wsUri = "";

    if (strAction == "Email") {
        wsUri = protocol + "//" + window.location.host + "/CheckEmailConfirmationStatus";
        operation = "CheckEmailConfirmationStatus";
    } else if (strAction == "GameInvitation") {
        wsUri = protocol + "//" + window.location.host + "/GameInvitationConfirmation";
        operation = "CheckGameInvitationConfirmationStatus";
    }
    
    var socket = new WebSocket(wsUri);
    
    socket.onmessage = function (response) {

        if (strAction == "Email" && response.data == "OK") {
            window.location.href = "/GameInvitation?email=" + parameter;
        } else if (strAction == "GameInvitation") {
            var data = $.parseJSON(response.data);
            
            if (data.Result == "OK") window.location.href = "/GameSession/Index/" + data.Id;
        }
    };

    socket.onopen = function () {
        var json = JSON.stringify({
            "Operation": operation,
            "Parameters": parameter
        });

        socket.send(json);
    };

    socket.onclose = function (event) { };
}
