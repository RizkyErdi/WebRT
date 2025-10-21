$(document).ready(function () {
    populateYears();

    $("#uploadForm").on("submit", function (event) {
        event.preventDefault();
        UploadPengumuman();
    });
});

function populateYears() {
    const tahunPengumumanSelect = $("#tahunPengumuman");
    if (tahunPengumumanSelect.length) {
        const tahunSekarang = new Date().getFullYear();
        for (let i = tahunSekarang; i >= 2010; i--) {
            tahunPengumumanSelect.append(new Option(i, i));
        }
    }
}

function validateInput() {
    if ($("#tipePengumuman").val() === "") {
        $("#message").text("Tipe Pengumuman Tidak Boleh Kosong").show();
        return false;
    } else if ($("#tahunPengumuman").val() === "") {
        $("#message").text("Tahun Pengumuman Tidak Boleh Kosong").show();
        return false;
    } else if (!$("#fileUpload")[0].files.length) {
        $("#message").text("File PDF Harus Dipilih").show();
        return false;
    }
    $("#message").hide();
    return true;
}

function UploadPengumuman() {
    var isValid = validateInput();
    if (!isValid) {
        return;
    }

    var formData = new FormData($("#uploadForm")[0]);

    $.ajax({
        type: "POST",
        url: "/Input/UploadPengumuman",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.Remarks !== true) {
                alert(response.Message);
                return;
            } else {
                alert(response.Message);
                $("#iframePengumuman").attr("src", "/Home/DaftarPengumuman");
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        },
    });
}
