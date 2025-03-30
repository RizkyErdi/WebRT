$(document).ready(function () {

});

debugger;
function Login() {
    var username = $("#username").val();
    var password = $("#password").val();

    $.ajax({
        type: "POST",
        url: "/Login/Login?username=" + username + "&password=" + password,
        dataType: "json",
        //contentType: "application/json",
        //@* data: data,*@
        success: function (result) {

            if (result.Status == true) {
                //alert("Login Berhasil");
                location.href = "../Home/About"

            }
            else if (result.Status == false) {

                console.log(result.Message);
                alert("Password Salah");

            }
            else {
                console.log(result.Message);
                alert("Login Error");
            }
        }
    });

}