////$(document).ready(function () {

////});

////debugger;
////function Login() {
////    var username = $("#username").val();
////    var password = $("#password").val();

////    $.ajax({
////        type: "POST",
////        url: "/Login/Login?username=" + username + "&password=" + password,
////        dataType: "json",
////        //contentType: "application/json",
////        //@* data: data,*@
////        success: function (result) {

////            if (result.Status == true) {
////                //alert("Login Berhasil");
////                location.href = "../Home/About"

////            }
////            else if (result.Status == false) {

////                console.log(result.Message);
////                alert("Password Salah");

////            }
////            else {
////                console.log(result.Message);
////                alert("Login Error");
////            }
////        }
////    });

////}

$(document).ready(function () {
    console.log("Login page loaded.");
});

function Login() {
    var username = $("#username").val();
    var password = $("#password").val();

    if (username.trim() === "" || password.trim() === "") {
        alert("Username dan password harus diisi!");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/Login?username=" + encodeURIComponent(username) + "&password=" + encodeURIComponent(password),
        dataType: "json",
        success: function (result) {
            console.log("Response dari server:", result);

            if (result.Status === true) {
                // ✅ Simpan token JWT dari server ke localStorage
                if (result.Token) {
                    localStorage.setItem("jwtToken", result.Token);
                    localStorage.setItem("username", result.Username);
                    localStorage.setItem("role", result.Role);
                    console.log("Token tersimpan di localStorage:", result.Token);
                } else {
                    console.warn("Token tidak ditemukan dalam response server.");
                }

                // Arahkan ke halaman setelah login
                location.href = "/Home/About";
            }
            else if (result.Status === false) {
                alert("Password salah!");
            }
            else {
                alert("Terjadi kesalahan saat login.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error AJAX:", error);
            alert("Gagal terhubung ke server.");
        }
    });
}
