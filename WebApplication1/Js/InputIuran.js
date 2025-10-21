$(document).ready(function () {
    BulanIuran()
    populateYears();
})

function UploadIuran() {
    $("#loader").addClass("is-active");

    var ClsHome = {
        username: $("#username").val(),
        password: $("#password").val(),
        role: $("#RoleUser").val(),

    };

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
        NIK: $("#NIK").val().trim(),
        Alamat: $("#Alamat").val().trim(),
        StatusIuran: $("#StatusIuran").val().trim(),
        Bulan: $("#BulanIuran").val() + " " + $("#tahunIuran").val(),
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
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    })
}


function validateInput() {
    var kontak = $("#Kontak").val().trim();
    var NIK = $("#NIK").val().trim();

    if ($("#Nama").val() == "") {
        $("#message").text("Nama Tidak Boleh Kosong")
        return false;
    } else if (NIK == "") {
        $("#message").text("NIK Tidak Boleh Kosong")
        return false;
    } else if (NIK.length != 16) {
        $("#message").text("NIK harus 16 digit")
        return false;
    } else if (!/^\d{16}$/.test(NIK)) {
        $("#message").text("NIK hanya boleh angka")
        return false;
    } else if ($("#Alamat").val() == "") {
        $("#message").text("Alamat Tidak Boleh Kosong")
        return false;
    } else if ($("#StatusIuran").val() == "") {
        $("#message").text("Status Iuran Harus Di Pilih")
        return false;
    } else if
        ($("#BulanIuran").val() == "") {
        $("#message").text("Silahkan Pilih Bulan Iuran")
        return false;
    } else if ($("#tahunIuran").val() == "") {
        $("#message").text("Silahkan Pilih Tahun Iuran")
        return false;
    } else if (kontak == "") {
        $("#message").text("Kontak Tidak Boleh Kosong")
        return false;
    } else if (!/^\d+$/.test(kontak)) {
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

$("#uploadForm").submit(function (e) {
    $("#loader").addClass("is-active");
    e.preventDefault(); // Mencegah form submit secara langsung

    var formData = new FormData();
    var fileInput = $("#fileInput")[0].files[0];

    if (!fileInput) {
        alert("Pilih file sebelum mengupload!");
        return;
    }

    formData.append("file", fileInput);

    $.ajax({
        url: "/Input/UploadExcel", // Sesuaikan dengan route controller
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            $(".btn-warning").prop("disabled", true).text("Uploading...");
        },
        success: function (response) {
            if (response.success) {
                alert(response.message); // Tampilkan pesan sukses
                location.reload(); // Refresh halaman setelah sukses
            } else {
                // Tampilkan pesan error jika ada
                alert(response.message); // Jika ada error dalam response
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat mengupload file.");
        },
        complete: function () {
            $(".btn-warning").prop("disabled", false).text("Upload");
        },
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
});

function BulanIuran() {
    $.ajax({
        type: "GET",
        url: "/Home/GetBulan",
        dataType: "json",
        success: function (response) {
            console.log("API Response:", response);
            if (response.success) {  // Perbaiki kondisi pengecekan
                console.log("Data Diterima:", response.data);
                var items = "<option value='' selected>Silahkan pilih</option>";

                $.each(response.data, function (key, item) {
                    items += "<option value='" + item.BULAN_HURUF + "'>" + item.BULAN_HURUF + "</option>";
                });

                $("#BulanIuran").html(items); // Gunakan .html() agar tidak duplikasi data
            } else {
                console.warn("Data kosong atau tidak ditemukan");
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
        }
    });
}

function populateYears() {
    var currentYear = new Date().getFullYear(); // Tahun saat ini
    var startYear = currentYear - 10; // Mulai dari 10 tahun yang lalu
    var endYear = currentYear + 5; // Hingga 5 tahun ke depan
    var options = "<option value='' selected>Silahkan pilih tahun</option>";

    // Mengisi dropdown dengan tahun
    for (var year = startYear; year <= endYear; year++) {
        options += "<option value='" + year + "'>" + year + "</option>";
    }

    // Menambahkan options ke dropdown
    $("#tahunIuran").html(options);
}