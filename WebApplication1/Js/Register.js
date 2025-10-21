$(document).ready(function () {

});

function Register() {
    $("#loader").addClass("is-active");

    var ClsHome = {
        //debugger;
        username: $("#username").val(),
        password: $("#password").val(),
        nik: $("#nik").val(),
        role: $("#RoleUser").val(),

    };
    

    $.ajax({
        type: "POST",
        url: "/Home/Registrasi",
        data: JSON.stringify(ClsHome),
        contentType: "application/json",
        success: function (response) {

            if (response.Remarks == true) {
                alert("Register Berhasil");
                location.href = "../Home/About"

            }
            else if (response.Remarks == false) {

                console.log(result.Message);
                alert("Register Gagal");

            }
            else {
                console.log(response.Message);
                alert("Register Error");
                return;
            }
        },
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });

}