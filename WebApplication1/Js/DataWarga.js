$(document).ready(function () {
    // You can add other DOM ready logic here if needed
});

function Submitdata() {
    $("#loader").addClass("is-active");
    var dataWarga = {
        NIK: $("#nik").val().trim(),
        NoKK: $("#nokk").val().trim(),
        Nama: $("#Nama").val().trim(),
        TempatLahir: $("#TempatLahir").val().trim(),
        TglLahir: $("#TglLahir").val(),
        JenisKelamin: $("#JenisKelamin").val(),
        Alamat: $("#Alamat").val().trim(),
        RT: $("#RT").val().trim(),
        RW: $("#RW").val().trim(),
        Kelurahan: $("#Kelurahan").val().trim(),
        Kecamatan: $("#Kecamatan").val().trim(),
        Agama: $("#Agama").val(),
        StatusKawin: $("#StatusKawin").val(),
        Pekerjaan: $("#Pekerjaan").val().trim(),
        Kewarganegaraan: $("#Kewarganegaraan").val().trim(),
        NoHp: $("#NoHp").val().trim(),
        Email: $("#Email").val().trim(),
        TglDaftar: $("#TglDaftar").val()
    };

    if (!validateInput(dataWarga)) {
        $("#message").show();
        $("#loader").removeClass("is-active");
        return;
    }

    $("#message").hide();

    $.ajax({
        type: "POST",
        url: "/Input/SimpanWarga", // Ganti sesuai URL controller kamu
        data: JSON.stringify(dataWarga),
        contentType: "application/json",
        success: function (response) {
            alert(response.Message);
            window.location.reload(); // atau redirect ke halaman lain
        },
        error: function (err) {
            alert("Terjadi kesalahan: " + err.responseText);
        }, 
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
}


function validateInput(data) {
    if (data.NIK == "") {
        showMessage("NIK tidak boleh kosong");
        return false;
    } else if (data.NIK.length != 16 || !/^\d{16}$/.test(data.NIK)) {
        showMessage("NIK harus 16 digit dan hanya angka");
        return false;
    } else if (data.NoKK == "") {
        showMessage("No KK tidak boleh kosong");
        return false;
    } else if (data.NoKK.length != 16 || !/^\d{16}$/.test(data.NoKK)) {
        showMessage("No KK harus 16 digit dan hanya angka");
        return false;
    } else if (data.Nama == "") {
        showMessage("Nama tidak boleh kosong");
        return false;
    } else if (data.TempatLahir == "") {
        showMessage("Tempat Lahir tidak boleh kosong");
        return false;
    } else if (data.TglLahir == "") {
        showMessage("Tanggal Lahir harus diisi");
        return false;
    } else if (data.JenisKelamin == "") {
        showMessage("Pilih jenis kelamin");
        return false;
    } else if (data.Alamat == "") {
        showMessage("Alamat tidak boleh kosong");
        return false;
    } else if (data.RT == "") {
        showMessage("RT tidak boleh kosong");
        return false;
    } else if (data.RW == "") {
        showMessage("RW tidak boleh kosong");
        return false;
    } else if (data.Kelurahan == "") {
        showMessage("kelurahan tidak boleh kosong");
        return false;
    } else if (data.NoKK == "") {
        showMessage("No KK tidak boleh kosong");
        return false;
    } else if (data.Kecamatan == "") {
        showMessage("Kecamatan tidak boleh kosong");
        return false;
    } else if (data.Agama == "") {
        showMessage("Agama Harus dipilih");
        return false;
    } else if (data.StatusKawin == "") {
        showMessage("Status Kawin Harus dipilih");
        return false;
    } else if (data.Pekerjaan == "") {
        showMessage("Pekerjaan tidak boleh kosong");
        return false;
    }else if (data.Kewarganegaraan == "") {
        showMessage("Kewarganegaraan tidak boleh kosong");
        return false;
    } else if (data.NoHp == "") {
        showMessage("No HP tidak boleh kosong");
        return false;
    } else if (!/^\d+$/.test(data.NoHp)) {
        showMessage("No HP hanya boleh angka");
        return false;
    } else if (!data.NoHp.startsWith("0")) {
        showMessage("No HP harus diawali 0");
        return false;
    } else if (data.NoHp.length < 10 || data.NoHp.length > 13) {
        showMessage("No HP harus 10–13 digit");
        return false;
    } else if (data.Email == "") {
        showMessage("Email tidak boleh kosong");
        return false;
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.Email)) {
        showMessage("Format email tidak valid");
        return false;
    } else if (data.TglDaftar == "") {
        showMessage("Tanggal terdaftar tidak boleh kosong");
        return false;
    }
    return true;
}

function showMessage(msg) {
    $("#message").text(msg);
}


$('#uploadForm').submit(function (e) {
    $("#loader").addClass("is-active");
    e.preventDefault();  // Mencegah form untuk submit secara normal

    var formData = new FormData(this); // Mengambil data form

    // Lakukan pengiriman data form ke server
    $.ajax({
        type: "POST",
        url: "/Input/UploadDataWarga", // URL controller untuk menangani upload
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            alert(response.Message); // Tampilkan pesan dari server setelah upload sukses
            window.location.reload(); // Reload halaman atau arahkan ke halaman lain
        },
        error: function (err) {
            alert("Terjadi kesalahan: " + err.responseText);
        },
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
})