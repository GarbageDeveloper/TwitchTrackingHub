$(function () {
    var notificationHub = $.connection.notificationHub;
    $.connection.hub.start();

    GetAllViewers()
    notificationHub.client.StreamerAdded = function (result) {
        $("#streamer-update").html("<span style='background-color:green'>Hey everyone! " +
            "<a href='https://www.twitch.tv/" + result.StreamerName + "'> " + result.StreamerName +
            "</a> Has been added to " + result.ViewerFirstName + " " + result.ViewerLastName + "'s list of streamers.</span>")
    }
    notificationHub.client.StreamerDeleted = function (result) {
        $("#streamer-update").html("<span style='background-color:red'>Hey everyone! " +
            "<a href='https://www.twitch.tv/" + result.StreamerName + "'> " + result.StreamerName +
            "</a> Has been removed from " + result.ViewerFirstName + " " + result.ViewerLastName + "'s list of streamers.</span>")
    }

    function GetAllViewers() {
        $.get("/home/GetViewers", function (result) {
            result.Viewers.forEach(function (viewer) {
                $("#list-of-viewers").append("<li><a href='/Home/StreamersByViewerId?id=" + viewer.Id + "'>" + viewer.FirstName + " " + viewer.LastName + "</a></li>")
            })
        })
    }
    $("#add-viewer").click(function () {
        $("#add-viewer-div").removeClass('hidden')
    })
})