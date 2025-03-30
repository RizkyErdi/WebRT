$(document).ready(function () {

});


function SaveUser() {
    $("#loader").addClass("is-active");
    //var isValid = validateInput();
    //if (!isValid) {
    //    $("#message").show();
    //    $("#loader").removeClass("is-active");
    //    return;
    //} else {
    //    $("#message").hide();
    //}

    var ClsSave = {
        Username: $("#txtNrp").val(),

    };
    $.ajax({
        type: "POST",
        url: "/Home/Insert",
        data: JSON.stringify(ClsSave),
        contentType: "application/json",
        success: function (response) {
            //console.log(data);
            if (response.Remarks != true) {
                alert(response.Message);
                //window.location.href = "/FormKetKerja/DataKetKerja";
                return;
            }
            else {
                alert(response.Message);
                //window.location.href = "/FormKetKerja/DataKetKerja";
                return;
            }
        },
        //failure: function (errMsg) {
        //    alert(errMsg);
        //},
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
}