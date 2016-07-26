$(function () {
    $("table tr:gt(0)").each(function () {
        var row = $(this)
        $.getJSON('https://api.twitch.tv/kraken/streams/' + row.data('streamer-name'), function (channel) {
            if (channel["stream"] == null) {
                row.find('td:eq(1)').html("is not live")
            } else {
                row.find('td:eq(1)').html("is live")
                row.find('td:eq(0)').html("<a href='https://www.twitch.tv/" + row.data('streamer-name') + "'>" + row.data('streamer-name') + "</a>")
            }
        });
    })
    $("#add-streamer").click(function () {
        $("#add-streamer-div").removeClass('hidden')
    })
    $(".delete-streamer").on('click', function () {
        $.post("/Home/DeleteStreamer", { streamerId: $(this).data('streamer-id'), viewerId: $('#viewer-id').val() }, function () {
            window.location.reload();
        })
    })
})