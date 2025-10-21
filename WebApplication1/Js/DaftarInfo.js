$(document).ready(function () {
    populateYears();
});

function populateYears() {
    let tahunSekarang = new Date().getFullYear();
    let tahunDropdown = $('#tahun');
    tahunDropdown.append(`<option value="">Semua</option>`);
    for (let i = tahunSekarang; i >= 2000; i--) {
        tahunDropdown.append(`<option value="${i}">${i}</option>`);
    }
}

function loadPengumuman() {
    let tahun = $('#tahun').val();
    let tipe = $('#tipe').val();
    $('#preview').hide().attr('src', '');

    $.ajax({
        url: '/Input/GetPengumuman',
        type: 'GET',
        data: { tahun: tahun, tipe: tipe },
        success: function (data) {
            console.log("Response from API:", data); // Debugging output
            let daftar = $('#daftarPengumuman');
            daftar.empty();
            if (data.length === 0) {
                daftar.append('<p>Tidak ada pengumuman.</p>');
                $('#preview').hide();
            } else {
                data.forEach(function (item) {
                    let link = `<p><a href="#" onclick="previewFile('${item.nama_file}')">${item.tipe_pengumuman} - ${item.tahun_pengumuman}</a></p>`;
                    daftar.append(link);
                });
            }
        }
    });
}

function previewFile(filePath) {
    $('#preview').attr('src', filePath).show();
}