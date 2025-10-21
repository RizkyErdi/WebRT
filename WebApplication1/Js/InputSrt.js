$(document).ready(function () {

})

function savesurat() {
    $("#loader").addClass("is-active");
    var nama = $("#nama").val();
    var nik = $("#nik").val();
    var alamat = $("#alamat").val();
    var jenisSurat = $("#jenis_surat").val();
    var tujuan = $("#tujuan").val();

    // Validasi form
    var isValid = true;
    if ($("#nama").val().trim() === "" || $("#nik").val().trim() === "" || $("#jenis_surat").val() === "") {
        isValid = false;
    }

    if (!isValid) {
        $("#message").show().text("Mohon lengkapi semua data wajib.");
        $("#loader").removeClass("is-active");
        return;
    } else {
        $("#message").hide();
    }

    // Ambil data form (non-file) dan buat objek FormData
    var formData = new FormData();
    formData.append("Nama", nama);
    formData.append("NIK", nik);
    formData.append("Alamat", alamat);
    formData.append("JenisSurat", jenisSurat);
    formData.append("Tujuan", tujuan);

    // Tambahkan file jika ada
    if ($("#dokumen")[0].files.length > 0) {
        formData.append("dokumen", $("#dokumen")[0].files[0]);
    }

    // Kirim data via AJAX
    $.ajax({
        url: "/Pengajuan/SaveSurat", // Sesuaikan dengan endpoint
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.Remarks != true) {
                alert(response.Message || "Pengajuan gagal.");
                window.location.href = "/pengajuan-surat/list"; // redirect jika gagal
            } else {
                alert(response.Message || "Pengajuan berhasil.");
                window.location.reload(); // reload jika berhasil
            }
        },
        error: function (errMsg) {
            alert("Terjadi kesalahan saat menyimpan data.");
            console.error(errMsg);
        },
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
}

