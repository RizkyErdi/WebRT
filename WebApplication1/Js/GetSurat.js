$(document).ready(function () {
    $.ajax({
        url: '/Pengajuan/GetListSurat',
        type: 'GET',
        success: function (data) {
            if (data && data.length > 0) {
                data.forEach((item, index) => {
                    const card = `
                        <div class="col-md-4 mb-3">
                            <div class="card shadow-sm border-0 rounded-4">
                                <div class="card-body">
                                    <h5 class="card-title">${item.nama_lengkap}</h5>
                                    <p class="card-text">${item.jenis_surat}</p>
                                    <button class="btn btn-outline-primary btn-sm" onclick="showDetail(${index})">Lihat Detail</button>
                                </div>
                            </div>
                        </div>
                    `;
                    $('#card-container').append(card);

                    // Simpan datanya sementara (agar bisa diakses saat modal dibuka)
                    window.suratList = window.suratList || [];
                    window.suratList[index] = item;
                });
            }
        },
        error: function () {
            alert('Gagal mengambil data pengajuan.');
        }
    });
});

function showDetail(index) {
    const data = window.suratList[index];
    let html = `
        <p><strong>Nama:</strong> ${data.nama_lengkap}</p>
        <p><strong>NIK:</strong> ${data.nik}</p>
        <p><strong>Alamat:</strong> ${data.alamat}</p>
        <p><strong>Jenis Surat:</strong> ${data.jenis_surat}</p>
        <p><strong>Tujuan:</strong> ${data.tujuan}</p>
        <p><strong>Email:</strong> ${data.email}</p>
        <input type="hidden" id="selectedId" value="${data.id}">
    `;
    $('#modalDetailBody').html(html);
    $('#detailModal').modal('show');
}

$('#btnUpload').click(function () {
    const id = $('#selectedId').val();
    $.post('/Pengajuan/ApproveSurat', { id: id }, function (response) {
        alert(response.Message);
        location.reload();
    });
});

$('#btnReject').click(function () {
    const id = $('#selectedId').val();
    const reason = $('#reasonField').val();
    if (!reason) {
        alert("Tolong isi reason untuk reject.");
        return;
    }

    $.post('/Pengajuan/RejectSurat', { id: id, reason: reason }, function (response) {
        alert(response.Message);
        location.reload();
    });
});
