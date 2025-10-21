$(document).ready(function () {
    console.log("JavaScript berhasil dimuat!");
});

$("#btnSimpan").click(function () {
    console.log("Tombol Simpan diklik!");
    var isValid = validateKegiatan();

    if (!isValid) {
        $("#message").show();
        return;
    } else {
        $("#message").hide();
    }
    // Ambil nilai input
    let data = {
        NamaKegiatan: $("#NamaKegiatan").val(),
        Tanggal: $("#Tanggal").val(),
        Lokasi: $("#Lokasi").val(),
        Deskripsi: $("#Deskripsi").val(),
    };

    let fileInput = $("#Undangan")[0].files[0];
    let formData = new FormData();
    formData.append("NamaKegiatan", data.NamaKegiatan);
    formData.append("Tanggal", data.Tanggal);
    formData.append("Lokasi", data.Lokasi);
    formData.append("Deskripsi", data.Deskripsi);

    if (fileInput) {
        formData.append("Undangan", fileInput);
    }

    console.log("Data yang dikirim:", data);

    $.ajax({
        url: "/Input/Create",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("jwtToken") 
        },
        success: function (response) {
            console.log("Response dari server:", response);
            if (response.success) {
                alert("Data berhasil disimpan!");
                window.location.href = "/Home/InputAct";
            } else {
                alert("Gagal menyimpan data.");
            }
        },
        error: function (xhr, status, error) {
            console.log("Error AJAX:", error);
            alert("Terjadi kesalahan saat menyimpan data.");
        }
    });
});


function validateKegiatan() {
    var namaKegiatan = $("#NamaKegiatan").val().trim();
    var tanggal = $("#Tanggal").val();
    var lokasi = $("#Lokasi").val().trim();
    var deskripsi = $("#Deskripsi").val().trim();
    var undangan = $("#Undangan")[0].files[0];

    // Validasi Nama Kegiatan
    if (namaKegiatan === "") {
        $("#message").text("Nama Kegiatan Tidak Boleh Kosong");
        return false;
    }

    // Validasi Tanggal
    if (!tanggal) {
        $("#message").text("Tanggal Kegiatan Harus Dipilih");
        return false;
    }

    // Validasi Lokasi
    if (lokasi === "") {
        $("#message").text("Lokasi Tidak Boleh Kosong");
        return false;
    }

    // Validasi Deskripsi
    if (deskripsi === "") {
        $("#message").text("Deskripsi Tidak Boleh Kosong");
        return false;
    }

    // Validasi Undangan (File Upload)
    if (!undangan) {
        $("#message").text("Silakan Upload Undangan");
        return false;
    }

    // Validasi ukuran file maksimal 2MB
    let maxSize = 2 * 1024 * 1024; // 2MB
    if (undangan.size > maxSize) {
        $("#message").text("Ukuran file terlalu besar! Maksimal 2MB.");
        return false;
    }

    // Validasi tipe file (hanya PDF, DOC, DOCX, JPG, PNG)
    let allowedExtensions = /(\.pdf|\.doc|\.docx|\.jpg|\.png)$/i;
    if (!allowedExtensions.test(undangan.name)) {
        $("#message").text("Format file tidak didukung! Pilih PDF, DOC, DOCX, JPG, atau PNG.");
        return false;
    }

    // Jika semua validasi lolos
    $("#message").text(""); // Hapus pesan error jika sudah benar
    return true;
}