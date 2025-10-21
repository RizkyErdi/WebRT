$(document).ready(function () {
    // Memanggil API untuk mendapatkan pesan dan jumlah pesan yang belum dibaca
    var messagesUrl = '@Session["MessagesUrl"]';  // Mendapatkan URL dari session
    console.log(messagesUrl);
    loadMessages();
    
});

// Fungsi untuk mengambil data pesan dan update UI
function loadMessages() {
    var messagesUrl = '@Session["MessagesUrl"]';
    // Mengambil pesan terbaru dan jumlah pesan yang belum dibaca
    $.ajax({
        url: "/Kontak/GetMessages",  // Menggunakan Razor untuk menghasilkan URL yang benar
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Menampilkan pesan di dropdown
            var messageList = $('#messageList');
            messageList.empty();  // Kosongkan daftar pesan sebelumnya
            data.forEach(function (message) {
                var messageItem = `
                    <a class="dropdown-item d-flex align-items-center" href="javascript:void(0);" onclick="viewMessage(${message.Id})">
                        <div class="dropdown-list-image mr-3">
                            <img class="rounded-circle" src="https://via.placeholder.com/50" alt="...">
                            <div class="status-indicator ${message.IsRead ? '' : 'bg-success'}"></div>
                        </div>
                        <div class="font-weight-bold">
                            <div class="text-truncate">
                                ${message.Message}
                            </div>
                            <div class="small text-gray-500">${message.Name} · ${new Date(message.CreatedDate).toLocaleTimeString()}</div>
                        </div>
                    </a>
                `;
                messageList.append(messageItem);
            });

            // Memperbarui jumlah pesan yang belum dibaca
            updateUnreadCount();
        }
    });
}

// Fungsi untuk menampilkan halaman baru dengan detail pesan
function viewMessage(messageId) {
    window.location.href = `/Kontak/DaftarPesan/${messageId}`;  // Pindahkan ke halaman detail
}

// Fungsi untuk memperbarui jumlah pesan yang belum dibaca
function updateUnreadCount() {
    $.ajax({
        url: "/Kontak/GetUnreadCount",  // Menggunakan Razor untuk menghasilkan URL yang benar
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#messageBadge').text(data);  // Memperbarui badge jumlah pesan yang belum dibaca
        }
    });
}

function viewMessage(messageId) {
    $.ajax({
        url: '/Kontak/DaftarPesan', // controller action yang return partial HTML atau plain text
        type: 'GET',
        data: { id: messageId },
        success: function (data) {
            $('#messageContent').html(data); // tampilkan ke modal
            $('#messageModal').modal('show'); // buka modal
        },
        error: function () {
            alert('Gagal memuat pesan.');
        }
    });
}