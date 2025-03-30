$(document).ready(function () {
    //$(".btn-success").on("click", function () {
    //    UploadIuran();
    //});
})

function UploadIuran() {
    //alert("Fungsi UploadIuran dipanggil!");
    var isValid = validateInput();

    if (!isValid) {
        $("#message").show();
        return;
    } else {
        $("#message").hide();
    }

    var clsinputiuran =
    {
        Nama: $("#Nama").val().trim(),
        Alamat: $("#Alamat").val().trim(),
        StatusIuran: $("#StatusIuran").val().trim(),
        Kontak: $("#Kontak").val().trim(),
    }

    $.ajax({
        type: "POST",
        url: "/Input/UploadIuran",
        data: JSON.stringify(clsinputiuran),
        contentType: "application/json",
        success: function (response) {
            if (response.Remarks != true) {
                alert(response.Message);
                window.location.href = "/Home/InputIuran";
                return;
            }
            else {
                alert(response.Message);
                window.location.href = "/Home/InputIuran";
                return;
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        },
    })
}


function validateInput() {
    var kontak = $("#Kontak").val().trim();

    if ($("#Nama").val() == "") {
        $("#message").text("Nama Tidak Boleh Kosong")
        return false;
    } else if ($("#Alamat").val() == "") {
        $("#message").text("Alamat Tidak Boleh Kosong")
        return false;
    } else if ($("#StatusIuran").val() == "") {
        $("#message").text("Status Iuran Harus Di Pilih")
        return false;
    } else if (kontak == "") {
        $("#message").text("Kontak Tidak Boleh Kosong")
        return false;
    } else if(!/^\d+$/.test(kontak)) {
        $("#message").text("Kontak hanya boleh berisi angka");
        return false;
    } else if (kontak.startsWith("00")) {
        $("#message").text("Kontak tidak boleh diawali dengan '00'");
        return false;
    } else if (kontak.length < 10 || kontak.length > 13) {
        $("#message").text("Kontak harus memiliki 10-13 digit");
        return false;
    } else if (!kontak.startsWith("0")) {
        $("#message").text("Kontak harus diawali dengan '0'");
        return false;  
    }
    return true;
}


//$("#formUploadExcel").submit(function (event) {
//    event.preventDefault(); // Mencegah reload halaman

//    let file = $("#fileExcel")[0].files[0];
//    if (!file) {
//        showAlert("Pilih file Excel terlebih dahulu!", "danger");
//        return;
//    }

//    let formData = new FormData();
//    formData.append("file", file);

//    $.ajax({
//        url: "/Input/UploadBulkExcel",
//        type: "POST",
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (response) {
//            showAlert(response.message, "success");
//        },
//        error: function (xhr) {
//            showAlert("Upload gagal: " + xhr.responseText, "danger");
//        }
//    });
//}});

//function showAlert(message, type) {
//        $("#alert-message")
//            .removeClass("d-none alert-success alert-danger")
//            .addClass("alert-" + type)
//            .html(message)
//            .fadeIn();

//        setTimeout(function () {
//            $("#alert-message").fadeOut();
//        }, 3000);
//    }
//});


$("#uploadForm").submit(function (e) {
    e.preventDefault(); // Mencegah form submit secara langsung

    var formData = new FormData();
    var fileInput = $("#fileInput")[0].files[0];

    if (!fileInput) {
        alert("Pilih file sebelum mengupload!");
        return;
    }

    formData.append("file", fileInput);

    $.ajax({
        url: "/Home/UploadExcel", // Sesuaikan dengan route controller
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            $(".btn-warning").prop("disabled", true).text("Uploading...");
        },
        success: function (response) {
            alert(response.message); // Tampilkan pesan sukses/gagal
            location.reload(); // Refresh halaman setelah sukses
        },
        error: function () {
            alert("Terjadi kesalahan saat mengupload file.");
        },
        complete: function () {
            $(".btn-warning").prop("disabled", false).text("Upload");
        }
    });
});

