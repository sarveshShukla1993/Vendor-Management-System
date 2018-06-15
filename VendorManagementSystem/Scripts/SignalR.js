//alert("Alert!! from SignalR.");
$.connection.hub.start()
     .done(function () {
         $("#btn").click(function () {
             var msg =  $("#chatmsg").val();
            if(msg == "" || msg == null)
              { }
             else
            {
                $.connection.myHub.server.announce(msg);
            }
         });
     })
     .fail(function () {
         alert("Error!!")
     });


$.connection.myHub.client.announce = function (message) {
    if (Session["Adminid"] != null)
    {
        var a = Session["Adminid"].ToString();
        $("#welcome-messages").append( a + ":" +message + "<br />")
    }
    else if (Session["superid"] != null)
    {
        var a = Session["superid"].ToString();
        $("#welcome-messages").append(a + ":" + message + "<br />")
    }
}