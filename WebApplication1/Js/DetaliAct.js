$(document).ready(function () {
    
})

$("#searchBtn").click(function () {
    let selectedKegiatan = $("#NamaKegiatan").val();
    let startDate = $("#startDate").val();
    let endDate = $("#endDate").val();

    $.ajax({
        url: "/Input/GetKegiatan",
        type: "GET",
        data: {
            nama: selectedKegiatan,
            start: startDate,
            end: endDate
        },
        success: function (response) {
            let tableBody = $("#kegiatanTable");
            tableBody.empty();

            if (response.length === 0) {
                tableBody.append("<tr><td colspan='5' class='text-center'>Data tidak ditemukan</td></tr>");
            } else {
                $.each(response, function (index, kegiatan) {
                    let undanganHTML = kegiatan.Undangan
                        ? `<iframe src="${kegiatan.Undangan}" width="100%" height="100"></iframe>`
                        : "Tidak ada undangan";

                    let row = `<tr>
                                <td>${kegiatan.NamaKegiatan}</td>
                                <td>${kegiatan.Tanggal}</td>
                                <td>${kegiatan.Lokasi}</td>
                                <td>${kegiatan.Deskripsi}</td>
                                <td>${undanganHTML}</td>
                            </tr>`;
                    tableBody.append(row);
                });
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat mengambil data.");
        }
    });
});