$(document).ready(function () {
    
});

$("#contactForm").submit(function (e) {
    e.preventDefault();

    // Reset error messages
    $(".text-danger").addClass("d-none");
    $("#messageBox").addClass("d-none");

    // Ambil data dari form
    var name = $("#name").val().trim();
    var email = $("#email").val().trim();
    var message = $("#message").val().trim();

    var isValid = true;

    // Validasi input
    if (name === "") {
        $("#nameError").removeClass("d-none");
        isValid = false;
    }

    if (email === "" || !validateEmail(email)) {
        $("#emailError").removeClass("d-none");
        isValid = false;
    }

    if (message === "") {
        $("#messageError").removeClass("d-none");
        isValid = false;
    }

    if (!isValid) return;

    // Kirim data menggunakan AJAX ke controller ASP.NET MVC
    $.ajax({
        url: "/Input/Submit", // Ganti dengan action yang sesuai di Controller
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ Name: name, Email: email, Message: message }),
        success: function (response) {
            $("#messageBox").removeClass("d-none alert-danger").addClass("alert-success").text("Pesan berhasil dikirim!");
            $("#contactForm")[0].reset();
            setTimeout(function () {
                window.location.href = window.location.href;
            }, 3000);
            
        },
        error: function () {
            $("#messageBox").removeClass("d-none alert-success").addClass("alert-danger").text("Terjadi kesalahan, coba lagi.");
        }
    });
});

function validateEmail(email) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

